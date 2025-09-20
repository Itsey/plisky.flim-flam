namespace Plisky.Diagnostics.FlimFlam; 

using System;
using System.Text.Json;

public class FFV4FormatLink : EventParserLinkBase {

    public FFV4FormatLink(IOriginIdentityProvider i) : base(i) {
    }

    public override SingleOriginEvent Handle(RawApplicationEvent source) {
        if (source.Text.Contains("ffv0004")) {
            var objdata = JsonSerializer.Deserialize<MessageMetadata>(source.Text);
            if (objdata != null) {
                string mname = objdata.MachineName ?? source.Machine;
                string proc = objdata.ProcessId ?? source.Process;
                var result = GetEvent(mname, proc);
                result.Filename = objdata.FileName;
                result.LineNumber = objdata.LineNumber;
                result.MethodName = objdata.MethodName;
                result.MoreLocInfo = objdata.ClassName + "::" + objdata.MethodName;
                result.Details = objdata.FurtherDetails;
                result.NetThreadId = objdata.NetThreadId;
                result.ThreadId = objdata.OsThreadId;
                if (result.ThreadId == null) {
                    result.ThreadId = result.NetThreadId;
                }
                result.Type = objdata.CommandType;
                result.SetRawText(objdata.Body);

                if (objdata.MessageTags != null) {
                    foreach (var l in objdata.MessageTags) {
                        result.AddTag(l.Key, l.Value);
                    }
                }

                if (objdata.StructuredData is JsonElement) {
                    // We dont really have a great way of dealing with strucutred content so we just add it to tags with structured on the front.
                    try {
                        JsonElement f = (JsonElement)objdata.StructuredData;
                        foreach (var l in f.EnumerateObject()) {
                            result.AddTag($"structured-{l.Name}", l.Value.ToString());
                        }
                    } catch (Exception) {
                    }
                }

#if DEBUG
                result.CreatedBy = nameof(FFV4FormatLink);
#endif

                return result;
            }
        }
        return base.Handle(source);
    }

    protected override SingleOriginEvent GetEvent(string identifier1, string identifier2) {
        int oi = originSource.GetOriginIdentity(identifier1, identifier2);
        var result = new SingleOriginEvent(oi);
        return result;
    }
}