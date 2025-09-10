//using Plisky.Plumbing.Legacy;

using Plisky.Diagnostics.FlimFlam;

namespace Plisky.FlimFlam {

    internal class Job_PartialPurgeApp : BaseJob {
        private int virtualIndexOfPurgerequest;

        internal Job_PartialPurgeApp(string machineName, int pid) {
            TracedApplication ta = MexCore.TheCore.DataManager.GetKnownApplicationByPid(pid, machineName);
            //Bilge.Assert(ta != null, "pid/machine name passed to Job_PartialPurge constructor do not map to a valid traced application.  This should not be possible");
            virtualIndexOfPurgerequest = ta.VirtualIndex;
        }

        internal Job_PartialPurgeApp(int virtualIndex) {
            virtualIndexOfPurgerequest = virtualIndex;
        }

        internal override bool CanPushBackUpStack() {
            return false;
        }

        internal override void ExecuteJob() {
            //Bilge.Log("Mex::WorkManager >> Found PartialPurgeAppByIndex request, asking the datamanager to do it");
            MexCore.TheCore.DataManager.PurgePartialKnownApplication(virtualIndexOfPurgerequest);
            MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.PurgeJobCompletes, UserMessageType.InformationMessage, null);
        }

        internal override string GetIdentifier() {
            return "Purge Job >> PurgeTracedAppByIndex";
        }

        internal override bool InitialiseJob(out bool requiresNotificationSuspense) {
            requiresNotificationSuspense = true;
            return true;
        }

        internal override void PerformPostJob() {
            MexCore.TheCore.ViewManager.ProcessChangeNotification(virtualIndexOfPurgerequest);
        }

        internal override JobVerificationResults VerifyOtherJobsOnStack(BaseJob alternative) {
            return JobVerificationResults.None;
        }
    }

    internal class Job_PurgeAllData : BaseJob {
        private int vIndexToExclude = -2;

        internal Job_PurgeAllData() {
        }

        internal Job_PurgeAllData(int vIndexToExclude) {
            //Bilge.Assert(vIndexToExclude > -2, "Passing -2 or less to a virtual index is not a valid use of the exclusion parameter in PurgeAllData");

            this.vIndexToExclude = vIndexToExclude;
        }

        internal override bool CanPushBackUpStack() {
            return false;
        }

        internal override void ExecuteJob() {
            MexCore.TheCore.MessageManager.PurgeAllData();

            MexCore.TheCore.DataManager.PurgeUnknownApplications();

            //Bilge.Log("Purging Known Appliactions");
            if (vIndexToExclude == -2) {
                MexCore.TheCore.DataManager.PurgeAllData();
            } else {
                MexCore.TheCore.DataManager.PurgeAllKnownApplicationsExceptThisOne(vIndexToExclude);
            }

            MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.PurgeJobCompletes, UserMessageType.InformationMessage, null);
        }

        internal override string GetIdentifier() {
            return "Purge Job >> PurgeAllData";
        }

        internal override bool InitialiseJob(out bool requiresNotificationSuspense) {
            requiresNotificationSuspense = true;
            return true;
        }

        internal override void PerformPostJob() {
            MexCore.TheCore.ViewManager.ProcessChangeNotification(null);
        }

        internal override JobVerificationResults VerifyOtherJobsOnStack(BaseJob alternative) {
            return JobVerificationResults.None;
        }
    }

    internal class Job_PurgeNonTracedApps : BaseJob {

        internal override bool CanPushBackUpStack() {
            return false;
        }

        internal override void ExecuteJob() {
            //Bilge.Log("Mex::WorkManager >> Found PurgeNonTracedApps request, asking data manager to do it");

            MexCore.TheCore.DataManager.PurgeUnknownApplications();
            MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.PurgeJobCompletes, UserMessageType.InformationMessage, null);
        }

        internal override string GetIdentifier() {
            return "Purge Job >> PurgeTracedAppByIndex";
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
    }

    internal class Job_PurgeTracedAppByIndex : BaseJob {
        private int vindex;

        internal Job_PurgeTracedAppByIndex(int theVindex) {
            vindex = theVindex;
        }

        internal override bool CanPushBackUpStack() {
            return false;
        }

        internal override void ExecuteJob() {
            MexCore.TheCore.DataManager.PurgeKnownApplication(vindex);
            MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.PurgeJobCompletes, UserMessageType.InformationMessage, null);
        }

        internal override string GetIdentifier() {
            return "Purge Job >> PurgeTracedAppByIndex";
        }

        internal override bool InitialiseJob(out bool requiresNotificationSuspense) {
            requiresNotificationSuspense = true;
            return true;
        }

        internal override void PerformPostJob() {
            MexCore.TheCore.ViewManager.ProcessChangeNotification(null);
        }

        internal override JobVerificationResults VerifyOtherJobsOnStack(BaseJob alternative) {
            return JobVerificationResults.None;
        }
    } // End Job_PurgeTracedAppByIndex

    // End job_ACtivateODSGatherer.

    // End job_ACtivateODSGatherer.

    // End job_ACtivateODSGatherer.
}