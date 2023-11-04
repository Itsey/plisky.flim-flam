//using Plisky.Plumbing.Legacy;
using System;
using System.Collections;
using System.Threading;

namespace Plisky.FlimFlam { 

    /// <summary>
    /// Summary description for NonTracedApplicationStructure.
    /// </summary>
    internal class NonTracedApplicationEntry {
        internal long AssignedIndex;

        internal string DebugEntry;

        internal int Pid;

        internal ViewSpecificData ViewData;

        internal NonTracedApplicationEntry() {
            Pid = -1;
            AssignedIndex = -1;
        }

        internal NonTracedApplicationEntry(int incommingPid, string incommingDebugEntry, long gIndex) {
            if (incommingPid == -1) {
                // This indicates that they did not know the pid at the time that this message was placed into the structure.
                // sometiems this means that the pid is stored as the first part of the string, with a comma separating that and the text
                // this occurs for example on file imports.
                if (incommingDebugEntry.IndexOf(',') > 0) {
                    try {
                        string possiblePid = incommingDebugEntry.Substring(0, incommingDebugEntry.IndexOf(','));
                        incommingDebugEntry = incommingDebugEntry.Substring(incommingDebugEntry.IndexOf(',') + 1);  // Get rid of ####,
                        Pid = int.Parse(possiblePid);
                    } catch (FormatException ex) {
                        //Bilge.Dump(ex, "This MUST be a invalidformatException");
                        // ok forget it
                        Pid = incommingPid;
                    }
                } else {
                    // Ok we couldnt find a comma therefore its unlikely to be in the format ####,log
                    Pid = incommingPid;
                }
            } else { // End if incomming Pid ==-1, else they passed a valid pid so we just use that
                Pid = incommingPid;
            }
            // Now do the other two parts
            DebugEntry = incommingDebugEntry; AssignedIndex = gIndex;
        }

        internal string GetDiagnosticStringData() {
            string result = "NTA event for PID: " + Pid.ToString() + "\r\n";
            result += "Entry: " + DebugEntry + "\r\n";
            result += "Index: " + AssignedIndex.ToString() + "\r\n";
            result += ViewData.GetDiagnosticStringData();
            return result;
        }

        // End NonTracedApplicationEntry default constructor

        // End NonTracedApplicationEntry overloaded constructor
    } // End NonTracedApplicationEntry class definition

    /// <summary>
    /// Non traced applications array list is a type specific array list type for holding all
    /// of the entries for the nontraced applications.  It will expand as the usage grows, although
    /// in future may be replaced by something more efficient.
    /// </summary>
    internal class NonTracedApplicationsArrayList : IEnumerable {
        internal ReaderWriterLock NonTracedApplicationsDataRWL = new ReaderWriterLock();

        private ArrayList m_store = null;

        internal NonTracedApplicationsArrayList()
            : base() {
            this.m_store = new ArrayList();
        }

        internal int Count {
            get { return this.m_store.Count; }
        }

        internal NonTracedApplicationEntry this[int index] {
            get {
                return (NonTracedApplicationEntry)this.m_store[index];
            } // End Get Indexer for logical index

            set { this.m_store[index] = value; }
        }

        internal int Add(NonTracedApplicationEntry nta) {
            return this.m_store.Add(nta);
        }

        internal int AddNewEntry(int pid, string theentry) {
            return this.m_store.Add(new NonTracedApplicationEntry(pid, theentry, MexCore.TheCore.DataManager.GetNextGlobalIndex()));
        }

        // end index based indexer
        internal void Clear() {
            this.m_store.Clear();
        }

        #region IEnumerable Members

        public IEnumerator GetEnumerator() {
            return this.m_store.GetEnumerator();
        }

        #endregion IEnumerable Members
    } // End TracedApplicationsArrayList Definition
}