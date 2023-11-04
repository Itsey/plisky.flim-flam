//using Plisky.Plumbing.Legacy;
using System;
using System.Drawing;

namespace Plisky.FlimFlam { 

    /// <summary>
    /// The view specific data is attached to each event entry and details how the envent entry should be viewed on the screen.  Eventually to
    /// conserve system resources this should be migrated to the view rather than the underlying data structures.
    /// </summary>
    internal struct ViewSpecificData {
        internal Color backgroundHighlightColor;

        internal Color foregroundHighlightColor;

        internal bool isBackgroundHighlighted;

        internal bool isForegroundHighlighted;

        internal bool isValid;

        internal bool isHighlighted {
            get { return isBackgroundHighlighted || isForegroundHighlighted; }
            // TODO:  This is pretty rough code, have forced the property to be set to false only which isnt even remotely
            // what a property should be about.  This is currently compatible with the code therefore left that like that.
            set {
                // TODO : this is horrid.  What awful design.
                //Bilge.Assert(value == false, "The is Highlighted property can only be used to clear a higlight rather than set it");
                if (value == false) {
                    throw new InvalidOperationException("Do not set this to true");
                }
                isBackgroundHighlighted = false;
                isForegroundHighlighted = false;
                backgroundHighlightColor = Color.Empty;
                foregroundHighlightColor = Color.Empty;
            }
        }

        internal string GetDiagnosticStringData() {
            string result = "VSD: " + (isValid ? " Is Vaid" : "Is Not Valid") + " \r\n";
            result += "Background Highlight (" + isBackgroundHighlighted.ToString() + ")(" + backgroundHighlightColor.ToString() + ")";
            result += "Foreground Highlight (" + isForegroundHighlighted.ToString() + ")(" + foregroundHighlightColor.ToString() + ")";
            return result;
        }

        //internal int bookMarkId;
    }
}