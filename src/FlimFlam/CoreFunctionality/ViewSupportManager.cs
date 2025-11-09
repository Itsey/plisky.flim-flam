namespace Plisky.FlimFlam;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OldFlimflam;
using Plisky.Diagnostics;
using Plisky.Diagnostics.FlimFlam;
using Plisky.Plumbing;

using TimerPartialInstancedataStructure = System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<Plisky.FlimFlam.TimeInstanceView>>;

/// <summary>
/// Summary description for UISupportManager.
/// </summary>
public class ViewSupportManager {

    public Bilge b = new Bilge("Plisky.FlimFlam.ViewSupportManager");

    internal enum ExtendedDetailsMode {
        Exception, LogMessage, Error, AssertionFailure,
        ImageData
    }

    internal ActiveFindStructure ActiveFind { get; set; }

    internal string LastViewSummary { get; set; } = "[None]";   // Used each time a view refresh is called, summarising the data displayed.
    internal bool CancelCurrentViewOperation { get; set; }

    internal ViewFilter CurrentFilter { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
    internal int KnownProcessCount {
        get { return MexCore.TheCore.DataManager.KnownProcessCount; }
    }

    internal int SelectedTracedAppIdx { get; set; } = -1;

    internal TracedApplication SelectedTracedApp {
        get {
            return SelectedTracedAppIdx > 0 ? MexCore.TheCore.DataManager.GetKnownApplication(SelectedTracedAppIdx) : null;
        }
    }

    internal string SelectedTracedAppName {
        get {
            // todo : cache for perf
            return MexCore.TheCore.DataManager.GetKnownApplication(SelectedTracedAppIdx).MachineName + "\\" +
                   MexCore.TheCore.DataManager.GetKnownApplication(SelectedTracedAppIdx).ProcessName;
        }
    }

    internal string SelectedTracedAppComment {
        get {
            // tODO : perf
            return MexCore.TheCore.DataManager.GetKnownApplication(SelectedTracedAppIdx).ProcessLabel;
        }
    }

    internal int SelectedTracedAppPid {
        // todo cache for perf?
        get { return MexCore.TheCore.DataManager.GetKnownApplication(SelectedTracedAppIdx).ProcessIdNo; }
    }

    internal ProcessSummary SelectedTracedAppSummary {
        // Todo : Cache for perf ??

        get {
            return new ProcessSummary(MexCore.TheCore.DataManager.GetKnownApplication(SelectedTracedAppIdx));
        }
    }

    /// <summary>
    /// This property will check wither the current value for SelectedTracedApp results in a valid application.
    /// </summary>
    internal bool IsCurrentSelectedAppValid {
        get {
            if (SelectedTracedAppIdx == -1) { return false; } // Short circuit when its never been set up.
            return MexCore.TheCore.DataManager.IsValidTracedApplicationIndex(SelectedTracedAppIdx);  // Check to see its still valid
        }
    } 

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
    internal void ShutDown() {        
        b.Info.Log("MexViewer::ViewSupportManager::Shutdown requested, ViewSupport is closing");
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
    internal string QueuedMessagesCount {
        get { return MexCore.TheCore.MessageManager.MessagesPendingImport.ToString(); }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
    internal string LastIssuedGlobalIndex {
        get { return MexCore.TheCore.DataManager.LastUsedGlobalIndex.ToString(); }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
    internal string LastMessageRecievedTime {
        get { return MexCore.TheCore.DataManager.LastMessageRecievedTime.ToShortTimeString(); }
    }

    internal void ProcessAlertNotification() {
        MexCore.TheCore.ViewManager.RefreshCurrentView(true);
    }

    private long userNotificationCount;
    private readonly List<string> userNotificationMessageLog = new();

    internal void RenderUserNotificationLog(ListBox lbx) {
        lbx.Items.Clear();
        foreach (string s in userNotificationMessageLog) {
            _ = lbx.Items.Add(s);
        }
    }

    internal string LastUserNotificationMessage {
        get {
            return userNotificationMessageLog.Count == 0 ? string.Empty : userNotificationMessageLog[^1];
        }
    }

    private UserMessages lastMessageWritten;

    internal void AddUserNotificationMessageByIndex(UserMessages msg, UserMessageType mstype, string parameter) {
        if (parameter == null) {
            parameter = string.Empty;
        }
        string theMsg;
        switch (msg) {
            case UserMessages.UnhandledExceptionOccured: theMsg = "There was an unhandled exception in the application. >"+parameter; break;
            case UserMessages.NoThreadsSelectedForView: theMsg = "No threads were selected, therefore the thread view could not be updated."; break;
            case UserMessages.ErrorDuringfileImport: theMsg = "There was an error during the file import.  The import did not succeed"; break;
            case UserMessages.CorruptStringFound: theMsg = "An entry scheduled for import was corrupt. Mex has thrown away this entry and continued."; break;
            case UserMessages.InvalidTruncateStringFound: theMsg = "A truncation string was found, however it was corrupt and could not be interpreted by Mex.  Most likely cause for this is the wrong version of//Bilge."; break;
            case UserMessages.FilterDirectoryChangeFailedToRelocateFilters: theMsg = "During filter directory change not all files were relocated, File failed: " + parameter; break;
            case UserMessages.FilterFileNotFound: theMsg = "After selecting to load a filter, the filter could not be found on the disk.  The file requested was " + parameter; break;
            case UserMessages.DefaultFilterFileNotFound: theMsg = "The profile asks to load a default filter, however this filter could not be found on disk.  The filter name was " + parameter; break;
            case UserMessages.WaitTimeoutOccured: theMsg = "A wait timeout occured, the operation did not complete in a timely manor.  It has been forcibly terminated. (" + parameter + ")"; break;
            case UserMessages.OptionsConfiguraitonError: theMsg = "An error occured altering Mex options: " + parameter; break;
            case UserMessages.InvalidDataStructureError: theMsg = "An internal error occured due to the state of the data structures.  " + parameter; break;
            case UserMessages.GeneralUIError: theMsg = "An error has occured with the Mex UI.  This operation should not have been possible: " + parameter; break;

            case UserMessages.BackgroundHighlightOperationBegins: theMsg = "The highlight operation has begun on a background thread"; break;
            case UserMessages.BackgroundHighlightOperationEnds: theMsg = "The highlight operation has completed."; break;
            case UserMessages.HighlightOperationRequestsViewRefresh: theMsg = "Current view refreshed, to apply newly chosen highlight settings"; break;
            case UserMessages.BackgroundApplyOptionsBegins: theMsg = "The options are being applied in the background."; break;
            case UserMessages.BackgroundFileImportBegins: theMsg = "The import of the file has started on a background thread."; break;
            case UserMessages.BackgroundfileImportEnds: theMsg = "The import of the file has finished."; break;
            case UserMessages.ImportFileStarts: theMsg = "A file is being loaded into Mex. (" + parameter + ")"; break;

            case UserMessages.MessageImportLongRunning: theMsg = "Mex is busy processing a lot of incomming messages in the background. " + parameter; break;
            case UserMessages.MexIsIdle:
                if (msg == lastMessageWritten) {
                    // dont do this more than once.
                    return;
                }
                theMsg = "Mex is currently idle." + parameter;
                break;

            case UserMessages.PurgeJobStarts: theMsg = "A Background purge of all data begins."; break;
            case UserMessages.PurgeJobCompletes: theMsg = "Purge all has completed."; break;
            case UserMessages.TCPListenerTurnedOff: theMsg = "The TCP Server Socket has been deactivated. " + parameter; break;
            case UserMessages.TCPListenerTurnedOn: theMsg = "The TCP Server Socket has been activated."; break;
            case UserMessages.ODSListenerTurnedOff: theMsg = "The OutputDebugString listener has been disabled."; break;
            case UserMessages.ODSListenerTurnedOn: theMsg = "The OutputDebugString listener has been activated."; break;
            case UserMessages.TCPListenerBindingError: theMsg = "The socket was not able to begin listening.  TCP Error." + parameter; break;
            case UserMessages.TCPListenerInvalidHostError: theMsg = "The IPAddress for binding caused an error. " + parameter; break;
            case UserMessages.TCPStatusMessage: theMsg = "TCP Listener Status Message >> " + parameter; break;
            case UserMessages.ODSStatusMessage: theMsg = "ODS Listener Status Message >> " + parameter; break;
            case UserMessages.PubSubEnable: theMsg = "OnDemand Listener Status >> " + parameter; break;
            default:
                //Bilge.Warning("ERROR Mising error message.");
                theMsg = "Internal Mex error - Missing information about this error condition.";
                break;
        }

        lastMessageWritten = msg;
        userNotificationCount++;
        theMsg = userNotificationCount.ToString("0000") + " >" + mstype.ToString() + " > " + theMsg;
        userNotificationMessageLog.Add(theMsg);

        if (userNotificationMessageLog.Count > MexCore.TheCore.Options.NoUserNotificationsToStoreInLog) {
            userNotificationMessageLog.RemoveAt(0);
        }

        if (MexCore.TheCore.Options.InteractiveNotifications) {
            _ = MessageBox.Show(theMsg);
        }
    }

    internal HighlightRequestsStore CurrentHighlightOptions { get; set; }
    private bool highlightChangeCausesRefresh;

    /// <summary>
    /// This is used on a callback visiting each event entry, hekce why it uses the globals
    /// </summary>
    /// <param name="ee">Event entry visited </param>
    /// <param name="appIdx">The id of the application being refreshed</param>
    /// <returns>Boolean indicating whether a refresh is required</returns>
    internal bool ApplyCurrentHighlightOptionsToEventEntry(EventEntry ee, int appIdx) {
        bool triggerRefresh = CurrentHighlightOptions.ModifyEventEntryForHighlight(ee);
        // TODO : Fix this - the highlighting isnt right.
        triggerRefresh &= CurrentHighlightOptions.ApplyDefaultHighlighting(ee);

        if (triggerRefresh && (appIdx == SelectedTracedAppIdx)) {
            highlightChangeCausesRefresh = true;
        }

        return true; // Continue with the search
    }

    internal bool HighlightUpdateComplete(bool everyEntryVisited) {
        if (highlightChangeCausesRefresh && (RegisterForSelectedProcessRefreshRequired != null)) {
            RegisterForSelectedProcessRefreshRequired(false);
        }
        highlightChangeCausesRefresh = false;
        return true; // ?
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
    internal void ReapplyHighlight() {
        MexCore.TheCore.WorkManager.AddJob(new Job_ApplyHighlightToView());
        MexCore.TheCore.WorkManager.AddJob(new Job_ApplyHighlightToStructures());
    }

    internal void UpdateHighlight() {
        //Bilge.Assert(CurrentHighlightOptions != null, "Current highlight options must be set to at least a default by this point in the code");

        using var fhl = new frmHighlighting();
        if (CurrentHighlightOptions != null) {
            fhl.PopulateFromRequests(CurrentHighlightOptions);
        }
        if (fhl.ShowDialog() == DialogResult.OK) {
            CurrentHighlightOptions = fhl.GetNewHighlightStore();
            ReapplyHighlight();
        }
    }

    internal EventEntry GetEventEntryFromSelectedAppByIndex(long idx) {
        //Bilge.Log("Mex::ViewSupportManager::GetEventEntryCopyByIndex >> Copying an entry based on index for selected app " + m_selectedTracedApp);
        EventEntry result;

        var ta = MexCore.TheCore.DataManager.GetKnownApplication(SelectedTracedAppIdx);
        ta.EventEntries.EventEntriesRWL.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);

        try {
            int offset = ta.EventEntries.ContainsThisGlobalIdx(idx);
            //Bilge.Assert(offset >= 0, "This should not be possible didnt the user just click this");

            result = new EventEntry(ta.EventEntries[offset]);
        } finally {
            ta.EventEntries.EventEntriesRWL.ReleaseReaderLock();
        }
        return result;
    }

    internal List<string> GetAllModules(int limit=-1) {
        #region entry code
        //Bilge.Assert(m_selectedTracedApp != -1, "MexViewer::ViewSupportManager::GetAllThreads >> Called while there was no selected application.  Cant return thread list when no application selected");
        //Bilge.Assert(MexCore.TheCore.DataManager.IsValidTracedApplicationIndex(m_selectedTracedApp), "MexViewer::ViewSupportManager::RefreshProcessView >> The index specified byt the currently selected application is out of range.  RefreshProcessView is trying to refresh an invalid process.");
        #endregion entry code
        //Bilge.Log("Mex::ViewSupportMAnager::GetAllModules >> Call made to retrieve module list for selected app " + m_selectedTracedApp);

        int count = 0;
        var moduleList = new List<string>();
        
        var ta = MexCore.TheCore.DataManager.GetKnownApplication(SelectedTracedAppIdx);

        if (ta != null) {
            // if you hit the filter just after a purge you need to skip this
            ta.EventEntries.EventEntriesRWL.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);
            try {
                foreach (EventEntry ee in ta.EventEntries) {
                    if (!string.IsNullOrEmpty(ee.module) && !moduleList.Contains(ee.module)) {
                        count++;
                        moduleList.Add(ee.module);
                        if ((limit > 0) && (count >= limit)) {
                            break;
                        }
                    }
                }
            } finally {
                ta.EventEntries.EventEntriesRWL.ReleaseReaderLock();
            }
        }
        return moduleList;
    }

    internal List<string> GetAllAdditionalLocations(int limit = -1) {
        #region entry code
        //Bilge.Assert(m_selectedTracedApp != -1, "GetAllAdditionalLocations >> Called while there was no selected application.  Cant return additional location list when no application selected");
        //Bilge.Assert(MexCore.TheCore.DataManager.IsValidTracedApplicationIndex(m_selectedTracedApp), "GetAllAdditionalLocations >> The index specified byt the currently selected application is out of range.  RefreshProcessView is trying to refresh an invalid process.");
        #endregion entry code
        //Bilge.Log("Mex::ViewSupportMAnager::GetAllAdditionalLocations >> Call made to retrieve module list for selected app " + m_selectedTracedApp);

        var additionalLocationsList = new List<string>();
        int count = 0;
        var ta = MexCore.TheCore.DataManager.GetKnownApplication(SelectedTracedAppIdx);
        if (ta != null) {
            ta.EventEntries.EventEntriesRWL.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);
            try {
                foreach (EventEntry ee in ta.EventEntries) {
                    if (!string.IsNullOrEmpty(ee.moreLocationData) && !additionalLocationsList.Contains(ee.moreLocationData)) {
                        additionalLocationsList.Add(ee.moreLocationData);
                        count++;
                        if ((limit > 0) && (count >= limit)) {
                            break;
                        }
                    }
                }
            } finally {
                ta.EventEntries.EventEntriesRWL.ReleaseReaderLock();
            }
        }
        return additionalLocationsList;
    }

    internal void GetAdditionalLocationsAndClasses(out List<string> fullLocationData, out List<string> classOnlyData) {
        // TODO : implement this better (single loop) for performance if used frequently.
        classOnlyData = GetAdditionalLocationClassesFromAdditionalLocations();
        fullLocationData = GetAllAdditionalLocations();
    }

    internal List<string> GetAdditionalLocationClassesFromAdditionalLocations() {
        // TODO : implement this better (single loop) for performance if used frequently.
        var results = GetAllAdditionalLocations();
        return GetAdditionalLocationClassesFromAdditionalLocations(results);
    }

    /// <summary>
    /// Many of the locations are autogenerated or are manually coded to use :: as the class / method separator as this is the C++ wont and
    /// lots of people have seen this and copied it.  This will separate these classes from the location stirngs so that the classes can be
    /// used as filters in their own right.
    /// </summary>
    /// <param name="fullLocationData">The list of full location data elemeents</param>
    /// <returns>List of any class names that could be pulled from the full location data.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
    internal List<string> GetAdditionalLocationClassesFromAdditionalLocations(List<string> fullLocationData) {
        var result = new List<string>();

        foreach (string s in fullLocationData) {
            if (s==null) { continue; }
            int idx = s.IndexOf("::");
            if (idx > 0) {
                string next = s[..idx];
                if (!string.IsNullOrWhiteSpace(next)) {
                    if (!result.Contains(next)) {
                        result.Add(next);
                    }
                }
            }
        }

        return result;
    } 

    /// <summary>
    /// This method is called when the user wants to duplicate an entry to create a pseudo view.
    /// </summary>
    /// <param name="startIdx">The lowest index to include</param>
    /// <param name="endIdx">The highest index to include</param>
    /// <param name="referenceName">A reference name for the dupe</param>
    internal void CreateDuplicateOfSelectedApplication(string referenceName, long startIdx, long endIdx) {
        var taDupe = MexCore.TheCore.DataManager.GetKnownApplication(SelectedTracedAppIdx);
        var taNew = new TracedApplication(taDupe, referenceName, startIdx, endIdx);
        _ = MexCore.TheCore.DataManager.AddDuplicatedTracedApplication(taNew);
    }

    internal void CreateDuplicateOfSelectedApplication(string referenceName) {
        var taDupe = MexCore.TheCore.DataManager.GetKnownApplication(SelectedTracedAppIdx);
        var taNew = new TracedApplication(taDupe, referenceName);
        _ = MexCore.TheCore.DataManager.AddDuplicatedTracedApplication(taNew);
    }

    internal string GetSelectedAppDescription() {
        var ta = MexCore.TheCore.DataManager.GetKnownApplication(SelectedTracedAppIdx);
        return "Process " + ta.ProcessIdAsString + " (" + ta.ProcessLabel + ")";
    }

    #region Dialog view refresh functions : GetMoreInfo, PopulateExceptionDialog

    /// <summary>
    /// This method will be called by the currently selected view passing us an index of the currently selected event entry.  This method will locate
    /// the event entry and try and get any further information related to this entry which will be returned as a string.  The string will not be formatted
    /// except for presentation to the user.
    /// </summary>
    /// <param name="theIndex">The index of the selected event for the currently seelcted application</param>
    /// <returns>String containing more information about that selected index</returns>
    internal string GetMoreInfoForEventIndexInSelectedApp(long theIndex) {
        #region entry code
        //Bilge.Assert(MexCore.TheCore.DataManager.IsValidTracedApplicationIndex(m_selectedTracedApp), "MexViewer::ViewSupportManager::GetMoreInfoForEventIndex >> The index specified byt the currently selected application is out of range.  RefreshProcessView is trying to refresh an invalid process.");
        //Bilge.Assert(MexCore.TheCore.DataManager.GetKnownApplication(m_selectedTracedApp).EventEntries.ContainsThisGlobalIdx(theIndex) >= 0, "MexViewer::ViewSupportManager::GetMoreInfoForEventIndex >> The index for the event entry im supposed to be looking up could not be found in the selected applications event entries.  This is an invalid request.");
        #endregion entry code

        //Bilge.Log("MexViewer::ViewSupportManager::GetMoreInfoForEventIndex >> Looknig for further info");
        string result = string.Empty;      // could use a  stirng builder but in the end string rocks

        var ta = MexCore.TheCore.DataManager.GetKnownApplication(SelectedTracedAppIdx);
        if (ta == null) { return string.Empty; }

        ta.EventEntries.EventEntriesRWL.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);
        try {
            int tempOffset = -1;  // Used to move around the event entries

            // Look at the event entry and see if it has more info.
            int offsetOfIndex = ta.EventEntries.ContainsThisGlobalIdx(theIndex);

            result += "Trace Entry Located in Module: " + ta.EventEntries[offsetOfIndex].module + " @Line: " + ta.EventEntries[offsetOfIndex].lineNumber + " entered Mex at :" + ta.EventEntries[offsetOfIndex].TimeMessageRecieved.ToString() + "\r\n";

            if (offsetOfIndex == -1) {
                //Bilge.Warning("How on earth did this happen ? weve been asked to find more info for an event thats not in the current selection");
                return "This entry could no longer be found in the structure.";
            }

            if ((ta.EventEntries[offsetOfIndex].secondaryMessage != null) && (ta.EventEntries[offsetOfIndex].secondaryMessage.Length > 0)) {
                // Now do secondary message processing, this is only unique for Trace Enter / Exits
                if (ta.EventEntries[offsetOfIndex].cmdType is TraceCommandTypes.TraceMessageIn or TraceCommandTypes.TraceMessageOut) {
                    var dt = GetDateTimeFromTraceMessage(ta.EventEntries[offsetOfIndex].secondaryMessage);
                    if (ta.EventEntries[offsetOfIndex].cmdType == TraceCommandTypes.TraceMessageIn) {
                        result += "Entered method at: " + dt.Hour.ToString() + ":" + dt.Minute.ToString() + ":" + dt.Second.ToString() + ":" + dt.Millisecond.ToString();
                    } else {
                        result += "Left method at: " + dt.Hour.ToString() + ":" + dt.Minute.ToString() + ":" + dt.Second.ToString() + ":" + dt.Millisecond.ToString();
                    }
                } else {
                    result += "Secondary Message: " + ta.EventEntries[offsetOfIndex].secondaryMessage;
                }
                result += "\r\n";
            }

            tempOffset = offsetOfIndex;
            while (ta.EventEntries[tempOffset].HasMoreInfo) {
                if (tempOffset + 1 < ta.EventEntries.Count) {
                    tempOffset++;
                    // This string is not beautified because it should include CRLF's for multiple line strings.
                    result += "\r\n" + ta.EventEntries[tempOffset].debugMessage;
                } else {
                    // This means that even though the first message THINKS there are follow up messages there arent and they
                    // have been deleted or something.
                    ta.EventEntries[tempOffset].HasMoreInfo = false;
                    break;
                }
                if (tempOffset > 100) {
                    result += "\r\n\r\n *** More than 100 messages have been displayed ***";
                    break;
                }
                //Bilge.Assert(tempOffset < 15000, "this should not be possible, 15000 more info messages? ");
            }
            if (result.Length == 0) {
                result = "There was no further information for this event.";
            }

            //Bilge.Log("MexViewer::ViewSupportManager::GetMoreInfoForEventIndex >> Looking backwards for trace info");
            tempOffset = offsetOfIndex;
        } finally {
            ta.EventEntries.EventEntriesRWL.ReleaseReaderLock();
        }
        return result;
    } 

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
    private string BeautifyDebugMessage(string msg) {
        if (MexCore.TheCore.Options.BeautifyDisplayedStrings) {
            return msg.Replace("\t", " [TAB] ").Replace("\r", "").Replace("\n", ""); ;
        } else {
            return msg;
        }
    }

    /// <summary>
    /// This method will be called by the currently selected view passing us an index of the currently selected event entry.  This method will locate
    /// the event entry and try and get any further information related to this entry which will be returned as a string.  The string will not be formatted
    /// except for presentation to the user.
    /// </summary>
    /// <param name="theIndex">The index of the selected event for the currently seelcted application</param>
    /// <returns>String containing more information about that selected index</returns>
    internal string GetMoreInfoForEventIndexGlobally(long theIndex) {
        #region entry code
        #endregion entry code

        //Bilge.Log("MexViewer::ViewSupportManager::GetMoreInfoForEventIndex >> Looknig for further info");
        int tracedIdx = MexCore.TheCore.DataManager.FindTracedAppIndexThatContainsGlobalIndex(theIndex);

        string result = string.Empty;      // could use a  stirng builder but in the end string rocks

        var ta = MexCore.TheCore.DataManager.GetKnownApplication(tracedIdx);
        ta.EventEntries.EventEntriesRWL.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);
        try {
            int tempOffset = -1;  // Used to move around the event entries

            // Look at the event entry and see if it has more info.
            int offsetOfIndex = ta.EventEntries.ContainsThisGlobalIdx(theIndex);

            result += "Trace Entry Located in Module: " + ta.EventEntries[offsetOfIndex].module + " @Line: " + ta.EventEntries[offsetOfIndex].lineNumber + "\r\n";

            if (offsetOfIndex == -1) {
                //Bilge.Warning("How on earth did this happen ? weve been asked to find more info for an event thats not in the current selection");
                return "This entry could no longer be found in the structure.";
            }

            if ((ta.EventEntries[offsetOfIndex].secondaryMessage != null) && (ta.EventEntries[offsetOfIndex].secondaryMessage.Length > 0)) {
                // Now do secondary message processing, this is only unique for Trace Enter / Exits
                if (ta.EventEntries[offsetOfIndex].cmdType is TraceCommandTypes.TraceMessageIn or TraceCommandTypes.TraceMessageOut) {
                    var dt = GetDateTimeFromTraceMessage(ta.EventEntries[offsetOfIndex].secondaryMessage);
                    if (ta.EventEntries[offsetOfIndex].cmdType == TraceCommandTypes.TraceMessageIn) {
                        result += "Entered method at: " + dt.Hour.ToString() + ":" + dt.Minute.ToString() + ":" + dt.Second.ToString() + ":" + dt.Millisecond.ToString();
                    } else {
                        result += "Left method at: " + dt.Hour.ToString() + ":" + dt.Minute.ToString() + ":" + dt.Second.ToString() + ":" + dt.Millisecond.ToString();
                    }
                } else {
                    result += "Secondary Message: " + ta.EventEntries[offsetOfIndex].secondaryMessage;
                }
                result += "\r\n";
            }

            tempOffset = offsetOfIndex;
            while (ta.EventEntries[tempOffset].HasMoreInfo && (tempOffset < ta.EventEntries.Count)) {
                tempOffset++;
                result += "\r\n" + BeautifyDebugMessage(ta.EventEntries[tempOffset].debugMessage);
                //Bilge.Assert(tempOffset < 15000, "this should not be possible, 15000 more info messages? ");
            }
            if (result.Length == 0) {
                result = "There was no further information for this event.";
            }

            //Bilge.Log("MexViewer::ViewSupportManager::GetMoreInfoForEventIndex >> Looking backwards for trace info");
            tempOffset = offsetOfIndex;


        } finally {
            ta.EventEntries.EventEntriesRWL.ReleaseReaderLock();
        }
        return result;
    } // End GetMoreInfoForEventIndex method

    #endregion Dialog view refresh functions : GetMoreInfo, PopulateExceptionDialog

    #region Refresh Views ( ProcessView / Timed View / Main View )

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
    public int ImageIndexFromEventType(TraceCommandTypes tct) {
        return tct switch {
            TraceCommandTypes.LogMessage => 32,
            TraceCommandTypes.LogMessageVerb => 34,
            TraceCommandTypes.InternalMsg => 2,
            TraceCommandTypes.TraceMessageIn => 15,
            TraceCommandTypes.TraceMessageOut => 17,
            //case TraceCommandTypes.TraceMessage: return 19;
            TraceCommandTypes.AssertionFailed => 13,
            TraceCommandTypes.MoreInfo => 7,
            TraceCommandTypes.CommandOnly => 0,
            TraceCommandTypes.Custom => 0,
            TraceCommandTypes.ErrorMsg => 21,
            TraceCommandTypes.WarningMsg => 37,
            TraceCommandTypes.ExceptionBlock => 22,
            TraceCommandTypes.ExceptionData => 23,
            TraceCommandTypes.ExcStart => 23,
            TraceCommandTypes.ExcEnd => 23,
            TraceCommandTypes.SectionStart => 6,
            TraceCommandTypes.SectionEnd => 6,
            TraceCommandTypes.ResourceEat => 4,
            TraceCommandTypes.ResourcePuke => 3,
            TraceCommandTypes.ResourceCount => 36,
            _ => 0
        };        
    }

    internal void RefreshView_Alerting(ListBox lbgAlertEntries, Label lblMostRecentText, Label lblSelectedAlertText, bool refreshMode) {
        lbgAlertEntries.Items.Clear();

        var ales = MexCore.TheCore.DataManager.GetAlertEntries().OrderByDescending(x => x.OccuredAt).ToArray();
        if (ales.Length > 0) {
            lbgAlertEntries.Items.AddRange(ales);
            lblMostRecentText.Text = ales[0].ToString();
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
    internal DateTime GetDateTimeFromTraceMessage(string traceMessage) {
        const string TMRIDENT = ">TRCTMR<|";

        if ((traceMessage == null) || (traceMessage.Length == 0) || (traceMessage.IndexOf(TMRIDENT) < 0)) {
            return DateTime.MinValue;
        }

        string[] splitone = traceMessage[(TMRIDENT.Length + 4)..].Split('|');
        try {
            int dd = int.Parse(splitone[0]);
            int mm = int.Parse(splitone[1]);
            int yy = int.Parse(splitone[2]);
            int hh = int.Parse(splitone[3]);
            int mi = int.Parse(splitone[4]);
            int ss = int.Parse(splitone[5]);
            int ms = int.Parse(splitone[6]);

            var enteredTime = new DateTime(yy, mm, dd, hh, mi, ss, ms);
            return enteredTime;
        } catch (FormatException) {
        }
        return DateTime.MinValue;
    }

    internal void RefreshView_ProcessTree(TreeView tvwTheView, KeyDisplayRepresentation threadIdentityToWorkOn) {
        long parsed = 0;
        long included = 0;

        TreeNode nextNode;
        TreeNode currentParent;

        tvwTheView.BeginUpdate();
        tvwTheView.Nodes.Clear();

        var ta = MexCore.TheCore.DataManager.GetKnownApplication(SelectedTracedAppIdx);
        ta.EventEntries.EventEntriesRWL.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);
        try {
            currentParent = new TreeNode(ta.PreferredDisplayName + " (" + ta.ProcessIdAsString + ") running on on " + ta.MachineName) {
                ImageIndex = 0
            };
            _ = tvwTheView.Nodes.Add(currentParent);

            foreach (EventEntry ee in ta.EventEntries) {
                parsed++;

                if (ee.CurrentThreadKey != threadIdentityToWorkOn.KeyIdentity) {
                    continue;  // Tree view only works on one thread at a time
                }

                if (ViewFilter.EventEntryIsInternalType(ee) && (!MexCore.TheCore.Options.DisplayInternalMessages)) { continue; }  // Skip internal messages
                if (MexCore.TheCore.Options.RespectFilter && (!CurrentFilter.IncludeThisEventEntry(ee))) { continue; }  // Skip messages based on filter

                included++;

                #region Cancellation support code
                if (MexCore.TheCore.Options.SupportCancellationOfRefresh) {
                    Application.DoEvents();

                    if (CancelCurrentViewOperation) {
                        //Bilge.Log("View operation canceled due to user request.");
                        LastViewSummary = "Cancel Requested";
                        CancelCurrentViewOperation = false;
                        return;
                    }
                }
                #endregion Cancellation support code

                nextNode = new TreeNode(BeautifyDebugMessage(ee.debugMessage));
                if (ee.viewData.isValid && ee.viewData.isHighlighted) {
                    if (ee.viewData.isBackgroundHighlighted) {
                        nextNode.BackColor = ee.viewData.backgroundHighlightColor;
                    }
                    if (ee.viewData.isForegroundHighlighted) {
                        nextNode.ForeColor = ee.viewData.foregroundHighlightColor;
                    }
                }

                if (ee.cmdType == TraceCommandTypes.TraceMessageIn) {
                    if (ee.secondaryMessage != null) {
                        var dt = GetDateTimeFromTraceMessage(ee.secondaryMessage);
                        if (dt != DateTime.MinValue) {
                            nextNode.Tag = dt;
                        }
                    }

                    nextNode.ImageIndex = ImageIndexFromEventType(ee.cmdType);
                    //nextNode.StateImageIndex = ImageIndexFromEventType(ee.cmdType);
                    _ = currentParent.Nodes.Add(nextNode);
                    currentParent = nextNode;
                    continue;
                }

                if (ee.cmdType == TraceCommandTypes.TraceMessageOut) {
                    if (ee.secondaryMessage != null) {
                        var dt = GetDateTimeFromTraceMessage(ee.secondaryMessage);
                        if (dt != DateTime.MinValue) {
                            nextNode.Tag = dt;
                            var elapsed = dt - ((DateTime)currentParent.Tag);
                            currentParent.Text += "     Elapsed Time " + elapsed.TotalMilliseconds + " ms.";
                        }
                    }

                    nextNode.ImageIndex = ImageIndexFromEventType(ee.cmdType);
                    _ = currentParent.Nodes.Add(nextNode);
                    currentParent = currentParent.Parent;
                    continue;
                }

                nextNode.ImageIndex = ImageIndexFromEventType(ee.cmdType);
                _ = currentParent.Nodes.Add(nextNode);
            } // End foreach
        } finally {
            ta.EventEntries.EventEntriesRWL.ReleaseReaderLock();
        }
        tvwTheView.EndUpdate();
    }

    #endregion Refresh Views ( ProcessView / Timed View / Main View )

    #region Process View Supporting Functions
    private long lastglobalIndexForProcViewIncrementalFind;

    internal void ProcView_Find(ListView lvwTheView, bool incremental, long overrideStartPointIndex) {
        //Bilge.Warning("FindInProcessView not implemented properly, its slow as buggery and doesent support incremental or forwards");

        //Bilge.Assert(ActiveFind != null, "The active find is null, finds cannot be performed while there is no active find");
        //Bilge.Assert(!lvwTheView.InvokeRequired, "InvokeRequired in order to update this control from this thread.");

        bool matchMade = false;

        // Make the incremental search restart if non incremental and go from last cursor pos if incremental.
        if (!incremental) { lastglobalIndexForProcViewIncrementalFind = -1; }
        if (overrideStartPointIndex >= 0) { lastglobalIndexForProcViewIncrementalFind = overrideStartPointIndex; }

        foreach (ListViewItem lvi in lvwTheView.Items) {
            long eventEntryIndex = (long)lvi.Tag;

            if (incremental && eventEntryIndex <= lastglobalIndexForProcViewIncrementalFind) { continue; }

            var ta = MexCore.TheCore.DataManager.GetKnownApplication(SelectedTracedAppIdx);
            ta.EventEntries.EventEntriesRWL.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);
            try {
                int offset = ta.EventEntries.ContainsThisGlobalIdx(eventEntryIndex);
                //Bilge.Assert(offset >= 0, "Dont know how this is possible, the view appears to be out of synch with the data.");
                if (ActiveFind.MatchedEventEntry(ta.EventEntries[offset])) {
                    // Match
                    matchMade = true;
                    lastglobalIndexForProcViewIncrementalFind = ta.EventEntries[offset].GlobalIndex;

                    lvwTheView.BeginUpdate();
                    int idx = lvwTheView.Items.IndexOf(lvi);   //todo: perf!
                    lvwTheView.EnsureVisible(idx);
                    lvwTheView.Items[idx].Selected = true;
                    lvwTheView.EndUpdate();
                    lvwTheView.Select();

                    break;
                }
            } finally {
                ta.EventEntries.EventEntriesRWL.ReleaseReaderLock();
            }
        }
        if (!matchMade) {
            // TODO : Add user notification
            _ = MessageBox.Show("No match");
        }
    }

    /// <summary>
    /// Hides all of the event entries by making the last filtered result false.
    /// </summary>
    internal void MaskEventEntries() {
        var ta = MexCore.TheCore.DataManager.GetKnownApplication(SelectedTracedAppIdx);
        //Bilge.Assert(ta != null, " The index could not be found in the currently selected application.   You shouldnt mask events not in the current application");

        for (int i = 0; i < ta.EventEntries.Count; i++) {
            ta.EventEntries[i].LastVisitedFilterResult = false;
        }
    }

    /// <summary>
    /// Hides all of the event entries by making the last filtered result false.  This ripples through each event entry in the traced application
    /// until it hits the one with the global index specified.
    /// </summary>
    /// <param name="idx"></param>
    internal void MaskEventEntriesUpTillGlobalIndex(long idx) {
        var ta = MexCore.TheCore.DataManager.GetKnownApplication(SelectedTracedAppIdx);

        //Bilge.Assert(ta.EventEntries.ContainsThisGlobalIdx(idx) >= 0, " The index could not be found in the currently selected application.   You shouldnt mask events not in the current application");

        for (int i = 0; i < ta.EventEntries.Count; i++) {
            if (ta.EventEntries[i].GlobalIndex == idx) {
                break;
            }

            ta.EventEntries[i].LastVisitedFilterResult = false;
        }
    }

    internal void MaskEventEntryByGlobalIndex(long idx) {
        var ta = MexCore.TheCore.DataManager.GetKnownApplication(SelectedTracedAppIdx);
        int idxMatch = ta.EventEntries.ContainsThisGlobalIdx(idx);
        //Bilge.Assert(idxMatch >= 0, " The index could not be found in the currently selected application.   You shouldnt mask events not in the current application");
        ta.EventEntries[idxMatch].LastVisitedFilterResult = false;
    }

    internal int processViewPhysicalOffsetCache;

    internal void RefreshStatusText(Label lblTitle, Label lblText) {
        var ta = MexCore.TheCore.DataManager.GetKnownApplication(SelectedTracedAppIdx);
        if (ta == null) {
            //Bilge.Warning("Selected application is invalid, it is not possible to refresh at this point. ");
            SelectedTracedAppIdx = -1;
            return;
        }

        lblTitle.Text = ta.StatusText;
        lblText.Text = ta.StatusContents;
    }

    private TransientDisplay ConvertCudToDisplay(EventEntry ee) {
        var td = new TransientDisplay();
        if (ee.debugMessage.Contains("[GRID]")) {
            td.Text = ee.secondaryMessage.Replace("|", Environment.NewLine);
            td.Title = "Transient " + ee.TimeMessageRecieved.ToString();
            td.TType = TransientType.PrimaryDisplayText;
        } else if (ee.debugMessage.Contains("[STAT]")) {
            td.Title = ee.debugMessage[7..^1];
            td.Text = ee.secondaryMessage;
            td.TType = TransientType.StatusValue;
        }

        return td;
    }

    internal void RefreshView_Transient(TextBox text, ComboBox cboCustoms, ListBox lbxStatus, bool incremental) {
        try {
            var lbxVals = new Dictionary<string, string>();
            foreach (string f in lbxStatus.Items) {
                string[] s = f.ToString().Split(':');
                lbxVals.Add(s[0].Trim(), s[1]);
            }

            var ta = MexCore.TheCore.DataManager.GetKnownApplication(SelectedTracedAppIdx);
            if (ta == null) {
                //Bilge.Warning("Selected application is invalid, it is not possible to refresh at this point. ");
                SelectedTracedAppIdx = -1;
                return;
            }

            long lastIdx = 0;
            if (cboCustoms.Tag != null) {
                lastIdx = (long)cboCustoms.Tag;
            }

            if (!incremental) {
                cboCustoms.Items.Clear();
            }

            foreach (var cud in ta.GetCustomData()) {
                if (incremental && cud.GlobalIndex < lastIdx) {
                    // Skip previously done ones on incremental
                    continue;
                }
                var disp = ConvertCudToDisplay(cud);
                if (disp.TType == TransientType.PrimaryDisplayText) {
                    _ = cboCustoms.Items.Add(disp);
                } else if (disp.TType == TransientType.StatusValue) {
                    if (lbxVals.ContainsKey(disp.Title)) {
                        lbxVals[disp.Title] = disp.Text;
                    } else {
                        lbxVals.Add(disp.Title, disp.Text);
                    }
                }
                lastIdx = cud.GlobalIndex;
            }

            cboCustoms.Tag = lastIdx;

            if (cboCustoms.Items.Count > 0) {
                cboCustoms.SelectedIndex = incremental ? cboCustoms.Items.Count - 1 : 1;
            }

            lbxStatus.Items.Clear();
            foreach (string f in lbxVals.Keys) {
                _ = lbxStatus.Items.Add($"{f,10}:{lbxVals[f]}");
            }

            //var cd = ta.GetCustomData();
        } finally {
        }
    }

    /// <summary>
    /// This method will be called by the view to present us with a list view that should be populated with the event entries from the currently selected process in accordance
    /// with the currently selected filtering and selection rules.  The listview that is passed in will be populated with all of the event entries.
    /// </summary>
    /// <param name="lvwTheView">The Process specific list view to populate</param>
    /// <param name="incremental">Whether or not the refresh should be complete (incremental == false) or just append to the list view (incremental==true)</param>
    internal void RefreshView_Process(ListView lvwTheView, bool incremental, long selectedIndex) {
        #region entry code
        //Bilge.Assert(MexCore.TheCore.DataManager.IsValidTracedApplicationIndex(m_selectedTracedApp), "MexViewer::ViewSupportManager::RefreshProcessView >> The index specified byt the currently selected application is out of range.  RefreshProcessView is trying to refresh an invalid process.");
        #endregion entry code

        //Bilge.E("Refresh incremental = " + incremental.ToString());
        //Bilge.Log("Refreshing ProcessView", "New process View");

        // Used to populate the last view data field.
        long included = 0;
        long parsed = 0;
        int forceVisibleIndex = -1;

        //Bilge.Assert(!lvwTheView.InvokeRequired, "InvokeRequired in order to update this control from this thread.");

        ArrayList al;

        try {
            ListViewItem lvi;

            var ta = MexCore.TheCore.DataManager.GetKnownApplication(SelectedTracedAppIdx);
            if (ta == null) {
                //Bilge.Warning("Selected application is invalid, it is not possible to refresh at this point. ");
                SelectedTracedAppIdx = -1;
                return;
            }

            ta.EventEntries.EventEntriesRWL.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);

            try {
                //Bilge.Log(MexCore.TheCore.Options.DisplayInternalMessages ? "Interal messages are being displayed." : "Internal messages hidden.");
                //Bilge.Log(MexCore.TheCore.Options.RespectFilter ? "Filter is being respected." : "Filter is not being respected.");

                // Work out where to start the loop.
                int loopStartOffset;

                if (incremental) {
                    loopStartOffset = processViewPhysicalOffsetCache;
                } else {
                    loopStartOffset = 0;
                    lvwTheView.Items.Clear();
                }

                //Bilge.Log("Process View  refresh called for " + (ta.EventEntries.Count - loopStartOffset).ToString() + " elements.  If this is ever zero its a wasted refresh.");

                // Omg this is so fast, its even faster than an array for some reason.  Dont even need to set its default size.
                al = new ArrayList();

                // The main reason for storing these locally is to protect against options changes mid loop.  The options are not locked
                // as primarily they dont change very often and due to the fact they are controlled by the UI its almost impossible to have
                // a refresh and options change occuring at once.

                bool colsShowGlobalIndex = MexCore.TheCore.Options.ShowGlobalIndexInView;

                string[] subStringElements = new string[4];

                int ssidx;
                EventEntry ee;

                for (int loopCnt = loopStartOffset; loopCnt < ta.EventEntries.Count; loopCnt++) {
                    ee = ta.EventEntries[loopCnt];

                    if (ee.cmdType != TraceCommandTypes.Custom) {
                        if (MexCore.TheCore.Options.RespectFilter && (!CurrentFilter.IncludeThisEventEntry(ee))) { continue; }  // Skip messages based on filter
                        if (ViewFilter.EventEntryIsInternalType(ee) && (!MexCore.TheCore.Options.DisplayInternalMessages)) { continue; }  // Skip internal messages

                        if (MexCore.TheCore.Options.RemoveDuplicatesOnView && (loopCnt > 1)) {
                            // This is nasty.  It checks the last rendered event and sees if this is a copy, if it is then we dont display this
                            // one.  Its a useful but dangerous feature.  The >1 check is to ensure that we dont try and check for the first entry.

                            int previousEntry = loopCnt - 1;
                            if (ta.EventEntries[previousEntry].Equals(ee)) {
                                // Do not display as we have found a duplicate
                                continue;
                            }
                        }
                    }

                    //updateOccured = true;
                    #region Cancellation support code
                    if (MexCore.TheCore.Options.SupportCancellationOfRefresh) {
                        Application.DoEvents();

                        if (CancelCurrentViewOperation) {
                            //Bilge.Log("View operation canceled due to user request.");
                            LastViewSummary = "Cancel Requested";
                            CancelCurrentViewOperation = false;
                            return;
                        }
                    }
                    #endregion Cancellation support code

                    // Now look which columns are displayed and put the data in the list view item.
                    ssidx = 0;

                    subStringElements[ssidx++] = colsShowGlobalIndex ? ee.GlobalIndex.ToString() : string.Empty;

                    subStringElements[ssidx++] = ta.GetThreadDisplayValue(ee).DisplayIdentity;
                    subStringElements[ssidx++] = ee.moreLocationData;

                    subStringElements[ssidx++] = BeautifyDebugMessage(ee.debugMessage);

                    lvi = new ListViewItem(subStringElements, ImageIndexFromEventType(ee.cmdType)) {
                        Tag = ee.GlobalIndex
                    };

                    // Now check whether color is being used for highlights.
                    if (ee.viewData.isValid && ee.viewData.isHighlighted) {
                        if (ee.viewData.isBackgroundHighlighted) {
                            lvi.BackColor = ee.viewData.backgroundHighlightColor;
                        }
                        if (ee.viewData.isForegroundHighlighted) {
                            lvi.ForeColor = ee.viewData.foregroundHighlightColor;
                        }
                    }

                    int indexedValue = al.Add(lvi);

                    if ((selectedIndex >= 0) && (ee.GlobalIndex == selectedIndex)) {
                        lvi.Selected = true;
                        forceVisibleIndex = indexedValue;
                    }
                }

                //if (updateOccured) {  // This is necessary otherwise when it skips the loop it sets the last cache back to 0
                processViewPhysicalOffsetCache = ta.EventEntries.Count;
                // }
            } finally {
                ta.EventEntries.EventEntriesRWL.ReleaseReaderLock();
            }

            if (al.Count == 0) { return; } // There was nothing to do therefore return.  If we dont do this the refresh below causes flicker.

            lvwTheView.Items.AddRange((ListViewItem[])al.ToArray(typeof(ListViewItem)));

            if (MexCore.TheCore.Options.AutoScroll) {
                if (lvwTheView.Items.Count > 10) {
                    // TODO : Do this properly, should be > than the no of rows that are currently visible
                    lvwTheView.EnsureVisible(lvwTheView.Items.Count - 1);
                }
            }
            // TODO :        SetLastViewSummary(parsed, included);
            LastViewSummary = "Refreshed, displaying " + included.ToString() + " of " + parsed.ToString();
            if (forceVisibleIndex >= 0) {
                lvwTheView.EnsureVisible(forceVisibleIndex);
            }
        } finally {
            lvwTheView.EndUpdate();
        }

        //Bilge.X();
    } // End MexViewer::ViewSupportManager::RefreshProcessView

    internal void RefreshView_CrossProcess(ListView lvwDisplay, int[] virtualIndexedPids, bool incremental, long forceVisibleIndex) {
        //Bilge.E(string.Format("Refreshing {0} processes, incremental is {1} ", virtualIndexedPids.Length.ToString(), incremental.ToString()));
        int indexToForce = -1;

        #region entry code

        //Bilge.Assert(!lvwDisplay.InvokeRequired, "InvokeRequired in order to update this control from this thread.");

#if DEBUG
        foreach (int i in virtualIndexedPids) {
            //Bilge.Assert(MexCore.TheCore.DataManager.IsValidTracedApplicationIndex(i), "MexViewer::ViewSupportManager::RefreshCrossProcessView >> The index specified byt the currently selected application is out of range.  RefreshCrossProcessView is trying to refresh an invalid process.");
        }
#endif
        #endregion entry code

        var al = new ArrayList();
        lvwDisplay.BeginUpdate();
        lvwDisplay.Items.Clear();

        try {
            ListViewItem lvi;

            for (int appCount = 0, nextApp = virtualIndexedPids[0]; appCount < virtualIndexedPids.Length; appCount++) {
                nextApp = virtualIndexedPids[appCount];

                var ta = MexCore.TheCore.DataManager.GetKnownApplication(nextApp);
                ta.EventEntries.EventEntriesRWL.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);

                try {
                    //Bilge.Log(MexCore.TheCore.Options.RespectFilter ? "Filter is being respected." : "Filter is not being respected.");

                    if (incremental) {
                        //Bilge.Warning("Incremental not currently supported for cross process view.");
                    }

                    // Omg this is so fast, its even faster than an array for some reason.  Dont even need to set its default size.

                    // The main reason for storing these locally is to protect against options changes mid loop.  The options are not locked
                    // as primarily they dont change very often and due to the fact they are controlled by the UI its almost impossible to have
                    // a refresh and options change occuring at once.

                    bool colsShowGlobalIndex = MexCore.TheCore.Options.ShowGlobalIndexInView;

                    string[] subStringElements = new string[5];

                    int ssidx;
                    EventEntry ee;

                    for (int loopCnt = 0; loopCnt < ta.EventEntries.Count; loopCnt++) {
                        ee = ta.EventEntries[loopCnt];

                        if (ViewFilter.EventEntryIsInternalType(ee) && (!MexCore.TheCore.Options.DisplayInternalMessages)) { continue; }  // Skip internal messages
                        if (MexCore.TheCore.Options.RespectFilter && (!CurrentFilter.IncludeThisEventEntry(ee))) { continue; }  // Skip messages based on filter

                        // Now look which columns are displayed and put the data in the list view item.
                        ssidx = 0;

                        subStringElements[ssidx++] = colsShowGlobalIndex ? ee.GlobalIndex.ToString() : string.Empty;

                        string thisprocident = ta.MachineName + "\\" + ta.ProcessIdAsString;
                        subStringElements[ssidx++] = thisprocident;
                        subStringElements[ssidx++] = ta.GetThreadDisplayValue(ee).DisplayIdentity;
                        subStringElements[ssidx++] = ee.moreLocationData;

                        subStringElements[ssidx++] = BeautifyDebugMessage(ee.debugMessage);

                        lvi = new ListViewItem(subStringElements, ImageIndexFromEventType(ee.cmdType)) {
                            Tag = ee.GlobalIndex
                        };

                        if (MexCore.TheCore.Options.CrossProcessViewHighlight) {
                            // The cross process view highlight option overrides the default highlight options.
                            lvi.BackColor = GetColorByIndex(appCount);
                        } else {
                            // Now check whether color is being used for highlights.
                            if (ee.viewData.isValid && ee.viewData.isHighlighted) {
                                if (ee.viewData.isBackgroundHighlighted) {
                                    lvi.BackColor = ee.viewData.backgroundHighlightColor;
                                }
                                if (ee.viewData.isForegroundHighlighted) {
                                    lvi.ForeColor = ee.viewData.foregroundHighlightColor;
                                }
                            }
                        }

                        int index = al.Add(lvi);
                        if (forceVisibleIndex >= 0) {
                            if (ee.GlobalIndex == forceVisibleIndex) {
                                indexToForce = index;
                                lvi.Selected = true;
                            }
                        }
                    }  // End for each event entry in selected app
                } finally {
                    ta.EventEntries.EventEntriesRWL.ReleaseReaderLock();
                }
            } // End foreach of the virtual indexes

            // Rather than using the overly complex visit each element we just visit the ones we know and then
            // do a sort on it.  This should be much faster.
            al.Sort(new SortListViewItemsByGIdx());
            lvwDisplay.Items.AddRange((ListViewItem[])al.ToArray(typeof(ListViewItem)));

            if (MexCore.TheCore.Options.AutoScroll) {
                if (lvwDisplay.Items.Count > 10) {
                    // TODO : Do this properly, should be > than the no of rows that are currently visible
                    lvwDisplay.EnsureVisible(lvwDisplay.Items.Count - 1);
                }
            }

            if (indexToForce >= 0) {
                //lvwDisplay.SelectedIndices.Add(indexToForce);
                lvwDisplay.EnsureVisible(indexToForce);
            }
        } finally {
            lvwDisplay.EndUpdate();
        }

        //Bilge.X();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
    private Color GetColorByIndex(int p) {
        return p switch {
            0 => Color.PaleGoldenrod,
            1 => Color.PaleGreen,
            2 => Color.PeachPuff,
            3 => Color.BlanchedAlmond,
            4 => Color.LightCoral,
            5 => Color.LightSteelBlue,
            6 => Color.LightSalmon,
            7 => Color.LightGray,
            8 => Color.IndianRed,
            9 => Color.Honeydew,
            10 => Color.Aqua,
            _ => Color.White,
        };
    }

    /// <summary>
    /// Sets the friendly name associated with the current process to the string passed as the parameter.
    /// </summary>
    /// <param name="newName">The name describing the process</param>
    internal void SetNameOfCurrentProcess(string newName) {
        if (SelectedTracedAppIdx > 0) {
            var ta = MexCore.TheCore.DataManager.GetKnownApplication(SelectedTracedAppIdx);
            // Its possible that the traced application that is in the view no longer exists.  Its unlikely, but possible.
            if (ta != null) {
                ta.ProcessLabel = newName;
            }
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
    internal int GetOffsetOfThisString(string[] theArray, string thisText) {
        for (int lc = 0; lc < theArray.Length; lc++) {
            if (theArray[lc] == thisText) {
                return lc;
            }
        }
        return -1;
    }

    /// <summary>
    /// returns an array of strings where each string contains the name identifier for a thread in the selected list of threads
    /// </summary>
    /// <returns>array list of thread names</returns>
    internal List<KeyDisplayRepresentation> GetThreadNameListForSelectedApp() {
        var kdrs = new List<KeyDisplayRepresentation>();

        var ta = MexCore.TheCore.DataManager.GetKnownApplication(SelectedTracedAppIdx);
        if (ta != null) {
            // If you hit the filter while there is not an active process then you need to not specfiy any threads etc.

            ta.EventEntries.EventEntriesRWL.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);
            //Bilge.ResourceGrab(ta.EventEntries.EventEntriesRWL, "EventEntriesRWL");
            try {
                foreach (EventEntry ee in ta.EventEntries) {
                    var next = ta.GetThreadDisplayValue(ee);

                    if (!kdrs.Contains(next)) {
                        kdrs.Add(next);
                    }
                }
            } finally {
                //Bilge.ResourceFree(ta.EventEntries.EventEntriesRWL, "EventEntriesRWL");
                ta.EventEntries.EventEntriesRWL.ReleaseReaderLock();
            }
        }
        return kdrs;
    }

    #region Source View Support Functions

    internal void RefreshView_SourceSynch(ListView lvwSourceView, string threadFilter) {
        lvwSourceView.BeginUpdate();
        try {
            lvwSourceView.Columns.Clear();
            lvwSourceView.Items.Clear();
            _ = lvwSourceView.Columns.Add("Index");

            var ta = MexCore.TheCore.DataManager.GetKnownApplication(SelectedTracedAppIdx);

            ta.EventEntries.EventEntriesRWL.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);
            //Bilge.ResourceGrab(ta.EventEntries.EventEntriesRWL, "EventEntriesRWL");
            try {
                foreach (EventEntry ee in ta.EventEntries) {
                    if (MexCore.TheCore.Options.RespectFilter && (!CurrentFilter.IncludeThisEventEntry(ee))) { continue; }

                    #region Cancellation support code
                    if (MexCore.TheCore.Options.SupportCancellationOfRefresh) {
                        Application.DoEvents();

                        if (CancelCurrentViewOperation) {
                            //Bilge.Log("View operation canceled due to user request.");
                            LastViewSummary = "Cancel Requested";
                            CancelCurrentViewOperation = false;
                            return;
                        }
                    }
                    #endregion Cancellation support code

                    if (ee.CurrentThreadKey != threadFilter) { continue; }

                    var lvi = new ListViewItem(ee.GlobalIndex.ToString()) {
                        Tag = ee.GlobalIndex,
                        ImageIndex = ImageIndexFromEventType(ee.cmdType)
                    };

                    // Now check whether color is being used for highlights.
                    if (ee.viewData.isValid && ee.viewData.isHighlighted) {
                        if (ee.viewData.isBackgroundHighlighted) {
                            lvi.BackColor = ee.viewData.backgroundHighlightColor;
                        }
                        if (ee.viewData.isForegroundHighlighted) {
                            lvi.ForeColor = ee.viewData.foregroundHighlightColor;
                        }
                    }

                    _ = lvwSourceView.Items.Add(lvi);
                }
            } finally {
                //Bilge.ResourceFree(ta.EventEntries.EventEntriesRWL, "EventEntriesRWL");
                ta.EventEntries.EventEntriesRWL.ReleaseReaderLock();
            }
            lvwSourceView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        } finally {
            lvwSourceView.EndUpdate();
        }
    }

    #endregion Source View Support Functions

    #region Process Thread View Support Functions

    internal void RefreshView_ProcessThread(ListView lvwDrawThreads, List<KeyDisplayRepresentation> includeTheseThreads, bool synch) {
        #region entry code
        //Bilge.Assert(includeTheseThreads.Count > 0, "You cant cask to draw a thread view specifying no threads.  UI Should be disabled to prevent this");
        //Bilge.Assert(lvwDrawThreads != null, "You must specify a valid panel to draw the threads listboxes on.  UI should not allow this");
        if (includeTheseThreads.Count == 0) { return; }
        #endregion entry code

        const int COLUMNWIDTH_INDEX = 50;
        const int BOARDERWIDTH = 16;

        int availableWidth = lvwDrawThreads.Width - (BOARDERWIDTH + COLUMNWIDTH_INDEX);
        lvwDrawThreads.BeginUpdate();
        try {
            lvwDrawThreads.Columns.Clear();
            lvwDrawThreads.Items.Clear();
            _ = lvwDrawThreads.Columns.Add("Index", "Index");
            lvwDrawThreads.Columns["Index"].Width = COLUMNWIDTH_INDEX;

            var ta = MexCore.TheCore.DataManager.GetKnownApplication(SelectedTracedAppIdx);

            ta.EventEntries.EventEntriesRWL.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);
            //Bilge.ResourceGrab(ta.EventEntries.EventEntriesRWL, "EventEntriesRWL");
            try {
                int threadColumnWidth = availableWidth / includeTheseThreads.Count;
                if (threadColumnWidth < 50) {
                    threadColumnWidth = 50;
                }

                for (int i = 0; i < includeTheseThreads.Count; i++) {
                    // For each thread add a listview column
                    string name = ta.ThreadNames.ContainsKey(includeTheseThreads[i].KeyIdentity)
                        ? ta.ThreadNames[includeTheseThreads[i].KeyIdentity]
                        : includeTheseThreads[i].DisplayIdentity;
                    _ = lvwDrawThreads.Columns.Add(name, name);
                    lvwDrawThreads.Columns[name].Width = threadColumnWidth;
                }

                foreach (EventEntry ee in ta.EventEntries) {
                    if (MexCore.TheCore.Options.RespectFilter && (!CurrentFilter.IncludeThisEventEntry(ee))) { continue; }

                    #region Cancellation support code
                    if (MexCore.TheCore.Options.SupportCancellationOfRefresh) {
                        Application.DoEvents();

                        if (CancelCurrentViewOperation) {
                            //Bilge.Log("View operation canceled due to user request.");
                            LastViewSummary = "Cancel Requested";
                            CancelCurrentViewOperation = false;
                            return;
                        }
                    }
                    #endregion Cancellation support code

                    int columnMatchIndex = -1;

                    for (int idx = 0; idx < includeTheseThreads.Count; idx++) {
                        if (ee.CurrentThreadKey == includeTheseThreads[idx].KeyIdentity) {
                            columnMatchIndex = idx;
                            break;
                        }
                    }
                    if ((!synch) && (columnMatchIndex == -1)) { continue; }
                    var lvi = new ListViewItem(ee.GlobalIndex.ToString()) {
                        Tag = ee.GlobalIndex,
                        ImageIndex = ImageIndexFromEventType(ee.cmdType)
                    };

                    // Now check whether color is being used for highlights.
                    if (ee.viewData.isValid && ee.viewData.isHighlighted) {
                        if (ee.viewData.isBackgroundHighlighted) {
                            lvi.BackColor = ee.viewData.backgroundHighlightColor;
                        }
                        if (ee.viewData.isForegroundHighlighted) {
                            lvi.ForeColor = ee.viewData.foregroundHighlightColor;
                        }
                    }

                    for (int idx = 0; idx < includeTheseThreads.Count; idx++) {
                        _ = idx == columnMatchIndex ? lvi.SubItems.Add(BeautifyDebugMessage(ee.debugMessage)) : lvi.SubItems.Add("");
                    }

                    _ = lvwDrawThreads.Items.Add(lvi);
                }
            } finally {
                //Bilge.ResourceFree(ta.EventEntries.EventEntriesRWL, "EventEntriesRWL");
                ta.EventEntries.EventEntriesRWL.ReleaseReaderLock();
            }
            // Used to do this  lvwDrawThreads.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            // but that only resized to the content not the window, therefore specifically sized the columns to the window
        } finally {
            lvwDrawThreads.EndUpdate();
        }
    }

    internal Tuple<string, byte[]> GetBlobData(long idx) {
        var ta = MexCore.TheCore.DataManager.GetKnownApplication(SelectedTracedAppIdx);
        ta.EventEntries.EventEntriesRWL.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);
        //Bilge.ResourceGrab(ta.EventEntries.EventEntriesRWL, "EventEntriesRWL");

        int eeidx = ta.FindEventIdxByGlobalIndex_NL(idx);
        var ee = ta.EventEntries[eeidx];
        byte[] bts = MexCore.TheCore.DataManager.largeObjectStore[ee.secondaryMessage].Item2;
        return new Tuple<string, byte[]>(ee.debugMessage, bts);
    }

#if false
    internal void RefreshView_ProcessThread(Panel pnlDrawArea, List<KeyDisplayRepresentation> includeTheseThreads, bool synch, bool incremental) {
    #region entry code
        //Bilge.Assert(includeTheseThreads.Count > 0, "You cant cask to draw a thread view specifying no threads.  UI Should be disabled to prevent this");
        //Bilge.Assert(pnlDrawArea != null, "You must specify a valid panel to draw the threads listboxes on.  UI should not allow this");

        if (includeTheseThreads.Count == 0) { return; }
    #endregion entry code

    #region constants for spacing the boxes on the panel
        const int PIX_BETWEEN_THREAD_BOXES = 2;
        const int PIX_SPARE_AT_TOP = 2;
        const int PIX_SPARE_AT_BOTTOM = 2;
        const int PIX_SPARE_AT_LEFT = 2;
        const int PIX_SPARE_AT_RIGHT = 2;
    #endregion constants for spacing the boxes on the panel

        // TODO : implement incremental
        pnlDrawArea.Controls.Clear();

        int i = -1;  // loop counter
        ListBox[] lbArray = new ListBox[includeTheseThreads.Count];

        int lbxThreadViewWidth = (((pnlDrawArea.Width - (PIX_SPARE_AT_LEFT + PIX_SPARE_AT_RIGHT)) / includeTheseThreads.Count) - (includeTheseThreads.Count * PIX_BETWEEN_THREAD_BOXES));

        // Create the listbox controls ready for the thread info to be added in
        for (i = 0; i < includeTheseThreads.Count; i++) {
            lbArray[i] = new ListBox();
            lbArray[i].SuspendLayout();  // stop it messing around while were sorting it out
            lbArray[i].BeginUpdate();
            lbArray[i].Top = PIX_SPARE_AT_TOP;
            lbArray[i].Left = PIX_SPARE_AT_LEFT + ((i * PIX_BETWEEN_THREAD_BOXES) + (i * lbxThreadViewWidth));
            lbArray[i].Height = pnlDrawArea.Height - (PIX_SPARE_AT_TOP + PIX_SPARE_AT_BOTTOM);
            lbArray[i].Width = lbxThreadViewWidth;
            lbArray[i].Parent = pnlDrawArea;
        }

        // This isnt really right - the try / finally should be applying the end update only to those that succeeded
        try {
            TracedApplication ta = MexCore.TheCore.DataManager.GetKnownApplication(m_selectedTracedApp);
            ta.EventEntries.EventEntriesRWL.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);
           //Bilge.ResourceGrab(ta.EventEntries.EventEntriesRWL, "EventEntriesRWL");
            try {
                foreach (EventEntry ee in ta.EventEntries) {
                    if ((MexCore.TheCore.Options.RespectFilter) && (!CurrentFilter.IncludeThisEventEntry(ee))) { continue; }

    #region Cancellation support code
                    if (MexCore.TheCore.Options.SupportCancellationOfRefresh) {
                        Application.DoEvents();

                        if (CancelCurrentViewOperation) {
                           //Bilge.Log("View operation canceled due to user request.");
                            this.LastViewSummary = "Cancel Requested";
                            this.CancelCurrentViewOperation = false;
                            return;
                        }
                    }
    #endregion Cancellation support code

                    for (int idx = 0; idx < includeTheseThreads.Count; idx++) {
                        if (ee.CurrentThreadKey == includeTheseThreads[idx].KeyIdentity) {
                            // We have found the matched thread for this entry.
                            lbArray[idx].Items.Add(ee.GlobalIndex + " " + BeautifyDebugMessage(ee.DebugMessage));
                            break;
                        } else if (synch) {
                            // If the thread was not matched by we are synchroising the thread display
                            // then add a blank line to all of the other monitored threads
                            lbArray[idx].Items.Add("");
                        }
                    }
                }
            } finally {
               //Bilge.ResourceFree(ta.EventEntries.EventEntriesRWL, "EventEntriesRWL");
                ta.EventEntries.EventEntriesRWL.ReleaseReaderLock();
            }
        } finally {
            // Now display all of the modified listbox controls
            for (i = 0; i < includeTheseThreads.Count; i++) {
                lbArray[i].Visible = true;
                lbArray[i].ResumeLayout(true);
                lbArray[i].EndUpdate();
            }
        }
    }
#endif
    #endregion Process Thread View Support Functions

    #region Main View Screen Support Functions

    private long lastUsedCountOfsetForMain;

    /// <summary>
    /// This method will be called by the view to present us with a list view that should be populated iwth the event entries from the main ODS output.  This includes some
    /// crossover events and any events that are not recognised as traced applications.
    /// </summary>
    /// <param name="lvwTheMainView">The list view to populate</param>
    /// <param name="incremental">Whether or not this should be an incremental update</param>
    internal void RefreshView_ODS(ListView lvwTheMainView, bool incremental) {
        long parsed = 0;
        long displayed = 0;

        var entriesToAdd = new ArrayList();

        //lvwTheMainView.BeginUpdate();

        MexCore.TheCore.DataManager.NonTracedApplicationEntries.NonTracedApplicationsDataRWL.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);
        //Bilge.ResourceGrab(MexCore.TheCore.DataManager.NonTracedApplicationEntries.NonTracedApplicationsDataRWL, "NonTracedApplicationsDataRWL");
        try {
            ListViewItem lvi;

            long startingIdx;

            if (!incremental) {
                startingIdx = 0;
                lvwTheMainView.Items.Clear();
            } else {
                startingIdx = lastUsedCountOfsetForMain;
            }

            for (long loop = startingIdx; loop < MexCore.TheCore.DataManager.NonTracedApplicationEntries.Count; loop++) {
                var nta = MexCore.TheCore.DataManager.NonTracedApplicationEntries[(int)loop];

                if (MexCore.TheCore.Options.RespectFilter && (!CurrentFilter.IncludeThisNonEventEntry(nta))) { continue; }
                lvi = new ListViewItem(new string[] { nta.assignedIndex.ToString(), nta.Pid.ToString(), nta.DebugEntry }) {
                    Tag = nta.assignedIndex
                };

                if (nta.viewData.isValid && nta.viewData.isHighlighted) {
                    if (nta.viewData.isBackgroundHighlighted) {
                        lvi.BackColor = nta.viewData.backgroundHighlightColor;
                    }
                    if (nta.viewData.isForegroundHighlighted) {
                        lvi.ForeColor = nta.viewData.foregroundHighlightColor;
                    }
                }
                _ = entriesToAdd.Add(lvi);
            } // End foreach

            // Important we gather the length before unlocking it
            lastUsedCountOfsetForMain = MexCore.TheCore.DataManager.NonTracedApplicationEntries.Count;
        } finally {
            //Bilge.ResourceFree(MexCore.TheCore.DataManager.NonTracedApplicationEntries.NonTracedApplicationsDataRWL, "NonTracedApplicationsDataRWL");
            MexCore.TheCore.DataManager.NonTracedApplicationEntries.NonTracedApplicationsDataRWL.ReleaseReaderLock();
        }

        lvwTheMainView.Items.AddRange((ListViewItem[])entriesToAdd.ToArray(typeof(ListViewItem)));

        if (MexCore.TheCore.Options.AutoScroll) {
            if (lvwTheMainView.Items.Count > 10) {  // TODO: Should be rows visible
                lvwTheMainView.EnsureVisible(lvwTheMainView.Items.Count - 1);
            }
        }

        //lvwTheMainView.EndUpdate();
        LastViewSummary = "View[ " + displayed.ToString() + " of " + parsed.ToString() + "]";
    }

    #endregion Main View Screen Support Functions

    #region Resource View Screen Support Functions

    /// <summary>
    /// Will return the resource counts by global index up to the index that is passed in as a parameter for the currently selected application.
    /// This will return an array, each element of which has an array of the variance of the resource consumption during the life of the app.
    /// Currently this always starts at the beginning of the apps life.
    /// </summary>
    /// <param name="idx">The global index to stop looking at</param>
    /// <returns>Array of resource consumption structs</returns>
    internal ResourceProfile[] GetAppResourceProfileToIndex(long idx) {
        var ht = new Hashtable();  // use this to build up the results

        var ta = MexCore.TheCore.DataManager.GetKnownApplication(SelectedTracedAppIdx);
        ta.EventEntries.EventEntriesRWL.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);
        //Bilge.ResourceGrab(ta.EventEntries.EventEntriesRWL, "EventEntriesRWL");
        try {
            foreach (EventEntry ee in ta.EventEntries) {
                if (ee.GlobalIndex > idx) {
                    // TODO : Strip this out or use it.
                    throw new InvalidOperationException("Hmmm was not expecting this check it out");
                }
                if (ee.cmdType != TraceCommandTypes.ResourceCount) { continue; }

                var resByIdx = GetResourceCounts(ee);
                foreach (var nvp in resByIdx) {
                    if (!ht.Contains(nvp.Name)) {
                        ht.Add(nvp.Name, new ArrayList());
                    }

                    _ = (ht[nvp.Name] as ArrayList).Add(new ValueIdxPair(ee.GlobalIndex, nvp.Value));
                } // end foreach nvp
            } // end foreach event entry
        } finally {
            //Bilge.ResourceFree(ta.EventEntries.EventEntriesRWL, "EventEntriesRWL");
            ta.EventEntries.EventEntriesRWL.ReleaseReaderLock();
        }

        // Hashtable now looks like this ht[resname] = { [gidx,val],[gidx,val],[gidx,val] }
        var result = new ResourceProfile[ht.Count];
        int resProfileLoopCount = 0;

        foreach (string s in ht.Keys) {
            result[resProfileLoopCount] = new ResourceProfile {
                Name = s,
                Consumption = (ValueIdxPair[])((ArrayList)ht[s]).ToArray(typeof(ValueIdxPair)),
                // now run through the array finding lowest and highest

                HighestValue = long.MaxValue,
                LowestValue = long.MinValue
            };

            for (int i = 0; i < result[resProfileLoopCount].Consumption.Length; i++) {
                if (result[resProfileLoopCount].Consumption[i].Value < result[resProfileLoopCount].LowestValue) { result[resProfileLoopCount].LowestValue = result[resProfileLoopCount].Consumption[i].Value; }
                if (result[resProfileLoopCount].Consumption[i].Value > result[resProfileLoopCount].HighestValue) { result[resProfileLoopCount].HighestValue = result[resProfileLoopCount].Consumption[i].Value; }
            }
            resProfileLoopCount++;
        } // end foreach key

        return result;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
    private NameValuePair[] GetResourceCounts(EventEntry ee) {
        //Bilge.Assert(false);
        string splitter = ee.debugMessage;

        if ((splitter.Length == 0) || (splitter == null) || (splitter.IndexOf(';') < 0)) { return new NameValuePair[0]; }
        if (splitter.EndsWith(";")) { splitter = splitter[..^1]; }

        string[] working = splitter.Split(';');

        var result = new NameValuePair[working.Length];
        for (int i = 0; i < working.Length; i++) {
            result[i] = new NameValuePair(working[i]);
        }
        return result;
    }

    /// <summary>
    /// Returns the name value pairings for the resource consumption at a specific global index.b
    ///
    /// </summary>
    /// <param name="globalIndex"></param>
    /// <returns></returns>
    internal NameValuePair[] GetResourceCountsByIndex(long globalIndex) {
        return GetResourceCounts(MexCore.TheCore.DataManager.GetKnownApplication(SelectedTracedAppIdx).FindEventByGlobalIndex(globalIndex));
    }

    #region Timing Data Support Functions

    /// <summary>
    /// Parses a specific entry looking for timing data. The entry must either be a section start or end command.  If it is a valid section start or end with
    /// valid timing data then the entry will be added to either the partials if its an unmatched enter or the instances if its a matched exit.
    /// This method does not have any knowledge of threads, this must be done before this method is called, therefore the stack of existing partials must be exactly that which
    /// is running on one thread.
    /// </summary>
    /// <param name="monitoredTimerInstances">The list of completed time entries</param>
    /// <param name="existingPartials">The list of unmatched enters</param>
    /// <param name="ee">The event entry to check</param>
    /// <returns>True if the parse was fine, false if there was an error and therefore timings are mismatched.</returns>
    private static bool ParseEntryForTimingData(Dictionary<string, List<TimeInstanceView>> monitoredTimerInstances, List<TimeInstanceView> existingPartials, EventEntry ee) {
        #region entry code
        //Bilge.Assert(ee != null, "eventEntry is null");
        //Bilge.Assert((ee.cmdType == TraceCommandTypes.SectionStart) || (ee.cmdType == TraceCommandTypes.SectionEnd), "Parse entry for timing data is only valid on section starts and ends.");
        //Bilge.Assert(monitoredTimerInstances != null, "timer instances is null");
        //Bilge.Assert(existingPartials != null, "partials are null");
        #endregion entry code

        bool result = true;

        if (ee.SupportingData is not TimerInstanceData tid) { return true; }  // This section did not support timing data therefore theres nothing we can do.

        TimeInstanceView tiv;

        if (ee.cmdType == TraceCommandTypes.SectionStart) {
            tiv = new TimeInstanceView {
                SinkName = tid.timeSinkName,
                InstanceName = tid.timeSinkInstanceDescription,
                EnterTime = tid.timeInstance
            };

            existingPartials.Add(tiv);
            return true;
        }

        if (ee.cmdType == TraceCommandTypes.SectionEnd) {
            // We are basically hoping that the exit matches the enter.

            int matchMade;
            for (matchMade = 0; matchMade < existingPartials.Count; matchMade++) {
                tiv = existingPartials[matchMade];
                if ((tid.timeSinkName == tiv.SinkName) && (tid.timeSinkInstanceDescription == tiv.InstanceName)) {
                    tiv.ExitTime = tid.timeInstance;
                    if (!monitoredTimerInstances.ContainsKey(tiv.SinkName)) {
                        monitoredTimerInstances.Add(tiv.SinkName, new List<TimeInstanceView>());
                    }

                    monitoredTimerInstances[tiv.SinkName].Add(tiv);
                    existingPartials.RemoveAt(matchMade);
                    return true;
                }
            } // End foreach of the partials

            //Bilge.Warning("Failed to match on partial");
        }

        return result;
    }

    /// <summary>
    /// Render all of the timings for the timings report.  The work which has been done so far has identified each of the
    /// timings in the trace stream and placed them into a category structure called monitoredTimerinstances which is passed
    /// to this method to render into a tree view.
    /// </summary>
    /// <param name="tvwTimingsReport">The timing control</param>
    /// <param name="monitoredTimerInstances">The instances of matched ones</param>
    /// <param name="partialMatchedEntries"></param>
    private static void RenderTimings(TreeView tvwTimingsReport, Dictionary<string, List<TimeInstanceView>> monitoredTimerInstances, Dictionary<string, List<TimeInstanceView>> partialMatchedEntries, AdditionalData additional) {
        var baseNode = new TreeNode("Timings") {
            Name = "bnMatched"
        };
        TreeNode categoryNode;
        TreeNode instanceNode;
        TreeNode detailNodes;

        tvwTimingsReport.BeginUpdate();
        try {
            tvwTimingsReport.Nodes.Clear();

            _ = tvwTimingsReport.Nodes.Add(baseNode);

            foreach (string s in monitoredTimerInstances.Keys) {
                var ltiv = monitoredTimerInstances[s];
                categoryNode = new TreeNode();  // Timings  /  Category  << this is the category being created.
                var totalCategoryElapsed = new TimeSpan();
                var nodesStashed = new List<TreeNode>();
                int filteredNodeCount = 0;

                // Now do the instances.
                foreach (var tiv in ltiv) {
                    bool autoInstance = false;
                    string thisInstanceName = tiv.InstanceName;

                    if (thisInstanceName.StartsWith(Constants.AUTOTIMER_PREFIX)) {
                        thisInstanceName = thisInstanceName[Constants.AUTOTIMER_PREFIX.Length..] + " (Auto)";
                        autoInstance = true;
                    }

                    instanceNode = new TreeNode();

                    var elapsedForInstance = TimeSpan.MinValue;

                    try {
                        elapsedForInstance = tiv.ExitTime - tiv.EnterTime;

                        if ((additional != null) && additional.UseExclusionFilters) {
                            // Additional allows you to skip certian elements from the render, unfortunately we need to
                            // run through and calculate the elapsed before we can skip them therefore this is done quite
                            // late in the process.
                            if ((elapsedForInstance.TotalMilliseconds < additional.ExcludeTimingsLessThan) || (elapsedForInstance.TotalMilliseconds > additional.ExcludeTimingsGreaterThan)) {
                                filteredNodeCount++;
                                continue;
                            }
                        }

                        // Coloring is done either as grey for an auto instance or in a gradient color for the rest
                        // to do this the nodes are added to a temporary list where they can be sorted and colored.
                        if (autoInstance) {
                            // Auto instances are generated by Tex itself.  These are not color graded
                            instanceNode.BackColor = Color.FromKnownColor(KnownColor.Gainsboro);
                        } else {
                            nodesStashed.Add(instanceNode);
                        }

                        totalCategoryElapsed += elapsedForInstance;
                        instanceNode.Tag = elapsedForInstance.TotalMilliseconds;
                        detailNodes = new TreeNode("Elapsed:" + elapsedForInstance.TotalMilliseconds.ToString() + " (ms)");
                    } catch (OverflowException) {
                        // Happens when one half of the timing is missing.
                        detailNodes = new TreeNode("Elapsed: Overflow exception trying to calculate elapsed.");
                    }

                    _ = instanceNode.Nodes.Add(detailNodes);
                    detailNodes = new TreeNode("Enter:" + tiv.EnterTime.ToString("HH:mm:ss:FFF"));
                    _ = instanceNode.Nodes.Add(detailNodes);
                    detailNodes = new TreeNode("Exit:" + tiv.ExitTime.ToString("HH:mm:ss:FFF"));
                    _ = instanceNode.Nodes.Add(detailNodes);

                    instanceNode.Text = thisInstanceName + " " + elapsedForInstance.TotalSeconds.ToString() + " (s)";
                    _ = categoryNode.Nodes.Add(instanceNode);
                }

                // Only apply color gradients to more than 2 nodes.
                string additionalCatText = string.Empty;

                if (nodesStashed.Count > 2) {
                    nodesStashed.Sort(new Comparison<TreeNode>(ViewSupportManager.CompareNodesByTag));
                    double running = 0;
                    double maxMili = (double)nodesStashed[^1].Tag;
                    double minMili = (double)nodesStashed[0].Tag;
                    double range = maxMili - minMili;

                    if (filteredNodeCount > 0) {
                        // If we have used "additional" to filter out nodes we count how many here.
                        additionalCatText = "(" + filteredNodeCount.ToString() + " filtered) ";
                    }

                    additionalCatText += " Min[ " + minMili.ToString() + "ms ] Max[ " + maxMili.ToString() + "ms ] Avg: [";
                    for (int i = 0; i < nodesStashed.Count; i++) {
                        double current = (double)nodesStashed[i].Tag;
                        running += current;
                        double working = (current - minMili) / range;
                        working *= 100;
                        int percentageColor = (int)working;

                        nodesStashed[i].BackColor = UIHelperRoutines.GetRedGreenRangeByPercentile(percentageColor);
                    }
                    running /= nodesStashed.Count;
                    additionalCatText += running.ToString("F3") + "ms ]";
                }
                categoryNode.Text = s + " Total:" + totalCategoryElapsed.TotalSeconds.ToString() + " (s) in " + ltiv.Count + " instances.    " + additionalCatText;
                _ = baseNode.Nodes.Add(categoryNode);
            }

            // Now that we have covered off all of the matched ones, its time to stick the unmatched ones in.
            baseNode = new TreeNode("Entries with no corresponding pair entry.") {
                Name = "bnUnmatched"
            };
            _ = tvwTimingsReport.Nodes.Add(baseNode);

            foreach (var ltiv in partialMatchedEntries.Values) {
                foreach (var unmatched in ltiv) {
                    instanceNode = new TreeNode(unmatched.InstanceName + " @ " + unmatched.SinkName);
                    _ = instanceNode.Nodes.Add(unmatched.EnterTime.ToShortTimeString());
                    _ = instanceNode.Nodes.Add(unmatched.ExitTime.ToShortTimeString());
                    _ = baseNode.Nodes.Add(instanceNode);
                }
            }
        } finally {
            tvwTimingsReport.EndUpdate();
        }
    }

    internal static int CompareNodesByTag(TreeNode x, TreeNode y) {
        //Bilge.Assert(x.Tag != null, "Tree node tag value must represent elapsed milliseconds");
        //Bilge.Assert(y.Tag != null, "Tree node tag value must represent elapsed milliseconds");

        return (int)(((double)x.Tag) - ((double)y.Tag));
    }

    #endregion Timing Data Support Functions

    internal void RefreshView_Timings(TreeView tvwTimingsReport, AdditionalData additional) {
        // The screen is essentially passed into this method.  This method runs through all of the elements in the selected process
        // and scans for resource and timing changes.  The changes are stored in dictionarys (ported from hashtables) and then rendered
        // to the screen.

        var monitoredTimerInstances = new Dictionary<string, List<TimeInstanceView>>();
        TimerPartialInstancedataStructure partialEntriesDictionary = null;

        var ta = MexCore.TheCore.DataManager.GetKnownApplication(SelectedTracedAppIdx);

        ta.EventEntries.EventEntriesRWL.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);
        //Bilge.ResourceGrab(ta.EventEntries.EventEntriesRWL, "EventEntriesRWL");
        try {
            partialEntriesDictionary = new Dictionary<string, List<TimeInstanceView>>();

            int loopCancelCount = 0;

            foreach (EventEntry ee in ta.EventEntries) {
                // Cancellation support in every big loop.
                #region Cancellation support code
                if (MexCore.TheCore.Options.SupportCancellationOfRefresh && (++loopCancelCount > MexCore.TheCore.Options.SupportCancellationRepeatCount)) {
                    Application.DoEvents();

                    if (CancelCurrentViewOperation) {
                        //Bilge.Log("View operation canceled due to user request.");
                        LastViewSummary = "Cancel Requested";
                        CancelCurrentViewOperation = false;
                        return;
                    }
                }
                #endregion Cancellation support code

                if (MexCore.TheCore.Options.RespectFilter && (!CurrentFilter.IncludeThisEventEntryTimingsView(ee))) { continue; }  // Skip messages based on filter

                // This is only required if the respectfilter is turned off.
                if (TraceMessageFormat.IsSectionCommand(ee.cmdType)) {
                    // multiple thread support for timings.
                    if (!partialEntriesDictionary.ContainsKey(ee.CurrentThreadKey)) {
                        partialEntriesDictionary.Add(ee.CurrentThreadKey, new List<TimeInstanceView>());
                    }

                    _ = ParseEntryForTimingData(monitoredTimerInstances, partialEntriesDictionary[ee.CurrentThreadKey], ee);
                }
            } // End foreach

            // Release the lock.
        } finally {
            //Bilge.ResourceFree(ta.EventEntries.EventEntriesRWL, "EventEntriesRWL");
            ta.EventEntries.EventEntriesRWL.ReleaseReaderLock();
        }

        // Now we have the data stored locally in big lists within dictionaries.  Time to run though this data and determine how we wish to
        // present it to the screen.

        RenderTimings(tvwTimingsReport, monitoredTimerInstances, partialEntriesDictionary, additional);

        var tn = tvwTimingsReport.Nodes["bnMatched"];
        tn?.Expand();
        tn = tvwTimingsReport.Nodes["bnUnmatched"];
        tn?.Expand();
    }

    internal void RefreshView_Resources(TreeView tvwTrackedResource, bool incremental) {
        // The screen is essentially passed into this method.  This method runs through all of the elements in the selected process
        // and scans for resource and timing changes.  The changes are stored in dictionarys (ported from hashtables) and then rendered
        // to the screen.

        if (incremental) {
            //Bilge.Warning("The incremental refresh of a resources view is not supported.");
        }

        var tracedResourceCountInstances = new Dictionary<string, List<ResourceTrackingInstance>>();
        var countedResourceCountInstances = new Dictionary<string, List<ResourceTrackingInstance>>();

        // Find the currently selected app and get a reader lock.
        var ta = MexCore.TheCore.DataManager.GetKnownApplication(SelectedTracedAppIdx);
        ta.EventEntries.EventEntriesRWL.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);
        //Bilge.ResourceGrab(ta.EventEntries.EventEntriesRWL, "EventEntriesRWL");

        try {
            // TODO : Known issue, if RESNAME1+CONTEXTNAME1 = RESNAME2+CONTEXTNAME2 then then the results will get screwed, this can happen as both
            // the resource names and the context names are within the user control.  So res of ABC with context DEF will match res ABCD context EF

            foreach (EventEntry ee in ta.EventEntries) {
                // Cancellation support in every big loop.
                #region Cancellation support code
                if (MexCore.TheCore.Options.SupportCancellationOfRefresh) {
                    Application.DoEvents();

                    if (CancelCurrentViewOperation) {
                        //Bilge.Log("View operation canceled due to user request.");
                        LastViewSummary = "Cancel Requested";
                        CancelCurrentViewOperation = false;
                        return;
                    }
                }
                #endregion Cancellation support code

                ParseEntryForResourceInformation(tracedResourceCountInstances, countedResourceCountInstances, ee);
            } // End foreach

            // Release the lock.
        } finally {
            //Bilge.ResourceGrab(ta.EventEntries.EventEntriesRWL, "EventEntriesRWL");
            ta.EventEntries.EventEntriesRWL.ReleaseReaderLock();
        }

        // Now we have the data stored locally in big lists within dictionaries.  Time to run though this data and determine how we wish to
        // present it to the screen.

        RenderResources(tvwTrackedResource, tracedResourceCountInstances, countedResourceCountInstances, true);
    }

    private static void ParseEntryForResourceInformation(Dictionary<string, List<ResourceTrackingInstance>> tracedResourceCountInstances, Dictionary<string, List<ResourceTrackingInstance>> countedResourceCountInstances, EventEntry ee) {
        string resName, resContext;
        long resValue;

        if (ee.cmdType is TraceCommandTypes.ResourcePuke or TraceCommandTypes.ResourceEat) {
            #region Assertions used to verify that the message found is a valid resource eat / puke message
            //Bilge.Assert(ee.SupportingData != null, "The supporting data for this resource string is missing");
            //Bilge.Assert(ee.SupportingData is ResourceInstanceData, "The supporting data for a resource was not the correct type");
            #endregion Assertions used to verify that the message found is a valid resource eat / puke message

            // If were in release mode or hit ignore on the assertions then we skip this resource instance if its not valid.
            if (ee.SupportingData is null or not ResourceInstanceData) {
                //Bilge.Warning("Invalid supporting data for a resource cound found.  SKIPPING");
                //return;
            }

            resName = ((ResourceInstanceData)ee.SupportingData).resourceName;
            resValue = ((ResourceInstanceData)ee.SupportingData).resourceValue;
            resContext = ((ResourceInstanceData)ee.SupportingData).contextName;

            // In order for the addition and running totals to be valid this assertion must hold true.
            //Bilge.Assert(ee.cmdType == TraceCommandTypes.ResourcePuke ? resValue < 0 : resValue > 0, "Asserting that a ResourcePuke has a resource value of <0 and that a ResourceEat has a resource value of >1 failed.",
            // "The resource type was " + ee.cmdType.ToString() + " and the resource value was " + resValue);

            // We build up all of the resource names and their contexts and values together in list.  Then we can go through and sort it out afterwads

            if (!tracedResourceCountInstances.ContainsKey(resName)) {
                tracedResourceCountInstances.Add(resName, new List<ResourceTrackingInstance>());
            }
            tracedResourceCountInstances[resName].Add(new ResourceTrackingInstance(resName, resValue, resContext, ee.moreLocationData, ee.module, ee.lineNumber, ee.GlobalIndex));
        }

        if (ee.cmdType == TraceCommandTypes.ResourceCount) {
            // TODO : Perf ta.precacheglobalindex (idx)

            //Bilge.Assert(ee.SupportingData != null, "The supporting data for this resource string is missing");
            //Bilge.Assert(ee.SupportingData is ResourceInstanceData, "The supporting data for a resource was not the correct type");

            if (ee.SupportingData is null or not ResourceInstanceData) {
                //Bilge.Warning("Invalid supporting data for a resource cound found.  SKIPPING");
                //return;
            }

            var rti = new ResourceTrackingInstance {
                Name = ((ResourceInstanceData)ee.SupportingData).resourceName,
                Value = ((ResourceInstanceData)ee.SupportingData).resourceValue,
                GlobalIndex = ee.GlobalIndex,
                Line = ee.lineNumber,
                Module = ee.module,
                MoreInfoData = ee.moreLocationData
            };

            if (!countedResourceCountInstances.ContainsKey(rti.Name)) {
                countedResourceCountInstances.Add(rti.Name, new List<ResourceTrackingInstance>());
            }
            countedResourceCountInstances[rti.Name].Add(rti);
        }
    }

    /// <summary>
    /// Part of the Refresh for the resources view, but that method was getting huge.  This method contains the rendering code for taking the
    /// list of resource instances and populating the treeview with resource instance data.
    /// </summary>
    /// <param name="tvwNumericResources">The tree view to render into</param>
    /// <param name="tracedResourceCountInstances">The data collected from the trace stream that relates to resource counts</param>
    /// <param name="numericCountInstances">The data collected from the trace stream that relates to resources that are measured in numeric instances</param>
    /// <param name="colorNodes">Determines whether color should be applied to the resource nodes based on their values</param>
    private static void RenderResources(TreeView tvwNumericResources, Dictionary<string, List<ResourceTrackingInstance>> tracedResourceCountInstances, Dictionary<string, List<ResourceTrackingInstance>> numericCountInstances, bool colorNodes) {
        var tracked = new TreeNode("Tracked Resources");
        var counted = new TreeNode("Numeric Resources");

        // Now populate the tree view with resource counts.

        tvwNumericResources.BeginUpdate();
        try {
            tvwNumericResources.Nodes.Clear();

            foreach (string s in tracedResourceCountInstances.Keys) {
                var resNode = new TreeNode(s);
                _ = tracked.Nodes.Add(resNode);
                long runningTotalForThisResource = 0;
                var treeNodeByContextStore = new Dictionary<string, TreeNode>();

                foreach (var rti in tracedResourceCountInstances[s]) {
                    TreeNode t;
                    if (!treeNodeByContextStore.ContainsKey(rti.Context)) {
                        t = new TreeNode {
                            Text = "Changes for context: " + rti.Context
                        };

                        treeNodeByContextStore.Add(rti.Context, t);
                        _ = resNode.Nodes.Add(t);
                    }
                    if (!treeNodeByContextStore.ContainsKey("All Contexts")) {
                        t = new TreeNode {
                            Text = "Changes for resource across all contexts"
                        };
                        treeNodeByContextStore.Add("All Contexts", t);
                        _ = resNode.Nodes.Add(t);
                    }

                    runningTotalForThisResource += rti.Value;

                    var contextInstanceCount = new TreeNode("Value Changed By (" + rti.Value.ToString() + ") [" + rti.SupportingData + "] - Total >" + runningTotalForThisResource.ToString());
                    _ = treeNodeByContextStore[rti.Context].Nodes.Add(contextInstanceCount);

                    var contextInstance = new TreeNode("Value Changed By (" + rti.Value.ToString() + ")   >  " + rti.Context + "[" + rti.SupportingData + "] - Total >" + runningTotalForThisResource.ToString()) {
                        Tag = rti
                    };

                    if (colorNodes) {
                        //contextInstance.ForeColor = Color.from Color.FromKnownColor(KnownColor.IndianRed);
                        //contextInstance.ForeColor = Color.
                    }
                    _ = treeNodeByContextStore[rti.Context].Nodes.Add(contextInstanceCount);
                    _ = treeNodeByContextStore["All Contexts"].Nodes.Add(contextInstance);
                }

                resNode.Text = s + " Running Total: " + runningTotalForThisResource.ToString() + " in " + tracedResourceCountInstances[s].Count.ToString() + " instances.";
            }

            foreach (string s in numericCountInstances.Keys) {
                var nextResource = new TreeNode(s);
                _ = counted.Nodes.Add(nextResource);
                long lastValueToDeltaWith = 0;
                long deltaTotalForThisResource = 0;

                foreach (var rti in numericCountInstances[s]) {
                    var contextInstance = new TreeNode("Value = " + rti.Value + "   >  " + rti.SupportingData) {
                        Tag = rti
                    };
                    deltaTotalForThisResource = rti.Value - lastValueToDeltaWith;
                    lastValueToDeltaWith = rti.Value;
                    contextInstance.Text += " Delta " + deltaTotalForThisResource.ToString();
                    _ = nextResource.Nodes.Add(contextInstance);
                }
            }

            _ = tvwNumericResources.Nodes.Add(tracked);
            _ = tvwNumericResources.Nodes.Add(counted);

            tracked.Expand();
            counted.Expand();
        } finally {
            tvwNumericResources.EndUpdate();
        }
    }

    #endregion Resource View Screen Support Functions

    /// <summary>
    /// Determines whether or not this event is one of the ones that we prefer to settle on for displaying further details/
    /// </summary>
    /// <param name="ee">The event entry to check</param>
    /// <returns>True if this event entry is suitable</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
    private bool IsPreferredDisplayDetailes(EventEntry ee) {
        const uint PREFERREDVIEWTYPES_FILTER = ~((uint)TraceCommandTypes.CommandOnly | (uint)TraceCommandTypes.ExcEnd |
                                                    (uint)TraceCommandTypes.ExceptionData | (uint)TraceCommandTypes.ExcStart |
                                                    (uint)TraceCommandTypes.MoreInfo);

        return ((uint)ee.cmdType & PREFERREDVIEWTYPES_FILTER) == (uint)ee.cmdType;
    }

    /// <summary>
    /// Returns the GlobalIndex of a preferred view starting from an EventEntry index into a specific TracedApplication.  Some event entries are prefereable to others
    /// for showing, therefore if your in the middle of an exception dump and you ask for extended details this method will actually choose the first message in the
    /// exception dump to display rather than the one you were looking at.
    /// </summary>
    /// <remarks> This method assumes that the traced application is readlocked</remarks>
    /// <param name="ta">The traced application where the entries reside</param>
    /// <param name="eeidx">The offset into the EventEntries in this applciation to start backtracking from</param>
    /// <returns>The physical offset into the TracedApplication array to use for the display</returns>
    private int GetPreferredDetailsIndex(TracedApplication ta, int eeidx) {
        // Track down the currently selected event and determine if that is suitable for detailed view.  If it is not work your way backwards
        // looking for the most suitable location.
        var ae = ta.EventEntries[eeidx];

        if (IsPreferredDisplayDetailes(ae)) { return eeidx; }

        if (MexCore.TheCore.Options.EnableBackTrace) {
            int indexTracker = eeidx;
            string threadMatch = ae.threadID;

            while (indexTracker > 0) {
                indexTracker--;
                ae = ta.EventEntries[indexTracker];
                if ((ae.threadID == threadMatch) && IsPreferredDisplayDetailes(ae)) {
                    // Match found.
                    return indexTracker;
                }
            }
        }

        // If we run out of data then just give um back the first one which is the one they were
        // actually wanting to look at.  Theres no additional context we can give thats worthwhile.
        return eeidx;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
    private int GetNextExceptionIndex(int eeidx, TracedApplication ta) {
        var ee = ta.EventEntries[eeidx];
        const long EXCEPTIONTYPEMATCH = (uint)TraceCommandTypes.ExcStart | (uint)TraceCommandTypes.ExceptionBlock | (uint)TraceCommandTypes.ExcEnd | (uint)TraceCommandTypes.ExceptionData;

        string threadIdMatch = ee.CurrentThreadKey;
        for (int i = eeidx + 1; i < ta.EventEntries.Count; i++) {
            if ((((long)ta.EventEntries[i].cmdType & EXCEPTIONTYPEMATCH) == (uint)ta.EventEntries[i].cmdType) && (ta.EventEntries[i].CurrentThreadKey == threadIdMatch)) {
                // found the next one
                return i;
            }
        }
        // Cant find the next part of the exception, hope it ended cleanly
        return -1;
    }

    /// <summary>
    /// This method identifies the type of message that is to be viewed but also performs backtracking when the actual index selected is part
    /// of a chain of messages.  This occurs in the case of things such as exceptions where the actual detail is sent as a series of
    /// messages.
    /// In this instance the backtracking will find the first of the series of messages and ensure that is displayed.
    /// </summary>
    /// <param name="idx">The index of the desired message to show, updated if necessary</param>
    /// <returns>The mode that should be used for the extended details form.</returns>
    internal ExtendedDetailsMode DetermineMessageTypeForView(ref long idx) {
        #region entry code
        //Bilge.Assert(MexCore.TheCore.DataManager.IsValidTracedApplicationIndex(m_selectedTracedApp), "MexViewer::ViewSupportManager::GetMoreInfoForEventIndex >> The index specified byt the currently selected application is out of range.  RefreshProcessView is trying to refresh an invalid process.");
        //Bilge.Assert(MexCore.TheCore.DataManager.GetKnownApplication(m_selectedTracedApp).EventEntries.ContainsThisGlobalIdx(idx) >= 0, "MexViewer::ViewSupportManager::GetMoreInfoForEventIndex >> The index for the event entry im supposed to be looking up could not be found in the selected applications event entries.  This is an invalid request.");
        #endregion entry code
        //Bilge.E("ViewerExtendedDetailsMode Requested, DetermineessageTypeForView trying to find which element to view.");

        var ta = MexCore.TheCore.DataManager.GetKnownApplication(SelectedTracedAppIdx);
        ta.EventEntries.EventEntriesRWL.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);
        //Bilge.ResourceGrab(ta.EventEntries.EventEntriesRWL, "EventEntriesRWL");
        try {
            int eeidx = ta.FindEventIdxByGlobalIndex_NL(idx);

            // Sometimes its preferable to start elsewhere, this method backtracks to see if thats the case.
            eeidx = GetPreferredDetailsIndex(ta, eeidx);
            idx = ta.EventEntries[eeidx].GlobalIndex;

            if (ta.EventEntries[eeidx].cmdType == TraceCommandTypes.Custom) {
                if (ta.EventEntries[eeidx].secondaryMessage.StartsWith("CUSDATA")) {
                    return ExtendedDetailsMode.ImageData;
                }
            }

            if (ta.EventEntries[eeidx].cmdType == TraceCommandTypes.ExceptionBlock) {
                return ExtendedDetailsMode.Exception;
            }
            if (ta.EventEntries[eeidx].cmdType == TraceCommandTypes.ErrorMsg) {
                return ExtendedDetailsMode.Error;
            }

            if (ta.EventEntries[eeidx].cmdType == TraceCommandTypes.AssertionFailed) {
                return ExtendedDetailsMode.AssertionFailure;
            }
            // For all other message types we display a log message view.
            return ExtendedDetailsMode.LogMessage;
        } finally {
            //Bilge.ResourceFree(ta.EventEntries.EventEntriesRWL, "EventEntriesRWL");
            ta.EventEntries.EventEntriesRWL.ReleaseReaderLock();
        }
    }

    internal void PopulateLogMessageDetailsScreen(long idx, TextBox txtDebugEntryMain, TextBox txtDebugEntrySecondary, TextBox txtDebugEntryMore) {
        var ta = MexCore.TheCore.DataManager.GetKnownApplication(SelectedTracedAppIdx);
        ta.EventEntries.EventEntriesRWL.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);

        try {
            int eeidx = ta.FindEventIdxByGlobalIndex_NL(idx);
            var ee = ta.EventEntries[eeidx];

            txtDebugEntryMain.Text = BeautifyDebugMessage(ee.debugMessage);
            var sb = new StringBuilder();
            _ = !string.IsNullOrEmpty(ee.secondaryMessage) ? sb.AppendLine(ee.secondaryMessage) : sb.AppendLine("No Secondary Information.");

            if (ee.Tags != null && ee.Tags.Keys.Count > 0) {
                _ = sb.Append($"{Environment.NewLine}{Environment.NewLine}____________ Tags ____________ {Environment.NewLine}");
                foreach (string tg in ee.Tags.Keys) {
                    _ = sb.AppendLine($"{tg}    :    {ee.Tags[tg]}");
                }
            }

            txtDebugEntrySecondary.Text = sb.ToString();

            string moreInfo = string.Empty;
            int nextIndex;
            if (ee.HasMoreInfo == true) {
                nextIndex = GetFurtherInfoIndex(eeidx, ta);
                while (nextIndex > 0) {
                    moreInfo += BeautifyDebugMessage(ta.EventEntries[nextIndex].debugMessage) + "\r\n";
                    nextIndex = GetFurtherInfoIndex(nextIndex, ta);
                }
            } else {
                moreInfo = "No further information";
            }
            txtDebugEntryMore.Text = moreInfo;
        } finally {
            //Bilge.ResourceFree(ta.EventEntries.EventEntriesRWL, "EventEntriesRWL");
            ta.EventEntries.EventEntriesRWL.ReleaseReaderLock();
        }
    }

    internal void PopulateExceptionDetailsScreen(ListBox lbxExceptionStack, TextBox txtPrimaryMessage, TextBox txtLocationFilename, TextBox txtLineNo, long idx) {
        #region entry code
        //Bilge.Assert(MexCore.TheCore.DataManager.IsValidTracedApplicationIndex(m_selectedTracedApp), "MexViewer::ViewSupportManager::GetMoreInfoForEventIndex >> The index specified byt the currently selected application is out of range.  RefreshProcessView is trying to refresh an invalid process.");
        //Bilge.Assert(MexCore.TheCore.DataManager.GetKnownApplication(m_selectedTracedApp).EventEntries.ContainsThisGlobalIdx(idx) >= 0, "MexViewer::ViewSupportManager::GetMoreInfoForEventIndex >> The index for the event entry im supposed to be looking up could not be found in the selected applications event entries.  This is an invalid request.");
        #endregion entry code
        //Bilge.E("Viewer double click >> Populate Details Dlg");

        var ta = MexCore.TheCore.DataManager.GetKnownApplication(SelectedTracedAppIdx);
        ta.EventEntries.EventEntriesRWL.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);
        //Bilge.ResourceGrab(ta.EventEntries.EventEntriesRWL, "EventEntriesRWL");
        try {
            int eeidx = ta.FindEventIdxByGlobalIndex_NL(idx);

            var ee = ta.EventEntries[eeidx];

            //Bilge.Assert(ee.cmdType == TraceCommandTypes.ExceptionBlock, "This method must be called targetting an exception block error");

            string primarymsg = "An exception occured:  " + BeautifyDebugMessage(ee.debugMessage) + Environment.NewLine + Environment.NewLine;
            if ((ee.secondaryMessage != null) && (ee.secondaryMessage.Length > 0)) {
                primarymsg += ee.secondaryMessage + Environment.NewLine + Environment.NewLine;
            }
            primarymsg += string.Format("Application : Name [{0,-20}]   Labeled As   [{1}] " + Environment.NewLine, ta.PreferredName, ta.ProcessLabel);
            primarymsg += string.Format("Process     : Pid  [{0,-20}]   Thread(.net) [{1} ({2})] " + Environment.NewLine, ta.ProcessIdAsString, ee.threadID, ee.threadNetId);
            primarymsg += string.Format("Actual      : {0}" + Environment.NewLine, ta.ProcessName);
            primarymsg += string.Format("WindowTitle : {0}" + Environment.NewLine, ta.WindowTitle);
            primarymsg += string.Format("Host        : {0}" + Environment.NewLine, ta.MachineName);
            primarymsg += string.Format("Location    : {0}" + Environment.NewLine, ee.moreLocationData);
            txtPrimaryMessage.Text = primarymsg;

            txtLocationFilename.Text = ee.module;
            txtLineNo.Text = ee.lineNumber;

            LooksLikeAnException llaeTop = null;
            LooksLikeAnException llae = null;
            string currentIndent = string.Empty;

            int nidx = GetNextExceptionIndex(eeidx, ta);
            while (nidx > 0) {// Cant be zero as it must have started somewhere
                ee = ta.EventEntries[nidx];
                if (ee.cmdType == TraceCommandTypes.ExceptionData) {
                    if (ee.debugMessage == Constants.EXCEPTIONENDTAG) {
                        // The whole block ended cleanly.
                        break;
                    }
                    if (ee.debugMessage.StartsWith("H:")) {
                        llae.HelpURL = ee.debugMessage[2..];
                    } else if (ee.debugMessage.StartsWith("R:")) {
                        llae.Source = ee.debugMessage[2..];
                    } else {
                        llae.MoreStuffAboutIt += ee.debugMessage + Environment.NewLine;
                    }

                    if (ee.secondaryMessage.StartsWith("T:")) {
                        llae.TargetSite = ee.secondaryMessage[2..];
                    } else if (ee.secondaryMessage.StartsWith("S:")) {
                        llae.StackTrace = ee.secondaryMessage[2..];
                    } else {
                        llae.MoreStuffAboutIt += ee.secondaryMessage + Environment.NewLine;
                    }
                }

                if (ee.cmdType == TraceCommandTypes.ExcStart) {
                    if (llaeTop == null) {
                        llae = new LooksLikeAnException();
                        llaeTop = llae;
                    } else {
                        llae.InnerException = new LooksLikeAnException();
                        llae = llae.InnerException;
                    }
                    llae.IndentFromNesting = currentIndent;
                    currentIndent += "  ";
                    llae.ExceptionMessage = BeautifyDebugMessage(ee.debugMessage);
                    llae.TypeName = ee.secondaryMessage;
                }

                if (ee.cmdType == TraceCommandTypes.ExceptionData) {
                    //Bilge.Assert(llae != null, "This shouldnt be possible, im seeing exception details in a trace stream without exception block tags - the trace streams in a mess");
                    if (llae == null) {
                        //Bilge.Warning("TraceStream invalid, exception details found in a trace stream without exception blocks starting it");
                        nidx = GetNextExceptionIndex(nidx, ta); // Look for the next one.
                        continue;
                    } // Still need to cater for the trace stream being messed up.

                    if (ee.debugMessage.StartsWith("H:")) {
                        // This is the Data element that has the help link and the Target in it
                        llae.HelpURL = ee.debugMessage[2..];
                        //Bilge.Assert(ee.SecondaryMessage.StartsWith("T:"), "The Exception Data format seems to be corrupt.  Was expecting a Target to follow a Help URL");
                        if (ee.secondaryMessage.StartsWith("T:")) {
                            llae.TargetSite = ee.secondaryMessage[2..];
                        }
                    } else if (ee.debugMessage.StartsWith("R:")) {
                        llae.Source = ee.debugMessage[2..];
                        //Bilge.Assert(ee.SecondaryMessage.StartsWith("S:"), "The exception data format seems to be corrupt. Was expecting a StackTrace to follow a source");
                        if (ee.secondaryMessage.StartsWith("S:")) {
                            llae.StackTrace = ee.secondaryMessage[2..];
                        }
                    } else {
                        llae.MoreStuffAboutIt += Environment.NewLine + ee.debugMessage + Environment.NewLine + ee.secondaryMessage;
                    }
                }

                if (ee.cmdType == TraceCommandTypes.ExcEnd) {
                    llae.MoreStuffAboutIt += Environment.NewLine + ee.debugMessage + Environment.NewLine + ee.secondaryMessage;
                }

                nidx = GetNextExceptionIndex(nidx, ta); // Look for the next one.
            } // End while theres more exception data to be explored.

            // Now we have built up a model of the exceptions use it.
            lbxExceptionStack.Items.Clear();
            llae = llaeTop;
            while (llae != null) {
                _ = lbxExceptionStack.Items.Add(llae);
                llae = llae.InnerException;
            }
        } finally {
            //Bilge.ResourceFree(ta.EventEntries.EventEntriesRWL, "EventEntriesRWL");
            ta.EventEntries.EventEntriesRWL.ReleaseReaderLock();
        }
    }

    /// <summary>
    /// Produces a set of traces backwards in time from a specific index and fills the provided listbox with the details of these
    /// traces.
    /// </summary>
    /// <remarks>THis must be called on the same thread that created the listbox.</remarks>
    /// <param name="lbxBackTraceDetails"></param>
    /// <param name="idxOfEntry"></param>
    /// <param name="followThread">If this is true a single thread is followed backwards, otherwise all messages are included</param>
    internal void GetBackTraceLeadingToMessage(ListBox lbxBackTraceDetails, long idxOfEntry, bool followThread) {
        const int BACKTRACKCOUNTLIMIT = 25;

        //Bilge.Assert(lbxBackTraceDetails.InvokeRequired == false, "The method GetBackTraceLeadingToMessage must be called on the same thread that created the listbox passed as the first parameter");

        lbxBackTraceDetails.Items.Clear();

        var ta = MexCore.TheCore.DataManager.GetKnownApplication(SelectedTracedAppIdx);
        ta.EventEntries.EventEntriesRWL.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);
        //Bilge.ResourceGrab(ta.EventEntries.EventEntriesRWL, "EventEntriesRWL");
        try {
            int eeidx = ta.FindEventIdxByGlobalIndex_NL(idxOfEntry);
            string threadMatchForFollow = ta.EventEntries[eeidx].threadID;

            int backTrackCount = 0;

            while (backTrackCount < BACKTRACKCOUNTLIMIT) {
                _ = lbxBackTraceDetails.Items.Add(string.Format("Exception-{0,-2}: {1,-15} on thread ({2,-4}): {3}", backTrackCount, ta.EventEntries[eeidx].cmdType.ToString(), ta.EventEntries[eeidx].threadID, BeautifyDebugMessage(ta.EventEntries[eeidx].debugMessage)));

                backTrackCount++;
                eeidx--;
                if (followThread) {
                    while ((eeidx >= 0) && (ta.EventEntries[eeidx].threadID != threadMatchForFollow)) { eeidx--; }
                }
                if (eeidx < 0) { break; }  // Stop if we run out of back track content prematurely
            }
        } finally {
            //Bilge.ResourceFree(ta.EventEntries.EventEntriesRWL, "EventEntriesRWL");
            ta.EventEntries.EventEntriesRWL.ReleaseReaderLock();
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
    private int GetFurtherInfoIndex(int eeidx, TracedApplication ta) {
        string threadIdMatch = ta.EventEntries[eeidx].threadID;

        if (ta.EventEntries[eeidx].HasMoreInfo) {
            for (int i = eeidx + 1; i < ta.EventEntries.Count; i++) {
                if ((ta.EventEntries[i].cmdType == TraceCommandTypes.MoreInfo) && (ta.EventEntries[i].threadID == threadIdMatch)) {
                    return i;
                }
            }
        }
        return -1;
    }

    #region Timed View Support Functions

    private ListView listViewToFillForTimedView;

    private bool EveryEntryInOrderCallbackForTimedView(EventEntry ee, NonTracedApplicationEntry nta, int indexOfApp) {
        var lvi = new ListViewItem();
        if (ee != null) {
            // RespectFilter is a fairly one way round thing, if i dont respect the filter then it will result to true and therefore short circuit
            // to the if body. If i do respect the filter it will result to false and the includeThisEventEntry will get executed
            if ((!MexCore.TheCore.Options.RespectFilter) || CurrentFilter.IncludeThisEventEntry(ee)) {
                lvi.Text = ee.GlobalIndex.ToString(); ;
                _ = lvi.SubItems.Add(MexCore.TheCore.DataManager.GetKnownApplication(indexOfApp).ProcessIdAsString);
                _ = lvi.SubItems.Add(ee.debugMessage);
                _ = CurrentHighlightOptions.ModifyEventEntryForHighlight(ee);

                if (ee.viewData.isValid && ee.viewData.isHighlighted) {
                    if (ee.viewData.isBackgroundHighlighted) {
                        lvi.BackColor = ee.viewData.backgroundHighlightColor;
                    }
                    if (ee.viewData.isForegroundHighlighted) {
                        lvi.ForeColor = ee.viewData.foregroundHighlightColor;
                    }
                }

                _ = listViewToFillForTimedView.Items.Add(lvi);
                return false;
            }
        }
        if (nta != null) {
            // RespectFilter is a fairly one way round thing, if i dont respect the filter then it will result to true and therefore short circuit
            // to the if body. If i do respect the filter it will result to false and the includeThisEventEntry will get executed
            if ((!MexCore.TheCore.Options.RespectFilter) || CurrentFilter.IncludeThisNonEventEntry(nta)) {
                if (CurrentHighlightOptions.ModifyNonTracedEventEntryForHighlight(nta)) {
                    lvi.Text = nta.assignedIndex.ToString();
                    _ = lvi.SubItems.Add(nta.Pid.ToString());
                    _ = lvi.SubItems.Add(nta.DebugEntry);

                    if (nta.viewData.isValid && nta.viewData.isHighlighted) {
                        if (nta.viewData.isBackgroundHighlighted) {
                            lvi.BackColor = nta.viewData.backgroundHighlightColor;
                        }
                        if (nta.viewData.isForegroundHighlighted) {
                            lvi.ForeColor = nta.viewData.foregroundHighlightColor;
                        }
                    }
                    _ = listViewToFillForTimedView.Items.Add(lvi);
                    return false;
                }
            }
        }
        return true;
    }

    /// <summary>
    /// Refresh Timed View attempts to loop through each of the data holding arrays finding each
    /// of the assigned global indexes in turn and then adding them to the timed display.  It will
    /// initially locate the lower bounds of each of the assigned indexes and then loop through
    /// each of the entry types trying to find the next available index.
    /// </summary>
    internal void RefreshView_Timed(ListView lvwView, bool incremental) {
        #region entry code
        //Bilge.Assert(!lvwView.InvokeRequired, "invoke required for timed view update");
        #endregion entry code

        if (incremental) {
            //Bilge.Warning("The incremental approach is not supported for the timed view");
        }
        lvwView.Items.Clear();
        lvwView.SuspendLayout();
        lvwView.BeginUpdate();
        try {
            listViewToFillForTimedView = lvwView;
            LastViewSummary = !MexCore.TheCore.DataManager.VisitEveryEventEntryInOrder(new DataStructureManager.VisitEachEntryCallback(EveryEntryInOrderCallbackForTimedView), null)
                ? "Refresh Cancelled"
                : "Refresh Done.";
            listViewToFillForTimedView = null;
        } finally {
            lvwView.EndUpdate();
            lvwView.ResumeLayout();
        }
    } // End refresh timed view routine

    #endregion Timed View Support Functions

    #region Diagnostic View Support Functions

    // Refreshes the diagnostic view
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
    internal void RefreshView_Diagnostics(Label lbl, TextBox output, long deepInspectIndex) {
        //Bilge.Assert(!lbl.InvokeRequired, "Invoke required for label update");
        //Bilge.Assert(!output.InvokeRequired, "Invoke required for treeview update");

        try {
            lbl.Text = "Diags queue length at " + MexCore.TheCore.WorkManager.JobsOutstanding;
            lbl.Text += "\r\n Mem usage at " + GC.GetTotalMemory(false).ToString();
        } catch (Win32Exception ex) {
            //Bilge.Dump(ex, "Error displaying diagnostics");
            lbl.Text = ex.Message;
        }

        //Bilge.Log("Going to get dianostics text from the datamanager");
        output.Text = MexCore.TheCore.DataManager.DiagnosticsText();

        string deepInspectString;

        var ee = MexCore.TheCore.DataManager.FindEventEntryInKnownAppsByIndex(deepInspectIndex);

        if (ee == null) {
            var nta = MexCore.TheCore.DataManager.FindNTAEntryByIndex(deepInspectIndex);

            deepInspectString = nta == null ? "No match found for index." : nta.GetDiagnosticStringData();
        } else {
            deepInspectString = ee.GetDiagnosticStringData();
        }

        output.Text += "\r\n\r\n Deep Inspect For Index : " + deepInspectIndex.ToString() + "\r\n" + deepInspectString;

        output.Text += "\r\n\r\n" + MexCore.TheCore.MessageManager.DiagnosticsText();
        output.Text += "\r\n\r\n" + MexCore.TheCore.CacheManager.DiagnosticsText();
        output.Text += "\r\n\r\n" + MexCore.TheCore.ViewManager.DiagnosticsText();
        output.Text += "\r\n\r\n" + MexCore.TheCore.WorkManager.DiagnosticsText();
        output.Text += "\r\n\r\n" + MexCore.TheCore.DiagnosticsText();
        output.Text = "\r\n\r\n" + MexCore.TheCore.Options.OptionsToString();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
    private string DiagnosticsText() {
        return "no view diagnostic infomration";
    }

    #endregion Diagnostic View Support Functions

    #endregion Process View Supporting Functions

    #region Work Trigger Methods

    internal void RequestSelectedAppPurge() {
        //Bilge.Log("View Requesting Operation  >> Purge Selected Requested");

        AddUserNotificationMessageByIndex(UserMessages.PurgeJobStarts, UserMessageType.InformationMessage, null);
        MexCore.TheCore.WorkManager.AddJob(new Job_PurgeTracedAppByIndex(SelectedTracedAppIdx));
        SelectedTracedAppIdx = -1;
    }

    internal void RequestMultiPurge(List<int> indexesToPurge) {
        AddUserNotificationMessageByIndex(UserMessages.PurgeJobStarts, UserMessageType.InformationMessage, null);
        foreach (int next in indexesToPurge) {
            MexCore.TheCore.WorkManager.AddJob(new Job_PurgeTracedAppByIndex(next));
        }
        SelectedTracedAppIdx = -1;
    }

    /// <summary>
    /// Puts a job on the queue to call back to the UI to perform a refresh.  This can even be called by the UI to request a refresh later.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
    internal void RequestViewRefreshNotification(bool withIncremental) {
        //Bilge.Log("View Requesting an Operation >> Request View Refresh");

        MexCore.TheCore.WorkManager.AddJob(new Job_NotifyRefreshRequired(withIncremental), false, true);
    }

    internal void RequestPurgeAll() {
        //Bilge.Log("View Requesting Operation  >> Purge All requested");
        AddUserNotificationMessageByIndex(UserMessages.PurgeJobStarts, UserMessageType.InformationMessage, null);
        MexCore.TheCore.WorkManager.ProcessJob(new Job_PurgeAllData());
    }

    internal void RequestPurgeAllExceptCurrentlySelected() {
        //Bilge.Log("View Requesting Operation >> Purge AllExceptCurrentlySelected requested");
        AddUserNotificationMessageByIndex(UserMessages.PurgeJobStarts, UserMessageType.InformationMessage, null);
        MexCore.TheCore.WorkManager.ProcessJob(new Job_PurgeAllData(SelectedTracedAppIdx));
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
    internal void RequestChangeODSGathererState(bool makeGathererActive) {
        //Bilge.Log("View Requesting Operation  >> Request to change ODS Gatherer state to " + makeGathererActive);
        MexCore.TheCore.WorkManager.AddJob(new Job_ActivateODSGatherer(makeGathererActive));
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
    internal void RequestChangeTCPGathererState(bool makeGathererActive) {
        //Bilge.Log("View Requesting Operation  >> Request to change TCP Gatherer state to " + makeGathererActive);
        MexCore.TheCore.WorkManager.AddJob(new Job_ChangeTCPGathererState(makeGathererActive));
    }

    internal void RequestSelectedAppClear() {
        //Bilge.Log("View Requesting Operation  >> SelectedAppPartialPurgeRequested");
        MexCore.TheCore.WorkManager.ProcessJob(new Job_PartialPurgeApp(SelectedTracedAppIdx));
    }

    #endregion Work Trigger Methods

    # region Notificaiton Callback Methods

    /// <summary>
    /// ProcessNewEventNotification tells the viewer to refresh itself when the selected application index is changed, the index of the
    /// changed application is passed in as the parameter.
    /// </summary>
    /// <remarks>If the changed index is -1 all processes are flagged as refresh required</remarks>
    /// <param name="affectedIndex">The index number of the affected application</param>
    internal void ProcessNewEventNotification(object affectedIndex) {
        //Bilge.Assert(affectedIndex != null, "Mex::ViewSupportManager::ProcessNewEventNotification -> affectedIndex is supposed to reflect the index that was changed, however it is null.  Cannot process this.");
        // The index of the process event that was affected
        int idx = (int)affectedIndex;
        if ((idx == SelectedTracedAppIdx) || (idx == -1)) {
            // The current one was affeced.  Do a refresh!
            //Bilge.LogIf(RegisterForSelectedProcessRefreshRequired == null, "Mex::ViewSupportManager::ProcessNewEventNotification - No one registered for selected app changes, skipping notification");
            RegisterForSelectedProcessRefreshRequired?.Invoke(true);
        }
    }

    /// <summary>
    /// The process change notification is called when there has been a change to the process list or details about a specific process.  It is
    /// called by the work manager and will call any registered view notifications that need to be updated.  If the parameter passed is an integer
    /// this is the index of the specific process to be updated, if it is null it is assumed that the entire process list needs to be changed.
    /// </summary>
    /// <param name="selectedProcess">An integer representing the process that has changed or null for all processes changed</param>
    internal void ProcessChangeNotification(object selectedProcess) {
        //Bilge.Log("Mex::ViewSupportManager >> ProcessChangeNotification job accepted. ");

        int theIdx = -1;

        //Bilge.LogIf(RegisterForProcessListChanges == null, "Mex::ViewSupportManager::ProcessChangeNotification -No one registerd for any process list changes, skipping all process changed notification");

        if (RegisterForProcessListChanges != null) {
            // Someone has registered for process changes.  We create a new list of summary views and send this to them as an array of the
            // process summaries.
            var pss = MexCore.TheCore.DataManager.GetAllProcessSummaries();
            RegisterForProcessListChanges(pss);
        }

        // If selectedProcess is specified it may be that we have the additional task of telling them that the currently selected process has changed
        // not just a general change to the processes.
        //Bilge.LogIf(RegisterForSelectedProcessChanges == null, "Mex::ViewSupportManager::ProcessChangeNotification - No one registered for selectedProcessChanged notification, skipping");
        //Bilge.LogIf(selectedProcess == null, "Mex::ViewSupportManager::ProcessChangeNotification -SelectedProcess param is null, skipping selectedPRocessChanged notification");

        if ((selectedProcess != null) && (RegisterForSelectedProcessChanges != null)) {
            try {
                theIdx = (int)selectedProcess;

                if (theIdx == SelectedTracedAppIdx) {
                    // Someone has registered for the specific currently selected process has changed notification.  Create a new summary fo the currentl
                    // selected change and call them.
                    RegisterForSelectedProcessChanges(MexCore.TheCore.DataManager.CreateSummaryForProcess(theIdx));
                }
            } catch (OverflowException) {
                //Bilge.Dump(ex, "MexViewer::CoreFunctionality::ProcessChangeNotification --> Invalid parameter passed, PROCESS NOTIFICATION IGNORED ");
            }
        }
    } // End ViewManager::ProcessChangeNotification method

    #region delegate and property support for registering for notification changes

    /* when applications support a view of the data they can request to be notified when the data changes underneath them.  The types of notification
   that they can register for are as follows:

   SelectedProcessChanged (ProcessSummary)  - The current selected process details have changed (eg a purge)
   ProcessListChanged (ProcessSummary[]) - One ore more of the processes have chaged (eg purge all / add new )
   ProrcessRefreshRequired (bool) - The data in the currently selected process has chaged (eg semi purge / highlight change / new event added)

*/

    // Notification support for process list / selected changes, delegates and callbacks
    internal delegate void SelectedProcessChanged(ProcessSummary ps);

    internal delegate void ProcessListChanged(ProcessSummary[] pss);

    internal delegate void SelectedProcessRefreshRequired(bool incrementalOk);

    internal delegate void ViewerOptionsRefreshRequired(string[] listOfFilters);

    internal delegate void CurrentViewChanged(bool incrementalOK);

    internal CurrentViewChanged RegisterForCurrentViewChanges { get; set; }

    internal SelectedProcessChanged RegisterForSelectedProcessChanges { get; set; }
    internal ProcessListChanged RegisterForProcessListChanges { get; set; }

    /// <summary>
    /// Called when the selected process has altered and a refresh needs to occur. Eg semi purge / highlight change / new event added
    /// </summary>
    internal SelectedProcessRefreshRequired RegisterForSelectedProcessRefreshRequired { get; set; }

    #endregion delegate and property support for registering for notification changes

    #endregion
#if !DEBUG
private void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e) {
  // This is the general thread exception
 //Bilge.Log("Unhandled exception occured in MEX " + e.Exception.Message);
  AddUserNotificationMessageByIndex(UserMessages.UnhandledExceptionOccured, UserMessageType.ErrorMessage,string.Empty);
}
#endif

    internal ViewSupportManager() {
        
        CurrentFilter = new ViewFilter();
        CurrentHighlightOptions = new HighlightRequestsStore();
#if !DEBUG
  Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
#endif
    }

    /// <summary>
    /// A method for the internals of the application to request a refresh of the current view whatever it is.  This is called when
    /// something more significant than a new event entry turns up, for example a complete restructure or highlight or filter change.
    /// </summary>
    /// <param name="allowIncremental"></param>
    internal void RefreshCurrentView(bool allowIncremental) {
        //Bilge.Log("Request for current view refresh being passed to interface incrementalOK(" + allowIncremental.ToString() + ")");
        if (RegisterForCurrentViewChanges != null) {
            RegisterForCurrentViewChanges(allowIncremental);
            //Bilge.Log("Current view refresh request completed");
        } else {
            //Bilge.Warning("No refresh delegate hooked up, RequestCurrentViewRefresh has been lost");
        }
    }

    internal bool ApplyCurrentHighlightOptionsToView() {
        if (SelectedTracedAppIdx < 0) { return false; }

        var ta = MexCore.TheCore.DataManager.GetKnownApplication(SelectedTracedAppIdx);
        ta.EventEntries.EventEntriesRWL.AcquireReaderLock(Consts.MS_TIMEOUTFORLOCKS);
        bool needsRefresh = false;
        try {
            foreach (EventEntry ee in ta.EventEntries) {
                if (CurrentHighlightOptions.ModifyEventEntryForHighlight(ee)) {
                    needsRefresh = true;
                }
                if (CurrentHighlightOptions.ApplyDefaultHighlighting(ee)) {
                    needsRefresh = true;
                }
            }
        } finally {
            ta.EventEntries.EventEntriesRWL.ReleaseReaderLock();
        }
        return needsRefresh;
    }

    /// <summary>
    /// Gets full assertion information from an assertion failure in the stream.  Currently does not need to be an assertion.
    /// </summary>
    /// <remarks>Assumes that its in a reader lock</remarks>
    /// <param name="idx"></param>
    /// <returns></returns>
    internal AssertionPopulationData GetAssertionFurtherInformation(long idx) {
        var result = new AssertionPopulationData();

        var ta = MexCore.TheCore.DataManager.GetKnownApplication(SelectedTracedAppIdx);
        int assertionOffset = ta.EventEntries.ContainsThisGlobalIdx(idx);
        var ee = ta.EventEntries[assertionOffset];

        result.ErrorText = ee.debugMessage + "\r\n" + ee.secondaryMessage;
        result.Line = ee.lineNumber;
        result.ModuleName = ee.module;
        result.Morelocinfo = ee.moreLocationData;
        result.NetThreadId = ee.threadNetId;
        result.Threadid = ee.threadID;
        result.MachineId = ta.MachineName;
        result.ProcessId = ta.ProcessIdAsString;
        result.ProcessName = ta.ProcessName;

        for (int i = assertionOffset + 1; i < ta.EventEntries.Count; i++) {
            var further = ta.EventEntries[i];
            if ((further.cmdType == TraceCommandTypes.MoreInfo) && (further.threadID == ee.threadID)) {
                ExtractFurtherAssertionInfoFromMoreInfo(result, further);
                break;
            }
        }

        return result;
    }

    /// <summary>
    /// To fully populate the assertion the data is sent in more than one string, this method extracts the information that is sent in a more info
    /// command after the assertion message.
    /// </summary>
    /// <param name="result">The assertionpopulationdata to populate</param>
    /// <param name="further">the event entry containing all of the information</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
    private void ExtractFurtherAssertionInfoFromMoreInfo(AssertionPopulationData result, EventEntry further) {
        string stackData = further.debugMessage;
        string intialMarker = FlimFlamConstants.ASSERTIONRECREATESTRING + "STK:";
        result.Stacktrace = stackData.StartsWith(intialMarker) ? stackData[intialMarker.Length..] : "Unknown.";
    }

    /// <summary>
    /// Retrieves the information that is required to populate an error box when investigating the details of an error message in mex
    /// </summary>
    /// <param name="idx">The global index of the error message</param>
    /// <returns>A structure with all of the informaiton in it</returns>
    internal ErrorDialogData GetErrorFurtherInformation(long idx) {
        var result = new ErrorDialogData();

        var ta = MexCore.TheCore.DataManager.GetKnownApplication(SelectedTracedAppIdx);
        int assertionOffset = ta.EventEntries.ContainsThisGlobalIdx(idx);

        if (assertionOffset == -1) {
            return null;
        }

        var ee = ta.EventEntries[assertionOffset];

        result.ErrorMessage = ee.debugMessage + "\r\n" + ee.secondaryMessage;

        // It is possible to stream a set of evidences out to the trace such that they can be displayed in Mex, if they
        // are in the trace they will be the next messages of type "CommandOnly".
        assertionOffset++;
        while (assertionOffset < ta.EventEntries.Count) {
            if (ta.EventEntries[assertionOffset].CurrentThreadKey == ee.CurrentThreadKey) {
                // Check to see if it is a command.  If it is not then we can safely stop looking for the commands
                // as the additional command data follows the error.
                if (ta.EventEntries[assertionOffset].cmdType != TraceCommandTypes.CommandOnly) { break; }
                result.AddEvidence(ta.EventEntries[assertionOffset].debugMessage, ta.EventEntries[assertionOffset].secondaryMessage);
            }
            assertionOffset++;
        }

        return result;
    }
}