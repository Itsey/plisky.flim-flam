namespace Plisky.FlimFlam {
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