namespace Plisky.FlimFlam;

internal class Job_NotifyNewEventAdded : Job_Notification {
    private int affectedIndex;

    internal Job_NotifyNewEventAdded(int appAffectedIndex) {
        affectedIndex = appAffectedIndex;
    }

    internal override bool CanPushBackUpStack() {
        return false;
    }

    internal override void ExecuteJob() {
        //Bilge.Log("Mex::WorkManager >> Found Notify_NewEventAdded request, asking view manager to do it");
        //Bilge.Warning("Mex::WorkManager >> WARNING >> Should check for duplicate jobs here.  TODO");
        MexCore.TheCore.ViewManager.ProcessNewEventNotification(affectedIndex);
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
