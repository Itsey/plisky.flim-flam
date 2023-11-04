// DO NOT USE

namespace Plisky.Diagnostics.FlimFlam {

    /// <summary>
    /// Takes the identifying features of an origin identity and returns an identity, or converts an identity back to its identifying features.
    /// </summary>
    public interface IOriginIdentityProvider {

        //string GetSourceIdentityName(int id);
        //OriginIdentityStore GetFullOriginDetails(int id);
        bool ContainsIdentity(int id);

        int GetDefaultOriginIdentity();

        OriginIdentity GetIdentity(int id);

        /// <summary>
        /// Normal usage sourceId1 = MachineName, sourceId2 = Process ID. However different importers can use different formats - e.g. File as source 1 and filename
        /// as source 2.  This will create a new identity if one is not found for the
        /// </summary>
        /// <param name="sourceId1">Machine/FileName</param>
        /// <param name="sourceId2">Pid</param>
        /// <returns></returns>
        int GetOriginIdentity(string sourceId1, string sourceId2);
    }
}