namespace Plisky.FlimFlam;  

internal abstract class Job_Notification : BaseJob {
    
} 

internal class Job_NotifyAlertRecieved : Job_Notification {

    internal Job_NotifyAlertRecieved() {
    }

    internal override bool CanPushBackUpStack() {
        return false;
    }

    internal override void ExecuteJob() {        
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
