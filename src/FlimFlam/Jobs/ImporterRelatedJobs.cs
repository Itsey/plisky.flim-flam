//using Plisky.Plumbing.Legacy;
using System.Diagnostics;
using System.IO;

namespace Plisky.FlimFlam {

    internal class Job_ActivateODSGatherer : BaseJob {
        private readonly bool activateTheGatherer;

        internal Job_ActivateODSGatherer(bool activate) {
            activateTheGatherer = activate;
        }

        internal override bool CanPushBackUpStack() {
            // There would be no point in delaying this request
            return false;
        }

        internal override void ExecuteJob() {
            //Bilge.Log("Mex::WorkManager >> Found ActivateODS Gatherer request, asking MessageManager to activate ODS capture");
            if (activateTheGatherer) {
                // Having this enabled while debugging is nasty.
                if (!Debugger.IsAttached) {
                    MexCore.TheCore.MessageManager.ActivateODSGatherer();
                }
            } else {
                MexCore.TheCore.MessageManager.DeactivateODSGatherer();
            }
        }

        internal override string GetIdentifier() {
            return "Activate ODS Gatherer Job";
        }

        internal override bool InitialiseJob(out bool requiresNotificationSuspense) {
            requiresNotificationSuspense = false;
            return true;
        }

        internal override void PerformPostJob() {
        }

        internal override JobVerificationResults VerifyOtherJobsOnStack(BaseJob alternative) {
            if (alternative is not Job_ActivateODSGatherer alt) { return JobVerificationResults.None; }   // No impact.

            // Essentially if there is another one which is the same then remove the other one, if its different then remove this one, as there
            // is no point turning it on then immediately off etc.
            return alt.activateTheGatherer == activateTheGatherer
                ? JobVerificationResults.CurrentJobRendersFutureJobRedundant
                : JobVerificationResults.FutureJobRendersCurrentJobRedundant;
        }
    } // End job_ACtivateODSGatherer.

    internal class Job_AddHttpPollGatherer : BaseJob {
        protected bool repeatIt;
        protected string uriToHit;

        internal Job_AddHttpPollGatherer(string fullUri, bool repeat) {
            uriToHit = fullUri;
            repeatIt = repeat;
        }

        internal override bool CanPushBackUpStack() {
            return false;
        }

        internal override void ExecuteJob() {
            MexCore.TheCore.MessageManager.RegisterHttpDataImport(uriToHit, repeatIt);
        }

        internal override string GetIdentifier() {
            return "Add HttpPoll Gatherer";
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

    internal class Job_ChangeTCPGathererState : BaseJob {
        private readonly bool changeStateToActivate;

        internal Job_ChangeTCPGathererState(bool activate) {
            changeStateToActivate = activate;
        }

        internal override bool CanPushBackUpStack() {
            // There would be no point in delaying this request
            return false;
        }

        internal override void ExecuteJob() {
            //Bilge.Log("Executing job Job_ChangeTCPGathererState", "Bringing the gatherer " + (changeStateToActivate ? "Online" : "Offline"));

            if (changeStateToActivate) {
                MexCore.TheCore.MessageManager.ActivateTCPGatherer();
            } else {
                MexCore.TheCore.MessageManager.DeactivateTCPGatherer();
            }
        }

        internal override string GetIdentifier() {
            return "ChangeTCPGatherer State Job";
        }

        internal override bool InitialiseJob(out bool requiresNotificationSuspense) {
            requiresNotificationSuspense = false;
            return true;
        }

        internal override void PerformPostJob() {
        }

        internal override JobVerificationResults VerifyOtherJobsOnStack(BaseJob alternative) {
            if (alternative is not Job_ChangeTCPGathererState alt) { return JobVerificationResults.None; }

            return alt.changeStateToActivate == changeStateToActivate
                ? JobVerificationResults.CurrentJobRendersFutureJobRedundant
                : JobVerificationResults.FutureJobRendersCurrentJobRedundant;
        }
    } // End Job_ActivateTCPGatherer

    internal class Job_LoadTraceFile : BaseJob {
        internal string assignIdentToProcess;
        private readonly string fileName;
        private bool initialised;
        private readonly FileImportMethod useThisImportMethod;

        internal Job_LoadTraceFile(string filename, FileImportMethod fmo) {
            fileName = filename;
            useThisImportMethod = fmo;

            initialised = true;
        }

        internal override bool CanPushBackUpStack() {
            return true;
        }

        internal override void ExecuteJob() {
            //Bilge.Log("Mex::WorkManager >> Found LoadFile Request, asking MessageManager to load file");
            MexCore.TheCore.MessageManager.LoadMessagesFromFile(fileName, useThisImportMethod);
        }

        internal override string GetIdentifier() {
            return "LoadTraceFile job";
        }

        internal override bool InitialiseJob(out bool requiresNotificationSuspense) {
            requiresNotificationSuspense = true;

            //Bilge.Assert(fileName != null, "Filename cant be null for the loadfromfile job being pulled from joq queue");
            //Bilge.Assert(File.Exists(fileName), "Filename does not exist, loadfromfile job cannot perfor on an empty file");

            if (!File.Exists(fileName)) { initialised = false; }

            return initialised;
        }

        internal override void PerformPostJob() {
        }

        internal override JobVerificationResults VerifyOtherJobsOnStack(BaseJob alternative) {
            return JobVerificationResults.None;
        }
    } // End job_ACtivateODSGatherer.
}