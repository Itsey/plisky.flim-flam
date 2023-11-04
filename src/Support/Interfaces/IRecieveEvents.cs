using Plisky.Diagnostics.FlimFlam;

namespace Plisky.FlimFlam.Interfaces {

    public interface IRecieveEvents {

        void AddEvent(SingleOriginEvent evt);

        void AddEvent(IEnumerable<SingleOriginEvent> evts);
    }
}