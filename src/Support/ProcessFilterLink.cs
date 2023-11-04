using Plisky.Diagnostics.FlimFlam;

namespace Plisky.FlimFlam {

    public class ProcessFilterLink : FilterLink {
        private List<int> IncludedProcesses;

        public ProcessFilterLink(List<int> IncludedProcesses) {
            // TODO: Complete member initialization
            this.IncludedProcesses = IncludedProcesses;
        }

        protected override bool Execute(SingleOriginEvent evt) {
            throw new NotImplementedException();
        }

        protected override bool ExecuteProcess(int originId) {
            throw new NotImplementedException();
        }
    }
}