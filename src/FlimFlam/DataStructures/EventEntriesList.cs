//using Plisky.Plumbing.Legacy;
using System.Collections;
using System.Threading;

namespace Plisky.FlimFlam { 

    internal class EventEntryArrayList : IEnumerable {
        internal ReaderWriterLock EventEntriesRWL = new ReaderWriterLock();

        private ArrayList theStore = null;

        internal EventEntryArrayList()
            : base() {
            theStore = new ArrayList();
        }

        internal int Count {
            get { return theStore.Count; }
        }

        internal EventEntry this[int index] {
            get {
                return (EventEntry)theStore[index];
            } // End Get Indexer for logical index

            set { theStore[index] = value; }
        }

        internal int Add(EventEntry eve) {
            EventEntriesRWL.AcquireWriterLock(Consts.MS_TIMEOUTFORLOCKS);
            //Bilge.ResourceGrab(EventEntriesRWL, "EventEntriesRWL");
            try {
                return theStore.Add(eve);
            } finally {
                //Bilge.ResourceFree(EventEntriesRWL, "EventEntriesRWL");
                EventEntriesRWL.ReleaseWriterLock();
            }
        }

        internal void Clear() {
            EventEntriesRWL.AcquireWriterLock(Consts.MS_TIMEOUTFORLOCKS);
            //Bilge.ResourceGrab(EventEntriesRWL, "EventEntriesRWL");
            try {
                theStore.Clear();
            } finally {
                //Bilge.ResourceFree(EventEntriesRWL, "EventEntriesRWL");
                EventEntriesRWL.ReleaseWriterLock();
            }
        }

        /// <summary>
        /// This will run through all of the event entries in this event entries list and return the index of
        /// an event entry that mateches the supplied global Index.  If no index matches then it returns -1.
        /// </summary>
        /// <param name="globalIdx">The global index that is being looked for</param>
        /// <returns>The index of a successfull match.  -1 if no match made.</returns>
        internal int ContainsThisGlobalIdx(long globalIdx) {
            EventEntriesRWL.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);
            //Bilge.ResourceGrab(EventEntriesRWL, "EventEntriesRWL");
            try {
                for (int storeCnt = 0; storeCnt < theStore.Count; storeCnt++) {
                    if (((EventEntry)theStore[storeCnt]).GlobalIndex == globalIdx) {
                        return storeCnt;
                    }
                }
            } finally {
                //Bilge.ResourceFree(EventEntriesRWL, "EventEntriesRWL");
                EventEntriesRWL.ReleaseReaderLock();
            }
            return -1;
        }

        /// <summary>
        /// This wil run through all of the event entries in this event entries list and return the index of an
        /// event entry that matches the supplied message string.  It will only check that the message string matches
        /// not that it is indeed the same event entry.
        /// </summary>
        /// <param name="theMessage">The message string to match on</param>
        /// <returns>The index of a successfull match, or -1 if no match is made</returns>
        internal int ContainsThisMessage(string theMessage) {
            EventEntriesRWL.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);
            //Bilge.ResourceGrab(EventEntriesRWL, "EventEntriesRWL");
            try {
                for (int storeCnt = 0; storeCnt < theStore.Count; storeCnt++) {
                    if (((EventEntry)theStore[storeCnt]).DebugMessage == theMessage) {
                        return storeCnt;
                    }
                }
            } finally {
                //Bilge.ResourceFree(EventEntriesRWL, "EventEntriesRWL");
                EventEntriesRWL.ReleaseReaderLock();
            }
            return -1;
        }

        internal void RemoveAt(int idx) {
            EventEntriesRWL.AcquireWriterLock(Consts.MS_TIMEOUTFORLOCKS);
            //Bilge.ResourceGrab(EventEntriesRWL, "EventEntriesRWL");
            try {
                theStore.RemoveAt(idx);
            } finally {
                //Bilge.ResourceFree(EventEntriesRWL, "EventEntriesRWL");
                EventEntriesRWL.ReleaseWriterLock();
            }
        }

        internal void RemoveAtUnsafe(int idx) {
            theStore.RemoveAt(idx);
        }

        // end index based indexer

        #region IEnumerable Members

        public IEnumerator GetEnumerator() {
            return theStore.GetEnumerator();
        }

        #endregion IEnumerable Members
    } // End EventEntryArrayList Class Definition
}