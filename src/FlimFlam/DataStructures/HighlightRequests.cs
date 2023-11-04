//using Plisky.Plumbing.Legacy;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Plisky.Diagnostics;

namespace Plisky.FlimFlam { 

    [Serializable]
    public class AHighlightRequest {
        internal TraceCommandTypes matchTct;
        private Color m_backgroundColor;
        private Color m_foregroundColor;

        public AHighlightRequest() {
            MakeMatchUsingString = true;
            MakeMatchUsingType = true;
        }

        internal AHighlightRequest(Color bkCol, Color fgCol, TraceCommandTypes matchType, bool invertMatch, bool checkCase) : this(bkCol, fgCol, matchType, null, invertMatch, checkCase) {
        }

        internal AHighlightRequest(Color bkCol, Color fgCol, string match, bool invertMatch, bool checkCase) : this(bkCol, fgCol, TraceCommandTypes.Unknown, match, invertMatch, checkCase) {
        }

        internal AHighlightRequest(Color bkCol, Color fgCol, TraceCommandTypes matchType, string match, bool invertMatch, bool checkCase) : this() {
            if (bkCol != Color.Empty) {
                BackColorSpecified = true;
                BackgroundColor = bkCol;
            } else {
                BackColorSpecified = false;
                BackgroundColor = Color.Empty;
            }

            if (fgCol != Color.Empty) {
                ForeColorSpecified = true;
                ForegroundColor = fgCol;
            } else {
                ForeColorSpecified = false;
                ForegroundColor = Color.Empty;
            }

            matchTct = matchType;
            MakeMatchUsingType = false; MakeMatchUsingString = false;
            if (matchTct != TraceCommandTypes.Unknown) { MakeMatchUsingType = true; }
            ComparisonStringToMatchOn = match;
            if ((ComparisonStringToMatchOn != null) && (ComparisonStringToMatchOn.Length > 0)) { MakeMatchUsingString = true; }
            NotMatch = invertMatch;
            CaseSensitive = checkCase;

            SetNamestringDefault();
            DescriptionString = NameString;

            // Now just check that we havent allowed them not to match on anything at all as that would be really quite lame.
            //Bilge.Assert(MakeMatchUsingType || MakeMatchUsingString, "Highlight object is invalid.  The UI should ensure that a highlight object is created to either highlight the event type or on a matching string or possibly both but it should not allow no match criteia.");
        }

        [XmlElement("BackColor")]
        public string BackColorAsString {
            get {
                return ColorTranslator.ToHtml(m_backgroundColor);
            }
            set {
                m_backgroundColor = ColorTranslator.FromHtml(value);
            }
        }

        public bool BackColorSpecified {
            get;
            set;
        }

        [XmlIgnore]
        public Color BackgroundColor {
            get { return m_backgroundColor; }
            set {
                m_backgroundColor = value;
                BackColorSpecified = value != Color.Empty;
            }
        }

        public bool CaseSensitive {
            get;
            set;
        }

        public string ComparisonStringToMatchOn {
            get;
            set;
        }

        public string DescriptionString {
            get;
            set;
        }

        [XmlElement("ForeColor")]
        public string ForeColorAsString {
            get {
                return ColorTranslator.ToHtml(m_foregroundColor);
            }
            set {
                m_foregroundColor = ColorTranslator.FromHtml(value);
            }
        }

        public bool ForeColorSpecified {
            get;
            set;
        }

        [XmlIgnore]
        public Color ForegroundColor {
            get { return m_foregroundColor; }
            set {
                m_foregroundColor = value;
                ForeColorSpecified = value != Color.Empty;
            }
        }

        public bool MakeMatchUsingString {
            get;
            set;
        }

        public bool MakeMatchUsingType {
            get;
            set;
        }

        public string NameString {
            get;
            set;
        }

        public bool NotMatch {
            get;
            set;
        }

        [XmlElement("TraceTypeMatch")]
        public uint TraceTypeSerialize {
            get { return (uint)matchTct; }
            set { matchTct = (TraceCommandTypes)value; }
        }

        /// <summary>
        /// Returns a description of this highlight
        /// </summary>
        /// <returns>String describing the highlight</returns>
        public override string ToString() {
            return this.NameString;
        }

        internal static string GetHighlightFilenameFromHighlightName(string name) {
            string result = Path.Combine(MexCore.TheCore.Options.FilterAndHighlightStoreDirectory, name);
            result = result + MexCore.TheCore.Options.HighlightExtension;
            return result;
        }

        internal static AHighlightRequest LoadHighlight(string textName) {
            XmlSerializer xmls = new XmlSerializer(typeof(AHighlightRequest));
            AHighlightRequest result = null;

            string path = Path.Combine(MexCore.TheCore.Options.FilterAndHighlightStoreDirectory, textName + MexCore.TheCore.Options.HighlightExtension);
            if (File.Exists(path)) {
                using (FileStream fs = new FileStream(path, FileMode.Open)) {
                    result = (AHighlightRequest)xmls.Deserialize(fs);
                    fs.Close();
                }
            }

            return result;
        }

        internal static AHighlightRequest[] LoadKnownHighlights() {
            XmlSerializer xmls = new XmlSerializer(typeof(AHighlightRequest));
            List<AHighlightRequest> result = new List<AHighlightRequest>();

            string[] matchedFilters = Directory.GetFiles(MexCore.TheCore.Options.FilterAndHighlightStoreDirectory, "*" + MexCore.TheCore.Options.HighlightExtension);

            foreach (string s in matchedFilters) {
                using (FileStream fs = new FileStream(s, FileMode.Open)) {
                    AHighlightRequest loaded = (AHighlightRequest)xmls.Deserialize(fs);
                    fs.Close();
                    result.Add(loaded);
                }
            }

            return result.ToArray();
        }

        internal static void SaveHighlight(AHighlightRequest ahr) {
            XmlSerializer xmls = new XmlSerializer(typeof(AHighlightRequest));

            string pathToSave = GetHighlightFilenameFromHighlightName(ahr.NameString);

            if (File.Exists(pathToSave)) { File.Delete(pathToSave); }

            using (FileStream fs = new FileStream(pathToSave, FileMode.CreateNew)) {
                xmls.Serialize(fs, ahr);
                fs.Close();
            }
        }

        private void SetNamestringDefault() {
            string result = "Highlighting ";

            if (MakeMatchUsingType) {
                result += matchTct.ToString() + " events ";
            }

            if (MakeMatchUsingString) {
                if (MakeMatchUsingType) { result += "and "; }

                result += "strings " + (NotMatch ? "not matching " : "matching ");
                result += ComparisonStringToMatchOn;
            }
            NameString = result.Trim();
        }
    }

    internal class Coloring {
        public string matchCase;
        internal bool BackColorSpecified;

        // Default false
        internal bool ForeColorSpecified;

        private Color m_backgroundColor;
        private Color m_foregroundColor;
        // Defualt false

        internal Coloring(Color fg, Color bg, string match) {
            backgroundColor = bg;
            foregroundColor = fg;
            matchCase = match;
        }

        internal Color backgroundColor {
            get { return m_backgroundColor; }
            set {
                m_backgroundColor = value;
                BackColorSpecified = value != Color.Empty;
            }
        }

        internal Color foregroundColor {
            get { return m_foregroundColor; }
            set {
                m_foregroundColor = value;
                ForeColorSpecified = value != Color.Empty;
            }
        }

        internal bool MatchStringHighlight(string matcher) {
            if (matchCase == matcher) return true;
            return false;
        }
    }
}