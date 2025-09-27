using System.Text.Json;

namespace Plisky.Diagnostics.FlimFlam {

    public class FFV3FormatLink : EventParserLinkBase {

        protected override SingleOriginEvent GetEvent(string identifier1, string identifier2) {
            int oi = originSource.GetOriginIdentity(identifier1, identifier2);
            var result = new SingleOriginEvent(oi);
            return result;
        }

        public FFV3FormatLink(IOriginIdentityProvider i) : base(i) {
        }

        public override SingleOriginEvent Handle(RawApplicationEvent source) {
            // Old style formatters start {[MACHINENAME - avoid trying to parse this
            if (!source.Text.StartsWith("{[") && !source.Text.Contains("ffv0004")) {
                try {
                    var objeda = JsonSerializer.Deserialize<MessageMetaDataTransport>(source.Text);
                    if (objeda != null) {
                        string mname = objeda.man ?? source.Machine;
                        string proc = objeda.ProcessName ?? source.Process;

                        var result = GetEvent(mname, proc);
                        result.LineNumber = objeda.l ?? "";
                        result.MoreLocInfo = objeda.mn ?? "";
                        result.NetThreadId = objeda.nt ?? "";
                        result.ThreadId = objeda.t ?? "";
                        if (objeda.mt != null) {
                            result.Type = objeda.GetCommandType();
                        } else {
                            result.Type = TraceCommandTypes.Unknown;
                        }
                        result.SetRawText(objeda.m ?? "");
#if DEBUG
                        result.createdBy = nameof(FFV3FormatLink);
#endif

                        return result;
                    }
                } catch (JsonException) {
                    // Ignore invalid json and just go to a different handler.
                }
            }
            return base.Handle(source);
        }
    }
}