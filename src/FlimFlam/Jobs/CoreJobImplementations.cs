namespace Plisky.FlimFlam {

    using System.Threading;
    using Plisky.Diagnostics.FlimFlam;

    internal class Job_ApplyHighlightToStructures : BaseJob {

        internal override bool CanPushBackUpStack() {
            return false;
        }

        internal override void ExecuteJob() {
            //Bilge.Log("Mex::WorkManager >> Found apply highlight to structures request, applying highlight now");
            _ = MexCore.TheCore.DataManager.VisitEachKnownApplicationEventEntry(new DataStructureManager.VisitEachEventEntryCallback(MexCore.TheCore.ViewManager.ApplyCurrentHighlightOptionsToEventEntry), new DataStructureManager.VisitEachEventryCompletedCallback(MexCore.TheCore.ViewManager.HighlightUpdateComplete));
        }

        internal override string GetIdentifier() {
            return "Highlight Job";
        }

        internal override bool InitialiseJob(out bool requiresNotificationSuspense) {
            requiresNotificationSuspense = true;
            MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.BackgroundHighlightOperationBegins, UserMessageType.BackgroundProcessingMessage, null);

            return true;
        }

        internal override void PerformPostJob() {
            MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.BackgroundHighlightOperationEnds, UserMessageType.BackgroundProcessingMessage, null);
        }

        internal override JobVerificationResults VerifyOtherJobsOnStack(BaseJob alternative) {
            return JobVerificationResults.None;
        }
    }

    internal class Job_ApplyHighlightToView : BaseJob {

        internal override bool CanPushBackUpStack() {
            return false;
        }

        internal override void ExecuteJob() {
            //Bilge.Log("Mex::WorkManager >> Found apply highlight to structures request, applying highlight now");
            //MexCore.GetCore().DataManager.VisitEachKnownApplicationEventEntry(new DataStructureManager.VisitEachEventEntryCallback(MexCore.GetCore().ViewManager.ApplyCurrentHighlightOptionsToEventEntry), new DataStructureManager.VisitEachEventryCompletedCallback(MexCore.GetCore().ViewManager.HighlightUpdateComplete));

            _ = MexCore.TheCore.ViewManager.ApplyCurrentHighlightOptionsToView();
            MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.HighlightOperationRequestsViewRefresh, UserMessageType.InformationMessage, null);
            MexCore.TheCore.ViewManager.RefreshCurrentView(false);
        }

        internal override string GetIdentifier() {
            return "Highlight Job";
        }

        internal override bool InitialiseJob(out bool requiresNotificationSuspense) {
            requiresNotificationSuspense = false;
            // TODO : Notify user ?

            return true;
        }

        internal override void PerformPostJob() {
            //MexCore.GetCore().ViewManager.AddUserNotificationMessageByIndex(ViewSupportManager.UserMessages.BackgroundHighlightOperationEnds, ViewSupportManager.UserMessageType.BackgroundProcessingMessage);
        }

        internal override JobVerificationResults VerifyOtherJobsOnStack(BaseJob alternative) {
            return JobVerificationResults.None;
        }
    }

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

    internal class Job_ShutDown : BaseJob {

        internal override bool CanPushBackUpStack() {
            return false;
        }

        internal override void ExecuteJob() {
            //Bilge.Log("Mex::WorkManager >> Found ShutDown request, Asking Core to shutdown");
            MexCore.TheCore.ShutDownCore();
        }

        internal override string GetIdentifier() {
            return "Shutdown Job";
        }

        internal override bool InitialiseJob(out bool requiresNotificationSuspense) {
            requiresNotificationSuspense = true;
            return true;
        }

        internal override void PerformPostJob() {
        }

        internal override JobVerificationResults VerifyOtherJobsOnStack(BaseJob alternative) {
            return JobVerificationResults.None;
        }
    } // End job_ACtivateODSGatherer.

    // End job_ACtivateODSGatherer.
    // End job_ACtivateODSGatherer.
}