using System.Collections;

namespace Plisky.FlimFlam;
// TODO : Refactor this properly, now its this big it should be interface based

internal enum JobList {
    LoadTraceFile,
    ActivateODSGatherer,
    DeactivateODSGatherer,
    CheckIncomingQueue,
    ShutDown,
    RefreshTimedView,

    /// <summary>
    /// Perform a purge based on the name in the parameter - a purgebynameparametertype with the exception of the pid in the parameter type. This is used
    /// to purge all other instances of an application not the one thats just been created.
    /// </summary>
    PurgeByMatchingName,

    PurgeAllData,
    PurgeTracedAppByIndex,
    PurgeNonTracedApps,

    /// <summary>
    /// Perform a partial purge whereby all of the entries before this point are removed from the application, but the application itself along
    /// with all resource information and tioming information is left in tact.
    /// </summary>
    PartialPurgeAppByIndex,

    /// <summary>
    /// The option set should be loaded - this either happens on the start of the application or when specifically requested.
    /// </summary>
    LoadOptionsForUser,

    /// <summary>
    /// This means that the list of processes that are known about the application has changed.  This job is a notification job and therefore will
    /// cause the calling of any callbacks that are registered for this notification.  If there is a parameter passed then the notification is that
    /// soemthing like the name has changed for the index of the parameter.  If the parameter is null then its a general change to process list and
    /// all should be updated.
    /// </summary>
    Notify_KnownProcessUpdate,

    Notify_NewEventAdded,
    Notify_PurgeAllCompleted,

    /// <summary>
    /// Notification when the options changed,  the parameter will either be null in which case the general options have changed or will be an
    /// instance of the notification options class which will contian specific options that the main view is interested in looking at.
    /// </summary>
    Notify_OptionsChanged,

    /// <summary>
    /// Called once a new highlight request has been issued.  The highlight request triggers a job which runs through the structures applying
    /// the highlight changes.  When this is done it will ask the view to refresh so that it picks up the highlight request.
    /// </summary>
    ApplyHighlightToStructures,

    Unknown
}

internal struct NotificationOptionsParameter {
    internal string[] listOfFilters;

    internal NotificationOptionsParameter(string[] newFilterList) {
        listOfFilters = newFilterList;
    }
}

internal struct PurgeByNameParameter {
    internal string appName;
    internal int pid;

    internal PurgeByNameParameter(string nm, int p) {
        appName = nm; pid = p;
    }
}

/// <summary>
/// This is the main class for the mex controll - it handles the interaction between the mex interface, the incomming messages and the data
/// structures that are being managed.  As such it owns and runs on its own thread and other parts of the system can add job requests to
/// its queue which will be processed opne at a time.
/// </summary>
internal class PrimaryWorkManager {
    private static PrimaryWorkManager sm_primaryWM;
    private Queue jobQueue;

    private bool notificationJobsSuspended;

    private bool refuseAllNewTasks;

    private PrimaryWorkManager() {
        jobQueue = Queue.Synchronized(new Queue());
    }

    internal int JobsOutstanding {
        get { return this.jobQueue.Count; }
    }

    /// <summary>
    /// Property indicates whether there is currently any work that needs processing in the work queue
    /// </summary>
    internal bool WorkOutstanding {
        get { return jobQueue.Count > 0; }
    }

    /// <summary>
    /// Returns the single instance of the primary work manager to ensure that there is never more than one primary work manger active at any one time
    /// this is the only approved method of getting the primary work manager.
    /// </summary>
    /// <returns>The instance of the primary work manager for the mex viewer</returns>
    internal static PrimaryWorkManager GetPrimaryWorkManager() {
        if (sm_primaryWM == null) {
            sm_primaryWM = new PrimaryWorkManager();
        }
        return sm_primaryWM;
    }

    internal void AddJob(BaseJob theJob) {
        Internal_AddJob(theJob, false, true);
    }

    internal void AddJob(BaseJob theJob, bool forceWakeup) {
        Internal_AddJob(theJob, forceWakeup, true);
    }

    internal void AddJob(BaseJob theJob, bool forceWakeup, bool onlyIfNotAlreadyPresent) {
        if (!onlyIfNotAlreadyPresent) {
            Internal_AddJob(theJob, forceWakeup, true);
            return;
        }
        foreach (BaseJob bj in jobQueue) {
            if (bj.jobDeleted) { continue; }

            var jrv = theJob.VerifyOtherJobsOnStack(bj);
            switch (jrv) {
                case JobVerificationResults.CurrentJobRendersFutureJobRedundant:
                    bj.jobDeleted = true;
                    //Bilge.VerboseLog("Destroying an exsiting refresh job and placing this one on the queue");
                    Internal_AddJob(theJob, forceWakeup, true);
                    return;

                case JobVerificationResults.FutureJobRendersCurrentJobRedundant:
                    //Bilge.VerboseLog("Job destroyed as one already in the queue.");
                    return;

                case JobVerificationResults.FutureJobModifiedSuchThatCurrentIsRedundant:
                    //Bilge.VerboseLog("Job destroyed as one already in the queue.");
                    return;
            }
        }

        // If the queue is empty just place it on.
        Internal_AddJob(theJob, forceWakeup, true);
    }

    internal string DiagnosticsText() {
        return "Internal work queue length : " + jobQueue.Count.ToString();
    }

    /// <summary>
    /// This will check the work queue for a job and then perform the next job on the queue
    /// </summary>
    internal void DoWork() {
        if (jobQueue.Count > 0) {
            // TODO : Error handling and simplify the dequeue mechanism

            var aJob = (BaseJob)jobQueue.Dequeue();
            ProcessJob(aJob);
        }
    }

    /// <summary>
    /// This will perform the action specified by a particular job and parameter.  This method understands all of the elements of the system
    /// required to perform the operations that can be listed as a job.  As a result this method is fairly crucial to the operation of mex.
    /// </summary>
    /// <param name="theJob"></param>
    internal void ProcessJob(BaseJob theJob) {
        //Bilge.Log("PrimaryWorkManager::ProcessJob -> called for Job (" + theJob.GetIdentifier() + ")");
        if (refuseAllNewTasks) {
            //Bilge.Warning("All new tasks being refused as shutdown has been called.  No new work should be being requested");
            return;
        }

        bool notificaitonSuspensionRequired = false;

        foreach (BaseJob bj in jobQueue) {
            if (!bj.jobDeleted) {
                var result = theJob.VerifyOtherJobsOnStack(bj);

                switch (result) {
                    case JobVerificationResults.CurrentJobRendersFutureJobRedundant:
                        bj.jobDeleted = true;
                        break;

                    case JobVerificationResults.FutureJobRendersCurrentJobRedundant:
                        theJob.jobDeleted = true;
                        break;

                    case JobVerificationResults.FutureJobModifiedSuchThatCurrentIsRedundant:
                        theJob.jobDeleted = true;
                        break;
                }
            }
        }

        if (theJob.jobDeleted) {
            //Bilge.VerboseLog("Job " + theJob.GetIdentifier() + " has been deleted, efficiency!");
            return;
        }

        if (!theJob.InitialiseJob(out notificaitonSuspensionRequired)) {
        }

        if (notificaitonSuspensionRequired) {
            SuspendNotificationJobs();

            try {
                theJob.ExecuteJob();
            } finally {
                ResumeNotificationJobs();
            }
        } else {
            theJob.ExecuteJob();
        }

        theJob.PerformPostJob();
    }

    /// <summary>
    /// Allows them to be added to the queue again
    /// </summary>
    internal void ResumeNotificationJobs() {
        //Bilge.Log("Mex::PrimaryWorkManager -> ***** NOTIFICAITONS RESUMED, All ok ****");
        notificationJobsSuspended = false;
    }

    internal void ShutDown() {
        // no threads here therefore time to shutdown
        refuseAllNewTasks = true;
        jobQueue.Clear();
    }

    /// <summary>
    /// Stops notification jobs being added to the queue
    /// </summary>
    internal void SuspendNotificationJobs() {
        //Bilge.Log("Mex::PrimaryWorkManager -> ***** NOTIFICAITONS SUSPENDED, Must match Resume... ****");
        notificationJobsSuspended = true;
    }

    // Interlal_Ad Job is called by all of the other add jobs. This migrates towards class based jobs now the apps more complex
    private void Internal_AddJob(BaseJob theJob, bool causeWakeup, bool allowQueue) {
        if (refuseAllNewTasks) {
            //Bilge.Log("Mex::Core::AddJob >> Shutdown has been called for WorkManager, yet new task was requested");
            //Bilge.Warning("Mex::Core::AddJob >> Refusing to queue task, Core Shutting Down -> Job refused was " + theJob.GetIdentifier());
            return;
        }

        if ((notificationJobsSuspended == true) && (theJob is Job_Notification)) {
            //if ((jobId == JobList.Notify_KnownProcessUpdate) || (jobId == JobList.Notify_NewEventAdded) || (jobId == JobList.Notify_PurgeAllCompleted)) {
            //Bilge.Log("Mex::Core::AddJob -> Job Skipped, " + theJob.GetIdentifier() + " skipped as notifications are disabled.");
            return;
            //}
        }

        if (allowQueue) {
            // Asynch Work request
            jobQueue.Enqueue(theJob);
        } else {
            // Synchronous work request
            ProcessJob(theJob);
        }

        if (causeWakeup) {
            MexCore.TheCore.PokeCoreThread();
        }
    }
}