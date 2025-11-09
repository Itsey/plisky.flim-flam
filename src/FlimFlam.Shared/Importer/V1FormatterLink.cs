using System.Text.RegularExpressions;

namespace Plisky.Diagnostics.FlimFlam {
    /// <summary>
    /// Support for the Legacy(V1) Text Parser.  V1 had an issue where procids and pids were limited to 5 characters therefore it was not compatible with
    /// 64 bit sources.  This has been totally replaced by v2 now.
    /// </summary>

    public class V1FormatterLink : EventParserLinkBase {

        #region legacy message parsing code

        private const int COMMANDSTRINGLENGTH = 5;
        private const string V1_LEGACY_REGEX = @"\{\[[0-9A-Za-z\._]{0,}\]\[[0-9]{1,5}\]\[[0-9]{1,5}\]\[[0-9A-Za-z\. ]{0,}\]\[[0-9]{0,8}\]\}\#[A-Z]{3,3}\#";
        private static Regex? v1RegexCache;

        internal static void ReturnStringBreakdown(string debugString, out string cmdType, out string procId, out string machineNamme, out string threadID,
        out string moduleName, out string lineNumber, out string debugOutput) {

            v1RegexCache ??= new Regex(@"\[[0-9A-Za-z\.:_]{0,}\]", RegexOptions.Compiled);

            var m = v1RegexCache.Match(debugString);
            // This should return 5 matches for a legit debug string

            // TODO I believe this deletes string names where [] is passed as initial or final chars test and fix.

            // Get each of the location identifiers from the string. - removing the surrouding
            // [] delimiters from each of the values.
            machineNamme = m.Captures[0].Value.Trim(new char[] { '[', ']' });
            m = m.NextMatch();
            procId = m.Captures[0].Value.Trim(new char[] { '[', ']' });
            m = m.NextMatch();
            threadID = m.Captures[0].Value.Trim(new char[] { '[', ']' });
            m = m.NextMatch();
            moduleName = m.Captures[0].Value.Trim(new char[] { '[', ']' });
            m = m.NextMatch();
            lineNumber = m.Captures[0].Value.Trim(new char[] { '[', ']' });

            // Now get the command type and turn it into an enum
            var cmdMatch = Regex.Match(debugString, "#[A-Z]{3,3}#");
            cmdType = cmdMatch.Captures[0].Value;

            // finally get the rest of the string as the debug message
            debugOutput = debugString[(cmdMatch.Index + COMMANDSTRINGLENGTH)..];  // commandstrlen currently 5
        }

        #endregion legacy message parsing code

        public V1FormatterLink(IOriginIdentityProvider i) : base(i) {
        }

        public override SingleOriginEvent? Handle(RawApplicationEvent source) {
            if (IsV1TextString(source.Text)) {
                ReturnStringBreakdown(source.Text, out string command, out string procid, out string machnm, out string thread, out string module, out string line, out string text);
                var result = GetEvent(machnm, procid);
                result.SetRawText(text);
                result.LineNumber = line;
                result.MethodName = module;
                result.ThreadId = thread;
                result.Type = TraceCommands.StringToTraceCommand(command); //FlimFlamMessageStructures.StringToTraceCommand(command);
                return result;
            }

            return base.Handle(source);
        }

        internal static bool IsV1TextString(string theString) {
            if ((theString == null) || (theString.Length == 0)) { return false; }
            if (!theString.StartsWith("{[")) { return false; }

            v1RegexCache ??= new Regex(V1_LEGACY_REGEX, RegexOptions.Compiled);

            return v1RegexCache.IsMatch(theString);
        }
    }
}