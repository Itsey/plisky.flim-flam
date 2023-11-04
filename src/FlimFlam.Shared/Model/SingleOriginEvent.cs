namespace Plisky.Diagnostics.FlimFlam {

    using System.Diagnostics;
    using System.Threading;

    [DebuggerDisplay("SOE: GIdx[ {Id} ]  OIdx[ {OriginIdentity} ]")]
    public class SingleOriginEvent {
#if DEBUG
        internal string CreatedBy = "none";
#endif

        private static long globallyUniqueIdentity = 0;

        public int OriginIdentity { get; set; }

        public string Text { get; private set; }
        public string Details { get; set; }

        public long Id { get; private set; }

        public TraceCommandTypes Type { get; set; }
        public string? LineNumber { get; set; }
        public string? NetThreadId { get; set; }
        public string? ThreadId { get; set; }
        public string? MoreLocInfo { get; set; }

        public string? Classname { get; set; }
        public string? MethodName { get; set; }
        public string? Filename { get; set; }

        public Dictionary<string, string> Tags { get; set; }

        public void AddTag(string tagKey, string tagValue) {
            if (Tags == null) { Tags = new Dictionary<string, string>(); }
            if (!Tags.ContainsKey(tagKey)) {
                Tags.Add(tagKey, tagValue);
            } else {
                Tags[tagKey] = tagValue;
            }
        }

        public void SetRawText(string debugMessage) {
            // TODO:  Turn To Test Based Span
            int markerPoint = debugMessage.IndexOf("~~#~~");

            if (markerPoint >= 0) {
                // This debug message has attached to it a secondary message.
                Details = debugMessage.Substring(markerPoint + 5);
                Text = debugMessage.Substring(0, markerPoint);
            } else {
                Text = debugMessage;
                Details = string.Empty;
            }
        }

        public SingleOriginEvent(int entry) : this(entry, null) {
        }

        public SingleOriginEvent(int entry, string? debugMessage) {
            OriginIdentity = entry;
            Interlocked.Increment(ref globallyUniqueIdentity);
            Id = globallyUniqueIdentity;
            if (debugMessage != null) {
                SetRawText(debugMessage);
            }
        }
    }
}