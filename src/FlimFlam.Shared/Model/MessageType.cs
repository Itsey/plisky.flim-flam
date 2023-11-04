namespace Plisky.Diagnostics.FlimFlam {
    // DONT KNOW WHY THIS EXISTS, REMOVING IT WHY NOT JUST USE STRAIGHT FROM DIAGS?

#if false
    public class Refactoring_TraceCommandTypes {
        public static MessageType TraceCommandToMessageType(TraceCommandTypes tct) {
            switch (tct) {
                case TraceCommandTypes.LogMessage: return MessageType.InfoMessage;
                case TraceCommandTypes.LogMessageVerb: return MessageType.VerboseMessage;
                case TraceCommandTypes.LogMessageMini: return MessageType.InfoMessage;
                case TraceCommandTypes.InternalMsg: return MessageType.InternalMsg;
                case TraceCommandTypes.TraceMessageIn: return MessageType.TraceMessageIn;
                case TraceCommandTypes.TraceMessageOut: return MessageType.TraceMessageOut;
                case TraceCommandTypes.TraceMessage: return MessageType.InfoMessage;
                case TraceCommandTypes.AssertionFailed: return MessageType.AssertionFailed;
                case TraceCommandTypes.MoreInfo: return MessageType.MoreInfo;
                case TraceCommandTypes.CommandOnly: return MessageType.CommandOnly;
                case TraceCommandTypes.ErrorMsg: return MessageType.ErrorMsg;
                case TraceCommandTypes.WarningMsg: return MessageType.WarningMsg;
                case TraceCommandTypes.ExceptionBlock: return MessageType.ExceptionBlock;
                case TraceCommandTypes.ExceptionData: return MessageType.ExceptionData;
                case TraceCommandTypes.ExcStart: return MessageType.ExceptionBlockStart;
                case TraceCommandTypes.ExcEnd: return MessageType.ExceptionBlockEnd;
                case TraceCommandTypes.SectionStart: return MessageType.SectionStart;
                case TraceCommandTypes.SectionEnd: return MessageType.SectionEnd;
                case TraceCommandTypes.ResourceEat: return MessageType.ResourceEat;
                case TraceCommandTypes.ResourcePuke: return MessageType.ResourcePuke;
                case TraceCommandTypes.ResourceCount: return MessageType.ResourceCount;
                case TraceCommandTypes.Standard: return MessageType.Standard;
                case TraceCommandTypes.CommandXML: return MessageType.CommandXML;
                case TraceCommandTypes.Custom: return MessageType.Custom;
                case TraceCommandTypes.Alert: return MessageType.Alert;
                case TraceCommandTypes.Unknown: return MessageType.Unknown;
            }
            throw new NotImplementedException("missing entry in TraceTypes to MessageType Mapping");
        }

        public static TraceCommandTypes MessageTypeToTraceCommandType(MessageType mt) {
            switch (mt) {
                case MessageType.InfoMessage: return TraceCommandTypes.LogMessage;
                case MessageType.VerboseMessage: return TraceCommandTypes.LogMessageVerb;
                case MessageType.InternalMsg: return TraceCommandTypes.InternalMsg;
                case MessageType.TraceMessageIn: return TraceCommandTypes.TraceMessageIn;
                case MessageType.TraceMessageOut: return TraceCommandTypes.TraceMessageOut;
                case MessageType.AssertionFailed: return TraceCommandTypes.AssertionFailed;
                case MessageType.MoreInfo: return TraceCommandTypes.MoreInfo;
                case MessageType.CommandOnly: return TraceCommandTypes.CommandOnly;
                case MessageType.ErrorMsg: return TraceCommandTypes.ErrorMsg;
                case MessageType.WarningMsg: return TraceCommandTypes.WarningMsg;
                case MessageType.ExceptionBlock: return TraceCommandTypes.ExceptionBlock;
                case MessageType.ExceptionData: return TraceCommandTypes.ExceptionData;
                case MessageType.ExceptionBlockStart: return TraceCommandTypes.ExcStart;
                case MessageType.ExceptionBlockEnd: return TraceCommandTypes.ExcEnd;
                case MessageType.SectionStart: return TraceCommandTypes.SectionStart;
                case MessageType.SectionEnd: return TraceCommandTypes.SectionEnd;
                case MessageType.ResourceEat: return TraceCommandTypes.ResourceEat;
                case MessageType.ResourcePuke: return TraceCommandTypes.ResourcePuke;
                case MessageType.ResourceCount: return TraceCommandTypes.ResourceCount;
                case MessageType.Standard: return TraceCommandTypes.Standard;
                case MessageType.CommandXML: return TraceCommandTypes.CommandXML;
                case MessageType.Custom: return TraceCommandTypes.Custom;
                case MessageType.Alert: return TraceCommandTypes.Alert;
                case MessageType.Unknown: return TraceCommandTypes.Unknown;
            }
            throw new NotImplementedException("missing entry in TraceTypes to MessageType Mapping");
        }
    }
#endif

#if false
TAKE FROM DIAGS

    /// <summary>
    /// Trace Command Types - represent all of the possible command types that can be issued as a trace command.
    /// This is legacy - to be replaced by the one below.  Mappers exist for now.
    /// </summary>
    [Flags]
    public enum TraceCommandTypes : uint {
        /// <summary>
        /// Standard Log Messages
        /// </summary>
        LogMessage = 0x00000001,
        /// <summary>
        /// Verbose Log Messages
        /// </summary>
        LogMessageVerb = 0x00000002,
        /// <summary>
        /// Minimal Log Messages
        /// </summary>
        LogMessageMini = 0x00000004,
        /// <summary>
        /// Internal Messages
        /// </summary>
        InternalMsg = 0x00000008,
        /// <summary>
        /// Trace Messages, Enter
        /// </summary>
        TraceMessageIn = 0x00000010,
        /// <summary>
        /// Trace Messages, Exit
        /// </summary>
        TraceMessageOut = 0x00000020,
        /// <summary>
        /// Trace Messages - Other
        /// </summary>
        TraceMessage = 0x00000040,
        /// <summary>
        /// Assertion Failure
        /// </summary>
        AssertionFailed = 0x00000080,
        /// <summary>
        /// Further Details to an existing message
        /// </summary>
        MoreInfo = 0x00000100,
        /// <summary>
        /// Trace Display Commands Only
        /// </summary>
        CommandOnly = 0x00000200,
        /// <summary>
        /// Errors
        /// </summary>
        ErrorMsg = 0x00000400,
        /// <summary>
        /// Warnings
        /// </summary>
        WarningMsg = 0x00000800,
        /// <summary>
        /// Exception Block OF Info
        /// </summary>
        ExceptionBlock = 0x00001000,  // used for exception type flag
        /// <summary>
        /// Exception Meta Data
        /// </summary>
        ExceptionData = 0x00002000,
        /// <summary>
        /// Exception Block Start
        /// </summary>
        ExcStart = 0x00004000,
        /// <summary>
        /// Exception Block End
        /// </summary>
        ExcEnd = 0x00008000,
        /// <summary>
        /// Section Start
        /// </summary>
        SectionStart = 0x00010000,
        /// <summary>
        /// Section End
        /// </summary>
        SectionEnd = 0x00020000,
        /// <summary>
        /// Resource Consumption - Developer Trace
        /// </summary>
        ResourceEat = 0x00040000,
        /// <summary>
        /// Resource Release - Developer Trace
        /// </summary>
        ResourcePuke = 0x00080000,
        /// <summary>
        /// Resource Current Value - Developer Trace
        /// </summary>
        ResourceCount = 0x00100000,
        /// <summary>
        /// Standard Message Type
        /// </summary>
        Standard = 0x00200000,
        /// <summary>
        /// XML Formattted Command Message
        /// </summary>
        CommandXML = 0x00400000,
        /// <summary>
        /// Custom and Third party messages
        /// </summary>
        Custom = 0x00800000,
        /// <summary>
        /// Alerting and Notification
        /// </summary>
        Alert = 0x010000000,

        /// <summary>
        /// Unknown, error or invalid configuration.
        /// </summary>
        Unknown = 0x00000000
    };

#endif
#if false
    /// <summary>
    /// Message type, each message that is placed into the input queue is categorized by its type.  This is only possible when supporting clients which
    /// send specially formatted messages, everyone else just gets a standard message type.
    /// </summary>
    [Flags]
    public enum MessageType : uint {
        InfoMessage = 0x00000001,
        VerboseMessage = 0x00000002,

        // Mini removed, new one can go here @ 0x04
        InternalMsg = 0x00000008,

        TraceMessageIn = 0x00000010,
        TraceMessageOut = 0x00000020,

        // TraceMessage removed, new one can go here at = 0x00000040,
        AssertionFailed = 0x00000080,

        MoreInfo = 0x00000100,
        CommandOnly = 0x00000200,
        ErrorMsg = 0x00000400,
        WarningMsg = 0x00000800,
        ExceptionBlock = 0x00001000,
        ExceptionData = 0x00002000,
        ExceptionBlockStart = 0x00004000,
        ExceptionBlockEnd = 0x00008000,
        SectionStart = 0x00010000,
        SectionEnd = 0x00020000,
        ResourceEat = 0x00040000,
        ResourcePuke = 0x00080000,
        ResourceCount = 0x00100000,
        Standard = 0x00200000,
        Unknown = 0x00000000,
        CommandXML = 0x400000,
        Alert = 0x800000,
        Custom = 0x1000000
    }

#endif
}