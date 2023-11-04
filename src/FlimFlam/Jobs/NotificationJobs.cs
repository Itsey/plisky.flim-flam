//using Plisky.Plumbing.Legacy;

namespace Plisky.FlimFlam { 

    internal abstract class Job_Notification : BaseJob {
        //private Job_Notification() { }
    } // End job_ACtivateODSGatherer.

    internal class Job_NotifyAlertRecieved : Job_Notification {

        internal Job_NotifyAlertRecieved() {
        }

        internal override bool CanPushBackUpStack() {
            return false;
        }

        internal override void ExecuteJob() {
            //Bilge.Log("Mex::WorkManager >> Found Notify_NewEventAdded request, asking view manager to do it");
            //Bilge.Warning("Mex::WorkManager >> WARNING >> Should check for duplicate jobs here.  TODO");
            //MexCore.TheCore.ViewManager.ProcessNewEventNotification(m_affectedIndex);
            //MexCore.TheCore.ViewManager.ProcessNewAlertNotification();
            MexCore.TheCore.ViewManager.ProcessAlertNotification();
        }

        internal override string GetIdentifier() {
            return "Notification >> NewAlert Job";
        }

        internal override bool InitialiseJob(out bool requiresNotificationSuspense) {
            requiresNotificationSuspense = false;
            return true;
        }

        internal override void PerformPostJob() {
        }

        internal override JobVerificationResults VerifyOtherJobsOnStack(BaseJob alternative) {
            return JobVerificationResults.None;
        }
    }

    internal class Job_NotifyKnownProcessUpdate : Job_Notification {
        private int m_virtIndexOfUpdatedProcess = 0;

        internal Job_NotifyKnownProcessUpdate() {
            m_virtIndexOfUpdatedProcess = -1; // all ?
        }

        internal Job_NotifyKnownProcessUpdate(int virtIdentityOfUpdatedProcess) {
            m_virtIndexOfUpdatedProcess = virtIdentityOfUpdatedProcess;
        }

        internal override bool CanPushBackUpStack() {
            return false;
        }

        internal override void ExecuteJob() {
            //Bilge.Log("Mex::WorkManager >> Found ProcessChangeNotificationJob request, asking view manager to do it");
            //Bilge.Warning("Mex::WorkManager >> WARNING >> Should check for duplicate jobs here.  TODO");

            if (m_virtIndexOfUpdatedProcess >= 0) {
                MexCore.TheCore.ViewManager.ProcessChangeNotification(m_virtIndexOfUpdatedProcess);
            } else {
                MexCore.TheCore.ViewManager.ProcessChangeNotification(null);
            }
        }

        internal override string GetIdentifier() {
            return "Notification >> KnownProcessUpdate Job";
        }

        internal override bool InitialiseJob(out bool requiresNotificationSuspense) {
            //Bilge.Log("Job NotifyKnownProcessUpdate initialised and ready to be processed");
            requiresNotificationSuspense = false;
            return true;
        }

        internal override void PerformPostJob() {
        }

        internal override JobVerificationResults VerifyOtherJobsOnStack(BaseJob alternative) {
            return JobVerificationResults.None;
        }
    }

    internal class Job_NotifyNewEventAdded : Job_Notification {
        private int m_affectedIndex;

        internal Job_NotifyNewEventAdded(int appAffectedIndex) {
            m_affectedIndex = appAffectedIndex;
        }

        internal override bool CanPushBackUpStack() {
            return false;
        }

        internal override void ExecuteJob() {
            //Bilge.Log("Mex::WorkManager >> Found Notify_NewEventAdded request, asking view manager to do it");
            //Bilge.Warning("Mex::WorkManager >> WARNING >> Should check for duplicate jobs here.  TODO");
            MexCore.TheCore.ViewManager.ProcessNewEventNotification(m_affectedIndex);
        }

        internal override string GetIdentifier() {
            return "Notification >> NewEventAdded Job";
        }

        internal override bool InitialiseJob(out bool requiresNotificationSuspense) {
            requiresNotificationSuspense = false;
            return true;
        }

        internal override void PerformPostJob() {
        }

        internal override JobVerificationResults VerifyOtherJobsOnStack(BaseJob alternative) {
            return JobVerificationResults.None;
        }
    }

    internal class Job_NotifyRefreshRequired : Job_Notification {
        private bool allowIncremental = true;
        private bool refreshAll; // Default false

        internal Job_NotifyRefreshRequired() {
            refreshAll = true;
        }

        internal Job_NotifyRefreshRequired(bool incrementalOk) {
            allowIncremental = incrementalOk;
        }

        internal override bool CanPushBackUpStack() {
            return false;
        }

        internal override void ExecuteJob() {
            //Bilge.Log("Mex::WorkManager >> Found Notify_Refresh request, asking view manager to do it");
            if (refreshAll) {
                MexCore.TheCore.ViewManager.ProcessNewEventNotification(-1);
            } else {
                MexCore.TheCore.ViewManager.RefreshCurrentView(allowIncremental);
            }
        }

        internal override string GetIdentifier() {
            return "Notification >> Refresh Required Job";
        }

        internal override bool InitialiseJob(out bool requiresNotificationSuspense) {
            requiresNotificationSuspense = false;
            return true;
        }

        internal override void PerformPostJob() {
        }

        internal override JobVerificationResults VerifyOtherJobsOnStack(BaseJob alternative) {
            return JobVerificationResults.None;
        }
    }
}