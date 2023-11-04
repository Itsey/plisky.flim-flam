namespace Plisky.FlimFlam { 

    /// <summary>
    /// Summary description for NewJob.
    /// </summary>
    internal class NewJob {
        internal JobList job;
        internal object Param;

        internal NewJob(JobList jl, object par) {
            job = jl;
            Param = par;
        }
    }
}