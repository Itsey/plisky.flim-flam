namespace Plisky.FlimFlam { 

    internal class ResourceCountEntryRepresentation {
        internal string Context;
        internal long Value;

        internal ResourceCountEntryRepresentation(string ctxt, long val) {
            Context = ctxt;
            Value = val;
        }
    }
}