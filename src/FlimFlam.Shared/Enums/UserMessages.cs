namespace Plisky.FlimFlam {

    /// <summary>
    /// The file import method determines how text files are read into the emx viewer.  This class must be public so that it can
    /// be serialised as an option within the mex options
    /// </summary>
    public enum FileImportMethod {

        /// <summary>
        /// This file format looks for CRLFs and assumes that they are part of the line that preceeds them.
        /// </summary>
        TextWriterWithTexSupport,

        /// <summary>
        /// ADPlusLog looks for elements of the log that it can use.
        /// </summary>
        ADPlusLog,

        /// <summary>
        /// Assumes that this is a raw log taken from output debug string.
        /// </summary>
        GenericLog,

        /// <summary>
        /// Assumes that debug view is used to capture the standard T output.
        /// </summary>
        DebugViewTexLog
    }

    /// <summary>
    /// Determines what thread information Mex will treat as the primary thread information for following thread streams.
    /// </summary>
    public enum ThreadDisplayMode {

        /// <summary>
        /// Use the .net numeric reference for the thread as the key
        /// </summary>
        UseDotNetThread,

        /// <summary>
        /// Use the operating system thread identifier as the key
        /// </summary>
        UseOSThread,

        /// <summary>
        /// Combine the operating system ident and dotnet name together.
        /// </summary>
        DefaultUseThreadNameAndOS,

        /// <summary>
        /// Use the threadname available from the traced application wherever possible.
        /// </summary>
        ShowFullInformation
    }

    public enum UserMessages {
        UnhandledExceptionOccured = 0x0000,
        NoThreadsSelectedForView = 0x0001,
        ErrorDuringfileImport = 0x0002,
        CorruptStringFound = 0x0003,
        InvalidTruncateStringFound = 0x0004,
        FilterDirectoryChangeFailedToRelocateFilters = 0x0005,
        FilterFileNotFound = 0x0006,
        DefaultFilterFileNotFound = 0x0007,
        WaitTimeoutOccured = 0x0008,
        OptionsConfiguraitonError = 0x0009,
        InvalidDataStructureError = 0x000A,
        GeneralUIError = 0x000B,

        BackgroundHighlightOperationBegins = 0x1001,
        BackgroundHighlightOperationEnds = 0x1002,
        HighlightOperationRequestsViewRefresh = 0x1003,
        BackgroundApplyOptionsBegins = 0x1004,

        BackgroundFileImportBegins = 0x100A,
        BackgroundfileImportEnds = 0x100B,
        ImportFileStarts = 0x100C,

        MessageImportLongRunning = 0x2000,
        PurgeJobStarts = 0x2001,
        PurgeJobCompletes = 0x2002,
        MexIsIdle = 0x2003,

        TCPListenerTurnedOff = 0x3001,
        TCPListenerTurnedOn = 0x3002,
        ODSListenerTurnedOff = 0x3003,
        ODSListenerTurnedOn = 0x3004,
        TCPStatusMessage = 0x3005,
        ODSStatusMessage = 0x3006,

        TCPListenerBindingError = 0x9001,
        TCPListenerInvalidHostError = 0x9002,
        PubSubEnable = 0x9003
    }

    public enum UserMessageType {
        ErrorMessage = 0x0001,
        WarningMessage = 0x0002,
        InformationMessage = 0x0003,
        BackgroundProcessingMessage = 0x0004
    }
}