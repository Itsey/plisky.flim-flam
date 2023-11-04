//using Plisky.Diagnostics;
using Plisky.Diagnostics.FlimFlam;

namespace Plisky.FlimFlam {

    /// <summary>
    /// Responsible for the currently active filter.  Events can be passed to this which will determine whether they are
    /// included or excluded by the current filter options.  This implements a chain of the different filter responsibilities
    /// which will be called one after the other to determine whether or not an event should actually be filtered out.
    /// </summary>
    public class ActiveEventFilter : IFilterProvider {

        public ActiveEventFilter(FilterLink initial /*, Bilge useThisTrace=null*/) {
            if (initial == null) {
                throw new InvalidOperationException("The chain can never be empty, you must have at least one filter link");
            }

            /*if (useThisTrace == null) {
                b = new Bilge(tl: System.Diagnostics.TraceLevel.Verbose);
            }*/
            this.chain = initial;
        }

        public FilterLink chain { get; set; }

        //protected Bilge b;
        public int LinkCount { get; set; }

        public void AddFilterLink(FilterLink fl) {
        }

        public bool IncludeEvent(SingleOriginEvent evt) {
            //b.Assert.True(chain != null, "Should not be possible, the constructor should force the chain to be initialised");

            if (evt == null) {
                throw new InvalidOperationException("The event can not be empty, you must have passed an event to check");
            }
            return chain.IncludeEvent(evt);
        }

        public bool IncludeProcess(int originId) {
            //b.Assert.True(chain != null, "Should not be possible, the constructor should force the chain to be initialised");

            if (originId == 0) {
                throw new InvalidOperationException("The origin Id must be set to a process for it to be included");
            }
            return chain.IncludeProcess(originId);
        }
    }
}