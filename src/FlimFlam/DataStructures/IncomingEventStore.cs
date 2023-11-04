namespace Plisky.FlimFlam {
    using System;
    using Plisky.Flimflam;

    // todo struct or class -> perf / boxing

    /// <summary>
    /// Temporarily stores incoming events, ready for processing by mex.
    /// </summary>
    internal class IncomingEventStore {
        internal long GlobalIndex;
        internal string MachineName;
        internal string MessageString;
        internal int Pid;
        internal InternalSource Source;
        internal DateTime TimeRecieved;

        /// <summary>
        /// This creates an instance of an incoming event store which is used to temporarily hold events while they are loaded into the mex viewer.  In
        /// order that the gatherers can have rapisd turn around times this structure stores the newly incoming events rapidly withoutabny parsing.
        /// </summary>
        /// <param name="machineName">The name of the machine on whcih the event occured</param>
        /// <param name="msgString">The text part of the incoming string</param>
        /// <param name="pidIfKnown">If the pid is known it should be passed otherwise pass -1</param>
        /// <param name="gIndex">The global index </param>
        internal IncomingEventStore(InternalSource source, string machineName, string msgString, int pidIfKnown, long gIndex) {
            Source = source;
            TimeRecieved = DateTime.Now;
            MachineName = machineName; MessageString = msgString; Pid = pidIfKnown; GlobalIndex = gIndex;
        }

        public override bool Equals(object obj) {
            if (obj.GetType() != typeof(IncomingEventStore)) {
                throw new InvalidOperationException("Unable to compare this type to an IncomingEventStore");
            }

            var ies = (IncomingEventStore)obj;
            bool result = true;

            if (MachineName != ies.MachineName) {
                result = false;
            }
            if (MessageString != ies.MessageString) {
                result = false;
            }
            if (Pid != ies.Pid) {
                result = false;
            }

            return result;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}