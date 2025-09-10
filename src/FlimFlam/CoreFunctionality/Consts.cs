namespace Plisky.FlimFlam;

internal static class Consts {

    /// <summary>
    /// At which point is a refresh unacceptably slow and should be reset on startup.
    /// </summary>
    internal const int REFRESHTIME_UNNACCEPTABLE = 5;

    internal const int DEFAULT_UIREFRESHTIME = 0;

    internal const int MS_TIMEOUTFORLOCKS = 125000;  // The app cant actually handle not getting a timeout, therefore it looses messages after two mins.
    internal const string OPTIONS_FILENAME = "MexOptions.Mex";

    internal const string CLMNHDR_THREADID = "Thread Id";
    internal const string CLMNHDR_GLOIDX = "Index";
    internal const string CLMNHDR_DBGMSG = "Event Entry";
    internal const string CLMNHDR_ADDLOC = "Location";
    internal const string CLMNHDR_PHYSLOC = "Physical Location";

    internal const string PROCNAMEIDENT_PREFIX = "ProcessName(";
    internal const string PROCNAMEIDENT_POSTFIX = ")";
}

#if DEBUG

internal static class DebugMexConsts {
    internal const string DATASTRUCTURESLOCK_RESNAME = "MainDataStructuresLock";
}

#endif
