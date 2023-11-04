using Plisky.Diagnostics.FlimFlam;

namespace Plisky.FlimFlam {

    /// <summary>
    /// Responsible for implementing the logic necessary to be a single link in the active filter chain of responsiblity.
    /// </summary>
    public abstract class FilterLink : IFilterProvider {
        public FilterLink Next { get; set; }

        public bool IncludeEvent(SingleOriginEvent evt) {
            bool res = Execute(evt);
            if (res) {
                if (Next != null) {
                    return Next.IncludeEvent(evt);
                } else {
                    return true;
                }
            }
            return false;
        }

        public bool IncludeProcess(int originId) {
            bool res = ExecuteProcess(originId);
            if (res) {
                if (Next != null) {
                    return Next.IncludeProcess(originId);
                } else {
                    return true;
                }
            }
            return false;
        }

        protected abstract bool Execute(SingleOriginEvent evt);

        protected abstract bool ExecuteProcess(int originId);
    }
}