//using Plisky.Plumbing.Legacy;
using System;
using System.Collections;
using System.Threading;

namespace Plisky.FlimFlam { 

    /// <summary>
    /// Mildly Type safe wrapper around the ArrayList for traced applications
    /// </summary>
    internal class TracedApplicationsArrayList : IEnumerator {
        internal ReaderWriterLock TracedApplicationsDataRWL = new ReaderWriterLock();

        #region locking code for aquiring  / freeing locks to all owned traced applications

        private object AllTracedAppsLockingObject = new object();

        internal bool AquireAllTracedApplicationsReaderLocks() {
            lock (AllTracedAppsLockingObject) {
                try {
                    foreach (TracedApplication ta in m_store) {
                        ta.EventEntries.EventEntriesRWL.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);
                    }
                    return true;
                } catch (ApplicationException /*aex*/) {
                    // timeout occured trying to get the locks
                    foreach (TracedApplication ta in m_store) {
                        if (ta.EventEntries.EventEntriesRWL.IsReaderLockHeld) { ta.EventEntries.EventEntriesRWL.ReleaseReaderLock(); }
                    }
                    return false;
                }
            } // End Lock statement
        } // End AqureAllTracedApplicationsReaderLocks

        internal bool AquireAllTracedApplicationsWriterLocks() {
            lock (AllTracedAppsLockingObject) {
                try {
                    foreach (TracedApplication ta in m_store) {
                        ta.EventEntries.EventEntriesRWL.AcquireWriterLock(Consts.MS_TIMEOUTFORLOCKS);
                    }
                    return true;
                } catch (ApplicationException /*aex*/) {
                    // timeout occured trying to get the locks
                    foreach (TracedApplication ta in m_store) {
                        if (ta.EventEntries.EventEntriesRWL.IsWriterLockHeld) { ta.EventEntries.EventEntriesRWL.ReleaseWriterLock(); }
                    }
                    return false;
                }
            } // end lock statement
        }

        internal bool FreeAllTracedApplicationsReaderLocks() {
            lock (AllTracedAppsLockingObject) {
                foreach (TracedApplication ta in m_store) {
                    ta.EventEntries.EventEntriesRWL.ReleaseReaderLock();
                }
                return true;
            } // end lock statement
        }

        internal bool FreeAllTracedApplicationsWriterLocks() {
            lock (AllTracedAppsLockingObject) {
                foreach (TracedApplication ta in m_store) {
                    ta.EventEntries.EventEntriesRWL.ReleaseWriterLock();
                }
                return true;
            } // end lock statement
        }

        #endregion locking code for aquiring  / freeing locks to all owned traced applications

        private int currentIndex = -1;
        private ArrayList m_store;

        internal TracedApplicationsArrayList() : base() {
            this.m_store = new ArrayList(20);
        }

        object IEnumerator.Current {
            // returns invalidoperationexecption if before first or after last element
            get {
                if (currentIndex == -1) {
                    throw new InvalidOperationException("Current positiioned before the collection");
                }
                return this.m_store[currentIndex];
            }
        }

        internal int Count {
            get { return this.m_store.Count; }
        }

        /// <summary>
        /// TracedApplication indexer using a logical index to identify the chosen applications. The index
        /// field here is not based on PID but is a logical and incremental index.
        /// </summary>
        internal TracedApplication this[int index] {
            get {
                //Bilge.Assert(index >= 0, "Request to get a TracedApplication with an invalid index was made - the index was out of range (<0)");
                //Bilge.Assert(index < m_store.Count, "Request to get a TracedApplication with an invalid index was made, the index was too high.");

                return (TracedApplication)this.m_store[index];
            } // End Get Indexer for logical index

            set {
                //Bilge.Assert(index >= 0, "Request to get a TracedApplication with an invalid index was made - the index was out of range (<0)");
                //Bilge.Assert(index < m_store.Count, "Request to get a TracedApplication with an invalid index was made, the index was too high.");

                this.m_store[index] = value;
            }
        }

        /// <summary>
        /// TracedApplication indexer using the processId and machine name to identify the chosen traced
        /// application.  NB the integer parameter for this indexer is NOT the index of the application but
        /// instead is the ProcessId.  Indexer will return null if no matching application could be found.
        /// </summary>
        internal TracedApplication this[int Pid, string machineName] {
            get {
                try {
                    TracedApplicationsDataRWL.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);
                    //Bilge.ResourceGrab(TracedApplicationsDataRWL, "TracedApplicationsDataRWL");
                    try {
                        foreach (TracedApplication ta in this.m_store) {
                            // TODO : This fix needs to be put back into Tex, having it rename the machine is a bad thing for this indexing.  Temporarily
                            // going to fix it here in Mex.

                            if ((ta.ProcessIdNo == Pid) && (ta.MachineName == machineName)) {
                                return ta;
                            }
                        } // End foreach traced application

                        // If its null try again ignoring machine name
                        foreach (TracedApplication ta in this.m_store) {
                            if (ta.ProcessIdNo == Pid) {
                                return ta;
                            }
                        }
                    } finally {
                        //Bilge.ResourceFree(TracedApplicationsDataRWL, "TracedApplicationsDataRWL");
                        TracedApplicationsDataRWL.ReleaseReaderLock();
                    }

                    // The traced application could not be found - return null
                    return null;
                } catch (ApplicationException aex) {
                    //Bilge.Dump(aex, "Exception occured, probably trying to get a reader lock for all traced applciations to run through them");
                    return null;
                }
            } // End get Accessor for pid,machinename indexer

            set { this[Pid, machineName] = value; }
        }

        public IEnumerator GetEnumerator() {
            return this.m_store.GetEnumerator();
        }

        bool IEnumerator.MoveNext() {
            // Keep moving till you run out or false if there is no more
            // Invalid operation instruction if there is a change since last reset
            do {
                currentIndex++;
                if ((this.m_store[currentIndex] != null)) {
                    return true;
                }
            } while (currentIndex < this.m_store.Count);
            return false;
        }

        void IEnumerator.Reset() {
            // Invalid operation exception if changed
            currentIndex = -1;
        }

        internal int Add(TracedApplication ta) {
            TracedApplicationsDataRWL.AcquireWriterLock(Consts.MS_TIMEOUTFORLOCKS);
            //Bilge.ResourceGrab(TracedApplicationsDataRWL, "TracedApplicationsDataRWL");
            try {
                return this.m_store.Add(ta);
            } finally {
                //Bilge.ResourceFree(TracedApplicationsDataRWL, "TracedApplicationsDataRWL");
                TracedApplicationsDataRWL.ReleaseWriterLock();
            }
        } // End TracedApplicationsArrayList::Add

        /// <summary>
        /// Adds a new traced application to the store of traced applications.  This will create and initialise
        /// the new traced application class and then return the index of the traced application.
        /// </summary>
        /// <param name="pid">Integer process Id of the application that generated the debug string</param>
        /// <param name="machineName">string machine name that the application is running on</param>
        /// <param name="virtualIndex"></param>
        /// <returns></returns>
        internal int AddNew(int pid, string machineName, int virtualIndex) {
            return this.m_store.Add(new TracedApplication(pid, machineName, virtualIndex));
        }

        internal void Clear() {
            this.m_store.Clear();
        }

        // end pid/machinename based indexer
        /// <summary>
        /// This routine will return the index of an application by using its PID and Machine name to locate it in the
        /// array of applications.  The Index can then be used as a rapid way of referring back to the application.
        /// </summary>
        /// <param name="pid">The Process ID of the application</param>
        /// <param name="machineName">The machine name of the machine the application runs on</param>
        /// <returns></returns>
        internal int GetIndexOfApplication(int pid, string machineName) {
            for (int a = 0; a < this.m_store.Count; a++) {
                if (((TracedApplication)this.m_store[a]).IsBeingPurged) {
                    continue;
                }

                if ((((TracedApplication)this.m_store[a]).ProcessIdNo == pid) && ((TracedApplication)this.m_store[a]).MachineName == machineName) {
                    return a;
                }
            }
            return -1;
        }

        internal void Remove(TracedApplication ta) {
            TracedApplicationsDataRWL.AcquireWriterLock(Consts.MS_TIMEOUTFORLOCKS);
            //Bilge.ResourceGrab(TracedApplicationsDataRWL, "TracedApplicationsDataRWL");
            try {
                this.m_store.Remove(ta);
            } finally {
                //Bilge.ResourceFree(TracedApplicationsDataRWL, "TracedApplicationsDataRWL");
                TracedApplicationsDataRWL.ReleaseWriterLock();
            }
        } // End TracedApplicationsArrayList::Remove

        internal void RemoveAt(int tracedApplicationIdx) {
            //Bilge.Assert(TracedApplicationsDataRWL.IsWriterLockHeld, "The WriterLock must be held before RemoveAt is called on the TracedApplicationsList");

            this.m_store.RemoveAt(tracedApplicationIdx);
        }

        // end index based indexer

        // end TracedApplicationsArrayList::GetIndexOfApplication
    }
}