namespace Plisky.FlimFlam {

    using System.Collections.Generic;
    using Plisky.Diagnostics.FlimFlam;

    /// <summary>
    /// Primary data store for the application, holds all of the data and the routes through it.
    /// </summary>
    public abstract class EventEntryStore {
        protected long currentCount = 0;

        public long Count {
            get { return ActualGetCount(); }
        }

        public void AddEntry(SingleOriginEvent ee) {
            currentCount++;
            ActualAddEntry(ee);
        }

        public abstract IEnumerable<SingleOriginEvent> GetEntries();

        protected abstract void ActualAddEntry(SingleOriginEvent ee);

        protected virtual long ActualGetCount() {
            return currentCount;
        }
    }
}