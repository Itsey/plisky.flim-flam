namespace Plisky.FlimFlam {
    using Plisky.Diagnostics.FlimFlam;
    using Plisky.FlimFlam.Interfaces;

    public class DataParser : IParseData {
        private readonly IOriginIdentityProvider originID;
        private readonly ImportParser prs;

        public DataParser(IOriginIdentityProvider iop, ImportParser chn, IRecieveEvents reciever) {
            EventReciever = reciever ?? throw new InvalidOperationException("The reciever can not be null, the DataParser can not parse entries into a null reciever");
            prs = chn;
            originID = iop;
        }

        public IRecieveEvents EventReciever { get; set; }

        public SingleOriginEvent AddRawEvent(RawApplicationEvent rae) {
            VerifyChainExists();
            var soe = prs.Parse(rae);
            if (soe == null) {
                // This is an exception because this shouldnt really be the case, something in the chain should retrieve all of the
                // logs so that nothing is lost.
                throw new InvalidOperationException("Failed to parse the event, nothing is returned");
            } else {
                EventReciever.AddEvent(soe);
            }
            return soe;
        }

        public SingleOriginEvent[] AddRawEvent(RawApplicationEvent[] rae) {
            VerifyChainExists();

            var soe = new List<SingleOriginEvent>();
            foreach (var v in rae) {
                soe.Add(prs.Parse(v));
            }
            EventReciever.AddEvent(soe);
            return soe.ToArray();
        }

        private void VerifyChainExists() {
            if (prs == null) {
                // TODO: Error handling.
                throw new InvalidOperationException("Invalid Use of the Data Parser, the chain needs to be completed before you can add things");
            }
        }
    }
}