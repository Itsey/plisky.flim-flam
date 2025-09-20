namespace Plisky.FlimFlam {
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

    // End job_ACtivateODSGatherer.
    // End job_ACtivateODSGatherer.
}