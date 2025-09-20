namespace Plisky.FlimFlam; 
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

