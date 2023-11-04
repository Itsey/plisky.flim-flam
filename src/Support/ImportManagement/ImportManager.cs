//using Plisky.Diagnostics;
namespace Plisky.FilmFlam {
    using Plisky.Diagnostics.FlimFlam;
    using Plisky.FilmFlam.Interfaces;
    using Plisky.FlimFlam.Interfaces;

    /// <summary>
    /// Responsible for holidng references to all of the importers, quering them for data and providing that data to the data importer.
    /// </summary>
    public class ImportManager {
        private readonly List<ISupportImporting> activeImporters = new();

        private readonly IParseData parser;

        private readonly Queue<RawApplicationEvent> pendingEvents = new();

        //protected Bilge b;
        private readonly object pendingEventsLock = new();

        public ImportManager(IParseData p) {
            parser = p;
        }

        public IEnumerable<RawApplicationEvent> EventsWaiting { get; set; }

        public bool HasEventsWaiting {
            get {
                return pendingEvents.Count > 0;
            }
        }

        public void Poll() {
            foreach (var v in activeImporters) {
                if (v.HasData) {
                    lock (pendingEventsLock) {
                        pendingEvents.Enqueue(v.GetNextEvent());
                    }
                }
            }
        }

        public void Process() {
            RawApplicationEvent? rae = null;
            lock (pendingEvents) {
                if (pendingEvents.Count > 0) {
                    rae = pendingEvents.Dequeue();
                }
            }
            _ = parser.AddRawEvent(rae);
        }

        internal void AddImporter(ISupportImporting ism) {
            activeImporters.Add(ism);
        }
    }
}