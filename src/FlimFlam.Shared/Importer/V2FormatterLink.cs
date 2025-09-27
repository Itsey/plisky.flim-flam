using System.Text.RegularExpressions;

namespace Plisky.Diagnostics.FlimFlam {

    /// <summary>
    /// This identifys the internal structure of the trace message.  The helper functions here
    /// depend on this internal structure and are all kept here.  There should be no dependance
    /// on the structure of the string outside of thsi class.
    ///
    /// {[MACHINENAME][PROCESSID][THREADID][NETTHREADID][MODULENAME][LINENUMBER][MOREDATA]}#CMD#TEXTOFDEBUGSTRING
    ///
    /// Where MACHINENAME = Current machine name taken from Environment
    /// Where PROCESSID   = The PID assigned to the process that outputed the string
    /// where THREADID    = The numeric ID assigned to the OS Thread running the commands
    /// where NETTHREADID = The name of the .net thread running the command.
    /// where MODULENAME  = The cs filename that was executing the commands
    /// where LINENUMBER  = the numeric line number that the debug string was written from
    /// where MOREDATA    = this is additional location data, using the form Class::Method when called within Tex.
    /// </summary>
    public class FFV2FormatLink : EventParserLinkBase {
        private Regex groupMatchRegexCache;
        private Regex isValidRegexCache;

        public override SingleOriginEvent Handle(RawApplicationEvent source) {
            if (source == null) { return null; }
            SingleOriginEvent result = null;

            if (IsValidV2FormattedString(source.Text)) {
                result = PopulateFromDebugString(source.Text);
                return result;
            }

            return base.Handle(source);
        }

        public FFV2FormatLink(IOriginIdentityProvider i) : base(i) {
        }

        #region private methods

        private bool IsValidV2FormattedString(string theString) {
            if ((theString == null) || (theString.Length == 0)) { return false; }
            if (!theString.StartsWith("{[")) { return false; }

            if (isValidRegexCache == null) {
                isValidRegexCache = new Regex(FlimFlamMessageStructures.IS_VALID_V2FORMATTEDSTRING_REGEX, RegexOptions.Compiled);
            }

            return isValidRegexCache.IsMatch(theString);
        }

        private SingleOriginEvent PopulateFromDebugString(string debugString) {
            if (groupMatchRegexCache == null) {
                groupMatchRegexCache = new Regex(FlimFlamMessageStructures.V2MESSAGEPARSERREGEX, RegexOptions.Compiled);
            }
            SingleOriginEvent output;

            Match m = groupMatchRegexCache.Match(debugString);
            // This should return 5 matches for a legit debug string
            string machineName = m.Captures[0].Value.Trim(new char[] { '[', ']' });
            m = m.NextMatch();
            string processId = m.Captures[0].Value.Trim(new char[] { '[', ']' });
            m = m.NextMatch();

            output = GetEvent(machineName, processId);

            output.ThreadId = m.Captures[0].Value.Trim(new char[] { '[', ']' });
            m = m.NextMatch();
            output.NetThreadId = m.Captures[0].Value.Trim(new char[] { '[', ']' });
            m = m.NextMatch();
            output.MethodName = m.Captures[0].Value.Trim(new char[] { '[', ']' });
            m = m.NextMatch();
            output.LineNumber = m.Captures[0].Value.Trim(new char[] { '[', ']' });
            m = m.NextMatch();
            output.MoreLocInfo = m.Captures[0].Value.Trim(new char[] { '[', ']' });

            // Now get the command type and turn it into an enum
            var cmdMatch = Regex.Match(debugString, FlimFlamMessageStructures.V2COMMANDIDENTIFIERREGEX);
            // TODO : REVERT after Bilge update

#if true
            output.Type = TraceCommands.StringToTraceCommand(cmdMatch.Captures[0].Value);
#else
            if (cmdMatch.Captures[0].Value != "#ALT#") {
                output.Type = TraceCommands.StringToTraceCommand(cmdMatch.Captures[0].Value);
            } else {
                output.Type = TraceCommandTypes.Alert;
            }
#endif

            // finally get the rest of the string as the debug message, from the command index + length of the actual command.
            output.SetRawText(debugString.Substring(cmdMatch.Index + FlimFlamMessageStructures.V2COMMANDSTRINGLENGTH));

#if DEBUG
            output.createdBy = nameof(FFV2FormatLink);
#endif

            return output;
        }

        #endregion private methods
    }
}