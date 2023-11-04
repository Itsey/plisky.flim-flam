namespace Plisky.FlimFlam {
    using System;

    internal class SaveAllDataJob : BaseJob {

        internal override bool CanPushBackUpStack() {
            throw new NotImplementedException();
        }

        internal override void ExecuteJob() {
            throw new NotImplementedException();
        }

        internal override string GetIdentifier() {
            return "Save Job >> Save All Data Job";
        }

        internal override bool InitialiseJob(out bool requiresNotificationSuspense) {
            throw new NotImplementedException();
        }

        internal override void PerformPostJob() {
            throw new NotImplementedException();
        }

        internal override JobVerificationResults VerifyOtherJobsOnStack(BaseJob alternative) {
            return alternative.GetIdentifier() == GetIdentifier()
                ? JobVerificationResults.FutureJobRendersCurrentJobRedundant
                : JobVerificationResults.None;
        }
    }
}