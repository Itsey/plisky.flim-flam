using System;
using System.Collections.Generic;

namespace Plisky.FlimFlam { 

    public class KeyDisplayRepresentation : IEquatable<KeyDisplayRepresentation>, IEquatable<string> {
        private string m_dispId;

        private string m_keyId;

        public KeyDisplayRepresentation(string key, string display) {
            KeyIdentity = key;
            DisplayIdentity = display;
        }

        public KeyDisplayRepresentation() {
            m_dispId = m_keyId = string.Empty;
        }

        public string DisplayIdentity {
            get { return m_dispId; }
            set { m_dispId = value; }
        }

        public string KeyIdentity {
            get { return m_keyId; }
            set { m_keyId = value; }
        }

        public static bool ContainsKey(List<KeyDisplayRepresentation> keyDisplayRepresentation, string key) {
            foreach (KeyDisplayRepresentation k in keyDisplayRepresentation) {
                if (k.KeyIdentity == key) {
                    return true;
                }
            }
            return false;
        }

        public static bool ContainsName(List<KeyDisplayRepresentation> keyDisplayRepresentation, string name) {
            foreach (KeyDisplayRepresentation k in keyDisplayRepresentation) {
                if (k.DisplayIdentity == name) {
                    return true;
                }
            }
            return false;
        }

        public override string ToString() {
            return DisplayIdentity;
        }

        #region IEquatable<KeyDisplayRepresentation> Members

        bool IEquatable<KeyDisplayRepresentation>.Equals(KeyDisplayRepresentation other) {
            if (other == null) { return false; }
            return (this.KeyIdentity == other.KeyIdentity);
        }

        #endregion IEquatable<KeyDisplayRepresentation> Members

        #region IEquatable<string> Members

        bool IEquatable<string>.Equals(string other) {
            if (other == null) { return false; }
            return (this.KeyIdentity == other);
        }

        #endregion IEquatable<string> Members
    }
}