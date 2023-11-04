//using Plisky.Plumbing.Legacy;
namespace Plisky.FlimFlam {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Flurl;
    using Flurl.Http;
    using Plisky.Diagnostics;
    using Plisky.Diagnostics.FlimFlam;
    using Plisky.FilmFlam;
    using Plisky.Flimflam;
    using Plisky.Plumbing;

    /// <summary>
    /// This class is the crossover between the data gatherers and the main functionality of mex.  This classhas the add incomming message method which will be called by
    /// the data gatherer threads and it has the getnextmessage method that will be called by the main job thread.  This class must be created using the static
    /// GetIncommingMessageManager method.
    /// </summary>
    public class IncomingMessageManager {
        private readonly OriginIdentityStore ois = new();
        private readonly ImportManager im;
        private readonly List<Task> ActiveTaskBasedImporters = new();
        private readonly List<Action> TimerEvents = new();
        private Queue m_incommingMsgQueue; // Default null
        protected static Timer timer;

        public void PurgeAllData() {
            m_incommingMsgQueue = new Queue();
        }

        #region Gatherer support - ODS / TCP Gatherers

        private Thread m_ODSThread; // Default null
        private readonly object threadlock = new();

        internal void ActivateODSGatherer() {
            // The ODS Gatherer is long running and is created now.
            if (m_ODSThread == null) {
                m_ODSThread = new Thread(new ThreadStart(ODSDataGathererThread.InterceptODS)) {
                    Name = "MEX::ODSGathererThread"
                };
                m_ODSThread.Start();
                //Bilge.Log("ODS gatherer thread is started.");
                MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.ODSListenerTurnedOn, UserMessageType.InformationMessage, "");
            } else {
                //Bilge.Log("Duplicate request detected to start the ODS thread, duplicate request ignored.");
                MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.ODSListenerTurnedOn, UserMessageType.WarningMessage, "No action Taken, ODS Gatherer already active.");
            }
        }

        internal void DeactivateODSGatherer() {
            //Bilge.Log("IncommingMessageManager::DeactivateODSGatherer called");
            ODSDataGathererThread.continueRunning = false;

            if (m_ODSThread == null) {
                return;  // Actually were not collecting them at the mo.
            }

            var dt = DateTime.Now;

            while ((m_ODSThread != null) && m_ODSThread.IsAlive) {
                var dtt = DateTime.Now;
                if ((dtt - dt).TotalSeconds > 5) {
                    // We have been waiting for this thread for 5 seconds, time to take drastic action.
                    //Bilge.Warning("The ODS Gatherer thread has not died after 5 seconds of waiting for it to shutdown nicely, terminating thread");
                    MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.WaitTimeoutOccured, UserMessageType.WarningMessage, "The ODS Thread did not respond to a shutdown within 5 seconds, its been aborted.");

                    // TODO: m_ODSThread?.Abort();
                }
                Thread.Sleep(0);
            }
            m_ODSThread = null;
            MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.ODSListenerTurnedOff, UserMessageType.InformationMessage, "");
        }

        private Thread m_TCPThread; // Default null

        internal void ActivateTCPGatherer() {
            lock (threadlock) {
                // While multiple requests can come in only one request can be processed at a time therefore I dont need to synch this
                // as the worker thread is the one that does this sort of thing and its on its lonesome.
                if (m_TCPThread == null) {
                    m_TCPThread = new Thread(new ThreadStart(TCPRecieverThread.InterceptTCPMessage));
                    //Bilge.InitialiseThread("MEX::TCPGathererThread", m_TCPThread);

                    m_TCPThread.Start();
                    //Bilge.Log("TCP Gatherer thread is online and listening.");
                    MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.TCPListenerTurnedOn, UserMessageType.InformationMessage, "IP:" + MexCore.TheCore.Options.IPAddressToBind + ":" + MexCore.TheCore.Options.PortAddressToBind);
                } else {
                    //Bilge.Warning("Duplicate request was made to start the TCP thread, the duplicate request was ignored.");
                }
            }
        }

        internal void DeactivateTCPGatherer() {
            //Bilge.Log("Deactivate TCP Gatherer called");
            lock (threadlock) {
                TCPRecieverThread.continueRunning = false;

                if (m_TCPThread == null) {
                    return;  // Actually were not collecting them at the mo.
                }

                MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.TCPListenerTurnedOff, UserMessageType.InformationMessage, "");

                var dt = DateTime.Now;

                while (m_TCPThread.IsAlive) {
                    var dtt = DateTime.Now;
                    if ((dtt - dt).TotalSeconds > 5) {
                        // We have been waiting for this thread for 5 seconds, time to take drastic action.
                        //Bilge.Warning("The TCP Gatherer thread has not died after 5 seconds of waiting for it to shutdown nicely, terminating thread");
                        MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.WaitTimeoutOccured, UserMessageType.WarningMessage, "The TCP Thread did not respond to a shutdown within 5 seconds, its been aborted.");
                        // TODO m_TCPThread.Abort();
                    }
                    Thread.Sleep(0);
                }
                //Bilge.Log("Deactivate TCP Gatherer completes");
                m_TCPThread = null;
            }
        }

        #endregion Gatherer support - ODS / TCP Gatherers

        internal string DiagnosticsText() {
            string result = "\n\n Incomming Message Manager Diagnostics \n\n";
            result += $"Incomming:  Queue length {m_incommingMsgQueue.Count}, Timer Events {TimerEvents.Count}, Active Tasks {ActiveTaskBasedImporters.Count}";
            return result;
        }

        private IncomingMessageManager() {
            m_incommingMsgQueue = Queue.Synchronized(new Queue());
        }

        internal void TimerMethod(object state) {
            foreach (var f in TimerEvents) {
                f();
            }
        }

        internal void LoadMessagesFromFile(string fileName, FileImportMethod style) {
            SavedFileGatherer.LoadFromFileAsynch(fileName, style);
        }

        public void AddTaskBasedImport(Task gatherer) {
            // TODO : Make this more managable

            // Just keep a reference hanging around.
            ActiveTaskBasedImporters.Add(gatherer);
        }

        public void RegisterHttpDataImport(string uriToHit, bool repeatIt) {
            RefreshFromHttpSource(uriToHit);
            if (repeatIt) {
                timer ??= new Timer(TimerMethod, null, 0, 30000);
                TimerEvents.Add(() => {
                    RefreshFromHttpSource(uriToHit);
                });
            }
        }

        private static void RefreshFromHttpSource(string uriToHit) {
            try {
                Url u = uriToHit;
                var t = u.GetAsync();
                t.Wait();
                if (t.Result.ResponseMessage.StatusCode == System.Net.HttpStatusCode.OK) {
                    string s = t.Result.ResponseMessage.Content.ReadAsStringAsync().Result;

                    foreach (string f in s.Split('\n')) {
                        MexCore.TheCore.MessageManager.AddIncomingMessage(InternalSource.HttpPoller, f, -1);
                    }
                }
            } catch (Exception) {
                // Swallow
            }
        }

        private readonly List<Task> activeImporters = new();

        public void RegisterTaskBasedImport(Task gatherer) {
            activeImporters.Add(gatherer);
        }

        /// <summary>
        /// This method is called to queue a new debug string message ready for processing.  This method should return very rapidly so as not to
        /// hold up any of the gatherer methods and is the preferred way of adding data entries into the mex data structures.
        /// </summary>
        /// <param name="machine">The machine from which the message was taken</param>
        /// <param name="msg">The text component of the message to be added</param>
        /// <param name="pid">The pid if the pid is supported by this gatherer, otherwise set this value to -1</param>
        public void AddIncomingMessage(InternalSource source, string machine, string msg, int processId) {
            m_incommingMsgQueue.Enqueue(new IncomingEventStore(source, machine, msg, processId, MexCore.TheCore.DataManager.GetNextGlobalIndex()));
        }

        /// <summary>
        /// This method is called to queue a new debug string message ready for processing.  This method should return very rapidly so as not to
        /// hold up any of the gatherer methods and is the preferred way of adding data entries into the mex data structures.
        /// </summary>
        /// <param name="msg">The text component of the message to be added</param>
        /// <param name="pid">The pid if the pid is supported by this gatherer, otherwise set this value to -1</param>
        public void AddIncomingMessage(InternalSource source, string msg, int processId) {
            m_incommingMsgQueue.Enqueue(new IncomingEventStore(source, MexCore.TheCore.CacheManager.MexRunningOnMachineName, msg, processId, MexCore.TheCore.DataManager.GetNextGlobalIndex()));
        }

        internal bool IncomingMessagesQueued {
            // This will determine whether or not there are messages queued by the incommingMessageManager that are waiting to be processed by the application.
            // This checks whether we are in the middle of an import or not and therefore theoretically this could be expanded to cater forthis.
            get {
                return m_incommingMsgQueue.Count > 0;
            }
        }

        public void BackgroundProcessAllMessages(object state) {
            ProcessNextStoredMessage(true, false);
        }

        internal bool m_shutdownRequested; // Default false

        // DUAL MODE
        private readonly OriginIdentityStore store = new();

        private ImportParser newImporter;

        private readonly EventRecieverForComparisons comparer = new();
        private DataParser dp = null;
        // END DUALMODE

        /// <summary>
        /// called when there are messages queued ready to be added to the data manager - this method will take either the next one or all available
        /// pending messaged from the queue and add it to the data structures that atre used to store the information.
        /// </summary>
        public void ProcessNextStoredMessage(bool doAll, bool reporting) {
            if (m_incommingMsgQueue.Count == 0) {
                //Bilge.Warning("WARNING INNEFFICIENT --> ProcessNextStoredMessage Called  when there are no messages on queue  INC_CHECKDUPEJOBS");
                return;
            }

#if true
            newImporter ??= new ImportParser(store);
            dp ??= new DataParser(store, newImporter, comparer);
#endif

            var appIndexesAffectedByImport = new List<int>();

            var startTime = DateTime.Now;   // how long have we been running
            var overallStartTime = DateTime.Now;

            FileStream everythingStream = null;
            StreamWriter everythingLog = null;

            if (MexCore.TheCore.Options.PersistEverything) {
                everythingStream = new FileStream(MexCore.TheCore.Options.CurrentFilename, FileMode.Append, FileAccess.Write);
                everythingLog = new StreamWriter(everythingStream);
            }

            try {
                do {
                    if (m_shutdownRequested) {
                        //Bilge.Log("Aborting import as shutdown has been requested.");
                        break;
                    }

                    // we have a piece of work

                    var nextEvent = (IncomingEventStore)m_incommingMsgQueue.Dequeue();
                    if (string.IsNullOrEmpty(nextEvent.MessageString)) {
                        continue;
                    }

                    if (MexCore.TheCore.Options.PersistEverything) {
                        everythingLog.WriteLine("\"" + nextEvent.MachineName + "\",\"" + nextEvent.Pid + "\",\"" + nextEvent.MessageString + "\",\"" + "DE\"");
                    }

                    if (MexCore.TheCore.Options.RemoveDuplicatesOnImport) {
                        bool dupeFound = true;
                        while (dupeFound && (m_incommingMsgQueue.Count > 0)) {
                            var potentialNext = (IncomingEventStore)m_incommingMsgQueue.Peek();
                            if (potentialNext != null) {
                                dupeFound = potentialNext.Equals(nextEvent);
                                if (dupeFound) {
                                    // Throw the dupe away.
                                    if (m_incommingMsgQueue.Count > 0) {
                                        m_incommingMsgQueue.Dequeue();
                                    }
                                }
                            }
                        }
                    }
                    // DUAL MODE
                    var rae = new RawApplicationEvent() {
                        ArrivalTime = nextEvent.TimeRecieved,
                        OriginId = store.GetOriginIdentity(nextEvent.MachineName, nextEvent.Pid.ToString()),
                        Machine = nextEvent.MachineName,
                        Process = nextEvent.Pid.ToString(),
                        Text = nextEvent.MessageString
                    };
                    dp.AddRawEvent(rae);

                    // END DUAL MODE
                    EventEntry ee = null;
                    string tempMachineName = null;
                    bool legacyMode = false;
                    bool isCommand = false;
                    // firstly is this a truncation ?
                    if (nextEvent.MessageString.StartsWith(FlimFlamConstants.MESSAGETRUNCATE)) {

                        #region This message was a truncation of the previous one, deal with it like that

                        // This is a truncation, it should be added to the last message that was added to the process with the same pid.

                        //Bilge.Warning("WARNING INNEFFICIENT --> Looking for non existant GI Sux in timed view could cache this -> Global Index " + nextEvent.GlobalIndex.ToString() + " skipped as its a truncate message");

                        int endMachineNameIdx = nextEvent.MessageString.IndexOf(']');
                        int startMachineNameIdx = FlimFlamConstants.MESSAGETRUNCATE.Length + 1;  // 1 is for length of "["
                        int endUniqueIdIdx = nextEvent.MessageString.IndexOf(FlimFlamConstants.TRUNCATE_DATAENDMARKER);

                        if ((endMachineNameIdx < 0) || (startMachineNameIdx < 0) || (endUniqueIdIdx < 0)) {
                            //Bilge.Warning("WARNING --> INVAID truncate join string found, probably old version of//Bilge.  Ignoring this command.");
                            MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.InvalidTruncateStringFound, UserMessageType.WarningMessage, null);
                            return;
                        }

                        string machineName = nextEvent.MessageString[startMachineNameIdx..endMachineNameIdx];
                        string truncateUniqueJoinId = nextEvent.MessageString[(endMachineNameIdx + 2)..endUniqueIdIdx];

                        bool anotherExpected = nextEvent.MessageString.EndsWith(FlimFlamConstants.MESSAGETRUNCATE);

                        ee = MexCore.TheCore.CacheManager.CacheGet_EventEntryExpectingTruncate(nextEvent.Pid, machineName, truncateUniqueJoinId, !anotherExpected);

                        if (ee == null) {
                            // This can only happen following a purge i guess
                            //Bilge.Warning("ProcessNextStoredMessage --> WARNING --> Invalid truncation message found. Couldnt find the EE in the structure to append this trunc to. Skipping it");
                            return;
                        }

                        // Patch it together, removing the truncate markers from the middle.
                        if ((ee.SecondaryMessage != null) && (ee.SecondaryMessage.Length > 0)) {
                            ee.SecondaryMessage = ee.SecondaryMessage[..^FlimFlamConstants.MESSAGETRUNCATE.Length] + nextEvent.MessageString[FlimFlamConstants.MESSAGETRUNCATE.Length..];
                        } else {
                            ee.SetDebugMessage(ee.DebugMessage[..^FlimFlamConstants.MESSAGETRUNCATE.Length] + nextEvent.MessageString[FlimFlamConstants.MESSAGETRUNCATE.Length..]);
                        }

                        #endregion This message was a truncation of the previous one, deal with it like that

                        continue;
                    }

                    // If we get here then this message is not a truncation so its either an ODS or Tex message

                    // Neither tex nor legacy tex....
                    if (Feature.GetFeatureByName("Bilge-ImportChain").Active) {
                        // New Import route established - we do this just before the other parser kicks in - as if we manage to create EE then the remainder of
                        // the old legacy import code is skipped.
                        // TODO: BUG EE not updatedo
                        var dp = new ImportParser(ois);
                        var raex = new RawApplicationEvent {
                            ArrivalTime = nextEvent.TimeRecieved,
                            Machine = nextEvent.MachineName,
                            Process = nextEvent.Pid.ToString(),
                            Text = nextEvent.MessageString
                        };
                        var parsed = dp.Parse(rae);

                        if (parsed != null) {
                            // Managed to retrieve it using the new importer, in which case map it back to the legacy structure.
                            if (nextEvent.Pid == -1) {
                                var oi = ois.GetIdentity(parsed.OriginIdentity);
                                tempMachineName = oi.Identifier1;
                                nextEvent.Pid = int.Parse(oi.Identifier2);
                            }
                            ee = new EventEntry(parsed);

                            if (ee.GlobalIndex == 0) {
                                throw new InvalidOperationException();
                            }
                            // Jammed V2 Support in here - intercepting unrecognised message type and then jumping down the code
                            // all to be replaced with new message manager
                            goto LEGACYSUPPORT;
                        }
                    }

                    // If it was not a truncation its either a message from another app or a tex message
                    if (!TraceMessageFormat.IsTexString(nextEvent.MessageString)) {
                        // Horrid workaround but whatever, patched this in after the code will refactor in phase 2
                        if (TraceMessageFormat.IsLegacyTexString(nextEvent.MessageString)) {
                            legacyMode = true;
                            goto LEGACYSUPPORT;
                        }

                        // if its not tex the assumption is that it has always come from this machine, although if mex gets v clever this could change
                        var newNte = new NonTracedApplicationEntry(nextEvent.Pid, nextEvent.MessageString, nextEvent.GlobalIndex);

                        if (MexCore.TheCore.Options.XRefMatchingProcessIdsIntoEventEntries && MexCore.TheCore.DataManager.IsPidAKnownApplication(nextEvent.Pid, nextEvent.MachineName)) {
                            // We need to check whether or not this came form the same PID if it did we need to cross reference it.

                            ee = EventEntry.CreatePseudoEE(nextEvent.GlobalIndex, nextEvent.MessageString);
                            tempMachineName = nextEvent.MachineName;

                            if (MexCore.TheCore.Options.XRefReverseCopyEnabled) {
                                // If were doing cross references by PID then we need to determine whether to dupe into the NTA list too.
                                MexCore.TheCore.DataManager.PlaceUnknownEventIntoDataStructure(newNte);
                            } // else we just throw away the NTA as its really a traced app malformed message
                        } else {
                            MexCore.TheCore.DataManager.PlaceUnknownEventIntoDataStructure(newNte);
                            continue;
                        }
                    }

                // The goto bypasses the non legacy code
                LEGACYSUPPORT:
                    int nextLogicalIndexAffected = -1;

                    if (ee == null) {
                        // Must be at least close to legit string as it passed our validation.  Therefore we create a new known event entry and provide it with
                        // the required information to be stored in the traced applications.

                        string tempCommandType;
                        string tempPid;
                        int tPid;
                        string tempDebugMessage;

                        ee = new EventEntry(nextEvent.GlobalIndex) {
                            m_timeMessageRecieved = nextEvent.TimeRecieved
                        };  // Create new entry giving it next index

                        if (legacyMode) {
                            TraceMessageFormat.ReturnPartsOfStringLegacy(nextEvent.MessageString, out tempCommandType, out tempPid, out tempMachineName, out ee.ThreadID, out ee.Module, out ee.LineNumber, out tempDebugMessage);
                            ee.ThreadNetId = "??";
                            ee.MoreLocationData = "LegacyTexSupport";
                            legacyMode = false;
                        } else {
                            TraceMessageFormat.ReturnPartsOfString(nextEvent.MessageString, out tempCommandType, out tempPid, out ee.ThreadNetId, out tempMachineName, out ee.ThreadID, out ee.Module, out ee.LineNumber, out ee.MoreLocationData, out tempDebugMessage);
                        }
                        tPid = int.Parse(tempPid);
                        ee.SetDebugMessage(tempDebugMessage);  // Checks for secondary data and splits accordingly
                        var tct = TraceCommands.StringToTraceCommand(tempCommandType);

                        if (tct == TraceCommandTypes.Alert) {
                            // Alerts are special case.  They dont arrive along with the rest of the data, but sometimes they return a normal event
                            // back to this code to put it into the right place.
                            ee = ManageAlertEvent(ee, tPid, tempMachineName);

                            MexCore.TheCore.WorkManager.AddJob(new Job_NotifyAlertRecieved());
                        }
                        if (tct == TraceCommandTypes.CommandData) {
                            var additionalDataTracedApp = GetTracedApplicationWithCreate(tempMachineName, tPid);
                            additionalDataTracedApp.SetStatusContentsFromXml(tempDebugMessage);
                            nextLogicalIndexAffected = additionalDataTracedApp.VirtualIndex;
                            isCommand = true;
                        } else {
                            if ((tct == TraceCommandTypes.MoreInfo) && tempDebugMessage.StartsWith(FlimFlamConstants.DATAINDICATOR)) {
                                // Data indicator messages include things like the initialisation strings sent when you call//Bilge.Initialise() - this is where additional
                                // information is given about the process and additionally where things like the intitialisation / partial purge on init takes place.
                                var additionalDataTracedApp = GetTracedApplicationWithCreate(tempMachineName, tPid);

                                #region Data indicator messages are not imported, they change the viewer behaviour.

                                tempDebugMessage = tempDebugMessage[FlimFlamConstants.DATAINDICATOR.Length..];
                                ee.DebugMessage = ee.DebugMessage[FlimFlamConstants.DATAINDICATOR.Length..];

                                // Chopped off ~~DATA::
                                switch (ee.DebugMessage[0]) {
                                    case 'P': //ProcessName

                                        // If they have specified their own preferred name for the application then it is passed through here, this preferred name
                                        // should replace most of the alternative views.
                                        if ((ee.SecondaryMessage != null) && (ee.SecondaryMessage.Length > 0)) {
                                            additionalDataTracedApp.PreferredName = ee.SecondaryMessage;
                                        }

                                        // The process name is contained in ProcessName()
                                        string actualAppName = ee.DebugMessage[Consts.PROCNAMEIDENT_PREFIX.Length..];
                                        actualAppName = actualAppName[..^Consts.PROCNAMEIDENT_POSTFIX.Length];
                                        ee.DebugMessage = actualAppName;

                                        if (MexCore.TheCore.Options.XRefAppInitialiseToMain) {
                                            // If this option is selected we place a message into the unknown events to indicate that theres an Xref occured
                                            MexCore.TheCore.DataManager.PlaceUnknownEventIntoDataStructure(new NonTracedApplicationEntry(tPid, "Process " + actualAppName + " Started with pid : (" + tPid.ToString() + ")", nextEvent.GlobalIndex));
                                        }

                                        if (MexCore.TheCore.Options.AutoPurgeApplicationOnMatchingName) {
                                            // BUG!!! Omg too dumb.  Realised only after coding this that if oyu add autopurge as a job option it kicks
                                            // in after the import and then purges all of the new messages.  Aysnchthink ftw.
                                            //Bilge.Log("Synchronous Purge By Matching Name starting.  Trying to purge name " + actualAppName, "However skipping new pid which is " + tPid);
                                            MexCore.TheCore.DataManager.PurgeByName(actualAppName, tempMachineName, tPid);
                                            //MexCore.TheCore.WorkManager.ProcessJob(new Job_PartialPurgeApp(tempMachineName,tPid));

                                            //Bilge.Log("Synchronous Purge By Matching Name Completes.");
                                        }

                                        additionalDataTracedApp.ProcessName = actualAppName;
                                        MexCore.TheCore.WorkManager.AddJob(new Job_NotifyKnownProcessUpdate(MexCore.TheCore.ViewManager.SelectedTracedAppIdx));

                                        break;

                                    case 'M': //MainModule
                                        additionalDataTracedApp.MainModule = ee.DebugMessage;
                                        break;

                                    case 'W':
                                        additionalDataTracedApp.WindowTitle = ee.DebugMessage;
                                        break;

                                    case 'I':
                                        // Initialise(MachineName\appName) skipped.
                                        string initName = ee.DebugMessage[10..];
                                        additionalDataTracedApp.AssemblyFullName = ee.SecondaryMessage;
                                        additionalDataTracedApp.InitialiseName = initName;
                                        ee.DebugMessage = initName;
                                        break;

                                    case 'C': // Calling Assembly
                                        string assemblyData = tempDebugMessage[16..^1];
                                        additionalDataTracedApp.CallingAssemblyInfo = assemblyData;
                                        break;

                                    case 'E': // Executing Assembly
                                        assemblyData = tempDebugMessage[18..^1];
                                        additionalDataTracedApp.ExecutingAssemblyInfo = assemblyData;
                                        break;

                                    case 'T': // ThreadName
                                              //Bilge.Warning("Not Implemented, thread name request recieved but this functionality is not implemented in MEX yet");
                                        break;

                                    default:
                                        //Bilge.Assert(false, "This should not be possible, we should never reach this bit of code.");
                                        break;
                                }

                                #endregion Data indicator messages are not imported, they change the viewer behaviour.

                                continue;
                            } // End if it starts with a data indicator

                            if ((tct == TraceCommandTypes.Custom) && tempDebugMessage.StartsWith("CUS:IMG:PNG")) {
                                // Image Data! Retrieve Image data and add it to the large object store, then put the unique key in the message.
                                byte[] bts = Convert.FromBase64String(ee.SecondaryMessage);
                                ee.SecondaryMessage = $"CUSDATA{ee.GlobalIndex}";
                                MexCore.TheCore.DataManager.AddLargeObject(ee.SecondaryMessage, bts);
                            }
                            // If we get here its a Tex message that is not a data indicator
                            // This is looking like a normal message.  IE Not a moreinfo / data string
                            if (nextEvent.Pid == -1) { nextEvent.Pid = int.Parse(tempPid); }

                            ee.cmdType = tct;

                            // Some messages can be cross referenced into the main view if the options say that.  This section deals with
                            // creating the duplicate messages for this cross reference to work.

                            #region support cross references to main view based on type of event

                            if (MexCore.TheCore.Options.XRefAssertionsToMain && ee.cmdType == TraceCommandTypes.AssertionFailed) {
                                MexCore.TheCore.DataManager.PlaceUnknownEventIntoDataStructure(new NonTracedApplicationEntry(tPid, ee.DebugMessage, ee.GlobalIndex));
                            }

                            if (MexCore.TheCore.Options.XRefErrorsToMain && ee.cmdType == TraceCommandTypes.ErrorMsg) {
                                MexCore.TheCore.DataManager.PlaceUnknownEventIntoDataStructure(new NonTracedApplicationEntry(tPid, ee.DebugMessage, ee.GlobalIndex));
                            }

                            if (MexCore.TheCore.Options.XRefExceptionsToMain && ee.cmdType == TraceCommandTypes.ExceptionBlock) {
                                MexCore.TheCore.DataManager.PlaceUnknownEventIntoDataStructure(new NonTracedApplicationEntry(tPid, ee.DebugMessage, ee.GlobalIndex));
                            }

                            if (MexCore.TheCore.Options.XRefLogsToMain && ee.cmdType == TraceCommandTypes.LogMessage) {
                                MexCore.TheCore.DataManager.PlaceUnknownEventIntoDataStructure(new NonTracedApplicationEntry(tPid, ee.DebugMessage, ee.GlobalIndex));
                            }

                            if (MexCore.TheCore.Options.XRefVerbLogsToMain && ee.cmdType == TraceCommandTypes.LogMessageVerb) {
                                MexCore.TheCore.DataManager.PlaceUnknownEventIntoDataStructure(new NonTracedApplicationEntry(tPid, ee.DebugMessage, ee.GlobalIndex));
                            }

                            if (MexCore.TheCore.Options.XRefResourceMessagesToMain) {
                                if (ee.cmdType is TraceCommandTypes.ResourceEat or TraceCommandTypes.ResourcePuke or TraceCommandTypes.ResourceCount) {
                                    MexCore.TheCore.DataManager.PlaceUnknownEventIntoDataStructure(new NonTracedApplicationEntry(tPid, ee.DebugMessage, ee.GlobalIndex));
                                }
                            }

                            if (MexCore.TheCore.Options.XRefWarningsToMain && ee.cmdType == TraceCommandTypes.WarningMsg) {
                                MexCore.TheCore.DataManager.PlaceUnknownEventIntoDataStructure(new NonTracedApplicationEntry(tPid, ee.DebugMessage, ee.GlobalIndex));
                            }

                            #endregion support cross references to main view based on type of event

                            if (tempDebugMessage.EndsWith(FlimFlamConstants.MESSAGETRUNCATE)) {
                                //Bilge.Log("IncommingMessageManager::ProcessNextStoredMessage --> Truncation message found, storing event for future");
                                // Truncated messages end with #TNK#JoinID#TNK# this lets us join them back together using the unique join id - however
                                // joinIds are only unique per process/machien therefore all of this info is used as a key.
                                int idxStart = tempDebugMessage.IndexOf(FlimFlamConstants.MESSAGETRUNCATE) + FlimFlamConstants.MESSAGETRUNCATE.Length;
                                string joinIdentifier = tempDebugMessage.Substring(idxStart, tempDebugMessage.Length - (idxStart + FlimFlamConstants.MESSAGETRUNCATE.Length));
                                MexCore.TheCore.CacheManager.CacheAdd_EventEntryExpectingTruncate(ee, nextEvent.Pid, tempMachineName, joinIdentifier);
                            }
                        }
                    } // End if it was a normal tex message (ee==null)

                    // END OF IMPORT PARSING< NOW DO STUFF WITH THE NEW MESSAGE

                    if (!isCommand) {
                        //Bilge.Assert(ee != null, "The event entry must be populated by now in the code");

                        // Whether or not it was a cross reference its imported into the data structures now.
                        //Bilge.Assert(tempMachineName != null, "machine name must be populated by the time we add it to the data structures");

                        if (ee.cmdType == TraceCommandTypes.Alert) {
                            // Alerts are special case.  They dont arrive along with the rest of the data, but sometimes they return a normal event
                            // back to this code to put it into the right place.
                            ee = ManageAlertEvent(ee, nextEvent.Pid, tempMachineName);

                            MexCore.TheCore.WorkManager.AddJob(new Job_NotifyAlertRecieved());
                        } else {
                            ee.SupportingData = SupportingMessageData.ParseAsMessageData(ee.cmdType, ref ee.DebugMessage, ref ee.SecondaryMessage);
                        }

                        MexCore.TheCore.ViewManager.CurrentHighlightOptions.ModifyEventEntryForHighlight(ee);

                        // *************** Actually add it into the structure here ***************
                        nextLogicalIndexAffected = MexCore.TheCore.DataManager.PlaceNewEventIntoDataStructure(ee, nextEvent.Pid, tempMachineName);
                    }

                    if (!appIndexesAffectedByImport.Contains(nextLogicalIndexAffected)) {
                        appIndexesAffectedByImport.Add(nextLogicalIndexAffected);
                    }

                    if (!comparer.Compare(ee)) {
                        // This is our porting code, if this fires then we have got  a problem with our new parser.
                        //Bilge.WarningLog("FAILED A COMPARISON");
                        if (Debugger.IsAttached) {
                            Debugger.Break();
                        }
#if DEBUG
                        throw new InvalidOperationException("The new parser is out of sync");
#endif
                    }


                    if (reporting) {
                        // For long running imports make sure that we tell the client side every now and again
                        var ts = DateTime.Now - startTime;
                        if (ts.TotalSeconds > MexCore.TheCore.Options.NoSecondsForRefreshOnImport) {
                            startTime = DateTime.Now;
                            var overall = DateTime.Now - overallStartTime;

                            MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.MessageImportLongRunning, UserMessageType.InformationMessage, overall.TotalSeconds.ToString() + "s elapsed.");

                            foreach (int appAffectedIdx in appIndexesAffectedByImport) {
                                MexCore.TheCore.WorkManager.ProcessJob(new Job_NotifyNewEventAdded(appAffectedIdx));
                            }
                        }
                    }
                } while ((m_incommingMsgQueue.Count > 0) && doAll);
            } finally {
                if (MexCore.TheCore.Options.PersistEverything) {
                    everythingLog.Close();
                    everythingStream.Close();
                    everythingLog.Dispose();
                    everythingStream.Dispose();
                }
            }

            foreach (int appAffectedIdx in appIndexesAffectedByImport) {
                MexCore.TheCore.WorkManager.AddJob(new Job_NotifyNewEventAdded(appAffectedIdx));
            }
        }

        private EventEntry ManageAlertEvent(EventEntry ee, int nextEventPid, string tempMachineName) {
            var ae = new AlertEntry {
                Message = ee.DebugMessage,
                Secondary = ee.SecondaryMessage,
                OccuredAt = DateTime.Now
            };

            if (ee.SecondaryMessage.EndsWith("{ \r\n")) {
                ee.SecondaryMessage = ee.SecondaryMessage[..^4] + "}";
                //ee.SecondaryMessage  = ee.SecondaryMessage.Replace("\"", "'");
                ee.SecondaryMessage = ee.SecondaryMessage.Replace("\" \"", "\", \"");

                var ap = System.Text.Json.JsonSerializer.Deserialize<AlertEntryProperties>(ee.SecondaryMessage);
                ee.SupportingData = ap;

                if (MexCore.TheCore.Options.AutoPurgeApplicationOnMatchingName) {
                    foreach (var l in MexCore.TheCore.DataManager.GetAllProcessSummaries()) {
                        if (l.ProcLabel == ap.AppName) {
                            MexCore.TheCore.DataManager.PurgeKnownApplication(l.InternalIndex);
                        }
                    }
                }

                int idx = MexCore.TheCore.DataManager.PlaceNewEventIntoDataStructure(ee, nextEventPid, tempMachineName);
                var ta = MexCore.TheCore.DataManager.GetKnownApplication(idx);
                ta.ProcessLabel = ap.AppName;
            }

            _ = MexCore.TheCore.DataManager.PlaceAlertInoDataStructure(ae);
            return ee;
        }

        private static TracedApplication GetTracedApplicationWithCreate(string tempMachineName, int tPid) {
            var additionalDataTracedApp = MexCore.TheCore.DataManager.GetKnownApplicationByPid(tPid, tempMachineName);
            if (additionalDataTracedApp == null) {
                _ = MexCore.TheCore.DataManager.CreateNewTracedApp(tPid, tempMachineName);
                additionalDataTracedApp = MexCore.TheCore.DataManager.GetKnownApplicationByPid(tPid, tempMachineName);
                //Bilge.Assert(additionalDataTracedApp != null, "The traced app could not be created and no error thrown!?");
            }
            return additionalDataTracedApp;
        }

        /// <summary>
        /// Shuts downt he incomming message manager, including closing the ODS gatherer and TCP gatherer if they are active.
        /// </summary>
        public void ShutDown() {
            //Bilge.Log("IncomingMessageManager::Shutdown - Shutdown requested", "Shutting down ODS and TCP Listeners");
            m_shutdownRequested = true;
            DeactivateODSGatherer();
            DeactivateTCPGatherer();
        }

        internal int MessagesPendingImport {
            get { return m_incommingMsgQueue.Count; }
        }

        #region static factory support for incoming message manager


        /// <summary>
        /// Factory for the incomming message manager class to ensure that there is only one incomming message manager
        /// </summary>
        /// <returns></returns>
        public static IncomingMessageManager Current { get; } = new IncomingMessageManager();

        #endregion static factory support for incoming message manager
    }
}