//using Plisky.Plumbing.Legacy;

namespace Plisky.Diagnostics.FlimFlam {

    /// <summary>
    /// Summary description for MexOptions.  Class must be public for serialization into the iso store.
    /// </summary>
    public class AppSettings {
        private string adPlus_Import_SplitTagIdent = "Debug session time: ";

        /// <summary>
        /// The identifier that is used to work out which lines within an ADPLus style log are identifiers
        /// </summary>
        public string ADPlusImportIdentifierToSplitTags {
            get { return adPlus_Import_SplitTagIdent; }
        }

        /// <summary>
        ///  ip address to listen on * for any
        /// </summary>
        public string IPAddressToBind { get; set; }

        /// <summary>
        /// Port which the TCP Listener is listening on.
        /// </summary>
        public int PortAddressToBind { get; set; }

        /*
        public bool PersistEverything { get; set; }
        public string CurrentFilename { get; set; }

        private int m_messagesToSpawnThreadsFor = 200000;

        /// <summary>
        /// Decides how many queued messages justify the creation of additional threads
        /// </summary>
        public int MessagesToSpawnThreadsFor {
            get { return m_messagesToSpawnThreadsFor; }
            set { m_messagesToSpawnThreadsFor = value; }
        }

        private int m_noSecondsForRefreshOnImport = 45;

        /// <summary>
        ///  determines how long an import runs before a refresh of the application should occur
        /// </summary>
        public int NoSecondsForRefreshOnImport {
            get { return m_noSecondsForRefreshOnImport; }
            set { m_noSecondsForRefreshOnImport = value; }
        }

        private bool removeDupesOnImport = true;

        /// <summary>
        /// Removes duplicate entries when arriving in Mex, reducing the overhead of having two listeners
        /// installed.
        /// </summary>
        public bool RemoveDuplicatesOnImport {
            get { return removeDupesOnImport; }
            set { removeDupesOnImport = value; }
        }

        private bool removeDupesOnView;

        /// <summary>
        /// Removes duplicate entries when being displayed in Mex, be careful when using this with code that loops
        /// </summary>
        public bool RemoveDuplicatesOnView {
            get { return removeDupesOnView; }
            set { removeDupesOnView = value; }
        }

        private bool m_resetRefreshOnStartup = true;

        /// <summary>
        /// Gets or Sets a flag to indicate whether or not Mex should reset the filter on start up if the refresh
        /// time is greater than 2 seconds.
        /// </summary>
        public bool ResetRefreshOnStartup {
            get { return m_resetRefreshOnStartup; }
            set { m_resetRefreshOnStartup = value; }
        }

        private int m_noSecondsForUIUpdate; // Default zero

        /// <summary>
        ///  Decides how long the application should wait before the UI is refreshed
        /// </summary>
        public int NoSecondsForUIUpdate {
            get { return m_noSecondsForUIUpdate; }
            set { m_noSecondsForUIUpdate = value; }
        }

        private int m_noUserNotificationsToStoreInLog = 150;

        /// <summary>
        /// Decides how many user notifications are stored in mex.
        /// </summary>
        public int NoUserNotificationsToStoreInLog {
            get { return m_noUserNotificationsToStoreInLog; }
            set { m_noUserNotificationsToStoreInLog = value; }
        }

        private bool displayPreferredNameInsteadOfPid;

        /// <summary>
        /// Gets or Sets an option to use a display name rather than a process identifier when Mex has been
        /// sent the display name.
        /// </summary>
        public bool UsePreferredNameInsteadOfProcessId {
            get { return displayPreferredNameInsteadOfPid; }
            set { displayPreferredNameInsteadOfPid = value; }
        }

        private bool m_beautifyDisplayedStrings;

        /// <summary>
        /// Removes unprintable characters and puts their replacenments into the displayed strings.
        /// </summary>
        public bool BeautifyDisplayedStrings {
            get { return m_beautifyDisplayedStrings; }
            set { m_beautifyDisplayedStrings = value; }
        }

        private bool m_normaliseRefreshActive;

        /// <summary>
        /// Makes sure that a refresh doesent occur more frequently than X number of seconds.  This allows you to slow down the
        /// jitter when lots of messages are streaming at the viewer.
        /// </summary>
        public bool NormaliseRefreshActive {
            get { return m_normaliseRefreshActive; }
            set { m_normaliseRefreshActive = value; }
        }

        private int m_normalisationLimitMilliseconds;

        /// <summary>
        /// This is how frequently the refresh is applied if normalisation is being applied.
        /// </summary>
        public int NormalisationLimitMilliseconds {
            get { return m_normalisationLimitMilliseconds; }
            set { m_normalisationLimitMilliseconds = value; }
        }

        private FileImportMethod m_import_TextFileBehaviour = FileImportMethod.TextWriterWithTexSupport;

        /// <summary>
        /// The control determining how the log files are read into mex
        /// </summary>
        public FileImportMethod ImportTextFileBehaviour {
            get { return m_import_TextFileBehaviour; }
            set { m_import_TextFileBehaviour = value; }
        }

        private bool m_selectingProcessSelectsProcessView = true;

        /// <summary>
        /// When you choose a process does it change the view to process view
        /// </summary>
        public bool SelectingProcessSelectsProcessView {
            get { return m_selectingProcessSelectsProcessView; }
            set { m_selectingProcessSelectsProcessView = value; }
        }

        private bool autoSelectFirstProcess = true;

        /// <summary>
        /// Selects the process view when the first process is entered into Mex, even if the option to select
        /// process view on process change is not ticked.
        /// </summary>
        public bool AutoSelectFirstProcess {
            get { return this.autoSelectFirstProcess; }
            set { this.autoSelectFirstProcess = value; }
        }

        private bool m_timedViewRespectsFilter; // Default false

        /// <summary>
        /// Should timed view respect the filter or dump everything to the screen.
        /// </summary>
        public bool TimedViewRespectsFilter {
            get { return m_timedViewRespectsFilter; }
            set { m_timedViewRespectsFilter = value; }
        }

        private bool m_supportCancellationOfRefresh = true;

        /// <summary>
        /// allow the cancel refresh button to function
        /// </summary>
        public bool SupportCancellationOfRefresh {
            get { return m_supportCancellationOfRefresh; }
            set { m_supportCancellationOfRefresh = value; }
        }

        private int m_supportCancellationRepeatCount = 50;

        /// <summary>
        /// Determines how responsive the cancellation request is, lower numbers are more responsive but slower.
        /// </summary>
        public int SupportCancellationRepeatCount {
            get { return m_supportCancellationRepeatCount; }
            set { m_supportCancellationRepeatCount = value; }
        }

        private bool m_autoPurgeApplicationOnMatchingName = true;

        /// <summary>
        /// Automatically clear the application data when the same named application arrives in the trace stream
        /// </summary>
        public bool AutoPurgeApplicationOnMatchingName {
            get { return m_autoPurgeApplicationOnMatchingName; }
            set { m_autoPurgeApplicationOnMatchingName = value; }
        }

        private int m_pushbackCountDelayLimitForInteractiveJobs = 10;

        /// <summary>
        /// How many times a job can be moved back up the queue
        /// </summary>
        public int PushbackCountDelayLimitForInteractiveJobs {
            get { return m_pushbackCountDelayLimitForInteractiveJobs; }
            set { m_pushbackCountDelayLimitForInteractiveJobs = value; }
        }

        /// <summary>
        /// should new messages automatically find their way into the view
        /// </summary>

        private bool m_autoRefresh = true;

        public bool AutoRefresh {
            get { return m_autoRefresh; }
            set { m_autoRefresh = value; }
        }

        /// <summary>
        /// should the view try and keep up with new messages arriving
        /// </summary>

        private bool m_autoScroll = true;

        public bool AutoScroll {
            get { return m_autoScroll; }
            set { m_autoScroll = value; }
        }

        /// <summary>
        /// Global on / off for the filter
        /// </summary>

        private bool m_respectFilter = true;  // right now this is global but could be per screen

        public bool RespectFilter {
            get { return m_respectFilter; }
            set { m_respectFilter = value; }
        }

        /// <summary>
        /// Dont know.
        /// </summary>

        private bool m_displayInternalMessages;

        public bool DisplayInternalMessages {
            get { return m_displayInternalMessages; }
            set { m_displayInternalMessages = value; }
        }

        /// <summary>
        /// The profile for highlighting that is to be loaded on startup
        /// </summary>

        private string m_highlightDefaultProfileName;

        public string HighlightDefaultProfileName {
            get { return m_highlightDefaultProfileName; }
            set { m_highlightDefaultProfileName = value; }
        }

        private string m_filterDefaultProfileName;

        /// <summary>
        /// The filter profile that is to be loaded on startup.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Filename", Justification = "Fale positive on ProfileName")]
        public string FilterFilenameToLoadOnStartup {
            get { return m_filterDefaultProfileName; }
            set { m_filterDefaultProfileName = value; }
        }

        /// <summary>
        /// A list of known highlight saves
        /// </summary>
        private string[] m_currentMexHighlightNames;  // Holds names of the files of highlights

        public string[] CurrentMexHighlightNames {
            get { return m_currentMexHighlightNames; }
            set { m_currentMexHighlightNames = value; }
        }

        /// <summary>
        /// A list of known filter names
        /// </summary>

        private string[] m_currentMexFilterNames;     // Holds names of the files of filters

        public string[] CurrentMexFilterNames {
            get { return m_currentMexFilterNames; }
            set { m_currentMexFilterNames = value; }
        }

        /// <summary>
        /// Extension for highlights
        /// </summary>

        private string m_highlightExtension = ".mx_hlt";

        public string HighlightExtension {
            get { return m_highlightExtension; }
            set { m_highlightExtension = value; }
        }

        private string m_filterExtension = ".mx_flt";

        /// <summary>
        /// Extension for filters
        /// </summary>
        public string FilterExtension {
            get { return m_filterExtension; }
            set { m_filterExtension = value; }
        }

        private bool m_showGlobalIndexInView;

        /// <summary>
        /// Determines whether process view shows you the global index of the message
        /// </summary>
        public bool ShowGlobalIndexInView {
            get { return m_showGlobalIndexInView; }
            set { m_showGlobalIndexInView = value; }
        }

        private bool m_autoLoadHighlightDefaultOnStartup;

        /// <summary>
        /// Do we load the default highlighting on startup
        /// </summary>
        public bool AutoLoadHighlightDefaultOnStartup {
            get { return m_autoLoadHighlightDefaultOnStartup; }
            set { m_autoLoadHighlightDefaultOnStartup = value; }
        }

        private string m_filterAndHighlightStoreDirectory;

        /// <summary>
        /// Directory for storing filters and highlights
        /// </summary>
        public string FilterAndHighlightStoreDirectory {
            get { return m_filterAndHighlightStoreDirectory; }
            set { m_filterAndHighlightStoreDirectory = value; }
        }

        private bool m_xRefWarningsToMain;

        /// <summary>
        /// Place warning messages in ods view
        /// </summary>
        public bool XRefWarningsToMain {
            get { return m_xRefWarningsToMain; }
            set { m_xRefWarningsToMain = value; }
        }

        private bool m_xRefAppInitialiseToMain = true;

        /// <summary>
        /// Place Application initialises in ods view
        /// </summary>
        public bool XRefAppInitialiseToMain {
            get { return m_xRefAppInitialiseToMain; }
            set { m_xRefAppInitialiseToMain = value; }
        }

        /// <summary>
        /// place assertions in ods view
        /// </summary>

        private bool m_xRefAssertionsToMain;

        public bool XRefAssertionsToMain {
            get { return m_xRefAssertionsToMain; }
            set { m_xRefAssertionsToMain = value; }
        }

        /// <summary>
        ///  place resource messages in ods view
        /// </summary>

        private bool m_xRefResourceMessagesToMain;

        public bool XRefResourceMessagesToMain {
            get { return m_xRefResourceMessagesToMain; }
            set { m_xRefResourceMessagesToMain = value; }
        }

        /// <summary>
        /// place verbose logs in ods view
        /// </summary>

        private bool m_xRefVerbLogsToMain;

        public bool XRefVerbLogsToMain {
            get { return m_xRefVerbLogsToMain; }
            set { m_xRefVerbLogsToMain = value; }
        }

        /// <summary>
        /// place mini logs in ods view
        /// </summary>

        private bool m_xRefMiniLogsToMain;

        public bool XRefMiniLogsToMain {
            get { return m_xRefMiniLogsToMain; }
            set { m_xRefMiniLogsToMain = value; }
        }

        /// <summary>
        /// place all logs into ods view
        /// </summary>

        private bool m_xRefLogsToMain;

        public bool XRefLogsToMain {
            get { return m_xRefLogsToMain; }
            set { m_xRefLogsToMain = value; }
        }

        /// <summary>
        /// place exceptions into ods view
        /// </summary>

        private bool m_xRefExceptionsToMain;

        public bool XRefExceptionsToMain {
            get { return m_xRefExceptionsToMain; }
            set { m_xRefExceptionsToMain = value; }
        }

        /// <summary>
        /// place errors in ods view
        /// </summary>

        private bool m_xRefErrorsToMain = true;

        public bool XRefErrorsToMain {
            get { return m_xRefErrorsToMain; }
            set { m_xRefErrorsToMain = value; }
        }

        /// <summary>
        /// putmatching pids into the application view
        /// </summary>

        private bool m_xRef_Reverse_PidsToEventEntries;// If a matching PID message comes through inject it into the event entry stream

        public bool XRefMatchingProcessIdsIntoEventEntries {
            get { return m_xRef_Reverse_PidsToEventEntries; }
            set { m_xRef_Reverse_PidsToEventEntries = value; }
        }

        /// <summary>
        ///  when matching pids occur copy them not move them
        /// </summary>

        private bool m_xRef_Reverse_CopyEnabled; // when doing this copy the entries dont move them.

        public bool XRefReverseCopyEnabled {
            get { return m_xRef_Reverse_CopyEnabled; }
            set { m_xRef_Reverse_CopyEnabled = value; }
        }

        /// <summary>
        /// dump notifications to log file
        /// </summary>

        private bool m_logNotifications;

        public bool LogNotifications {
            get { return m_logNotifications; }
            set { m_logNotifications = value; }
        }

        /// <summary>
        /// msgbox notifications
        /// </summary>

        private bool m_interactiveNotifications;

        public bool InteractiveNotifications {
            get { return m_interactiveNotifications; }
            set { m_interactiveNotifications = value; }
        }

        /// <summary>
        /// status bar notifications
        /// </summary>

        private bool m_statusBarNotifications;

        public bool StatusBarNotifications {
            get { return m_statusBarNotifications; }
            set { m_statusBarNotifications = value; }
        }

        /// <summary>
        /// delete all copies as well as the app on match
        /// </summary>

        private bool m_matchingNamePurgeAlsoClearsPartials = true;

        public bool MatchingNamePurgeAlsoClearsPartials {
            get { return m_matchingNamePurgeAlsoClearsPartials; }
            set { m_matchingNamePurgeAlsoClearsPartials = value; }
        }

        //public string OptionsStoreDirectory;

        /// <summary>
        /// Determines which type of thread information to use as the unique key.
        /// </summary>

        private ThreadDisplayMode m_threadDisplayOption;

        public ThreadDisplayMode ThreadDisplayOption {
            get { return m_threadDisplayOption; }
            set { m_threadDisplayOption = value; }
        }

        /// <summary>
        /// For .net threads allows names to be used instead of identities so that its cleaner.
        /// </summary>

        private bool m_useThreadNamingWhereverPossible;

        public bool UseThreadNamingWhereverPossible {
            get { return m_useThreadNamingWhereverPossible; }
            set { m_useThreadNamingWhereverPossible = value; }
        }

        /// <summary>
        /// When a mismatch is found in the timing entries this option will let mex look through all of the current timing entries and see
        /// if it can find the right match.  If it can not then it will throw away the exit.  If it can then it will throw away the enters
        /// unti it finds the match and highlight the fact that the match was invalid.
        /// </summary>

        private bool m_attemptToFixMismatchedTimingEntries;

        public bool AttemptToFixMismatchedTimingEntries {
            get { return m_attemptToFixMismatchedTimingEntries; }
            set { m_attemptToFixMismatchedTimingEntries = value; }
        }

        /// <summary>
        /// Decides whether thread information is saved with filters by default.
        /// </summary>

        private bool m_filterDefault_SaveThreads;

        public bool FilterDefaultSaveThreads {
            get { return m_filterDefault_SaveThreads; }
            set { m_filterDefault_SaveThreads = value; }
        }

        /// <summary>
        /// Decides whether Module information is saved with filters by default
        /// </summary>

        private bool m_filterDefault_SaveModules;

        public bool FilterDefaultSaveModules {
            get { return m_filterDefault_SaveModules; }
            set { m_filterDefault_SaveModules = value; }
        }

        /// <summary>
        /// Decides whether location data is saved with filters by default
        /// </summary>

        private bool m_filterDefault_SaveLocations;

        public bool FilterDefaultSaveLocations {
            get { return m_filterDefault_SaveLocations; }
            set { m_filterDefault_SaveLocations = value; }
        }

        /// <summary>
        /// Decides whether class location data is saved with filters by default
        /// </summary>

        private bool m_filterDefault_SaveClassLocs;

        public bool FilterDefaultSaveClassLocation {
            get { return m_filterDefault_SaveClassLocs; }
            set { m_filterDefault_SaveClassLocs = value; }
        }

        /// <summary>
        /// Determines whether a process based highlight should be applied to cross process view only.
        /// </summary>
        public bool CrossProcessViewHighlight {
            get;
            set;
        }

        /// <summary>
        /// Determines when extended details are ticked whether the application should walk back along the message to find the
        /// best position to display information from. If this is set to True then moreinfo and exception detail messages will
        /// walk back to their owning event.
        /// </summary>
        public bool EnableBackTrace {
            get;
            set;
        }

        /// <summary>
        /// Makes the timings view  assume that there is only one thread, this will allow you to use background worker threads and have
        /// the timer stop arrive on a different thread to the tiemrstart but will give you erronious results if more tha one thing
        /// occurs at once.
        /// </summary>

        private bool m_timingsViewIgnoresThreads;

        public bool TimingsViewIgnoresThreads {
            get { return m_timingsViewIgnoresThreads; }
            set { m_timingsViewIgnoresThreads = value; }
        }

        #region Options related methods.

        /// <summary>
        /// Returns a summary of the options that are currently loaded so that they can be displayed in the diagnostics view.
        /// </summary>
        /// <returns>A Rough summary of the options that are selected</returns>
        public string OptionsToString() {
            string result = string.Empty;
            result += "this.AttemptToFixMismatchedTimingEntries=" + this.AttemptToFixMismatchedTimingEntries.ToString() + Environment.NewLine;
            result += "this.AutoLoadHighlightDefaultOnStartup         =" + this.AutoLoadHighlightDefaultOnStartup.ToString() + Environment.NewLine; ;
            result += "this.AutoPurgeApplicationOnMatchingName        =" + this.AutoPurgeApplicationOnMatchingName.ToString() + Environment.NewLine; ;
            result += "this.AutoRefresh                               =" + this.AutoRefresh.ToString() + Environment.NewLine; ;
            result += "this.AutoScroll                                =" + this.AutoScroll.ToString() + Environment.NewLine; ;
            result += "this.BeautifyDisplayedStrings                  =" + this.BeautifyDisplayedStrings.ToString() + Environment.NewLine; ;
            result += "this.CurrentMexFilterNames                     =" + "";//this.CurrentMexFilterNames == null ? "Null" : this.CurrentMexFilterNames.Length.ToString() + Environment.NewLine; ;
            result += "this.CurrentMexHighlightNames                  =" + ""; // this.CurrentMexHighlightNames == null ? "Null" : this.CurrentMexFilterNames.Length.ToString() + Environment.NewLine; ;
            result += "this.DisplayInternalMessages                   =" + this.DisplayInternalMessages ?? "Null" + Environment.NewLine; ;
            result += "this.FilterAndHighlightStoreDirectory          =" + this.FilterAndHighlightStoreDirectory ?? "Null" + Environment.NewLine; ;
            result += "this.FilterDefaultProfileName                  =" + this.FilterFilenameToLoadOnStartup ?? "Null" + Environment.NewLine; ;
            result += "this.FilterExtension                           =" + this.FilterExtension.ToString() ?? "Null" + Environment.NewLine; ;
            result += "this.HighlightDefaultProfileName               =" + this.HighlightDefaultProfileName ?? "Null" + Environment.NewLine; ;
            result += "this.HighlightExtension                        =" + this.HighlightExtension ?? "Null" + Environment.NewLine; ;
            result += "this.Import_TextFileBehaviour                  =" + this.ImportTextFileBehaviour.ToString() ?? "Null" + Environment.NewLine; ;
            result += "this.InteractiveNotifications                  =" + this.InteractiveNotifications.ToString() ?? "Null" + Environment.NewLine; ;
            result += "this.IPAddressToBind                           =" + this.IPAddressToBind ?? "Null" + Environment.NewLine; ;
            result += "this.LogNotifications                          =" + this.LogNotifications.ToString() + Environment.NewLine; ;
            result += "this.MatchingNamePurgeAlsoClearsPartials       =" + this.MatchingNamePurgeAlsoClearsPartials.ToString() + Environment.NewLine; ;
            result += "this.NormalisationLimitMilliseconds            =" + this.NormalisationLimitMilliseconds.ToString() + Environment.NewLine; ;
            result += "this.NormaliseRefreshActive                    =" + this.NormaliseRefreshActive.ToString() + Environment.NewLine; ;
            result += "this.PortAddressToBind                         =" + this.PortAddressToBind.ToString() + Environment.NewLine; ;
            result += "this.PushbackCountDelayLimitForInteractiveJobs =" + this.PushbackCountDelayLimitForInteractiveJobs.ToString() + Environment.NewLine; ;
            result += "this.RespectFilter                             =" + this.RespectFilter.ToString() + Environment.NewLine; ;
            result += "this.SelectingProcessSelectsProcessView        =" + this.SelectingProcessSelectsProcessView.ToString() + Environment.NewLine; ;
            result += "this.AutoSelectFirstProcess                    =" + this.AutoSelectFirstProcess.ToString() + Environment.NewLine;
            result += "this.ShowGlobalIndexInView                     =" + this.ShowGlobalIndexInView.ToString() + Environment.NewLine; ;
            result += "this.StatusBarNotifications                    =" + this.StatusBarNotifications.ToString() + Environment.NewLine; ;
            result += "this.SupportCancellationOfRefresh              =" + this.SupportCancellationOfRefresh.ToString() + Environment.NewLine; ;
            result += "this.SupportCancellationRepeatCount            =" + this.SupportCancellationRepeatCount.ToString() + Environment.NewLine;
            result += "this.ThreadKeyType                             =" + this.ThreadDisplayOption.ToString() + Environment.NewLine; ;
            result += "this.TimedViewRespectsFilter                   =" + this.TimedViewRespectsFilter.ToString() + Environment.NewLine; ;
            result += "this.UseThreadNamingWhereverPossible           =" + this.UseThreadNamingWhereverPossible.ToString() + Environment.NewLine; ;
            result += "this.XRef_Reverse_CopyEnabled                  =" + this.XRefReverseCopyEnabled.ToString() + Environment.NewLine; ;
            result += "this.XRef_Reverse_PidsToEventEntries           =" + this.XRefMatchingProcessIdsIntoEventEntries.ToString() + Environment.NewLine; ;
            result += "this.XRefAppInitialiseToMain                   =" + this.XRefAppInitialiseToMain.ToString() + Environment.NewLine; ;
            result += "this.XRefAssertionsToMain                      =" + this.XRefAssertionsToMain.ToString() + Environment.NewLine; ;
            result += "this.XRefErrorsToMain                          =" + this.XRefErrorsToMain.ToString() + Environment.NewLine; ;
            result += "this.XRefExceptionsToMain                      =" + this.XRefExceptionsToMain.ToString() + Environment.NewLine; ;
            result += "this.XRefLogsToMain                            =" + this.XRefLogsToMain.ToString() + Environment.NewLine; ;
            result += "this.XRefMiniLogsToMain                        =" + this.XRefMiniLogsToMain.ToString() + Environment.NewLine; ;
            result += "this.XRefResourceMessagesToMain                =" + this.XRefResourceMessagesToMain.ToString() + Environment.NewLine; ;
            result += "this.XRefVerbLogsToMain                        =" + this.XRefVerbLogsToMain.ToString() + Environment.NewLine; ;
            result += "this.XRefWarningsToMain                        =" + this.XRefWarningsToMain.ToString() + Environment.NewLine; ;
            result += "this.FilterDefault_SaveClassLocs               =" + this.FilterDefaultSaveClassLocation.ToString() + Environment.NewLine;
            result += "this.FilterDefault_SaveLocations               =" + this.FilterDefaultSaveLocations.ToString() + Environment.NewLine;
            result += "this.FilterDefault_SaveModules                 =" + this.FilterDefaultSaveModules.ToString() + Environment.NewLine;
            result += "this.FilterDefault_SaveThreads                 =" + this.FilterDefaultSaveThreads.ToString() + Environment.NewLine;
            result += "this.CrossProcessViewHiglight                  =" + this.CrossProcessViewHighlight.ToString() + Environment.NewLine;
            result += "this.EnableBacktrace                           =" + this.EnableBackTrace.ToString() + Environment.NewLine;
            result += "this.ThreadDisplayOption                       =" + this.ThreadDisplayOption.ToString() + Environment.NewLine;
            result += "this.UsePreferredNameInsteadOfPid              =" + this.UsePreferredNameInsteadOfProcessId + Environment.NewLine;
            return result;
        }

        internal void SaveToFile(Stream savedest) {
            XmlSerializer xmls = new XmlSerializer(typeof(MexOptions));
            xmls.Serialize(savedest, this);
            savedest.Close();
        }

        private void PopulateFromMe(MexOptions src) {
            this.AutoLoadHighlightDefaultOnStartup = src.AutoLoadHighlightDefaultOnStartup;
            this.AutoPurgeApplicationOnMatchingName = src.AutoPurgeApplicationOnMatchingName;
            this.AutoRefresh = src.AutoRefresh;
            this.AutoScroll = src.AutoScroll;
            this.CurrentMexFilterNames = src.CurrentMexFilterNames;
            this.CurrentMexHighlightNames = src.CurrentMexHighlightNames;
            this.DisplayInternalMessages = src.DisplayInternalMessages;
            this.FilterAndHighlightStoreDirectory = src.FilterAndHighlightStoreDirectory;
            this.FilterFilenameToLoadOnStartup = src.FilterFilenameToLoadOnStartup;
            this.FilterExtension = src.FilterExtension;
            this.HighlightDefaultProfileName = src.HighlightDefaultProfileName;
            this.HighlightExtension = src.HighlightExtension;
            this.ImportTextFileBehaviour = src.ImportTextFileBehaviour;
            this.InteractiveNotifications = src.InteractiveNotifications;
            this.LogNotifications = src.LogNotifications;
            this.NormalisationLimitMilliseconds = src.NormalisationLimitMilliseconds;
            this.NormaliseRefreshActive = src.NormaliseRefreshActive;

            this.PushbackCountDelayLimitForInteractiveJobs = src.PushbackCountDelayLimitForInteractiveJobs;
            this.RespectFilter = src.RespectFilter;
            this.SelectingProcessSelectsProcessView = src.SelectingProcessSelectsProcessView;
            this.AutoSelectFirstProcess = src.AutoSelectFirstProcess;
            this.ShowGlobalIndexInView = src.ShowGlobalIndexInView;
            this.StatusBarNotifications = src.StatusBarNotifications;
            this.SupportCancellationOfRefresh = src.SupportCancellationOfRefresh;
            this.SupportCancellationRepeatCount = src.SupportCancellationRepeatCount;
            this.TimedViewRespectsFilter = src.TimedViewRespectsFilter;
            this.XRefReverseCopyEnabled = src.XRefReverseCopyEnabled;
            this.XRefMatchingProcessIdsIntoEventEntries = src.XRefMatchingProcessIdsIntoEventEntries;
            this.XRefAppInitialiseToMain = src.XRefAppInitialiseToMain;
            this.XRefAssertionsToMain = src.XRefAssertionsToMain;
            this.XRefErrorsToMain = src.XRefErrorsToMain;
            this.XRefExceptionsToMain = src.XRefExceptionsToMain;
            this.XRefLogsToMain = src.XRefLogsToMain;
            this.XRefMiniLogsToMain = src.XRefMiniLogsToMain;
            this.XRefResourceMessagesToMain = src.XRefResourceMessagesToMain;
            this.XRefVerbLogsToMain = src.XRefVerbLogsToMain;
            this.XRefWarningsToMain = src.XRefWarningsToMain;
            this.BeautifyDisplayedStrings = src.BeautifyDisplayedStrings;
            this.IPAddressToBind = src.IPAddressToBind;
            this.PortAddressToBind = src.PortAddressToBind;
            this.MatchingNamePurgeAlsoClearsPartials = src.MatchingNamePurgeAlsoClearsPartials;
            this.ThreadDisplayOption = src.ThreadDisplayOption;
            this.AttemptToFixMismatchedTimingEntries = src.AttemptToFixMismatchedTimingEntries;
            this.UseThreadNamingWhereverPossible = src.UseThreadNamingWhereverPossible;
            this.FilterDefaultSaveClassLocation = src.FilterDefaultSaveClassLocation;
            this.FilterDefaultSaveLocations = src.FilterDefaultSaveLocations;
            this.FilterDefaultSaveModules = src.FilterDefaultSaveModules;
            this.FilterDefaultSaveThreads = src.FilterDefaultSaveThreads;
            this.CrossProcessViewHighlight = src.CrossProcessViewHighlight;
            this.EnableBackTrace = src.EnableBackTrace;
            this.ThreadDisplayOption = src.ThreadDisplayOption;

            this.NoSecondsForRefreshOnImport = src.NoSecondsForUIUpdate;
            this.NoSecondsForUIUpdate = src.NoSecondsForUIUpdate;
            this.ResetRefreshOnStartup = src.ResetRefreshOnStartup;

            this.RemoveDuplicatesOnImport = src.RemoveDuplicatesOnImport;
            this.RemoveDuplicatesOnView = src.RemoveDuplicatesOnView;

            this.NoUserNotificationsToStoreInLog = src.NoUserNotificationsToStoreInLog;
            this.UsePreferredNameInsteadOfProcessId = src.UsePreferredNameInsteadOfProcessId;
        }

        /// <summary>
        /// Loads up all of the options and applys them to the application. NB this takes a stream and loads from it but does not
        /// close the stream.  It is assumed this is done by the caller.
        /// </summary>
        /// <param name="storeStream">Stream containing a serialised MexOptions structure</param>
        internal static MexOptions LoadFromFile(Stream storeStream) {
            MexOptions result = new MexOptions();

            // Make sure nothing gets missed.
            result.LoadOptionsDefaults();

           //Bilge.Log("About to load MexOptions settings from stream. ");
            XmlSerializer xmls = new XmlSerializer(typeof(MexOptions));
            result = (MexOptions)xmls.Deserialize(storeStream);
            if ((result.ResetRefreshOnStartup) && (result.NoSecondsForRefreshOnImport > Consts.REFRESHTIME_UNNACCEPTABLE)) {
                result.NoSecondsForRefreshOnImport = Consts.DEFAULT_UIREFRESHTIME;
            }
           //Bilge.Log("Load completed, options now imported");

            // Find all of the filter files

            int i; // loop counter

            if (!Directory.Exists(result.FilterAndHighlightStoreDirectory)) {
               //Bilge.Warning("Invalid filter directory found on startup, resetting to current directory");
                result.FilterAndHighlightStoreDirectory = Environment.CurrentDirectory;
            }

            string[] filenames = Directory.GetFiles(result.FilterAndHighlightStoreDirectory, result.FilterExtension);
            result.CurrentMexFilterNames = new string[filenames.Length];
            for (i = 0; i < filenames.Length; i++) {
                result.CurrentMexFilterNames[i] = Path.GetFileNameWithoutExtension(filenames[i]);
            }

            result.CurrentMexHighlightNames = new string[filenames.Length];
            for (i = 0; i < filenames.Length; i++) {
                result.CurrentMexHighlightNames[i] = Path.GetFileNameWithoutExtension(filenames[i]);
            }

            // then create tasks to ensure options are honoured
           //Bilge.Log("Filters and so on are found, applying options settings to the application");
            return result;
        }

        /// <summary>
        /// Synchronous method to apply changed options to the application, this method locks down all of the Mex Threads
        /// therefore should be used sparingly!
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        internal void ApplyOptionsToApplication(MexOptions newOptions, bool isStartup) {
           //Bilge.Log("Dispatching notificaiton event of options changed");
            MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(ViewSupportManager.UserMessages.BackgroundApplyOptionsBegins, ViewSupportManager.UserMessageType.InformationMessage, "");

            bool forceResetOfIPListener = false;
            bool fullRefreshRequired = false;

            // Some of the options occur during application startup.
            if (isStartup) {
                fullRefreshRequired = true;
                if ((newOptions.FilterFilenameToLoadOnStartup != null) && (newOptions.FilterFilenameToLoadOnStartup.Length > 0) && (File.Exists(newOptions.FilterFilenameToLoadOnStartup))) {
                    ViewFilter vf = ViewFilter.LoadFilterFromFile(newOptions.FilterFilenameToLoadOnStartup);
                    MexCore.TheCore.ViewManager.CurrentFilter = vf;
                }

                if ((newOptions.HighlightDefaultProfileName != null) && (newOptions.HighlightDefaultProfileName.Length > 0)) {
                    string[] matchedHighlights = newOptions.HighlightDefaultProfileName.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    XmlSerializer xmls = new XmlSerializer(typeof(AHighlightRequest));
                    foreach (string s in matchedHighlights) {
                        string path = Path.Combine(MexCore.TheCore.Options.FilterAndHighlightStoreDirectory, s + MexCore.TheCore.Options.HighlightExtension);
                        if (File.Exists(path)) {
                            using (FileStream fs = new FileStream(path, FileMode.Open)) {
                                AHighlightRequest loaded = (AHighlightRequest)xmls.Deserialize(fs);
                                fs.Close();
                                MexCore.TheCore.ViewManager.CurrentHighlightOptions.HighlightRequests.Add(loaded);
                                MexCore.TheCore.ViewManager.ReapplyHighlight();
                            }
                        }
                    }
                }
            }

            #region check the option differences and decide if a refresh or IP reset required

            // This is not thread safe but instead we suspend the other thread that could be accessing it
            if ((newOptions.IPAddressToBind != MexCore.TheCore.Options.IPAddressToBind) ||
                (newOptions.PortAddressToBind != MexCore.TheCore.Options.PortAddressToBind)) {
                forceResetOfIPListener = true;
            }

            if ((newOptions.BeautifyDisplayedStrings != MexCore.TheCore.Options.BeautifyDisplayedStrings)) {
                fullRefreshRequired = true;
            }
            if (newOptions.DisplayInternalMessages != MexCore.TheCore.Options.DisplayInternalMessages) {
                fullRefreshRequired = true;
            }
            if (newOptions.UsePreferredNameInsteadOfProcessId != MexCore.TheCore.Options.UsePreferredNameInsteadOfProcessId) {
                fullRefreshRequired = true;
            }

            #endregion check the option differences and decide if a refresh or IP reset required

            MexCore.TheCore.Options = newOptions;
            if (forceResetOfIPListener) {
                MexCore.TheCore.WorkManager.AddJob(new Job_ChangeTCPGathererState(false));
                MexCore.TheCore.WorkManager.AddJob(new Job_ChangeTCPGathererState(true));
            }
            if (fullRefreshRequired) {
                MexCore.TheCore.WorkManager.AddJob(new Job_NotifyRefreshRequired());
            }
            // When the threads come back online they should process any of the changes that are requested.
        } // End MexOptions.ApplyOptionstoApplication

        internal void LoadOptionsDefaults() {
            this.CurrentMexFilterNames = this.CurrentMexHighlightNames = new string[0];
            AutoSelectFirstProcess = true;
            SelectingProcessSelectsProcessView = true;

            NormaliseRefreshActive = false;
            NormalisationLimitMilliseconds = 0;
            ShowGlobalIndexInView = false;

            FilterAndHighlightStoreDirectory = Environment.CurrentDirectory;

            AutoPurgeApplicationOnMatchingName = true;
            PushbackCountDelayLimitForInteractiveJobs = 10;
            AutoRefresh = true;
            AutoScroll = true;
            RespectFilter = true;  // right now this is global but could be per screen
            DisplayInternalMessages = false; // Internal messages not normally necessary
            HighlightDefaultProfileName = null;
            FilterFilenameToLoadOnStartup = null;
            XRefAppInitialiseToMain = true;
            XRefAssertionsToMain = false;
            XRefResourceMessagesToMain = false;
            XRefVerbLogsToMain = false;
            XRefMiniLogsToMain = false;
            XRefLogsToMain = false;
            XRefExceptionsToMain = false;
            XRefErrorsToMain = true;
            XRefWarningsToMain = false;
            XRefMatchingProcessIdsIntoEventEntries = true; // If a matching PID message comes through inject it into the event entry stream
            XRefReverseCopyEnabled = false; // when doing this copy the entries dont move them.
            LogNotifications = true;
            InteractiveNotifications = false;
            StatusBarNotifications = true;
            BeautifyDisplayedStrings = true;
            MatchingNamePurgeAlsoClearsPartials = true;
            IPAddressToBind = FlimFlamConstants.DEFAULTIP_ADDRESS;
            PortAddressToBind = FlimFlamConstants.DEFAULTPORT_NUMBER;
            this.ThreadDisplayOption = ThreadDisplayMode.UseDotNetThread;
            this.AttemptToFixMismatchedTimingEntries = true;
            this.UseThreadNamingWhereverPossible = true;
            this.FilterDefaultSaveClassLocation = true;
            this.FilterDefaultSaveLocations = true;
            this.FilterDefaultSaveModules = false;
            this.FilterDefaultSaveThreads = false;
            this.CrossProcessViewHighlight = true;
            this.EnableBackTrace = true;
            this.UsePreferredNameInsteadOfProcessId = true;
            this.removeDupesOnImport = true;
            this.removeDupesOnView = false;
        }

        internal List<String> m_workstationNameMappings = new List<string>();

        internal void AddNameMappingForWorkstation(string phyiscalName, string displayName) {
            m_workstationNameMappings.Add(phyiscalName + "\\" + displayName);
        }

        /// <summary>
        /// Provides a display name mapping for a specific host name so that the display reflects a user preference to display
        /// a specific name instead of the hostname of the machine that the trace message came from.
        /// </summary>
        /// <param name="hostName">The hostname of the machine to be mapped.</param>
        /// <returns>A prefered display name</returns>
        internal string GetPreferredWorkstationName(string hostName) {
            hostName = hostName.ToLower();

            foreach (string s in m_workstationNameMappings) {
                string[] splt = s.Split('\\');
                string host = splt[0];
                string replace = splt[1];
                if (host == hostName) {
                    return replace;
                }
            }
            // TODO : implement this
            throw new NotImplementedException();
        }

        internal MexOptions() {
            LoadOptionsDefaults();
        }

        internal MexOptions(MexOptions mo) {
            LoadOptionsDefaults();
            this.PopulateFromMe(mo);
        }

        #endregion Options related methods.

        */
    }
}