using System;
using System.Drawing;

namespace Plisky.UIWinforms.Controls {

    /// <summary>
    /// Event arguments describing the change of a color in the color combo box.  The event arguments hold the colors that have changed when
    /// a color is selected.
    /// </summary>
    public class ColorChangeEventArgs : EventArgs {

        /// <summary>
        /// Used by the background property
        /// </summary>
        private Color m_background;

        /// <summary>
        /// Used by the foreground property
        /// </summary>
        private Color m_foreground;

        /// <summary>
        /// Creates a new ColorChangeEventArgs type passing in the changed colors.  Specify Color.Empty when no change has occured
        /// </summary>
        /// <param name="newFGCol"></param>
        /// <param name="newBGCol"></param>
        public ColorChangeEventArgs(Color newFGCol, Color newBGCol) {
            Foreground = newFGCol;
            Background = newBGCol;
        }

        /// <summary>
        /// The new background color, or Color.Empty when no background change occured
        /// </summary>
        public Color Background {
            get { return m_background; }
            set { m_background = value; }
        }

        /// <summary>
        /// The new foreground color, or Color.Empty when no foreground change occured
        /// </summary>
        public Color Foreground {
            get { return m_foreground; }
            set { m_foreground = value; }
        }
    }
}