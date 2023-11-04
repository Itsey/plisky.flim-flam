namespace Plisky.FlimFlam { 

    internal enum JobVerificationResults {
        None,
        CurrentJobRendersFutureJobRedundant,
        FutureJobRendersCurrentJobRedundant,
        FutureJobModifiedSuchThatCurrentIsRedundant
    }

    internal abstract class BaseJob {
        //private int m_backoutCount;
        //private DateTime m_pushBackStartTime;

        /// <summary>
        /// This indicates that this job has been deleted.  We do it this way as its simpler than managing the stack of jobs and adding and removing
        /// them.
        /// </summary>
        internal bool JobDeleted;

        internal BaseJob() {
        }

        internal abstract bool CanPushBackUpStack();

        internal abstract void ExecuteJob();

        internal abstract string GetIdentifier();  // tring ident

        internal abstract bool InitialiseJob(out bool requiresNotificationSuspense);

        internal abstract void PerformPostJob();

        internal abstract JobVerificationResults VerifyOtherJobsOnStack(BaseJob alternative);

        // Defaults to false.
    }
}