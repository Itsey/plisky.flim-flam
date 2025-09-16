using System;
using System.Collections.Generic;

namespace Plisky.FlimFlam;

public class KeyDisplayRepresentation : IEquatable<KeyDisplayRepresentation>, IEquatable<string> {
    public KeyDisplayRepresentation(string key, string display) {
        KeyIdentity = key;
        DisplayIdentity = display;
    }

    public KeyDisplayRepresentation() {
        DisplayIdentity = KeyIdentity = string.Empty;
    }

    public string DisplayIdentity { get; set; }

    public string KeyIdentity { get; set; }

    public static bool ContainsKey(List<KeyDisplayRepresentation> keyDisplayRepresentation, string key) {
        foreach (var k in keyDisplayRepresentation) {
            if (k.KeyIdentity == key) {
                return true;
            }
        }
        return false;
    }

    public static bool ContainsName(List<KeyDisplayRepresentation> keyDisplayRepresentation, string name) {
        foreach (var k in keyDisplayRepresentation) {
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
        return other != null && KeyIdentity == other.KeyIdentity;
    }

    #endregion IEquatable<KeyDisplayRepresentation> Members

    #region IEquatable<string> Members

    bool IEquatable<string>.Equals(string other) {
        return other != null && KeyIdentity == other;
    }

    #endregion IEquatable<string> Members
}