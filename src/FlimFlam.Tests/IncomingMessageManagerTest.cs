using Plisky.FlimFlam;
using Plisky.Plumbing;

namespace Plisky.FlimFlam.Tests {
    internal class IncomingMessageManagerTest : IncomingMessageManager2 {

        public int GetMessageQueueLength() {
            return incommingMsgQueue.Count;
        }

        public IncomingMessageManagerTest(Hub hub, MockOdsGatherer mods, DataStructureManager d)  : base(hub,mods,d){
        }
    }
}