//using Plisky.Plumbing.Legacy;
using System.Collections;
using System.Threading;

namespace Plisky.FlimFlam {
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