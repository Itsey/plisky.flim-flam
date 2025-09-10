namespace Plisky.FlimFlam {
    using System;
    using System.Collections.Generic;
    using Plisky.Diagnostics.FlimFlam;
    using Plisky.FlimFlam.Interfaces;

    public class EventRecieverForComparisons : IRecieveEvents {
        private SingleOriginEvent lastEvent;

        public void AddEvent(SingleOriginEvent evt) {
            lastEvent = evt;
        }

        public void AddEvent(IEnumerable<SingleOriginEvent> evts) {
            throw new NotImplementedException();
        }

        internal bool Compare(EventEntry toThis) {
            if (lastEvent==null) { return true; }

            bool result = true;
            if (lastEvent.LineNumber != toThis.LineNumber) {
                result = false;
            }
            if (lastEvent.MethodName != toThis.Module) {
                result = false;
            }
            if (lastEvent.NetThreadId != toThis.ThreadNetId) {
                result = false;
            }

            if ((int)lastEvent.Type != (int)toThis.cmdType) {
                result = false;
            }

            switch (lastEvent.Type) {
                case Plisky.Diagnostics.TraceCommandTypes.LogMessage:
                case Plisky.Diagnostics.TraceCommandTypes.LogMessageVerb:
                case Plisky.Diagnostics.TraceCommandTypes.LogMessageMini:
                case Plisky.Diagnostics.TraceCommandTypes.InternalMsg:
                case Plisky.Diagnostics.TraceCommandTypes.TraceMessageIn:
                case Plisky.Diagnostics.TraceCommandTypes.TraceMessageOut:
                case Plisky.Diagnostics.TraceCommandTypes.TraceMessage:
                case Plisky.Diagnostics.TraceCommandTypes.AssertionFailed:
                case Plisky.Diagnostics.TraceCommandTypes.MoreInfo:
                    // Only check the text for a subset of message types where the two parsers behave identically.
                    if (lastEvent.Text != toThis.DebugMessage) {
                        result = false;
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
            return result;
        }
    }
}