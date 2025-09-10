namespace Plisky.FlimFlam {

    /// <summary>
    /// Summary description for NewJob.
    /// </summary>
    internal class NewJob {
        internal JobList Job { get; set; }
        internal object Param { get; set; }

        internal NewJob(JobList jl, object par) {
            Job = jl;
            Param = par;
        }
    }
}