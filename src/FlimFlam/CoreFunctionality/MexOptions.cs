//using Plisky.Plumbing.Legacy;
namespace Plisky.FlimFlam;

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Plisky.Diagnostics.FlimFlam;
using Plisky.Plumbing;

/// <summary>
/// Summary description for MexOptions.  Class must be public for serialization into the iso store.
/// </summary>
public class MexOptions {
    internal List<string> workstationNameMappings = new();
    private bool filterDefault_SaveThreads;
    private int pushbackCountDelayLimitForInteractiveJobs = 10;
    private bool xRefWarningsToMain;

    // when doing this copy the entries dont move them.
    internal MexOptions() {
        LoadOptionsDefaults();
    }

    // If a matching PID message comes through inject it into the event entry stream
    /// <summary>
    ///  when matching pids occur copy them not move them
    /// </summary>
    internal MexOptions(MexOptions mo) {
        LoadOptionsDefaults();
        PopulateFromMe(mo);
    }

    /// <summary>
    /// The identifier that is used to work out which lines within an ADPLus style log are identifiers
    /// </summary>
    public string ADPlusImportIdentifierToSplitTags { get; } = "Debug session time: ";

    /// <summary>
    /// When a mismatch is found in the timing entries this option will let mex look through all of the current timing entries and see
    /// if it can find the right match.  If it can not then it will throw away the exit.  If it can then it will throw away the enters
    /// unti it finds the match and highlight the fact that the match was invalid.
    /// </summary>
    public bool AttemptToFixMismatchedTimingEntries { get; set; }

    /// <summary>
    /// Do we load the default highlighting on startup
    /// </summary>
    public bool AutoLoadHighlightDefaultOnStartup { get; set; }

    /// <summary>
    /// Automatically clear the application data when the same named application arrives in the trace stream
    /// </summary>
    public bool AutoPurgeApplicationOnMatchingName { get; set; } = true;

    /// <summary>
    /// should new messages automatically find their way into the view
    /// </summary>
    public bool AutoRefresh { get; set; } = true;

    /// <summary>
    /// should the view try and keep up with new messages arriving
    /// </summary>
    public bool AutoScroll { get; set; } = true;

    /// <summary>
    /// Selects the process view when the first process is entered into Mex, even if the option to select
    /// process view on process change is not ticked.
    /// </summary>
    public bool AutoSelectFirstProcess { get; set; } = true;

    /// <summary>
    /// Removes unprintable characters and puts their replacenments into the displayed strings.
    /// </summary>
    public bool BeautifyDisplayedStrings { get; set; }

    /// <summary>
    /// Determines whether a process based highlight should be applied to cross process view only.
    /// </summary>
    public bool CrossProcessViewHighlight {
        get;
        set;
    }

    public string CurrentFilename { get; set; }

    public string[] CurrentMexFilterNames { get; set; }

    public string[] CurrentMexHighlightNames { get; set; }

    /// <summary>
    /// Dont know.
    /// </summary>
    public bool DisplayInternalMessages { get; set; }

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
    /// Directory for storing filters and highlights
    /// </summary>
    public string FilterAndHighlightStoreDirectory { get; set; }

    /// <summary>
    /// Decides whether class location data is saved with filters by default
    /// </summary>
    public bool FilterDefaultSaveClassLocation { get; set; }

    /// <summary>
    /// Decides whether location data is saved with filters by default
    /// </summary>
    public bool FilterDefaultSaveLocations { get; set; }

    /// <summary>
    /// Decides whether Module information is saved with filters by default
    /// </summary>
    public bool FilterDefaultSaveModules { get; set; }

    /// <summary>
    /// Decides whether thread information is saved with filters by default.
    /// </summary>
    public bool FilterDefaultSaveThreads {
        get { return filterDefault_SaveThreads; }
        set { filterDefault_SaveThreads = value; }
    }

    /// <summary>
    /// Extension for filters
    /// </summary>
    public string FilterExtension { get; set; } = ".mx_flt";

    /// <summary>
    /// The filter profile that is to be loaded on startup.
    /// </summary>
    public string FilterFilenameToLoadOnStartup { get; set; }

    /// <summary>
    /// The profile for highlighting that is to be loaded on startup
    /// </summary>
    public string HighlightDefaultProfileName { get; set; }

    // Holds names of the files of filters
    /// <summary>
    /// Extension for highlights
    /// </summary>
    public string HighlightExtension { get; set; } = ".mx_hlt";

    /// <summary>
    /// The control determining how the log files are read into mex
    /// </summary>
    public FileImportMethod ImportTextFileBehaviour { get; set; } = FileImportMethod.TextWriterWithTexSupport;

    /// <summary>
    /// msgbox notifications
    /// </summary>
    public bool InteractiveNotifications { get; set; }

    /// <summary>
    ///  ip address to listen on * for any
    /// </summary>
    public string IPAddressToBind { get; set; }

    /// <summary>
    /// dump notifications to log file
    /// </summary>
    public bool LogNotifications { get; set; }

    /// <summary>
    /// delete all copies as well as the app on match
    /// </summary>
    public bool MatchingNamePurgeAlsoClearsPartials { get; set; } = true;

    /// <summary>
    /// Decides how many queued messages justify the creation of additional threads
    /// </summary>
    public int MessagesToSpawnThreadsFor { get; set; } = 200000;

    /// <summary>
    /// This is how frequently the refresh is applied if normalisation is being applied.
    /// </summary>
    public int NormalisationLimitMilliseconds { get; set; }

    /// <summary>
    /// Makes sure that a refresh doesent occur more frequently than X number of seconds.  This allows you to slow down the
    /// jitter when lots of messages are streaming at the viewer.
    /// </summary>
    public bool NormaliseRefreshActive { get; set; }

    /// <summary>
    ///  determines how long an import runs before a refresh of the application should occur
    /// </summary>
    public int NoSecondsForRefreshOnImport { get; set; } = 45;

    /// <summary>
    ///  Decides how long the application should wait before the UI is refreshed
    /// </summary>
    public int NoSecondsForUIUpdate { get; set; }

    // Default zero
    /// <summary>
    /// Decides how many user notifications are stored in mex.
    /// </summary>
    public int NoUserNotificationsToStoreInLog { get; set; } = 150;

    public bool PersistEverything { get; set; }

    /// <summary>
    /// port to listen on
    /// </summary>
    public int PortAddressToBind { get; set; }

    /// <summary>
    /// How many times a job can be moved back up the queue
    /// </summary>
    public int PushbackCountDelayLimitForInteractiveJobs {
        get { return pushbackCountDelayLimitForInteractiveJobs; }
        set { pushbackCountDelayLimitForInteractiveJobs = value; }
    }

    /// <summary>
    /// Removes duplicate entries when arriving in Mex, reducing the overhead of having two listeners
    /// installed.
    /// </summary>
    public bool RemoveDuplicatesOnImport { get; set; } = true;

    /// <summary>
    /// Removes duplicate entries when being displayed in Mex, be careful when using this with code that loops
    /// </summary>
    public bool RemoveDuplicatesOnView { get; set; }

    /// <summary>
    /// Gets or Sets a flag to indicate whether or not Mex should reset the filter on start up if the refresh
    /// time is greater than 2 seconds.
    /// </summary>
    public bool ResetRefreshOnStartup { get; set; } = true;

    public bool RespectFilter { get; set; } = true;

    /// <summary>
    /// When you choose a process does it change the view to process view
    /// </summary>
    public bool SelectingProcessSelectsProcessView { get; set; } = true;

    // right now this is global but could be per screen
    // Holds names of the files of highlights
    /// <summary>
    /// A list of known filter names
    /// </summary>
    /// <summary>
    /// Determines whether process view shows you the global index of the message
    /// </summary>
    public bool ShowGlobalIndexInView { get; set; }

    /// <summary>
    /// status bar notifications
    /// </summary>
    public bool StatusBarNotifications { get; set; }

    /// <summary>
    /// allow the cancel refresh button to function
    /// </summary>
    public bool SupportCancellationOfRefresh { get; set; } = true;

    /// <summary>
    /// Determines how responsive the cancellation request is, lower numbers are more responsive but slower.
    /// </summary>
    public int SupportCancellationRepeatCount { get; set; } = 50;

    /// <summary>
    /// Determines which type of thread information to use as the unique key.
    /// </summary>
    public ThreadDisplayMode ThreadDisplayOption { get; set; }

    /// <summary>
    /// Should timed view respect the filter or dump everything to the screen.
    /// </summary>
    public bool TimedViewRespectsFilter { get; set; }

    /// <summary>
    /// Makes the timings view  assume that there is only one thread, this will allow you to use background worker threads and have
    /// the timer stop arrive on a different thread to the tiemrstart but will give you erronious results if more tha one thing
    /// occurs at once.
    /// </summary>
    public bool TimingsViewIgnoresThreads { get; set; }

    /// <summary>
    /// Gets or Sets an option to use a display name rather than a process identifier when Mex has been
    /// sent the display name.
    /// </summary>
    public bool UsePreferredNameInsteadOfProcessId { get; set; }

    [XmlIgnore]
    public Dictionary<string, string> UserDefaults { get; set; } = new Dictionary<string, string>();

    //public string OptionsStoreDirectory;
    /// <summary>
    /// For .net threads allows names to be used instead of identities so that its cleaner.
    /// </summary>
    public bool UseThreadNamingWhereverPossible { get; set; }

    /// <summary>
    /// Place Application initialises in ods view
    /// </summary>
    public bool XRefAppInitialiseToMain { get; set; } = true;

    /// <summary>
    /// place assertions in ods view
    /// </summary>
    public bool XRefAssertionsToMain { get; set; }

    /// <summary>
    /// place errors in ods view
    /// </summary>
    public bool XRefErrorsToMain { get; set; } = true;

    /// <summary>
    /// place exceptions into ods view
    /// </summary>
    public bool XRefExceptionsToMain { get; set; }

    /// <summary>
    /// place all logs into ods view
    /// </summary>
    public bool XRefLogsToMain { get; set; }

    public bool XRefMatchingProcessIdsIntoEventEntries { get; set; }

    /// <summary>
    /// place mini logs in ods view
    /// </summary>
    public bool XRefMiniLogsToMain { get; set; }

    /// <summary>
    ///  place resource messages in ods view
    /// </summary>
    public bool XRefResourceMessagesToMain { get; set; }

    public bool XRefReverseCopyEnabled { get; set; }

    /// <summary>
    /// place verbose logs in ods view
    /// </summary>
    public bool XRefVerbLogsToMain { get; set; }

    // Default false
    /// <summary>
    /// Global on / off for the filter
    /// </summary>
    /// <summary>
    /// Place warning messages in ods view
    /// </summary>
    public bool XRefWarningsToMain {
        get { return xRefWarningsToMain; }
        set { xRefWarningsToMain = value; }
    }

    /// <summary>
    /// putmatching pids into the application view
    /// </summary>
    /// <summary>
    /// Returns a summary of the options that are currently loaded so that they can be displayed in the diagnostics view.
    /// </summary>
    /// <returns>A Rough summary of the options that are selected</returns>
    public string OptionsToString() {
        string result = string.Empty;
        result += "this.AttemptToFixMismatchedTimingEntries=" + AttemptToFixMismatchedTimingEntries.ToString() + Environment.NewLine;
        result += "this.AutoLoadHighlightDefaultOnStartup         =" + AutoLoadHighlightDefaultOnStartup.ToString() + Environment.NewLine; ;
        result += "this.AutoPurgeApplicationOnMatchingName        =" + AutoPurgeApplicationOnMatchingName.ToString() + Environment.NewLine; ;
        result += "this.AutoRefresh                               =" + AutoRefresh.ToString() + Environment.NewLine; ;
        result += "this.AutoScroll                                =" + AutoScroll.ToString() + Environment.NewLine; ;
        result += "this.BeautifyDisplayedStrings                  =" + BeautifyDisplayedStrings.ToString() + Environment.NewLine; ;
        result += "this.CurrentMexFilterNames                     =" + "";//this.CurrentMexFilterNames == null ? "Null" : this.CurrentMexFilterNames.Length.ToString() + Environment.NewLine; ;
        result += "this.CurrentMexHighlightNames                  =" + ""; // this.CurrentMexHighlightNames == null ? "Null" : this.CurrentMexFilterNames.Length.ToString() + Environment.NewLine; ;
        result += "this.DisplayInternalMessages                   =" + DisplayInternalMessages ?? "Null" + Environment.NewLine; ;
        result += "this.FilterAndHighlightStoreDirectory          =" + FilterAndHighlightStoreDirectory ?? "Null" + Environment.NewLine; ;
        result += "this.FilterDefaultProfileName                  =" + FilterFilenameToLoadOnStartup ?? "Null" + Environment.NewLine; ;
        result += "this.FilterExtension                           =" + FilterExtension.ToString() ?? "Null" + Environment.NewLine; ;
        result += "this.HighlightDefaultProfileName               =" + HighlightDefaultProfileName ?? "Null" + Environment.NewLine; ;
        result += "this.HighlightExtension                        =" + HighlightExtension ?? "Null" + Environment.NewLine; ;
        result += "this.Import_TextFileBehaviour                  =" + ImportTextFileBehaviour.ToString() ?? "Null" + Environment.NewLine; ;
        result += "this.InteractiveNotifications                  =" + InteractiveNotifications.ToString() ?? "Null" + Environment.NewLine; ;
        result += "this.IPAddressToBind                           =" + IPAddressToBind ?? "Null" + Environment.NewLine; ;
        result += "this.LogNotifications                          =" + LogNotifications.ToString() + Environment.NewLine; ;
        result += "this.MatchingNamePurgeAlsoClearsPartials       =" + MatchingNamePurgeAlsoClearsPartials.ToString() + Environment.NewLine; ;
        result += "this.NormalisationLimitMilliseconds            =" + NormalisationLimitMilliseconds.ToString() + Environment.NewLine; ;
        result += "this.NormaliseRefreshActive                    =" + NormaliseRefreshActive.ToString() + Environment.NewLine; ;
        result += "this.PortAddressToBind                         =" + PortAddressToBind.ToString() + Environment.NewLine; ;
        result += "this.PushbackCountDelayLimitForInteractiveJobs =" + PushbackCountDelayLimitForInteractiveJobs.ToString() + Environment.NewLine; ;
        result += "this.RespectFilter                             =" + RespectFilter.ToString() + Environment.NewLine; ;
        result += "this.SelectingProcessSelectsProcessView        =" + SelectingProcessSelectsProcessView.ToString() + Environment.NewLine; ;
        result += "this.AutoSelectFirstProcess                    =" + AutoSelectFirstProcess.ToString() + Environment.NewLine;
        result += "this.ShowGlobalIndexInView                     =" + ShowGlobalIndexInView.ToString() + Environment.NewLine; ;
        result += "this.StatusBarNotifications                    =" + StatusBarNotifications.ToString() + Environment.NewLine; ;
        result += "this.SupportCancellationOfRefresh              =" + SupportCancellationOfRefresh.ToString() + Environment.NewLine; ;
        result += "this.SupportCancellationRepeatCount            =" + SupportCancellationRepeatCount.ToString() + Environment.NewLine;
        result += "this.ThreadKeyType                             =" + ThreadDisplayOption.ToString() + Environment.NewLine; ;
        result += "this.TimedViewRespectsFilter                   =" + TimedViewRespectsFilter.ToString() + Environment.NewLine; ;
        result += "this.UseThreadNamingWhereverPossible           =" + UseThreadNamingWhereverPossible.ToString() + Environment.NewLine; ;
        result += "this.XRef_Reverse_CopyEnabled                  =" + XRefReverseCopyEnabled.ToString() + Environment.NewLine; ;
        result += "this.XRef_Reverse_PidsToEventEntries           =" + XRefMatchingProcessIdsIntoEventEntries.ToString() + Environment.NewLine; ;
        result += "this.XRefAppInitialiseToMain                   =" + XRefAppInitialiseToMain.ToString() + Environment.NewLine; ;
        result += "this.XRefAssertionsToMain                      =" + XRefAssertionsToMain.ToString() + Environment.NewLine; ;
        result += "this.XRefErrorsToMain                          =" + XRefErrorsToMain.ToString() + Environment.NewLine; ;
        result += "this.XRefExceptionsToMain                      =" + XRefExceptionsToMain.ToString() + Environment.NewLine; ;
        result += "this.XRefLogsToMain                            =" + XRefLogsToMain.ToString() + Environment.NewLine; ;
        result += "this.XRefMiniLogsToMain                        =" + XRefMiniLogsToMain.ToString() + Environment.NewLine; ;
        result += "this.XRefResourceMessagesToMain                =" + XRefResourceMessagesToMain.ToString() + Environment.NewLine; ;
        result += "this.XRefVerbLogsToMain                        =" + XRefVerbLogsToMain.ToString() + Environment.NewLine; ;
        result += "this.XRefWarningsToMain                        =" + XRefWarningsToMain.ToString() + Environment.NewLine; ;
        result += "this.FilterDefault_SaveClassLocs               =" + FilterDefaultSaveClassLocation.ToString() + Environment.NewLine;
        result += "this.FilterDefault_SaveLocations               =" + FilterDefaultSaveLocations.ToString() + Environment.NewLine;
        result += "this.FilterDefault_SaveModules                 =" + FilterDefaultSaveModules.ToString() + Environment.NewLine;
        result += "this.FilterDefault_SaveThreads                 =" + FilterDefaultSaveThreads.ToString() + Environment.NewLine;
        result += "this.CrossProcessViewHiglight                  =" + CrossProcessViewHighlight.ToString() + Environment.NewLine;
        result += "this.EnableBacktrace                           =" + EnableBackTrace.ToString() + Environment.NewLine;
        result += "this.ThreadDisplayOption                       =" + ThreadDisplayOption.ToString() + Environment.NewLine;
        result += "this.UsePreferredNameInsteadOfPid              =" + UsePreferredNameInsteadOfProcessId + Environment.NewLine;
        return result;
    }

    /// <summary>
    /// Loads up all of the options and applys them to the application. NB this takes a stream and loads from it but does not
    /// close the stream.  It is assumed this is done by the caller.
    /// </summary>
    /// <param name="storeStream">Stream containing a serialised MexOptions structure</param>
    internal static MexOptions LoadFromFile(Stream storeStream) {
        var result = new MexOptions();

        // Make sure nothing gets missed.
        result.LoadOptionsDefaults();

        //Bilge.Log("About to load MexOptions settings from stream. ");
        var xmls = new XmlSerializer(typeof(MexOptions));
        result = (MexOptions)xmls.Deserialize(storeStream);
        if (result.ResetRefreshOnStartup && result.NoSecondsForRefreshOnImport > Consts.REFRESHTIME_UNNACCEPTABLE) {
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

    internal void AddNameMappingForWorkstation(string phyiscalName, string displayName) {
        workstationNameMappings.Add(phyiscalName + "\\" + displayName);
    }

    /// <summary>
    /// Synchronous method to apply changed options to the application, this method locks down all of the Mex Threads
    /// therefore should be used sparingly!
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
    internal void ApplyOptionsToApplication(MexOptions newOptions, bool isStartup) {
        //Bilge.Log("Dispatching notificaiton event of options changed");
        MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.BackgroundApplyOptionsBegins, UserMessageType.InformationMessage, "");

        bool forceResetOfIPListener = false;
        bool fullRefreshRequired = false;

        // Some of the options occur during application startup.
        if (isStartup) {
            fullRefreshRequired = true;
            if (newOptions.FilterFilenameToLoadOnStartup != null && newOptions.FilterFilenameToLoadOnStartup.Length > 0 && File.Exists(newOptions.FilterFilenameToLoadOnStartup)) {
                var vf = ViewFilter.LoadFilterFromFile(newOptions.FilterFilenameToLoadOnStartup);
                MexCore.TheCore.ViewManager.CurrentFilter = vf;
            }

            if (newOptions.HighlightDefaultProfileName != null && newOptions.HighlightDefaultProfileName.Length > 0) {
                string[] matchedHighlights = newOptions.HighlightDefaultProfileName.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                var xmls = new XmlSerializer(typeof(AHighlightRequest));
                foreach (string s in matchedHighlights) {
                    string path = Path.Combine(MexCore.TheCore.Options.FilterAndHighlightStoreDirectory, s + MexCore.TheCore.Options.HighlightExtension);
                    if (File.Exists(path)) {
                        using var fs = new FileStream(path, FileMode.Open);
                        var loaded = (AHighlightRequest)xmls.Deserialize(fs);
                        fs.Close();
                        MexCore.TheCore.ViewManager.CurrentHighlightOptions.highlightRequests.Add(loaded);
                        MexCore.TheCore.ViewManager.ReapplyHighlight();
                    }
                }
            }
        }

        #region check the option differences and decide if a refresh or IP reset required

        // This is not thread safe but instead we suspend the other thread that could be accessing it
        if (newOptions.IPAddressToBind != MexCore.TheCore.Options.IPAddressToBind ||
            newOptions.PortAddressToBind != MexCore.TheCore.Options.PortAddressToBind) {
            forceResetOfIPListener = true;
        }

        if (newOptions.BeautifyDisplayedStrings != MexCore.TheCore.Options.BeautifyDisplayedStrings) {
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
    }

    /// <summary>
    /// Provides a display name mapping for a specific host name so that the display reflects a user preference to display
    /// a specific name instead of the hostname of the machine that the trace message came from.
    /// </summary>
    /// <param name="hostName">The hostname of the machine to be mapped.</param>
    /// <returns>A prefered display name</returns>
    internal string GetPreferredWorkstationName(string hostName) {
        hostName = hostName.ToLower();

        foreach (string s in workstationNameMappings) {
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

    internal void LoadOptionsDefaults() {
        CurrentMexFilterNames = CurrentMexHighlightNames = new string[0];
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
        ThreadDisplayOption = ThreadDisplayMode.UseDotNetThread;
        AttemptToFixMismatchedTimingEntries = true;
        UseThreadNamingWhereverPossible = true;
        FilterDefaultSaveClassLocation = true;
        FilterDefaultSaveLocations = true;
        FilterDefaultSaveModules = false;
        FilterDefaultSaveThreads = false;
        CrossProcessViewHighlight = true;
        EnableBackTrace = true;
        UsePreferredNameInsteadOfProcessId = true;
        RemoveDuplicatesOnImport = true;
        RemoveDuplicatesOnView = false;
    }

    internal void SaveToFile(Stream savedest) {
        var xmls = new XmlSerializer(typeof(MexOptions));
        xmls.Serialize(savedest, this);
        savedest.Close();
    }

    private void PopulateFromMe(MexOptions src) {
        AutoLoadHighlightDefaultOnStartup = src.AutoLoadHighlightDefaultOnStartup;
        AutoPurgeApplicationOnMatchingName = src.AutoPurgeApplicationOnMatchingName;
        AutoRefresh = src.AutoRefresh;
        AutoScroll = src.AutoScroll;
        CurrentMexFilterNames = src.CurrentMexFilterNames;
        CurrentMexHighlightNames = src.CurrentMexHighlightNames;
        DisplayInternalMessages = src.DisplayInternalMessages;
        FilterAndHighlightStoreDirectory = src.FilterAndHighlightStoreDirectory;
        FilterFilenameToLoadOnStartup = src.FilterFilenameToLoadOnStartup;
        FilterExtension = src.FilterExtension;
        HighlightDefaultProfileName = src.HighlightDefaultProfileName;
        HighlightExtension = src.HighlightExtension;
        ImportTextFileBehaviour = src.ImportTextFileBehaviour;
        InteractiveNotifications = src.InteractiveNotifications;
        LogNotifications = src.LogNotifications;
        NormalisationLimitMilliseconds = src.NormalisationLimitMilliseconds;
        NormaliseRefreshActive = src.NormaliseRefreshActive;

        PushbackCountDelayLimitForInteractiveJobs = src.PushbackCountDelayLimitForInteractiveJobs;
        RespectFilter = src.RespectFilter;
        SelectingProcessSelectsProcessView = src.SelectingProcessSelectsProcessView;
        AutoSelectFirstProcess = src.AutoSelectFirstProcess;
        ShowGlobalIndexInView = src.ShowGlobalIndexInView;
        StatusBarNotifications = src.StatusBarNotifications;
        SupportCancellationOfRefresh = src.SupportCancellationOfRefresh;
        SupportCancellationRepeatCount = src.SupportCancellationRepeatCount;
        TimedViewRespectsFilter = src.TimedViewRespectsFilter;
        XRefReverseCopyEnabled = src.XRefReverseCopyEnabled;
        XRefMatchingProcessIdsIntoEventEntries = src.XRefMatchingProcessIdsIntoEventEntries;
        XRefAppInitialiseToMain = src.XRefAppInitialiseToMain;
        XRefAssertionsToMain = src.XRefAssertionsToMain;
        XRefErrorsToMain = src.XRefErrorsToMain;
        XRefExceptionsToMain = src.XRefExceptionsToMain;
        XRefLogsToMain = src.XRefLogsToMain;
        XRefMiniLogsToMain = src.XRefMiniLogsToMain;
        XRefResourceMessagesToMain = src.XRefResourceMessagesToMain;
        XRefVerbLogsToMain = src.XRefVerbLogsToMain;
        XRefWarningsToMain = src.XRefWarningsToMain;
        BeautifyDisplayedStrings = src.BeautifyDisplayedStrings;
        IPAddressToBind = src.IPAddressToBind;
        PortAddressToBind = src.PortAddressToBind;
        MatchingNamePurgeAlsoClearsPartials = src.MatchingNamePurgeAlsoClearsPartials;
        ThreadDisplayOption = src.ThreadDisplayOption;
        AttemptToFixMismatchedTimingEntries = src.AttemptToFixMismatchedTimingEntries;
        UseThreadNamingWhereverPossible = src.UseThreadNamingWhereverPossible;
        FilterDefaultSaveClassLocation = src.FilterDefaultSaveClassLocation;
        FilterDefaultSaveLocations = src.FilterDefaultSaveLocations;
        FilterDefaultSaveModules = src.FilterDefaultSaveModules;
        FilterDefaultSaveThreads = src.FilterDefaultSaveThreads;
        CrossProcessViewHighlight = src.CrossProcessViewHighlight;
        EnableBackTrace = src.EnableBackTrace;
        ThreadDisplayOption = src.ThreadDisplayOption;

        NoSecondsForRefreshOnImport = src.NoSecondsForUIUpdate;
        NoSecondsForUIUpdate = src.NoSecondsForUIUpdate;
        ResetRefreshOnStartup = src.ResetRefreshOnStartup;

        RemoveDuplicatesOnImport = src.RemoveDuplicatesOnImport;
        RemoveDuplicatesOnView = src.RemoveDuplicatesOnView;

        NoUserNotificationsToStoreInLog = src.NoUserNotificationsToStoreInLog;
        UsePreferredNameInsteadOfProcessId = src.UsePreferredNameInsteadOfProcessId;
    }

    // End MexOptions.ApplyOptionstoApplication
}