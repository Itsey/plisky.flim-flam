namespace Plisky.UIWinforms.Controls {

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// A delegate type allowing the change of colors to raise events/
    /// </summary>
    /// <param name="sender">The control triggering the change</param>
    /// <param name="e">The event args describing the color change</param>
    public delegate void ColorChangedEventHandler(object sender, ColorChangeEventArgs e);

    /// <summary>
    /// Determines the list of colors which is to be presented by the ColorComboList Control, each of the enum values
    /// refers to a set of colors which will be displayed within the control.
    /// </summary>
    public enum ColorListSet {

        /// <summary>
        /// Represents all of the known colors within the .net Known Colors enum.
        /// </summary>
        KnownColors,

        /// <summary>
        /// A list of system colors, a subset of the known colors enum.
        /// </summary>
        SystemColors,

        /// <summary>
        /// A set of quite distinct colors, a subset of other color lists
        /// </summary>
        FineColors,

        /// <summary>
        /// A set of defined foreground / background highlight colors
        /// </summary>
        DefaultHighlights
    }

    /// <summary>
    /// A ColorComboList presents a list of colors and allows a user to select one.  The color list is dependent on the set of colors
    /// used which is specified by the ColorSet property.  Selected color changes fire an OnSelectedColorChange event which identifies
    /// which colors have been selected.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class ColorComboList : UserControl {

        /// <summary>
        /// Holds the currently selected color set.
        /// </summary>
        private ColorListSet m_colorSetSelected = ColorListSet.KnownColors;

        /// <summary>
        /// Holds the currently selected color, starts with Color.Empty
        /// </summary>
        private ColorRepresentation m_currentlySelectedColor = new ColorRepresentation(Color.Empty, Color.Empty, "");

        /// <summary>
        /// Creates a new ColorComboList control
        /// </summary>
        public ColorComboList() {
            InitializeComponent();
            PopulateWithColors();
        }

        /// <summary>
        /// The event handler called when any color is selected.
        /// </summary>
        public event ColorChangedEventHandler SelectedColorChangedEventHandler;

        /// <summary>
        /// Gets or Sets the color set which the control will use to display colors to the user.
        /// </summary>
        public ColorListSet ColorSet {
            get {
                return this.m_colorSetSelected;
            }
            set {
                if (value != m_colorSetSelected) {
                    //Bilge.VerboseLog("Color Set Change Requested, changing to " + value.ToString());
                    this.m_colorSetSelected = value;
                    PopulateWithColors();
                }
            }
        }

        /// <summary>
        /// Gets the currently selected background color
        /// </summary>
        public Color SelectedBackgroundColor {
            get {
                return this.m_currentlySelectedColor.Background;
            }
        }

        /// <summary>
        /// Gets the name of the currently selected color or empty string for no selected color.
        /// </summary>
        public string SelectedColorName {
            get {
                return this.m_currentlySelectedColor.Text;
            }
        }

        /// <summary>
        /// Gets the currently selected foreground color
        /// </summary>
        public Color SelectedForegroundColor {
            get { return this.m_currentlySelectedColor.ForeGround; }
        }

        /// <summary>
        /// Sets the color selected to the named color, the named colur must exist in the set of currently selected colors.
        /// </summary>
        /// <param name="colorName"></param>
        public void SetSelectedColor(string colorName) {
            for (int i = 0; i < cboColorList.Items.Count; i++) {
                if (((ColorRepresentation)cboColorList.Items[i]).Text == colorName) {
                    cboColorList.SelectedIndex = i;
                    break;
                }
            }
        }

        /// <summary>
        /// Occurs when the selected color is changed, the event args describe the color change using Color.Empty for either the foreground
        /// or bacground when that color has not changed.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnSelectedColorChanged(ColorChangeEventArgs e) {
            if (SelectedColorChangedEventHandler != null) {
                SelectedColorChangedEventHandler(this, e);
            }
        }

        private static void DrawHighlight(bool ctrlHasFocus, Graphics ItemGraphics, Rectangle ItemRectangle) {
            if (!ctrlHasFocus) {
                ItemGraphics.FillRectangle(new SolidBrush(SystemColors.Window), ItemRectangle.Left, ItemRectangle.Top, ItemRectangle.Width, ItemRectangle.Height);
            } else {
                // use a highlight rectangle
                Pen BorderPen = new Pen(Color.FromKnownColor(KnownColor.Highlight));
                SolidBrush BackgroundBrush = new SolidBrush(System.Drawing.Color.FromKnownColor(KnownColor.Highlight));

                ItemGraphics.FillRectangle(BackgroundBrush, ItemRectangle.Left, ItemRectangle.Top, ItemRectangle.Width, ItemRectangle.Height);

                BorderPen.Dispose();
                BackgroundBrush.Dispose();
            }
        }

        private static ColorRepresentation[] GetColorRepresentationArray(ColorListSet array) {
            List<ColorRepresentation> result = new List<ColorRepresentation>();

            switch (array) {
                case ColorListSet.KnownColors:
                    // All of the colors in the known colors array.
                    foreach (KnownColor col in Enum.GetValues(typeof(KnownColor))) {
                        result.Add(new ColorRepresentation(Color.FromKnownColor(col)));
                    }
                    break;

                case ColorListSet.SystemColors:
                    string[] sysCols = new string[] { "ActiveBorder", "ActiveCaption", "ActiveCaptionText", "AppWorkspace", "Control", "ControlDark", "ControlDarkDark", "ControlLight", "ControlLightLight", "ControlText", "Desktop", "GrayText", "HighLight", "HighLightText", "HotTrack", "InactiveBorder", "InactiveCaption", "InactiveCaptionText", "Info", "InfoText", "Menu", "MenuText", "ScrollBar", "Window", "WindowFrame", "WindowText" };
                    foreach (string s in sysCols) {
                        result.Add(new ColorRepresentation(Color.FromName(s)));
                    }
                    break;

                case ColorListSet.FineColors:
                    string[] otherCols = new string[] { "Transparent", "Black", "DimGray", "Gray", "DarkGray", "Silver", "LightGray", "Gainsboro", "WhiteSmoke", "White", "RosyBrown", "IndianRed", "Brown", "Firebrick", "LightCoral", "Maroon", "DarkRed", "Red", "Snow", "MistyRose", "Salmon", "Tomato", "DarkSalmon", "Coral", "OrangeRed", "LightSalmon", "Sienna", "SeaShell", "Chocalate", "SaddleBrown", "SandyBrown", "PeachPuff", "Peru", "Linen", "Bisque", "DarkOrange", "BurlyWood", "Tan", "AntiqueWhite", "NavajoWhite", "BlanchedAlmond", "PapayaWhip", "Mocassin", "Orange", "Wheat", "OldLace", "FloralWhite", "DarkGoldenrod", "Cornsilk", "Gold", "Khaki", "LemonChiffon", "PaleGoldenrod", "DarkKhaki", "Beige", "LightGoldenrod", "Olive", "Yellow", "LightYellow", "Ivory", "OliveDrab", "YellowGreen", "DarkOliveGreen", "GreenYellow", "Chartreuse", "LawnGreen", "DarkSeaGreen", "ForestGreen", "LimeGreen", "PaleGreen", "DarkGreen", "Green", "Lime", "Honeydew", "SeaGreen", "MediumSeaGreen", "SpringGreen", "MintCream", "MediumSpringGreen", "MediumAquaMarine", "YellowAquaMarine", "Turquoise", "LightSeaGreen", "MediumTurquoise", "DarkSlateGray", "PaleTurquoise", "Teal", "DarkCyan", "Aqua", "Cyan", "LightCyan", "Azure", "DarkTurquoise", "CadetBlue", "PowderBlue", "LightBlue", "DeepSkyBlue", "SkyBlue", "LightSkyBlue", "SteelBlue", "AliceBlue", "DodgerBlue", "SlateGray", "LightSlateGray", "LightSteelBlue", "CornflowerBlue", "RoyalBlue", "MidnightBlue", "Lavender", "Navy", "DarkBlue", "MediumBlue", "Blue", "GhostWhite", "SlateBlue", "DarkSlateBlue", "MediumSlateBlue", "MediumPurple", "BlueViolet", "Indigo", "DarkOrchid", "DarkViolet", "MediumOrchid", "Thistle", "Plum", "Violet", "Purple", "DarkMagenta", "Magenta", "Fuchsia", "Orchid", "MediumVioletRed", "DeepPink", "HotPink", "LavenderBlush", "PaleVioletRed", "Crimson", "Pink", "LightPink" };
                    foreach (string s in otherCols) {
                        result.Add(new ColorRepresentation(Color.FromName(s)));
                    }
                    break;

                case ColorListSet.DefaultHighlights:
                    result.Add(new ColorRepresentation(Color.AntiqueWhite, Color.Navy, "SelectedHighlightText"));
                    result.Add(new ColorRepresentation(Color.AntiqueWhite, Color.DarkRed, "AngryHighlight"));
                    result.Add(new ColorRepresentation(Color.Black, Color.Yellow, "Highlight"));
                    result.Add(new ColorRepresentation(Color.Black, Color.Pink, "GirlyHighlight"));
                    result.Add(new ColorRepresentation(Color.Black, Color.Coral, "CoralWithBlackText"));
                    result.Add(new ColorRepresentation(Color.Black, Color.Gold, "GoldenShower"));
                    result.Add(new ColorRepresentation(Color.Red, Color.White, "RedText"));
                    result.Add(new ColorRepresentation(Color.Blue, Color.White, "BlueText"));
                    result.Add(new ColorRepresentation(Color.Green, Color.White, "GreenText"));
                    result.Add(new ColorRepresentation(Color.Maroon, Color.White, "MaroonText"));
                    result.Add(new ColorRepresentation(Color.White, Color.Red, "RedAlert"));
                    result.Add(new ColorRepresentation(Color.Black, Color.Orange, "AmberAlert"));
                    result.Add(new ColorRepresentation(Color.White, Color.DarkGreen, "GreenAlert"));
                    result.Add(new ColorRepresentation(Color.MediumSpringGreen, Color.Gray, "LuminousGreen"));
                    result.Add(new ColorRepresentation(Color.MediumSpringGreen, Color.DarkOrchid, "LuminousGreen"));
                    break;
            }

            return result.ToArray();
        }

        private void cboColorList_DrawItem(object sender, DrawItemEventArgs e) {
            // Draw the item?
            if (e.Index >= 0) {
                ColorRepresentation cr = (ColorRepresentation)cboColorList.Items[e.Index];

                if ((e.State & DrawItemState.Selected) > 0) {
                    ColorComboList.DrawHighlight(true, e.Graphics, e.Bounds);
                    this.ColorItem(e.Graphics, e.Bounds, cr, true);
                } else {
                    ColorComboList.DrawHighlight(false, e.Graphics, e.Bounds);
                    this.ColorItem(e.Graphics, e.Bounds, cr, false);
                }
            }
        }

        private void cboColorList_SelectedIndexChanged(object sender, EventArgs e) {
            ColorRepresentation cr = (ColorRepresentation)cboColorList.SelectedItem;
            this.m_currentlySelectedColor = cr;

            ColorChangeEventArgs ccArgs = new ColorChangeEventArgs(cr.ForeGround, cr.Background);
            if (!cr.UsesBackground) {
                ccArgs.Background = Color.Empty;
            }
            OnSelectedColorChanged(new ColorChangeEventArgs(cr.ForeGround, cr.Background));
        }

        private void ColorComboList_FontChanged(object sender, EventArgs e) {
            cboColorList.Font = this.Font;
        }

        private void ColorComboList_SizeChanged(object sender, EventArgs e) {
            cboColorList.Height = this.Height;
            cboColorList.Width = this.Width;
        }

        private void ColorItem(Graphics surfaceGraphics, Rectangle surfaceRect, ColorRepresentation colorRep, bool ctlHasFocus) {
            SolidBrush foregroundBrush = new SolidBrush(colorRep.ForeGround);
            SolidBrush backgroundBrush = new SolidBrush(colorRep.Background);

            bool dualColorMode = colorRep.UsesBackground;

            Color textBackgroundColor = colorRep.Background;

            if (ctlHasFocus) {
                textBackgroundColor = Color.FromKnownColor(KnownColor.HighlightText);
            } else {
                if (colorRep.UsesBackground) {
                    textBackgroundColor = colorRep.Background;
                } else {
                    textBackgroundColor = Color.FromKnownColor(System.Drawing.KnownColor.MenuText);
                }
            }

            SolidBrush textBrush = new SolidBrush(textBackgroundColor);

            // Sort out the color for the text brush first of all.  Either highlighted or background or normal

            Rectangle box_OuterRectangleFill = new Rectangle(surfaceRect.Left + 2, surfaceRect.Top + 2, 20, surfaceRect.Height - 4);
            Rectangle box_OuterRectangleOutline = new Rectangle(box_OuterRectangleFill.X - 1, box_OuterRectangleFill.Y - 1, box_OuterRectangleFill.Width + 1, box_OuterRectangleFill.Height + 1);
            Rectangle box_InnerRectangle = new Rectangle(box_OuterRectangleFill.Left + 4, box_OuterRectangleFill.Top + 2, box_OuterRectangleFill.Width - 8, box_OuterRectangleFill.Height - 4);
            Rectangle box_TextBackground = new Rectangle(surfaceRect.Left + 28, surfaceRect.Top + 1, surfaceRect.Width - 32, surfaceRect.Height - 2);

            SolidBrush colorSquareBox = new SolidBrush(colorRep.ForeGround);
            Pen p = new Pen(Color.Black, 1);

            surfaceGraphics.DrawRectangle(p, box_OuterRectangleOutline);

            if (dualColorMode) {
                surfaceGraphics.FillRectangle(backgroundBrush, box_OuterRectangleFill);
                surfaceGraphics.FillRectangle(foregroundBrush, box_InnerRectangle);
                surfaceGraphics.FillRectangle(backgroundBrush, box_TextBackground);

                surfaceGraphics.DrawString(colorRep.Text, cboColorList.Font, foregroundBrush, surfaceRect.Left + 28, surfaceRect.Top + 1);
            } else {
                surfaceGraphics.FillRectangle(colorSquareBox, box_OuterRectangleFill);
                surfaceGraphics.DrawRectangle(p, box_OuterRectangleOutline);
                surfaceGraphics.DrawString(colorRep.Text, cboColorList.Font, textBrush, surfaceRect.Left + 28, surfaceRect.Top + (surfaceRect.Height - cboColorList.Font.GetHeight()) / 2);
            }
            colorSquareBox.Dispose();
            p.Dispose();
            textBrush.Dispose();
        }

        private void PopulateWithColors() {
            //Bilge.VerboseLog("Populating Colors in Combo");
            cboColorList.Items.Clear();
            ColorRepresentation[] cols = GetColorRepresentationArray(m_colorSetSelected);

            cboColorList.Items.AddRange(cols);

            //Bilge.FurtherInfo("Color population complete, there are now " + cboColorList.Items.Count.ToString() + " colors loaded.");
        }
    }
}