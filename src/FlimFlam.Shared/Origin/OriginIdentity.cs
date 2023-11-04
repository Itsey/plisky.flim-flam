namespace Plisky.Diagnostics.FlimFlam {

    using System.Threading;

    public class OriginIdentity {
        private static int lastAllocatedOriginId = 0;

        public OriginIdentity(string id1, string id2) {
            if (id1 == null) { id1 = "default"; }
            if (id2 == null) { id2 = "default"; }

            IdentifyingKey = ConvertIdentitiesToKey(id1, id2);
            Identifier1 = id1;
            Identifier2 = id2;
            Display = id1 + "\\" + id2;
            Id = Interlocked.Increment(ref lastAllocatedOriginId);
        }

        public string Display { get; set; }
        public int Id { get; private set; }
        public string Identifier1 { get; set; }
        public string Identifier2 { get; set; }
        public string IdentifyingKey { get; set; }

        public static string ConvertIdentitiesToKey(string id1, string id2) {
            return id1.ToLower() + "\\" + id2.ToLower();
        }
    }
}