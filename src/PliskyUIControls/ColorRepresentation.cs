using System.Drawing;

namespace Plisky.UIWinforms.Controls {

    /// <summary>
    /// The internal representation of a selected color which is used, this includes a foreground and background color
    /// as well as a string to describe the color and an indicator as to whether the background color is used.
    /// </summary>
    internal class ColorRepresentation {
        internal Color Background;
        internal Color ForeGround;
        internal string Text;
        internal bool UsesBackground;

        /// <summary>
        /// Creates a color representation specifying only a foreground, the background will be set to emtpy and the name set
        /// to the name of the foreground color.
        /// </summary>
        /// <param name="foreground">The color to set the foreground to</param>
        internal ColorRepresentation(Color foreground) {
            ForeGround = foreground;
            Background = Color.Empty;
            Text = ForeGround.Name;
            // UsesBackground = false; CA1805
        }

        /// <summary>
        /// Creates a color representation specifying all of the information
        /// </summary>
        /// <param name="fore">The foreground color to set</param>
        /// <param name="back">The background color to set</param>
        /// <param name="description">The name describing the foreground and background color</param>
        internal ColorRepresentation(Color fore, Color back, string description) {
            ForeGround = fore;
            Background = back;
            Text = description;
            UsesBackground = true;
        }
    }
}