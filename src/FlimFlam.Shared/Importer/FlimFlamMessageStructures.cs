namespace Plisky.Diagnostics.FlimFlam {

    public class FlimFlamMessageStructures {
        public const string MSGFMT_XMLCOMMAND = "#XCM#";

        public const string LOGMESSAGE = "#LOG#";
        public const string LOGMESSAGEVERB = "#LGV#";
        public const string LOGMESSAGEMINI = "#LGM#";
        public const string INTERNALMSG = "#INT#";
        public const string TRACEMESSAGEIN = "#TRI#";
        public const string TRACEMESSAGEOUT = "#TRO#";
        //public const string TRACEMESSAGE    = "#TRC#";

        public const string ASSERTIONFAILED = "#AST#";

        public const string MOREINFO = "#MOR#";
        public const string COMMANDONLY = "#CMD#";
        public const string ERRORMSG = "#ERR#";
        public const string WARNINGMSG = "#WRN#";
        public const string EXCEPTIONBLOCK = "#EXC#";
        public const string EXCEPTIONDATA = "#EXD#";
        public const string EXCSTART = "#EXS#";
        public const string EXCEND = "#EXE#";

        public const string SECTIONSTART = "#SEC#";  // Sections are used for timings now too.
        public const string SECTIONEND = "#SXX#";

        public const string RESOURCEEAT = "#REA#";
        public const string RESOURCEPUKE = "#RPU#";
        public const string RESOURCECOUNT = "#RSC#";
        public const string TIMERNAME = "TexTimer";

        public const string TIMER_SECTIONIDENTIFIER = "|TMRCHK|";  // Timers use sections for sending to Mex therefore this identifies a timer rather than a normal section
        public const string TIMER_STRINGENDDELIMITER = "#X_X#]";
        public const string TIMER_STRINGSTARTDELIMITER = "[#X_X#";
        public const string TIMER_STRINGSTARTCONTENTSTRING = "TMRDATA" + TIMER_STRINGSTARTDELIMITER;
        public const string AUTOTIMER_PREFIX = "TAT_PFX";

        public const string TCPEND_MARKERTAG = "#TCPLIST-END#";
        public const int TCPEND_MARKERTAGLEN = 13;    // TCPEND_MARKERTAG.Length doesent work.  Used to work in uber.

        #region message format constants and regexes

        public const int V2COMMANDSTRINGLENGTH = 5;
        public const string V2MESSAGEPARSERREGEX = @"[0-9A-Za-z\.\:_<>\-`]{0,}\]";  //@"\[[0-9A-Za-z\.:<>`_\-]{0,}\]";
        public const string V2COMMANDIDENTIFIERREGEX = @"#[A-Z]{3,3}#";
        public const string IS_VALID_V2FORMATTEDSTRING_REGEX = @"\{\[[0-9A-Za-z\._-]{0,}\]\[[0-9]{1,}\]\[[0-9]{1,}\]\[[0-9A-Za-z\._\:\\]{0,}\]\[[0-9A-Za-z\._\-\\:]{0,}\]\[[0-9]{0,8}\]\[[0-9A-Za-z\.\:_<>\-`\*]{0,}\]\}\#[A-Z]{3,3}\#";

        // OLD @"\{\[[0-9A-Za-z\._]{0,}\]\[[0-9]{1,}\]\[[0-9]{1,}\]\[[0-9A-Za-z\._]{0,}\]\[[0-9A-Za-z\._]{0,}\]\[[0-9]{0,8}\]\[[0-9A-Za-z\.\:_<>`]{0,}\]\}\#[A-Z]{3,3}\#";

        #endregion message format constants and regexes

#if false
        //[SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Long switch statement but very simple to read")]
        public static MessageType StringToTraceCommand(string theCmdText) {
            switch (theCmdText) {
                case LOGMESSAGE: return MessageType.InfoMessage;
                case LOGMESSAGEVERB: return MessageType.VerboseMessage;
                //case LOGMESSAGEMINI : return MessageType.LogMessageMini;
                case publicMSG: return MessageType.InternalMsg;
                case TRACEMESSAGEIN: return MessageType.TraceMessageIn;
                case TRACEMESSAGEOUT: return MessageType.TraceMessageOut;
                //case TRACEMESSAGE   : return MessageType.TraceMessage;
                case ASSERTIONFAILED: return MessageType.AssertionFailed;
                case MOREINFO: return MessageType.MoreInfo;
                case COMMANDONLY: return MessageType.CommandOnly;
                case ERRORMSG: return MessageType.ErrorMsg;
                case WARNINGMSG: return MessageType.WarningMsg;
                case EXCEPTIONBLOCK: return MessageType.ExceptionBlock;
                case EXCEPTIONDATA: return MessageType.ExceptionData;
                case EXCSTART: return MessageType.ExceptionBlockStart;
                case EXCEND: return MessageType.ExceptionBlockEnd;
                case SECTIONSTART: return MessageType.SectionStart;
                case SECTIONEND: return MessageType.SectionEnd;
                case RESOURCEEAT: return MessageType.ResourceEat;
                case RESOURCEPUKE: return MessageType.ResourcePuke;
                case RESOURCECOUNT: return MessageType.ResourceCount;
                case MSGFMT_XMLCOMMAND: return MessageType.CommandXML;
            }

            throw new ArgumentException("Unreachable Code, the value of the parameter is invalid, this code should not be executed.", "theCmdText");
        }
        //[SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Long switch statement but very simple to read")]
        public static string TraceCommandToString(MessageType theCommand) {
            switch (theCommand) {
                case MessageType.InfoMessage: return LOGMESSAGE;
                case MessageType.VerboseMessage: return LOGMESSAGEVERB;
                case MessageType.InternalMsg: return publicMSG;
                case MessageType.TraceMessageIn: return TRACEMESSAGEIN;
                case MessageType.TraceMessageOut: return TRACEMESSAGEOUT;
                case MessageType.AssertionFailed: return ASSERTIONFAILED;
                case MessageType.MoreInfo: return MOREINFO;
                case MessageType.CommandOnly: return COMMANDONLY;
                case MessageType.CommandXML: return MSGFMT_XMLCOMMAND;
                case MessageType.ErrorMsg: return ERRORMSG;
                case MessageType.WarningMsg: return WARNINGMSG;
                case MessageType.ExceptionData: return EXCEPTIONDATA;
                case MessageType.ExceptionBlock: return EXCEPTIONBLOCK;
                case MessageType.ExceptionBlockStart: return EXCSTART;
                case MessageType.ExceptionBlockEnd: return EXCEND;
                case MessageType.SectionStart: return SECTIONSTART;
                case MessageType.SectionEnd: return SECTIONEND;
                case MessageType.ResourceEat: return RESOURCEEAT;
                case MessageType.ResourcePuke: return RESOURCEPUKE;
                case MessageType.ResourceCount: return RESOURCECOUNT;
            }

            throw new ArgumentException("Unreachable Code, the value (" + theCommand.ToString() + ")of the parameter is invalid, this code should not be executed.", "theCommand");
        }
#endif
    }
}