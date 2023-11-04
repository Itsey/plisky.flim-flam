namespace Support.ImportManagement {
    using System;
    using System.Reactive.Linq;
    using Plisky.Diagnostics.FlimFlam;

    public class EventImport {
        private IObservable<RawApplicationEvent>? events;

        public IObservable<RawApplicationEvent> Events => events ?? throw new InvalidOperationException("DEV - Events have not been provided");

        public void ProvideEvents(IObservable<RawApplicationEvent> eventSource) {
            events = events == null ? eventSource : events.Merge(eventSource);
        }
    }
}
