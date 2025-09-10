////using Plisky.Plumbing.Legacy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Plisky.Diagnostics;
using Plisky.Plumbing;

/*
  Data manager holds both the data structures themsevles and the routines needed to process the data structures.  The principal
  is that there is a que of new arrivals to the data structures.  These new arrivals are added using the AddNewDebugString function
  and will be queued until the processor thread finds them there.  The processor thread will process them one at a time and add
  them to the data model.  The last thing that the processor thread does is set the "active" tag on the data elements in the model
  therefore the lock needs to be only aquired on the data model for the brefist of time, and it is entirely feasable although
  unwise that the lock need not be aquired at all.  This will entirely depend on whether setting a boolean is an atomic operation
  or not and therefore whether you can safely not lock it.

*/

namespace Plisky.FlimFlam {

    internal class DataStructureManager {
        internal ReaderWriterLock DataStructuresLock;

        internal string DiagnosticsText() {
            var diagReturnString = new StringBuilder();

            diagReturnString.Append("Non traced app entries : " + this.NonTracedApplicationEntries.Count.ToString() + "\r\n");
            for (int i = 0; i < this.TracedApplications.Count; i++) {
                diagReturnString.Append("<App>\r\n");
                diagReturnString.Append("<AppId>" + TracedApplications[i].ProcessIdNo.ToString() + "</AppId>");
                diagReturnString.Append("<Entries>" + this.TracedApplications[i].EventEntries.Count.ToString() + "</Entry>");
                diagReturnString.Append("\r\n</App>");
            }

            return diagReturnString.ToString();
        }

        public Dictionary<string, Tuple<DateTime, byte[]>> LargeObjectStore = new Dictionary<string, Tuple<DateTime, byte[]>>();

        public void AddLargeObject(string ident, byte[] data) {
            LargeObjectStore.Add(ident, new Tuple<DateTime, byte[]>(DateTime.Now, data));
        }

        public List<AlertEntry> Alerts { get; set; } = new List<AlertEntry>();

        internal int PlaceAlertInoDataStructure(AlertEntry aa) {
            Alerts.Add(aa);
            return Alerts.Count;
        }

        internal List<AlertEntry> GetAlertEntries() {
            return Alerts;
        }

        /// <summary>
        /// This will insert a newly arrived and prepopulated unknown event into the data structures so that
        /// the main viewer application can find it.  The last thing prior to this event being called that
        /// happens is that the newly inserted event is set active.
        /// </summary>
        /// <param name="nte">The new non traced application event</param>
        /// <returns>The index of the newly inserted item</returns>
        internal int PlaceUnknownEventIntoDataStructure(NonTracedApplicationEntry nte) {
            return NonTracedApplicationEntries.Add(nte);
        }

        /// <summary>
        /// This method will insert a newly created duplicate application object into the structures and will return the logical index assoicated with this
        /// duplicated application.  NB duplicated applications are not valid applciations for new events to arrive and therefore this is only used to
        /// provide snapshot functionality.
        /// The PID must not be left in tact during an application dupe, this method will fail if the PID is left in tact.
        /// </summary>
        /// <param name="ta">The duplicated traced application</param>
        /// <returns>The logical index referring to the duplicated entry</returns>
        internal int AddDuplicatedTracedApplication(TracedApplication ta) {
            //Bilge.Assert(TracedApplications.GetIndexOfApplication(ta.ProcessIdNo, ta.MachineName) == -1, "This application is already in the structure.  Add duplicated Traced application cannot duplicate an application as new events would get lost");
            ta.VirtualIndex = TracedAppGetNextVirtualIndex();
            TracedApplications.Add(ta);
            MexCore.TheCore.WorkManager.AddJob(new Job_NotifyKnownProcessUpdate());
            return ta.VirtualIndex;
        }

        /// <summary>
        /// Creates a new empty traced application and returns its new physical index.
        /// </summary>
        /// <param name="thePid">The proces identifier of this process</param>
        /// <param name="machineName">The machine name which is hosting the process</param>
        /// <returns>The physical index of the newly created process</returns>
        internal int CreateNewTracedApp(int thePid, string machineName) {
            int result = TracedApplications.AddNew(thePid, machineName, TracedAppGetNextVirtualIndex());

            if (result < 0) {
                //Bilge.Warning("Placing new event into the data structure, required a new application slot to be created BUT this failed.  MexViewer cant store any more data.");
                throw new InvalidOperationException("Cant allocate storage space or error creating the new application");
            }
            return result;
        }

        /// <summary>
        /// This will insert a newly arrived and prepopulated event into the data structures so that the main
        /// application can find it.  The last thing that occurs is that the event is set active so that the
        /// data structures know that they are able to process this event.
        /// </summary>
        /// <param name="newEvt">The EventEntry structure containing the event data</param>
        /// <param name="thePid">The integer process ID of the application that generated the event</param>
        /// <param name="eventMachineName">The machine name of the machine hosting the application</param>
        /// <exception cref="InvalidOperationException">Thrown if it was not possible to allocate the next internal TracedApplication storage</exception>
        /// <returns>The logical index of the application the event was inserted into.</returns>
        internal int PlaceNewEventIntoDataStructure(EventEntry newEvt, int thePid, string eventMachineName) {
            bool insertionCausedChangeToProcesses = false;

            int physicalIndex = TracedApplications.GetIndexOfApplication(thePid, eventMachineName);

            if (physicalIndex == -1) {
                //Bilge.Log("PlaceNewEvent requires a new application to be created, the application with PID " + thePid.ToString() + " being added.");
                // There was no application that matched these criteria therefore create a new one.
                physicalIndex = CreateNewTracedApp(thePid, eventMachineName);

                // A new known application has been added - we need to cause a notify event so that anything that needs to know about the
                // current number of applications is updated.  All notifications are handed to the work manager to deal with, no index is passed
                // therefore this is a newly added process or a big change.
                insertionCausedChangeToProcesses = true;
            }

            // Now based on the type of the event use its data.

            // There is some enabling and disabling of functionality in the view that is required to ensure that the user doesent have to go
            // to for example timings related views when there is no timing data.  Therefore new messages are checked against type and their
            // type information sets some boolean flags on the application itself.
            // Checks for timings / resources/ more than one thread / trace in and out info

            #region Set values in traced app to determine the types of message that have been sent to this apps stream

            // check for trace in out etc.
            TracedApplications[physicalIndex].AddEventEntry(newEvt);

            if (!TracedApplications[physicalIndex].ContainsCustomData && (FlimFlamTraceMessageFormat.IsCustom(newEvt.cmdType))) {
                TracedApplications[physicalIndex].ContainsCustomData = true;
                insertionCausedChangeToProcesses = true;
            }

            if ((!TracedApplications[physicalIndex].ContainsTraceData) && (TraceMessageFormat.IsTraceCommand(newEvt.cmdType))) {
                TracedApplications[physicalIndex].ContainsTraceData = true;
                insertionCausedChangeToProcesses = true;
            }
            // check for timing data
            if ((!TracedApplications[physicalIndex].ContainsTimingData) && (TraceMessageFormat.IsSectionCommand(newEvt.cmdType))) {
                TracedApplications[physicalIndex].ContainsTimingData = true;
                insertionCausedChangeToProcesses = true;
            }
            // Check for multiple threads
            if ((!TracedApplications[physicalIndex].ContainsMoreThanOneThread)) {
                if (string.IsNullOrWhiteSpace(TracedApplications[physicalIndex].CurrentlyUsedThreadIdx)) {
                    TracedApplications[physicalIndex].CurrentlyUsedThreadIdx = newEvt.ThreadID ?? string.Empty;
                } else {
                    TracedApplications[physicalIndex].ContainsMoreThanOneThread = (TracedApplications[physicalIndex].CurrentlyUsedThreadIdx != newEvt.ThreadID);
                    if (TracedApplications[physicalIndex].ContainsMoreThanOneThread) { insertionCausedChangeToProcesses = true; }
                }
            }
            // Check for resource increments
            if ((!TracedApplications[physicalIndex].ContainsResourceData) && (TraceMessageFormat.IsResourceCommand(newEvt.cmdType))) {
                TracedApplications[physicalIndex].ContainsResourceData = true;
                insertionCausedChangeToProcesses = true;
            }

            #endregion Set values in traced app to determine the types of message that have been sent to this apps stream

            // Now we check whether a thread initialise was contained in this message, if it was we update the Traced Application thread
            // name cache so that it knows of the name for this thread.
            if ((newEvt.cmdType == TraceCommandTypes.InternalMsg) && (newEvt.SecondaryMessage.StartsWith(FlimFlamConstants.THREADINITIDENTIFIER))) {
                if (!MexCore.TheCore.DataManager.TracedApplications[physicalIndex].ThreadNames.ContainsKey(newEvt.CurrentThreadKey)) {
                    string tname = newEvt.SecondaryMessage.Substring(FlimFlamConstants.THREADINITIDENTIFIER.Length + 1);
                    tname = tname.Substring(0, tname.Length - 1);
                    MexCore.TheCore.DataManager.TracedApplications[physicalIndex].ThreadNames.Add(newEvt.CurrentThreadKey, tname);
                }
            }

            if (insertionCausedChangeToProcesses) {
                //Bilge.Log("Insertion caused an update to the process, notifying view that there was a change it should pay attention to");
                MexCore.TheCore.WorkManager.AddJob(new Job_NotifyKnownProcessUpdate(TracedAppVirtualIndexFromPhysicalIndex(physicalIndex)));
            }
            return TracedAppVirtualIndexFromPhysicalIndex(physicalIndex);
        } // end PlaceNewEventIntoDataStructure

        // TODO : Review the design here i suspect that this is not the best way of doing it.
        /// <summary>
        /// Using amazing bad practice this delegate is called when every entry is being visited. Either ee or nta will be null and appLogicalIdx
        /// will be -1 if nta!=null.  This is a horrid method of doing it and I wish I could think of a relaible better way
        /// </summary>
        internal delegate bool VisitEachEntryCallback(EventEntry ee, NonTracedApplicationEntry nta, int appLogicalIdx);

        internal delegate bool VisitEachEventEntryCallback(EventEntry ee, int appLogicalIdx);

        internal delegate bool VisitEachEventryCompletedCallback(bool everyEntryVisited);

        private bool visitEveryEntryCallbackMethodForKnownEntrys(EventEntry ee, int appLogicalIdx) {
            if (m_currentIdxFinderMatch == ee.GlobalIndex) {
                m_indexItMatchedOn = appLogicalIdx;
                m_itMatchedOn = ee;
                return false;
            }
            return true;
        }

        private long m_currentIdxFinderMatch; // Default 0
        private EventEntry m_itMatchedOn; // Default null
        private int m_indexItMatchedOn = -1;

        internal bool VisitEveryEventEntryInOrder(VisitEachEntryCallback everyCallback, VisitEachEventryCompletedCallback finalCallback) {
            long currentLowestIdx = 0;
            long lastIndexIssued = m_currentGlobalIndex;
            bool beenHereOnce = false;  // Indicates how many times weve gone round looking for the same index

            if (everyCallback != null) {
                while (currentLowestIdx < lastIndexIssued) {
                    foreach (NonTracedApplicationEntry nta in NonTracedApplicationEntries) {
                        // TODO : if nta.assignedinex > currentindex break
                        if (nta.AssignedIndex == currentLowestIdx) {
                            beenHereOnce = false;
                            currentLowestIdx++;
                            if (!everyCallback(null, nta, -1)) {
                                return false;
                            }
                            continue;
                        }
                    } // End of for each non traced app entry

                    m_currentIdxFinderMatch = currentLowestIdx;

                    if (!VisitEachKnownApplicationEventEntry(new VisitEachEventEntryCallback(visitEveryEntryCallbackMethodForKnownEntrys), null)) {
                        // If this method returns false then the operation was aborted.  In this instance probably because the user hit cancel.
                        //return false;
                    }

                    if (m_indexItMatchedOn != -1) {
                        everyCallback(m_itMatchedOn, null, m_indexItMatchedOn);
                        m_itMatchedOn = null; m_indexItMatchedOn = -1;
                        currentLowestIdx++;
                        beenHereOnce = false;
                        continue;
                    } else {
                        // No match was made we dont want to come here too often
                        //Bilge.Log("No match looking for index " + currentLowestIdx.ToString() + " Index not found in structure");
                        if (!beenHereOnce) {
                            beenHereOnce = true;
                        } else {
                            //Bilge.Log("Cant find index " + currentLowestIdx.ToString() + " skipping");
                            currentLowestIdx++;
                            beenHereOnce = false;
                        }
                    }
                } // End while we are still looking for entries
            }

            // Somehow we made it through all that - tell um were done.
            if (finalCallback != null) {
                finalCallback(true);
            }
            return true;
        }

        /// <summary>
        /// Visits each of the known application entrys using the callback on each one and then the final callback once all of the entries have
        /// been visited.
        /// </summary>
        /// <param name="everyCallback">Method called for every known entry visited</param>
        /// <param name="finalCallback">Method called once all the entries are visited</param>
        /// <returns>True if every entry was visited, false if one of the callbacks indicated no more callbacks should be made</returns>
        internal bool VisitEachKnownApplicationEventEntry(VisitEachEventEntryCallback everyCallback, VisitEachEventryCompletedCallback finalCallback) {
            // By default we visit everything
            bool result = true;

            try {
                if (everyCallback != null) {
                    // actually visit each node

                    this.TracedApplications.TracedApplicationsDataRWL.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);
                    //Bilge.ResourceGrab(this.TracedApplications.TracedApplicationsDataRWL, "TracedApplicationsDataRWL");

                    try {
                        for (int appcount = 0; appcount < this.TracedApplications.Count; appcount++) {
                            TracedApplications[appcount].EventEntries.EventEntriesRWL.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);
                            //Bilge.ResourceGrab(TracedApplications[appcount].EventEntries.EventEntriesRWL, "EventEntriesRWL");
                            try {
                                int TracedAppVirtIndex = TracedAppVirtualIndexFromPhysicalIndex(appcount);

                                for (int entryCount = 0; entryCount < TracedApplications[appcount].EventEntries.Count; entryCount++) {
                                    if (!everyCallback(this.TracedApplications[appcount].EventEntries[entryCount], TracedAppVirtIndex)) {
                                        result = false;
                                        break;
                                    }
                                }
                            } finally {
                                //Bilge.ResourceFree(TracedApplications[appcount].EventEntries.EventEntriesRWL, "EventEntriesRWL");
                                TracedApplications[appcount].EventEntries.EventEntriesRWL.ReleaseReaderLock();
                            }
                        }
                    } finally {
                        // Give the lock back for reading.
                        //Bilge.ResourceFree(this.TracedApplications.TracedApplicationsDataRWL, "TracedApplicationsDataRWL");
                        this.TracedApplications.TracedApplicationsDataRWL.ReleaseReaderLock();
                    }
                }
            } catch (ApplicationException) {
                //Bilge.Dump(aex, "Likely timeout trying to retrieve a reader lock in visit every event in order method");
                if (finalCallback != null) { finalCallback(false); }
                return false;
            } catch (Exception) {
                //Bilge.Dump(ex, "something wrong visiting each of the traced applications, exception occurred");
                if (finalCallback != null) { finalCallback(false); }
                throw;
            }

            //done:

            if (finalCallback != null) {
                finalCallback(true);
            }

            return result;
        }

        /// <summary>
        /// This method will count up the number of entries that are currently within the traced and non traced applications.  This is usefull if
        /// trying to visit all entries or to identify how much data is currently in use.
        /// </summary>
        /// <returns>a long indicating the number of entries currently in the Mex data structures</returns>
        internal long GetNumberActiveEntries() {
            long result = 0;

            for (int a = 0; a < TracedApplications.Count; a++) {
                result += TracedApplications[a].EventEntries.Count;
            }
            result += NonTracedApplicationEntries.Count;
            return result;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        internal void ShutDown() {
            //Bilge.Log("Mex::DataManager::Shutdown -> MexData structures shutdown requested");
        }

        private long m_currentGlobalIndex; // Defualt 0

        /// <summary>
        /// Returns the last allcoated global index - this was the last value given out to a message and if no purges have taken place
        /// then this value wil be somewhere in one of the structures.  If a purge has taken place this value may no longer be present.
        /// </summary>
        /// <returns></returns>
        internal long LastUsedGlobalIndex {
            get {
                return m_currentGlobalIndex; // atomic ?
            }
        }

        private DateTime m_lastMessageRecieved = DateTime.Now;

        internal DateTime LastMessageRecievedTime {
            get {
                return m_lastMessageRecieved;
            }
        }

        internal int KnownProcessCount {
            get {
                return TracedApplications.Count;
            }
        }

        /// <summary>
        /// This will return the next avalable global index - this index will not have been allocated previously and is suitable for assigning to a new
        /// incomming message.
        /// </summary>
        /// <returns>The next available index</returns>
        internal long GetNextGlobalIndex() {
            try {
                m_lastMessageRecieved = DateTime.Now;
                return ++m_currentGlobalIndex;
            } catch (OverflowException) {
                throw;
            }
        }

        internal long LasatUsedGlobalIndexWas {
            get { return m_currentGlobalIndex; }
        }

        /// <summary>
        /// Traced and non traced applications are the main data structures that hold all of the collected
        /// debug data from the applications.
        /// </summary>
        private TracedApplicationsArrayList TracedApplications = null;

        internal NonTracedApplicationsArrayList NonTracedApplicationEntries = null;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        internal EventEntry GetLastEventEntryByPid(int thePid, string machineName) {
            // Special - look up the last event entry matching on the Pid provided. This is so truncates can be added into messages, as truncates
            // often come in a group the last one found could be cached.
            //Bilge.Log("INNEFFICIENT --> Cache the last one as its likely that we hit it again, as truncates often come in heards");

            var ta = MexCore.TheCore.DataManager.TracedApplications[thePid, machineName];
            if (ta != null) {
                return ta.EventEntries[ta.EventEntries.Count];
            } else {
                return null;
            }
        }

        /// <summary>
        /// Returns the virtual index of the traced application which contains the global index that is passed in.
        /// </summary>
        /// <param name="index">The global index to track down</param>
        /// <returns>-1 if no match is found, otherwise the virtual index of the traced application</returns>
        internal int FindTracedAppIndexThatContainsGlobalIndex(long index) {
            if (index > 0) {
                this.TracedApplications.TracedApplicationsDataRWL.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);
                //Bilge.ResourceGrab(this.TracedApplications.TracedApplicationsDataRWL, "TracedApplicationsDataRWL");
                try {
                    for (int taCount = 0; taCount < this.TracedApplications.Count; taCount++) {
                        this.TracedApplications[taCount].EventEntries.EventEntriesRWL.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);
                        //Bilge.ResourceGrab(this.TracedApplications[taCount].EventEntries.EventEntriesRWL, "EventEntriesRWL");
                        try {
                            if (TracedApplications[taCount].EventEntries.ContainsThisGlobalIdx(index) >= 0) {
                                return TracedAppVirtualIndexFromPhysicalIndex(taCount);
                            }
                        } finally {
                            //Bilge.ResourceFree(this.TracedApplications[taCount].EventEntries.EventEntriesRWL, "EventEntriesRWL");
                            this.TracedApplications[taCount].EventEntries.EventEntriesRWL.ReleaseReaderLock();
                        }
                    }
                } finally {
                    //Bilge.ResourceFree(this.TracedApplications.TracedApplicationsDataRWL, "TracedApplicationsDataRWL");
                    this.TracedApplications.TracedApplicationsDataRWL.ReleaseReaderLock();
                }

                // If we get here weve searched all of the known apps and cant find the index.
                return -1;
            } else {
                return -1;
            }
        }

        internal EventEntry FindEventEntryInKnownAppsByIndex(long index) {
            if (index > 0) {
                // Reader lock the traced applications list
                this.TracedApplications.TracedApplicationsDataRWL.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);
                //Bilge.ResourceGrab(this.TracedApplications.TracedApplicationsDataRWL, "TracedApplicationsDataRWL");
                try {
                    for (int taCount = 0; taCount < this.TracedApplications.Count; taCount++) {
                        this.TracedApplications[taCount].EventEntries.EventEntriesRWL.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);
                        //Bilge.ResourceGrab(this.TracedApplications[taCount].EventEntries.EventEntriesRWL, "EventEntriesRWL");

                        try {
                            // go through the event entries looking for the index were looknig for.
                            for (int eeCount = 0; eeCount < this.TracedApplications[taCount].EventEntries.Count; eeCount++) {
                                if (this.TracedApplications[taCount].EventEntries[eeCount].GlobalIndex == index) {
                                    // Match made.
                                    return this.TracedApplications[taCount].EventEntries[eeCount];
                                }
                            }
                        } finally {
                            //Bilge.ResourceGrab(this.TracedApplications[taCount].EventEntries.EventEntriesRWL, "EventEntriesRWL");
                            this.TracedApplications[taCount].EventEntries.EventEntriesRWL.ReleaseReaderLock();
                        }
                    }
                } finally {
                    //Bilge.ResourceFree(this.TracedApplications.TracedApplicationsDataRWL, "TracedApplicationsDataRWL");
                    this.TracedApplications.TracedApplicationsDataRWL.ReleaseReaderLock();
                }

                // If we get here weve searched all of the known apps and cant find the index.
                return null;
            } else {
                return null;
            }
        }

        internal NonTracedApplicationEntry FindNTAEntryByIndex(long index) {
            this.NonTracedApplicationEntries.NonTracedApplicationsDataRWL.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);
            //Bilge.ResourceGrab(this.NonTracedApplicationEntries.NonTracedApplicationsDataRWL, "NonTracedApplicationsDataRWL");

            try {
                foreach (NonTracedApplicationEntry ntaee in NonTracedApplicationEntries) {
                    if (ntaee.AssignedIndex == index) {
                        return ntaee;
                    }
                }
            } finally {
                //Bilge.ResourceFree(this.NonTracedApplicationEntries.NonTracedApplicationsDataRWL, "NonTracedApplicationsDataRWL");
                this.NonTracedApplicationEntries.NonTracedApplicationsDataRWL.ReleaseReaderLock();
            }
            return null;
        }

        internal ProcessSummary[] GetAllProcessSummaries() {
            var pss = new ProcessSummary[MexCore.TheCore.DataManager.TracedApplications.Count];
            int theIdx = 0;

            this.TracedApplications.TracedApplicationsDataRWL.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);
            //Bilge.ResourceGrab(this.TracedApplications.TracedApplicationsDataRWL, "TracedApplicationsDataRWL");
            try {
                foreach (TracedApplication ta in MexCore.TheCore.DataManager.TracedApplications) {
                    pss[theIdx] = MexCore.TheCore.DataManager.CreateSummaryForProcess(ta.VirtualIndex);
                    theIdx++;
                }
            } finally {
                //Bilge.ResourceFree(this.TracedApplications.TracedApplicationsDataRWL, "TracedApplicationsDataRWL");
                this.TracedApplications.TracedApplicationsDataRWL.ReleaseReaderLock();
            }
            return pss;
        }

        /// <summary>
        /// Will return a process summary structure for the specified process.  The idx parameter refers to a valid index of the process that is
        /// to be looked at.
        /// </summary>
        /// <param name="virtualIndex">The process for which to return the summary</param>
        /// <returns>A newly created summary for the process</returns>
        internal ProcessSummary CreateSummaryForProcess(int virtualIndex) {

            #region entry code

            //Bilge.Assert(virtualIndex >= 0, "Mex::DataManager::CreateSummaryForProcess >> The virtualindex passed to CreateSummaryForPRocess was out of range - a summary cannot be created for index <0 - index passed was " + virtualIndex.ToString());
            //Bilge.Assert(virtualIndex <= m_tracedAppVirtualIndexLastUsed, "Mex::DataManager::CreateSummaryForProcess >> The virtualindex passed to CreateSummaryForProcess was out of range - a summary cannot be created for index " + virtualIndex.ToString() + " as there are only " + m_tracedAppVirtualIndexLastUsed.ToString() + " allocated virtual indexes");

            #endregion entry code

            int physicalIndex = TracedAppPhysicalIndexFromVirtualIndex(virtualIndex);
            //Bilge.Assert(physicalIndex >= 0, "Mex::DataManager::CreateSummaryForProcess >> PhysicalIndex for traced app, generated from passed virtual index was invalid.  The passed virtualIndex was not a legitimate index");
            //Bilge.Assert(physicalIndex < TracedApplications.Count, "Mex::DataManager::CreateSummaryForProcess >> PhysicalIndex for traced app, generated from passed virtual index was invalid.  The conversion routine is faulty, index returned was too high");

            return new ProcessSummary(TracedApplications[physicalIndex]);
        } // End CreateSummaryForProcess

        #region Purge Methods

        /// <summary>
        /// This will clear all of the traced applications from the logs.  Therefore it iwll leave in tact the non traced
        /// applications strucutres but remove all of the application data that has followed the format of a traced
        /// application.NB the data is only deactivated one of the data
        /// manager threads will remove the references physically.
        /// </summary>
        internal void PurgeAllKnownApplications() {
            //Bilge.E("Request made to purge all of the known data");
            try {
                TracedApplications.TracedApplicationsDataRWL.AcquireWriterLock(Consts.MS_TIMEOUTFORLOCKS);
                //Bilge.ResourceGrab(this.TracedApplications.TracedApplicationsDataRWL, "TracedApplicationsDataRWL");
                try {
                    // This code has been put in as its possible that an importer thread has a reference to the TA and is still
                    // importing into an app that is being purged.
                    foreach (TracedApplication ta in TracedApplications) {
                        ta.IsBeingPurged = true;
                    }
                    TracedApplications.Clear();
                } finally {
                    //Bilge.ResourceFree(TracedApplications.TracedApplicationsDataRWL, "TracedApplicationsDataRWL");
                    TracedApplications.TracedApplicationsDataRWL.ReleaseWriterLock();
                }
            } finally {
                //Bilge.X();
            }
        }

        internal void PurgeAllKnownApplicationsExceptThisOne(int vIndexNotToPurge) {
            //Bilge.E("PurgeAllExcept called with parameter " + vIndexNotToPurge.ToString());
            try {
                TracedApplications.TracedApplicationsDataRWL.AcquireWriterLock(Consts.MS_TIMEOUTFORLOCKS);
                //Bilge.ResourceGrab(this.TracedApplications.TracedApplicationsDataRWL, "TracedApplicationsDataRWL");
                try {
                    int i = 0;
                    //Bilge.Log("About to run through all of the applications removing them");
                    while (i < TracedApplications.Count) {
                        if (TracedApplications[i].VirtualIndex != vIndexNotToPurge) {
                            // Added is being purged in to tell anyone that has a rereference to this class that its dying.
                            TracedApplications[i].IsBeingPurged = true;
                            TracedApplications.RemoveAt(i);
                        } else {
                            //Bilge.Log("Found the virtual index of the application not to purge, skipping this application.");
                            i++;
                        }
                    }
                } finally {
                    //Bilge.ResourceFree(this.TracedApplications.TracedApplicationsDataRWL, "TracedApplicationsDataRWL");
                    TracedApplications.TracedApplicationsDataRWL.ReleaseWriterLock();
                }
                //Bilge.Log("PurgeAllExceptThisOne completes its execution.");
            } finally {
                //Bilge.X();
            }
        }

        /// <summary>
        /// This will performa  partial purge of the application mentione din the index parameter.  A partial purge will remove the data and log entries
        /// from the application but will leave long running data such as resource and timing counts and the actual application name itself.
        /// </summary>
        /// <param name="theAppLogicalIdx">The logical index of the application that is to be partially purged</param>
        internal void PurgePartialKnownApplication(int theAppLogicalIdx) {
            // todo aquire writer lock for this app

            int actualIdx = TracedAppPhysicalIndexFromVirtualIndex(theAppLogicalIdx);

            int eeCount = 0;

            uint deleteMatch = ViewFilter.GetFlagTypeByBools(true, true, false, true, true, true, true, true, true, true, true, true, true, true, false, false, false, false);

            TracedApplications[actualIdx].EventEntries.EventEntriesRWL.AcquireWriterLock(Consts.MS_TIMEOUTFORLOCKS);
            //Bilge.ResourceGrab(TracedApplications[actualIdx].EventEntries.EventEntriesRWL, "EventEntriesRWL-Writer");
            try {
                while (eeCount < TracedApplications[actualIdx].EventEntries.Count) {
                    if (((uint)TracedApplications[actualIdx].EventEntries[eeCount].cmdType & deleteMatch) == (uint)TracedApplications[actualIdx].EventEntries[eeCount].cmdType) {
                        // Its deletable
                        TracedApplications[actualIdx].EventEntries.RemoveAtUnsafe(eeCount);
                    } else {
                        eeCount++;
                    }
                }
            } finally {
                //Bilge.ResourceFree(TracedApplications[actualIdx].EventEntries.EventEntriesRWL, "EventEntriesRWL-Writer");
                TracedApplications[actualIdx].EventEntries.EventEntriesRWL.ReleaseWriterLock();
            }
        }

        /// <summary>
        /// Looks through all of the currently stored applications trying to find onet that matches in process name and machine name.  If a match is found
        /// then all matches except those with the pid specificed in the third parameter will be deleted.
        /// </summary>
        /// <remarks>NB the internal identifier of -1 is used for PIDS internal to Mex therefore to include all apps in the purge specify -2 as the final parameter</remarks>
        /// <param name="Name">The name of the app to match on.</param>
        /// <param name="machineName">The machine name of the app to match on</param>
        /// <param name="excludeThisPid">A specific pid to be excluded from the hunt</param>
        internal void PurgeByName(string Name, string machineName, int excludeThisPid) {
            //Bilge.E(string.Format("PurgeByName called for app name {0} on machine {1}", Name, machineName));
            try {
                int i = 0;

                // This can change the number of traced apps therefore we lock the entire structure.
                TracedApplications.TracedApplicationsDataRWL.AcquireWriterLock(Consts.MS_TIMEOUTFORLOCKS);
                //Bilge.ResourceGrab(TracedApplications.TracedApplicationsDataRWL, "TracedApplicationsDataRWL");
                try {
                    //Bilge.Log("TracedApplicationsDataRWL is now locked for the purge by name, starting to loop through all applications");
                    do {
                        if (TracedApplications[i].ProcessName == "Unknown") { i++; continue; }   // Never purge unknown as that is all non inintialised apps

                        if ((TracedApplications[i].ProcessName == Name) && (TracedApplications[i].MachineName == machineName) && (TracedApplications[i].ProcessIdNo != excludeThisPid)) {
                            // Added in after the loop therefore messy.  If we dont clear partials and we have found a partial then we need to skip this PID.
                            // This allows the option to determine whether all mathcing name with a pid of -1 are removed when you remove the main name.
                            if ((!MexCore.TheCore.Options.MatchingNamePurgeAlsoClearsPartials) && (TracedApplications[i].ProcessIdNo == -1)) {
                                i++; continue;
                            }

                            TracedApplications.RemoveAt(i);
                        } else {
                            i++;
                        }
                    } while (i < TracedApplications.Count);
                    //Bilge.Log("Loop completes, freeing TracedApplicationsDataRWL");
                } finally {
                    //Bilge.ResourceFree(TracedApplications.TracedApplicationsDataRWL, "TracedApplicationsDataRWL");
                    TracedApplications.TracedApplicationsDataRWL.ReleaseWriterLock();
                }
            } finally {
                //Bilge.X();
            }
        }

        /// <summary>
        /// This will purge  a single known application - as identified by its index into the array
        /// of known applications.NB the data is only deactivated one of the data
        /// manager threads will remove the references physically.
        /// </summary>
        /// <param name="virtualIndex">The index of the application to remove</param>
        internal void PurgeKnownApplication(int virtualIndex) {
            // TODO Aquire Locks
            //Bilge.Log("Mex::DataMAnager::PurgeKnownApplication >> Purge known application called for application w/index " + virtualIndex.ToString());
            //Bilge.Assert(IsValidTracedApplicationIndex(virtualIndex), "Mex::DataManager::PurgeKnownApplication - Failure, the virtual index passed is not a valid index");

            TracedApplications.TracedApplicationsDataRWL.AcquireWriterLock(Consts.MS_TIMEOUTFORLOCKS);
            try {
                TracedApplications.RemoveAt(TracedAppPhysicalIndexFromVirtualIndex(virtualIndex));
            } finally {
                TracedApplications.TracedApplicationsDataRWL.ReleaseWriterLock();
            }
        }

        /// <summary>
        /// This will remove all of the data for the non traced applications.  It wil not affect those applications that
        /// have the special format of traced data with them.NB the data is only deactivated one of the data
        /// manager threads will remove the references physically.
        /// </summary>
        internal void PurgeUnknownApplications() {
            //Bilge.Log("Mex::DataMAnager::PurgeUnknownApplications >> Purge Unknown application called");
            bool iGrabbed = false;

            try {
                if (!DataStructuresLock.IsWriterLockHeld) {
                    DataStructuresLock.AcquireWriterLock(Consts.MS_TIMEOUTFORLOCKS);
                    iGrabbed = true;
                }
                try {
                    NonTracedApplicationEntries.Clear();
                } finally {
                    if (iGrabbed) { DataStructuresLock.ReleaseWriterLock(); }
                }
            } catch (ApplicationException) {
                //Bilge.Dump(aex, "PurgeAllData, timeout occured trying to get the writer lock");
                //Bilge.Warning("Timeout occured waiting for a writer lock to purge, what do we do now!? Exception swallowed ");
            }
        }

        /// <summary>
        /// This will completely remove all of the data from the viewier.  NB the data is only deactivated one of the data
        /// manager threads will remove the references physically.
        /// </summary>
        internal void PurgeAllData() {
            try {
                //Bilge.Log("PurgeAll, -> Trying to get the writer lock");
                DataStructuresLock.AcquireWriterLock(Consts.MS_TIMEOUTFORLOCKS);
                try {
                    PurgeAllKnownApplications();
                    PurgeUnknownApplications();
                } finally {
                    //Bilge.Log("PurgeAll -> releasing writer lock");
                    DataStructuresLock.ReleaseWriterLock();
                }
            } catch (ApplicationException) {
                //Bilge.Dump(aex, "PurgeAllData, timeout occured trying to get the writer lock");
                //Bilge.Warning("Timeout occured waiting for a writer lock to purge, what do we do now!? Exception swallowed ");
            }
        }

        #endregion Purge Methods

        /// <summary>
        /// Converts a View compatible traced application virtual index into the required physical index for loooking up the actual applicaiton itself.  The view uses virtual
        /// indexes to protect it from changes to the data managers implementation and to protect it from purges and deletions, however this does mean that a mapping is required
        /// each time the viewmanager passes the datamanager an application index to work with.
        /// </summary>
        /// <param name="virtualIndex">The virtual index that the view uses to refer to an application</param>
        /// <returns>The physical store index of the application matching the passed virtual index</returns>
        private int TracedAppPhysicalIndexFromVirtualIndex(int virtualIndex) {
            // Run through all of the tracedApplications looking for the matching virtual index.
            this.TracedApplications.TracedApplicationsDataRWL.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);
            try {
                for (int physicalIndex = 0; physicalIndex < TracedApplications.Count; physicalIndex++) {
                    if (TracedApplications[physicalIndex].VirtualIndex == virtualIndex) {
                        return physicalIndex;
                    }
                }
            } finally {
                this.TracedApplications.TracedApplicationsDataRWL.ReleaseReaderLock();
            }
            // Could not find the specified virtual index
            //Bilge.Log("Mex::DataManager::TracedAppPhysicalIndexFromVirtualIndex >> WARNING >> Returning -1, the lookup for the specified virtual index (" + virtualIndex.ToString() + " failed");
            return -1;
        } // End Mex::DataManager::TracedAppPhysicalIndexFromVirtualIndex

        /// <summary>
        /// IsValidTracedApplicationIndex will verify that the views virtual index that it is using is indeed a valid tracedapplication index and can be used to a currently
        /// valid traced application for display purposes.  This method checks both the range of the specified virtualIndex and that it actually mapps back to a physical
        /// application.  This method is designed for debugging and checking UI functionality but can be called at any time.
        /// </summary>
        /// <param name="virtualIndex">The virtual index to verify is a traced application</param>
        /// <returns>Boolean indicating whether the specified virtual index was valid at the point the method was called</returns>
        internal bool IsValidTracedApplicationIndex(int virtualIndex) {
            if (virtualIndex < 0) return false;  // Less than 0 means not initialised
            if (virtualIndex > m_tracedAppVirtualIndexLastUsed) return false;  // should not be possible - higher than ever allocated
            if (TracedAppPhysicalIndexFromVirtualIndex(virtualIndex) < 0) return false;  // Within range but does not represent valid application

            return true;
        }

        private int TracedAppVirtualIndexFromPhysicalIndex(int physicalIndex) {
            //Bilge.Assert(physicalIndex >= 0, "Mex::DataManager::TracedAppVirtualIndexFromPhysicalIndex >> ERROR Specified index passed was not valid - index must be >= 0 ");
            //Bilge.Assert(physicalIndex < TracedApplications.Count, "Mex::DataManager::TracedAppVirtualIndexFromPhysicalIndex >> ERROR Specified index passed was not valid - index must be < current app count ");
            return TracedApplications[physicalIndex].VirtualIndex;
        } // End Mex::DataManager::TracedAppVirtualIndexFromPhysicalIndex

        // Do not start at a logical place - this will help identify when the physical index is being passed outside of the datamanger
        private int m_tracedAppVirtualIndexLastUsed;

        private int TracedAppGetNextVirtualIndex() {
            //Bilge.Log("Mex::DataManager::TracedAppGetNextVirtualIndex >> Called, allocating new index at " + (m_tracedAppVirtualIndexLastUsed + 1).ToString());
#if DEBUG
            // Do not go up incrementally, this will help identify problems where the physical index is used outside of the datamanger
            m_tracedAppVirtualIndexLastUsed += new Random().Next(200);
            return m_tracedAppVirtualIndexLastUsed;
#else
      return ++m_tracedAppVirtualIndexLastUsed;
#endif
        } // End Mex::DataManager::TracedAppGetNextVirtualIndex

        /// <summary>
        /// This will return the whole traced application that can be used to query the data, this is right now the actual application but may
        /// be turned into a copy in the future.  it is important that no updates are performed to this application.
        /// </summary>
        /// <param name="virtualIndex"></param>
        /// <returns></returns>
        internal TracedApplication GetKnownApplication(int virtualIndex) {

            #region entry code

            //Bilge.Assert(virtualIndex >= 0, "Mex::DataManager::GetKnownApplication - Failure, virtualIndex was less than zero - no process has been selected for us to get");
            //Bilge.Assert(IsValidTracedApplicationIndex(virtualIndex), "Mex::DataManager::GetKnownApplication - Failure, the virtual index passed is not a valid index");

            #endregion entry code

            //Bilge.Log("Mex::DataManager::GetKnownApplication called for virtual index " + virtualIndex);
            int physicalIndex = TracedAppPhysicalIndexFromVirtualIndex(virtualIndex);
            if (physicalIndex == -1) {
                //Bilge.Warning("The application referenced by GetKnownApplication was not found, it has been purged. Returning Null");
                return null;
            }
            return this.TracedApplications[TracedAppPhysicalIndexFromVirtualIndex(virtualIndex)];
        }

        internal bool IsPidAKnownApplication(int pid, string machineName) {
            if (pid == -1) return false;
            if ((machineName == null) || (machineName.Length == 0)) return false;

            return (this.TracedApplications[pid, machineName] != null);
        }

        internal TracedApplication GetKnownApplicationByPid(int pid, string machineName) {
            //Bilge.Log("Mex::DataManager::GetKnownApplicationByPid called  for pid " + pid);
            return this.TracedApplications[pid, machineName];
        }

        #region Constructor / Destructor

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        internal void ShutDownDataManager() {
            //Bilge.Log("DataManager::ShutDownDataManager - Datamanager shutdown requested");
        } // End shutdown data manager

        internal DataStructureManager() {
            //Bilge.Log("DataManager::Constructor");

#if DEBUG
            // Do not start at a logical place - this will help identify when the physical index is being passed outside of the datamanger
            var r = new Random();
            m_tracedAppVirtualIndexLastUsed = r.Next(24999);
#else
      m_tracedAppVirtualIndexLastUsed=-1;
#endif

            DataStructuresLock = new ReaderWriterLock();

            try {
                DataStructuresLock.AcquireWriterLock(Consts.MS_TIMEOUTFORLOCKS);
#if DEBUG
                //Bilge.ResourceGrab(DebugMexConsts.DATASTRUCTURESLOCK_RESNAME, "DataManagerShutdown");
#endif
                try {
                    // Bring the traced applications store up....
                    TracedApplications = new TracedApplicationsArrayList();
                    NonTracedApplicationEntries = new NonTracedApplicationsArrayList();
                } finally {
#if DEBUG
                    //Bilge.ResourceFree(DebugMexConsts.DATASTRUCTURESLOCK_RESNAME, "DataManagerShutdown");
#endif
                    DataStructuresLock.ReleaseWriterLock();
                }
            } catch (ApplicationException) {
                //Bilge.Dump(aex, "Create DataStructureManager, timeout occured trying to get the writer lock");
                //Bilge.Warning("Timeout occured waiting for a writer lock to purge, what do we do now!? Exception swallowed ");
            }
        }

        #endregion Constructor / Destructor
    }
}