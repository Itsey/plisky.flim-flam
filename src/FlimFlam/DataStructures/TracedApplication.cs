//using Plisky.Plumbing.Legacy;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Linq;
using Plisky.Diagnostics;
using Plisky.Diagnostics.FlimFlam;

namespace Plisky.FlimFlam { 

    /// <summary>
    /// The traced application class holds all of the trace data that has been captured for a single application
    /// when this is set not active the data manager will free up the resources that are allocated by it and
    /// then set it to be reclaimable.  Once it is reclaimable then it can be reusued for the next application
    /// that comes along.  The EventEntries contians a list of all of the logs that have been captured for this application
    /// while the other fields contain additional data about the application.
    /// </summary>
    /// <remarks>While the internal interface is available, the pulich method should be used whenever possible</remarks>
    internal class TracedApplication {
        internal string CallingAssemblyInfo = string.Empty;
        internal bool ContainsMoreThanOneThread;
        internal bool ContainsResourceData;

        // True if any event entry contains resource data
        internal bool ContainsTimingData;

        internal bool ContainsTraceData;

        // True if any entry contains trace data.
        // True if any event entry contains timing data
        // True if more than one thread is in the event entries
        internal string CurrentlyUsedThreadIdx = string.Empty;

        internal EventEntryArrayList EventEntries;
        internal string ExecutingAssemblyInfo = string.Empty;
        internal bool IsBeingPurged;
        internal DateTime LastUpdatedOrViewed;
        internal string MachineName = string.Empty;
        internal string MainModule = string.Empty;
        internal string PreferredName = string.Empty;

        // TODO : Replace this with something less boxy - it must be perf hit
        internal int ProcessIdNo = -1;

        internal string ProcessLabel = string.Empty;
        internal string ProcessName = string.Empty;
        internal Dictionary<string, string> ThreadNames = new Dictionary<string, string>();
        internal DateTime TimeLastMessageRecvd;
        internal int VirtualIndex = -1;
        internal string WindowTitle = string.Empty;
        private string assemblyFullNameValue = string.Empty;
        private List<EventEntry> CustomDataStore = new List<EventEntry>();

        // Locks the list of event entries so that nasties dont happen - this is only supported for this interface.
        private ReaderWriterLock eeLock = null;

        private string initialiseNameValue = string.Empty;
        private string m_pidStringIdent = "none";

        /// <summary>
        /// Default TracedApplication constructor.  TracedApplication.Initialise must be called before the
        /// traced application can be used.
        /// </summary>
        internal TracedApplication()
            : base() {
            EventEntries = new EventEntryArrayList();
            eeLock = new ReaderWriterLock();
            this.ProcessIdNo = -1;
            this.m_pidStringIdent = "-1";
            this.PreferredName = null;
            this.MachineName = null;
            this.ProcessName = null;
            this.MainModule = null;
            this.WindowTitle = null;
            this.VirtualIndex = -1;
            this.TimeLastMessageRecvd = DateTime.Now;
        }

        /// <summary>
        /// TraceApplication constructor that creates the object and also calls Initialise.  Once this has
        /// been called the traced application is directly available for use.
        /// </summary>
        /// <param name="Pid">Process Id of the application being traced</param>
        /// <param name="machineName">Machine name that the application is running on</param>
        /// <param name="allocatedVirtualIndex">The newly allocated virtual index to use</param>
        internal TracedApplication(int Pid, string machineName, int allocatedVirtualIndex)
            : this() {
            this.ProcessIdNo = Pid;
            this.m_pidStringIdent = Pid.ToString();
            this.MachineName = machineName;
            this.ProcessName = "Unknown";
            this.VirtualIndex = allocatedVirtualIndex;
        }

        internal TracedApplication(TracedApplication dupe, string identName, long lowIndex, long hiIndex)
            : this() {
            dupe.eeLock.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);
            //Bilge.ResourceGrab(dupe.eeLock, "eeLock");
            try {
                this.ProcessIdNo = -1;
                this.m_pidStringIdent = "Dupe: " + dupe.m_pidStringIdent;
                this.PreferredName = dupe.PreferredName;
                this.MachineName = dupe.MachineName;
                this.MainModule = dupe.MainModule;
                this.ProcessLabel = identName;
                this.ProcessName = dupe.ProcessName;
                this.WindowTitle = dupe.WindowTitle;
                this.CallingAssemblyInfo = dupe.CallingAssemblyInfo;
                this.ContainsMoreThanOneThread = dupe.ContainsMoreThanOneThread;
                this.ContainsResourceData = dupe.ContainsResourceData;
                this.ContainsTimingData = dupe.ContainsTimingData;
                this.ContainsTraceData = dupe.ContainsTraceData;
                this.CurrentlyUsedThreadIdx = dupe.CurrentlyUsedThreadIdx;
                this.ExecutingAssemblyInfo = dupe.ExecutingAssemblyInfo;
                // Threadnames!?

                this.eeLock.AcquireWriterLock(Consts.MS_TIMEOUTFORLOCKS);
                //Bilge.ResourceGrab(eeLock, "eeLock");
                try {
                    // Now copy across all of the event entries.
                    for (int i = 0; i < dupe.EventEntries.Count; i++) {
                        if ((lowIndex >= 0) && (dupe.EventEntries[i].GlobalIndex < lowIndex)) { continue; }
                        if ((hiIndex >= 0) && (dupe.EventEntries[i].GlobalIndex > hiIndex)) { break; }
                        this.EventEntries.Add(dupe.EventEntries[i]);  // This is done by reference.  Therefore two hooks to the same entry. Be wary.
                    }
                } finally {
                    this.eeLock.ReleaseWriterLock();
                    //Bilge.ResourceFree(eeLock, "eeLock");
                }
            } finally {
                //Bilge.ResourceFree(dupe.eeLock, "eeLock");
                dupe.eeLock.ReleaseReaderLock();
            }
        }

        // TracedApplication Constructor override
        internal TracedApplication(TracedApplication dupe, string identName)
            : this(dupe, identName, -1, -1) {
        }

        public bool ContainsCustomData { get; internal set; }

        public string StatusContents { get; set; }

        // TracedApplication default constructor
        public string StatusText { get; set; }

        internal string AssemblyFullName {
            get { return assemblyFullNameValue; }
            set { assemblyFullNameValue = value; }
        }

        internal string InitialiseName {
            get { return initialiseNameValue; }
            set { initialiseNameValue = value; }
        }

        // Holds the first thread idx to be used, if it doesent match on event add then the above is true

        internal string PreferredDisplayName {
            get {
                if ((this.PreferredName != null) && (this.PreferredName.Length > 0)) {
                    return PreferredName;
                } else {
                    return ProcessIdAsString;
                }
            }
            set { this.PreferredName = value; }
        }

        internal string ProcessIdAsString {
            get { return m_pidStringIdent; }
            set {
                if (ProcessIdNo == -1) {
                    m_pidStringIdent = value;
                }
            }
        }

        internal void AddCustomData(EventEntry custom) {
            lock (CustomDataStore) {
                CustomDataStore.Add(custom);
            }
        }

        /// <summary>
        /// When an event entry is to be added to the traced application this method is used - this will
        /// insert the event entry and then ensure that it is set active if the application itself is active.
        /// If the application itself is not active then an exception will be thrown.
        /// </summary>
        /// <param name="newone"></param>
        internal int AddEventEntry(EventEntry newone) {

            #region entry code

            //Bilge.Assert(newone != null, "cant add null event entry");

            #endregion entry code

            int idx = EventEntries.Add(newone);
            this.TimeLastMessageRecvd = newone.m_timeMessageRecieved;

            if (newone.cmdType == TraceCommandTypes.Custom) {
                lock (CustomDataStore) {
                    CustomDataStore.Add(newone);
                }
            }

            if (newone.cmdType == TraceCommandTypes.MoreInfo) {
                // If this command is a more info then tell the command before it that it has more info.
                if (idx > 0) {
                    // Its possible that we start mex at a bad moment.
                    EventEntries[idx - 1].HasMoreInfo = true;
                }
            }

            return idx;
        }

        /// <summary>
        /// Locks and returns the entry valid for the index.
        /// </summary>
        /// <param name="theIndex"></param>
        /// <returns></returns>
        internal EventEntry FindEventByGlobalIndex(long theIndex) {
            this.eeLock.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);
            //Bilge.ResourceGrab(eeLock, "eeLock");

            try {
                foreach (EventEntry ee in this.EventEntries) {
                    if (ee.GlobalIndex == theIndex) {
                        return ee;
                    }
                } // end for all of the entries
            } finally {
                //Bilge.ResourceFree(eeLock, "eeLock");
                this.eeLock.ReleaseReaderLock();
            }
            return null;
        }

        /// <summary>
        /// This method DOES NOT LOCK as it returns the index, should be within a lock to esnure valididity of the index
        /// </summary>
        /// <param name="theIndex"></param>
        /// <returns></returns>
        internal int FindEventIdxByGlobalIndex_NL(long theIndex) {
            for (int i = 0; i < this.EventEntries.Count; i++) {
                if (this.EventEntries[i].GlobalIndex == theIndex) {
                    return i;
                }
            }

            return -1;
        }

        internal IEnumerable<EventEntry> GetCustomData() {
            EventEntry[] ee;

            lock (CustomDataStore) {
                ee = CustomDataStore.ToArray();
            }

            foreach (var f in ee) {
                yield return f;
            }
        }

        internal KeyDisplayRepresentation GetThreadDisplayValue(EventEntry ee) {
            KeyDisplayRepresentation result;

            switch (MexCore.TheCore.Options.ThreadDisplayOption) {
                case ThreadDisplayMode.UseDotNetThread:
                    result = new KeyDisplayRepresentation(ee.CurrentThreadKey, ee.ThreadNetId);
                    break;

                case ThreadDisplayMode.UseOSThread:
                    result = new KeyDisplayRepresentation(ee.CurrentThreadKey, ee.ThreadID);
                    break;

                case ThreadDisplayMode.DefaultUseThreadNameAndOS:
                    if (ThreadNames.ContainsKey(ee.CurrentThreadKey)) {
                        result = new KeyDisplayRepresentation(ee.CurrentThreadKey, ThreadNames[ee.CurrentThreadKey] + " on " + ee.ThreadID);
                    } else {
                        result = new KeyDisplayRepresentation(ee.CurrentThreadKey, ee.ThreadNetId + " on " + ee.ThreadID);
                    }
                    break;

                case ThreadDisplayMode.ShowFullInformation:
                    if (ThreadNames.ContainsKey(ee.CurrentThreadKey)) {
                        result = new KeyDisplayRepresentation(ee.CurrentThreadKey, ThreadNames[ee.CurrentThreadKey] + " on " + ee.ThreadNetId + " on os: " + ee.ThreadID);
                    } else {
                        result = new KeyDisplayRepresentation(ee.CurrentThreadKey, "Unknown on " + ee.ThreadNetId + " on os: " + ee.ThreadID);
                    }
                    break;

                default:
                    throw new InvalidOperationException("This code should not be reachable");
            }

            return result;
        }

        //internal ProcViewFilter CurrentFilter=null;  this should not be here

        // End TracedApplication::AddEventEntry
        internal void SetStatusContentsFromXml(string tempDebugMessage) {
            string commandId = null;
            string title = null;
            string statusContenst = null;

            XDocument x = XDocument.Parse(tempDebugMessage);
            var tel = x.Element("cmdxml");
            XElement el = null;

            if (tel != null) {
                el = tel.Element("cmdid");
                if (el != null) {
                    commandId = el.Value;
                }
                el = tel.Element("hdr");
                if (el != null) {
                    title = el.Value;
                }
                el = tel.Element("cmdcontents");
                if (el != null) {
                    statusContenst = el.Value;
                }
                StatusText = title;
                StatusContents = statusContenst;
            }
        }

        /// <summary>
        /// Updates the lastupdatedorviewed time with the current time to ensure that the application
        /// is not purged by the data manager as it is still being used.
        /// </summary>
        internal void Touch() {
            LastUpdatedOrViewed = DateTime.Now;
        }
    }
}