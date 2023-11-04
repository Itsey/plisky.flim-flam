namespace Plisky.Diagnostics.FlimFlam {
    using Plisky.Plumbing;

    /// <summary>
    /// RawApplicationEvent is responsible for holidng the data retrieved from the importers so that it can be passed to the data store.  RawApplicationEvents
    /// are transient and are replaced by OriginEvents once the datastore has finished importing the information.
    /// </summary>
    public class RawApplicationEvent {
        private static long masterEventEntryIndex = 0;

        public RawApplicationEvent() {
            Id = Interlocked.Increment(ref masterEventEntryIndex);
            Machine = Process = Text = string.Empty;
        }

        public RawApplicationEvent(int processId, string applicationBody) {
            Text = applicationBody;
            Process = processId.ToString();
            ArrivalTime = ConfigHub.Current.GetNow();
            Machine = Process = Text = string.Empty;
        }

        public DateTime ArrivalTime { get; set; }
        public long Id { get; private set; }
        public string Machine { get; set; }
        public int OriginId { get; set; }

        // Depending on the source someitmes we know the machine name from the message itself, sometimes from the importer.  Therefore
        // we have these values here.
        public string Process { get; set; }

        public string Text { get; set; }
    }
}