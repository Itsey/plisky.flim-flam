// DONOT USE

namespace Plisky.Diagnostics.FlimFlam {

    /// <summary>
    /// Responsible for maintaining a mapping of the origin identities into single uint values that are the internal unique
    /// origin references.
    /// </summary>
    public class OriginIdentityStore : IOriginIdentityProvider {
        protected Bilge b = new Bilge("ff-core-originidentitystore");

        private int defaultId;
        private Dictionary<int, OriginIdentity> existingOrigins = new Dictionary<int, OriginIdentity>();

        public OriginIdentityStore() {
            b.Info.Flow();
            var defaultOid = ConvertIdentifiersToOriginIdentity(null, null);
            defaultId = defaultOid.Id;
            AddNewOrigin(defaultOid);
        }

        public bool ContainsIdentity(int selectedOriginIdentity) {
            // Must create a unit test that makes this method work.
            lock (existingOrigins) {
                return existingOrigins.ContainsKey(selectedOriginIdentity);
            }
        }

        public int GetDefaultOriginIdentity() {
            return defaultId;
        }

        public OriginIdentity GetIdentity(int id) {
            OriginIdentity result;
            lock (existingOrigins) {
                result = existingOrigins[id];
            }
            return result;
        }

        public int GetOriginIdentity(string sourceIdentity, string sourceIndex) {
            string origKey = OriginIdentity.ConvertIdentitiesToKey(sourceIdentity, sourceIndex);

            lock (existingOrigins) {
                foreach (int v in existingOrigins.Keys) {
                    if (existingOrigins[v].IdentifyingKey == origKey) {
                        return existingOrigins[v].Id;
                    }
                }
                var oi = new OriginIdentity(sourceIdentity, sourceIndex);
                AddNewOrigin(oi);

                return oi.Id;
            }
        }

        public IEnumerable<int> ListOriginIdentities() {
            lock (existingOrigins) {
                foreach (int f in existingOrigins.Keys) {
                    yield return f;
                }
            }
        }

        private void AddNewOrigin(OriginIdentity oi) {
            b.Verbose.Log($"New Origin discovered {oi.Id}={oi.Display}");
            lock (existingOrigins) {
                existingOrigins.Add(oi.Id, oi);
            }
        }

        private OriginIdentity ConvertIdentifiersToOriginIdentity(string source1, string source2) {
            return new OriginIdentity(source1, source2);
        }
    }
}