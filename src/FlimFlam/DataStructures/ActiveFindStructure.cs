//using Plisky.Plumbing.Legacy;

namespace Plisky.FlimFlam { 

    internal class ActiveFindStructure {
        internal string FindMatch;
        internal bool IgnoreCase;

        internal ActiveFindStructure(string matcher, bool ignoreCaseSensitivity) {
            //Bilge.Warning("FindStructure created, find is not implemented properly.");
            FindMatch = matcher; IgnoreCase = ignoreCaseSensitivity;
        }

        /// <summary>
        /// Describes the find in a string form
        /// </summary>
        /// <returns>A string describing the match condiditon for the find.</returns>
        public override string ToString() {
            return "Looking for " + FindMatch;
        }

        internal bool MatchedEventEntry(EventEntry ee) {
            if (ee.DebugMessage.IndexOf(FindMatch) > 0) {
                return true;
            } else {
                return false;
            }
        }

        internal bool MatchedNonTracedEntry(NonTracedApplicationEntry nta) {
            if (nta.DebugEntry.IndexOf(FindMatch) > 0) {
                return true;
            } else {
                return false;
            }
        }
    }
}