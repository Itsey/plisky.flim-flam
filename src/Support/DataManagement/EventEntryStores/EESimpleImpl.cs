namespace Plisky.FlimFlam {

    using System.Collections.Generic;
    using Plisky.Diagnostics.FlimFlam;

    /// <summary>
    /// This implementation is not designed to be the end solution, hence the strange use of a list of EEStoredElements, its designed as an interim while we get the
    /// code up and running.
    /// </summary>
    public class EESimpleImpl : EventEntryStore {
        private List<EventEntryStoredElement> lst = new List<EventEntryStoredElement>();

        protected override void ActualAddEntry(SingleOriginEvent ee) {
            EventEntryStoredElement newElement = new EventEntryStoredElement(ee, null, null);
            lock (lst) {
                lst.Add(newElement);
            }
        }

#if TESTACTIVE

        public bool Test_IsStoreEmpty() {
            lock (lst) {
                return lst.Count == 0;
            }
        }

        public long Test_GetEventEntryCount() {
            lock (lst) {
                return lst.Count;
            }
        }

#endif

        public override IEnumerable<SingleOriginEvent> GetEntries() {
            List<SingleOriginEvent> result = new List<SingleOriginEvent>();
            foreach (var v in lst) {
                result.Add(v.Entry);
            }
            return result;
        }
    }
}