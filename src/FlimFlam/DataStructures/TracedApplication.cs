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
        internal string CallingAssemblyInfo { get; set; } = string.Empty;
        internal bool ContainsMoreThanOneThread { get; set; }
        internal bool ContainsResourceData { get; set; }
        internal bool ContainsTimingData { get; set; }
        internal bool ContainsTraceData { get; set; }
        internal string CurrentlyUsedThreadIdx { get; set; } = string.Empty;
        internal EventEntryArrayList EventEntries { get; set; }
        internal string ExecutingAssemblyInfo { get; set; } = string.Empty;
        internal bool IsBeingPurged { get; set; }
        internal DateTime LastUpdatedOrViewed { get; set; }
        internal string MachineName { get; set; } = string.Empty;
        internal string MainModule { get; set; } = string.Empty;
        internal string PreferredName { get; set; } = string.Empty;
        internal int ProcessIdNo { get; set; } = -1;
        internal string ProcessLabel { get; set; } = string.Empty;
        internal string ProcessName { get; set; } = string.Empty;
        internal Dictionary<string, string> ThreadNames { get; set; } = new();
        internal DateTime TimeLastMessageRecvd { get; set; }
        internal int VirtualIndex { get; set; } = -1;
        internal string WindowTitle { get; set; } = string.Empty;
        private string assemblyFullName = string.Empty;
        private List<EventEntry> customDataStore = new();
        private ReaderWriterLock eventEntriesLock = null;
        private string initialiseName = string.Empty;
        private string pidStringIdent = "none";

        /// <summary>
        /// Default TracedApplication constructor.  TracedApplication.Initialise must be called before the
        /// traced application can be used.
        /// </summary>
        internal TracedApplication()
            : base() {
            EventEntries = new EventEntryArrayList();
            eventEntriesLock = new ReaderWriterLock();
            ProcessIdNo = -1;
            pidStringIdent = "-1";
            PreferredName = null;
            MachineName = null;
            ProcessName = null;
            MainModule = null;
            WindowTitle = null;
            VirtualIndex = -1;
            TimeLastMessageRecvd = DateTime.Now;
        }

        /// <summary>
        /// TraceApplication constructor that creates the object and also calls Initialise.  Once this has
        /// been called the traced application is directly available for use.
        /// </summary>
        /// <param name="pid">Process Id of the application being traced</param>
        /// <param name="machineName">Machine name that the application is running on</param>
        /// <param name="allocatedVirtualIndex">The newly allocated virtual index to use</param>
        internal TracedApplication(int pid, string machineName, int allocatedVirtualIndex)
            : this() {
            ProcessIdNo = pid;
            pidStringIdent = pid.ToString();
            MachineName = machineName;
            ProcessName = "Unknown";
            VirtualIndex = allocatedVirtualIndex;
        }

        internal TracedApplication(TracedApplication dupe, string identName, long lowIndex, long hiIndex)
            : this() {
            dupe.eventEntriesLock.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);
            //Bilge.ResourceGrab(dupe.eventEntriesLock, "eeLock");
            try {
                ProcessIdNo = -1;
                pidStringIdent = "Dupe: " + dupe.pidStringIdent;
                PreferredName = dupe.PreferredName;
                MachineName = dupe.MachineName;
                MainModule = dupe.MainModule;
                ProcessLabel = identName;
                ProcessName = dupe.ProcessName;
                WindowTitle = dupe.WindowTitle;
                CallingAssemblyInfo = dupe.CallingAssemblyInfo;
                ContainsMoreThanOneThread = dupe.ContainsMoreThanOneThread;
                ContainsResourceData = dupe.ContainsResourceData;
                ContainsTimingData = dupe.ContainsTimingData;
                ContainsTraceData = dupe.ContainsTraceData;
                CurrentlyUsedThreadIdx = dupe.CurrentlyUsedThreadIdx ?? string.Empty;
                ExecutingAssemblyInfo = dupe.ExecutingAssemblyInfo;
                // Threadnames!?

                eventEntriesLock.AcquireWriterLock(Consts.MS_TIMEOUTFORLOCKS);
                //Bilge.ResourceGrab(eventEntriesLock, "eeLock");
                try {
                    // Now copy across all of the event entries.
                    for (int i = 0; i < dupe.EventEntries.Count; i++) {
                        if ((lowIndex >= 0) && (dupe.EventEntries[i].GlobalIndex < lowIndex)) { continue; }
                        if ((hiIndex >= 0) && (dupe.EventEntries[i].GlobalIndex > hiIndex)) { break; }
                        EventEntries.Add(dupe.EventEntries[i]);  // This is done by reference.  Therefore two hooks to the same entry. Be wary.
                    }
                } finally {
                    eventEntriesLock.ReleaseWriterLock();
                    //Bilge.ResourceFree(eventEntriesLock, "eeLock");
                }
            } finally {
                //Bilge.ResourceFree(dupe.eventEntriesLock, "eeLock");
                dupe.eventEntriesLock.ReleaseReaderLock();
            }
        }

        // TracedApplication Constructor override
        internal TracedApplication(TracedApplication dupe, string identName)
            : this(dupe, identName, -1, -1) {
        }

        public bool ContainsCustomData { get; internal set; }
        public string StatusContents { get; set; }
        public string StatusText { get; set; }
        internal string AssemblyFullName {
            get { return assemblyFullName; }
            set { assemblyFullName = value; }
        }
        internal string InitialiseName {
            get { return initialiseName; }
            set { initialiseName = value; }
        }
        internal string PreferredDisplayName {
            get {
                if ((PreferredName != null) && (PreferredName.Length > 0)) {
                    return PreferredName;
                } else if (!string.IsNullOrEmpty(ProcessName)) {
                    return ProcessName;
                } else { 
                    return ProcessIdAsString;
                }
            }
            set { PreferredName = value; }
        }
        internal string ProcessIdAsString {
            get { return pidStringIdent; }
            set {
                if (ProcessIdNo == -1) {
                    pidStringIdent = value;
                }
            }
        }
        internal void AddCustomData(EventEntry custom) {
            lock (customDataStore) {
                customDataStore.Add(custom);
            }
        }
        internal int AddEventEntry(EventEntry newone) {
            int idx = EventEntries.Add(newone);
            TimeLastMessageRecvd = newone.TimeMessageRecieved;
            if (newone.cmdType == TraceCommandTypes.Custom) {
                lock (customDataStore) {
                    customDataStore.Add(newone);
                }
            }
            if (newone.cmdType == TraceCommandTypes.MoreInfo) {
                if (idx > 0) {
                    EventEntries[idx - 1].HasMoreInfo = true;
                }
            }
            return idx;
        }
        internal EventEntry FindEventByGlobalIndex(long theIndex) {
            eventEntriesLock.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);
            try {
                foreach (EventEntry ee in EventEntries) {
                    if (ee.GlobalIndex == theIndex) {
                        return ee;
                    }
                }
            } finally {
                eventEntriesLock.ReleaseReaderLock();
            }
            return null;
        }
        internal int FindEventIdxByGlobalIndex_NL(long theIndex) {
            for (int i = 0; i < EventEntries.Count; i++) {
                if (EventEntries[i].GlobalIndex == theIndex) {
                    return i;
                }
            }
            return -1;
        }
        internal IEnumerable<EventEntry> GetCustomData() {
            EventEntry[] ee;
            lock (customDataStore) {
                ee = customDataStore.ToArray();
            }
            foreach (var f in ee) {
                yield return f;
            }
        }
        internal KeyDisplayRepresentation GetThreadDisplayValue(EventEntry ee) {
            KeyDisplayRepresentation result;
            switch (MexCore.TheCore.Options.ThreadDisplayOption) {
                case ThreadDisplayMode.UseDotNetThread:
                    result = new KeyDisplayRepresentation(ee.CurrentThreadKey, ee.threadNetId);
                    break;
                case ThreadDisplayMode.UseOSThread:
                    result = new KeyDisplayRepresentation(ee.CurrentThreadKey, ee.threadID);
                    break;
                case ThreadDisplayMode.DefaultUseThreadNameAndOS:
                    if (ThreadNames.ContainsKey(ee.CurrentThreadKey)) {
                        result = new KeyDisplayRepresentation(ee.CurrentThreadKey, ThreadNames[ee.CurrentThreadKey] + " on " + ee.threadID);
                    } else {
                        result = new KeyDisplayRepresentation(ee.CurrentThreadKey, ee.threadNetId + " on " + ee.threadID);
                    }
                    break;
                case ThreadDisplayMode.ShowFullInformation:
                    if (ThreadNames.ContainsKey(ee.CurrentThreadKey)) {
                        result = new KeyDisplayRepresentation(ee.CurrentThreadKey, ThreadNames[ee.CurrentThreadKey] + " on " + ee.threadNetId + " on os: " + ee.threadID);
                    } else {
                        result = new KeyDisplayRepresentation(ee.CurrentThreadKey, "Unknown on " + ee.threadNetId + " on os: " + ee.threadID);
                    }
                    break;
                default:
                    throw new InvalidOperationException("This code should not be reachable");
            }
            return result;
        }
        internal void SetStatusContentsFromXml(string tempDebugMessage) {
            string commandId = null;
            string title = null;
            string statusContenst = null;
            var x = XDocument.Parse(tempDebugMessage);
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
        internal void Touch() {
            LastUpdatedOrViewed = DateTime.Now;
        }
    }
}