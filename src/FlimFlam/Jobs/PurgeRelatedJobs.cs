//using Plisky.Plumbing.Legacy;

using Plisky.Diagnostics.FlimFlam;

namespace Plisky.FlimFlam { 

    internal class Job_PartialPurgeApp : BaseJob {
        private int m_virtualIndexOfPurgerequest;

        internal Job_PartialPurgeApp(string machineName, int pid) {
            TracedApplication ta = MexCore.TheCore.DataManager.GetKnownApplicationByPid(pid, machineName);
            //Bilge.Assert(ta != null, "pid/machine name passed to Job_PartialPurge constructor do not map to a valid traced application.  This should not be possible");
            m_virtualIndexOfPurgerequest = ta.VirtualIndex;
        }

        internal Job_PartialPurgeApp(int virtualIndex) {
            m_virtualIndexOfPurgerequest = virtualIndex;
        }

        internal override bool CanPushBackUpStack() {
            return false;
        }

        internal override void ExecuteJob() {
            //Bilge.Log("Mex::WorkManager >> Found PartialPurgeAppByIndex request, asking the datamanager to do it");
            MexCore.TheCore.DataManager.PurgePartialKnownApplication(m_virtualIndexOfPurgerequest);
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
            MexCore.TheCore.ViewManager.ProcessChangeNotification(m_virtualIndexOfPurgerequest);
        }

        internal override JobVerificationResults VerifyOtherJobsOnStack(BaseJob alternative) {
            return JobVerificationResults.None;
        }
    }

    internal class Job_PurgeAllData : BaseJob {
        private int m_vIndexToExclude = -2;

        internal Job_PurgeAllData() {
        }

        internal Job_PurgeAllData(int vIndexToExclude) {
            //Bilge.Assert(vIndexToExclude > -2, "Passing -2 or less to a virtual index is not a valid use of the exclusion parameter in PurgeAllData");

            m_vIndexToExclude = vIndexToExclude;
        }

        internal override bool CanPushBackUpStack() {
            return false;
        }

        internal override void ExecuteJob() {
            MexCore.TheCore.MessageManager.PurgeAllData();

            MexCore.TheCore.DataManager.PurgeUnknownApplications();

            //Bilge.Log("Purging Known Appliactions");
            if (m_vIndexToExclude == -2) {
                MexCore.TheCore.DataManager.PurgeAllData();
            } else {
                MexCore.TheCore.DataManager.PurgeAllKnownApplicationsExceptThisOne(m_vIndexToExclude);
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
        private int m_vindex;

        internal Job_PurgeTracedAppByIndex(int theVindex) {
            m_vindex = theVindex;
        }

        internal override bool CanPushBackUpStack() {
            return false;
        }

        internal override void ExecuteJob() {
            MexCore.TheCore.DataManager.PurgeKnownApplication(m_vindex);
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