namespace Plisky.FlimFlam;

internal class Job_NotifyKnownProcessUpdate : Job_Notification {
    private int virtIndexOfUpdatedProcess = 0;

    internal Job_NotifyKnownProcessUpdate() {
        virtIndexOfUpdatedProcess = -1; // all ?
    }

    internal Job_NotifyKnownProcessUpdate(int virtIdentityOfUpdatedProcess) {
        virtIndexOfUpdatedProcess = virtIdentityOfUpdatedProcess;
    }

    internal override bool CanPushBackUpStack() {
        return false;
    }

    internal override void ExecuteJob() {
        //Bilge.Log("Mex::WorkManager >> Found ProcessChangeNotificationJob request, asking view manager to do it");
        //Bilge.Warning("Mex::WorkManager >> WARNING >> Should check for duplicate jobs here.  TODO");

        if (virtIndexOfUpdatedProcess >= 0) {
            MexCore.TheCore.ViewManager.ProcessChangeNotification(virtIndexOfUpdatedProcess);
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
