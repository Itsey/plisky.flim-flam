namespace Plisky.FlimFlam;

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