namespace Plisky.FlimFlam {

    using System.Threading;

    internal class Job_CheckIncommingQueue : BaseJob {

        internal override bool CanPushBackUpStack() {
            return false;
        }

        internal override void ExecuteJob() {// TODO : look for any and all of the same jobs on the queue
                                             //Bilge.Log("Mex::WorkManager >> Found PRocessNextStoredMessage request, asking message manager to do it");
                                             //Bilge.Warning("Mex::WorkManager >> WARNING >> Should check for duplicate jobs here.  TODO");

            // if theres LOTS of messages waiting give the threadpool some work to do, but only twice at maximum
            if (MexCore.TheCore.MessageManager.MessagesPendingImport > MexCore.TheCore.Options.MessagesToSpawnThreadsFor) {
                _ = ThreadPool.QueueUserWorkItem(new WaitCallback(MexCore.TheCore.MessageManager.BackgroundProcessAllMessages), null);
                if (MexCore.TheCore.MessageManager.MessagesPendingImport > (MexCore.TheCore.Options.MessagesToSpawnThreadsFor * 2)) {
                    _ = ThreadPool.QueueUserWorkItem(new WaitCallback(MexCore.TheCore.MessageManager.BackgroundProcessAllMessages), null);
                }
            }
            // This is the UI reporting one that lets the UI know every now and again that there is still stuff happening
            MexCore.TheCore.MessageManager.ProcessNextStoredMessage(true, true);
        }

        internal override string GetIdentifier() {
            return "ProcessNextStoredMessage Job";
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

    // End job_ACtivateODSGatherer.
    // End job_ACtivateODSGatherer.
}