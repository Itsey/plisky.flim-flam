//using Plisky.Diagnostics;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Plisky.UIWinforms.Controls {

    /// <summary>
    /// Events for the dropdown list
    /// </summary>
    /// <param name="sender">Sending control</param>
    /// <param name="e">Arguments</param>
    public delegate void DropDownListDisplayEventHandler(object sender, EventArgs e);

    /// <summary>
    /// Events for the dropdown list
    /// </summary>
    /// <param name="sender">Control triggering the event</param>
    /// <param name="e">Arguments</param>
    public delegate void DropDownListItemClickedEventHandler(object sender, SbtEventArgs e);

    /// <summary>
    /// Event Argument for when a toolstrip item that has click detection is clicked.  This will be passed
    /// to the event handler.
    /// </summary>
    public class SbtEventArgs : EventArgs {

        /// <summary>
        /// Creates a new instance of the SbtEventArgs class.
        /// </summary>
        public SbtEventArgs() {
            KeepPreventCloseActive = false;
        }

        /// <summary>
        /// Whether the click was in the image area
        /// </summary>
        public bool ClickInImageArea { get; set; }

        /// <summary>
        /// Whether the Close should continue to be blocked (default is false)
        /// </summary>
        public bool KeepPreventCloseActive { get; set; }

        /// <summary>
        /// The toolstrip item that was clicked
        /// </summary>
        public ToolStripItem SelectedItem { get; set; }

        /// <summary>
        /// The display text for the item that was clicked
        /// </summary>
        public string SelectedItemText { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public partial class SplitButton : Button {
        internal static TraceSwitch traceSbtControls = new TraceSwitch("traceSbtControl", "Trace enabled for split buttons", "Off");
        private const int PushButtonWidth = 14;

        private static int BorderSize = SystemInformation.Border3DSize.Width * 2;

        //protected Bilge b;
        private PushButtonState _state;

        private Rectangle dropDownRectangle = new Rectangle();
        private bool skipNextOpen;

        public SplitButton(/*Bilge useThisTrace=null*/) {
            InitializeComponent();

            /*if (useThisTrace==null) {
               b = new Bilge(tl: System.Diagnostics.TraceLevel.Verbose);
           }*/

            this.ContextMenuStrip = new ContextMenuStrip();
            this.AutoSize = true;
            this.PreventClose = false;
        }

        public SplitButton(IContainer container)
            : this() {
            container.Add(this);
        }

        public event DropDownListDisplayEventHandler OnBeforeSplitDropdownDisplayed;

        public event DropDownListItemClickedEventHandler OnSplitDropdownItemClicked;

        /// <summary>
        /// Gets or Sets a property indicating that the dropdown menu should not close when the image has been clicked rather than the text
        /// in one of the contextmenu items.
        /// </summary>
        public bool PreventClose { get; set; }

        internal bool IsImageClick { get; set; }

        internal bool NextCloseNotAllowed { get; set; }

        private PushButtonState State {
            get {
                return _state;
            }
            set {
                if (!_state.Equals(value)) {
                    _state = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Adds an item to the contextmenu strip and sends a specific custom on click which will determine whether the image has been clicked
        /// or not
        /// </summary>
        /// <param name="text">The text of the item to display</param>
        /// <param name="imageIndex">An image index for an assigned imagelist</param>
        public void AddStripItem(string text, int imageIndex) {
            //b.Info.Log("Add new item directly to the strip.  ", text);

            if ((this.ContextMenuStrip.ImageList == null) && (imageIndex >= 0) && (this.ImageList != null)) {
                this.ContextMenuStrip.ImageList = this.ImageList;
            }

            ToolStripMenuItem tsmi = new ToolStripMenuItem(text);
            tsmi.ImageIndex = imageIndex;
            tsmi.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            tsmi.MouseDown += new MouseEventHandler(tsmi_MouseDown);
            tsmi.Click += new EventHandler(tsmi_Click);

            this.ContextMenuStrip.Items.Add(tsmi);
        }

        public override Size GetPreferredSize(Size proposedSize) {
            Size preferredSize = base.GetPreferredSize(proposedSize);
            if (!string.IsNullOrEmpty(Text) && TextRenderer.MeasureText(Text, Font).Width + PushButtonWidth > preferredSize.Width) {
                return preferredSize + new Size(PushButtonWidth + BorderSize * 2, 0);
            }
            return preferredSize;
        }

        internal void TestKeyDownAndUp(KeyEventArgs ka) {
            OnKeyDown(ka);
            OnKeyUp(ka);
        }

        internal void TestMouseDown(MouseEventArgs ma) {
            OnMouseDown(ma);
            OnMouseUp(ma);
        }

        internal void TestMouseEnterLeave(EventArgs ea) {
            OnMouseEnter(ea);
            OnMouseLeave(ea);
        }

        protected override bool IsInputKey(Keys keyData) {
            if (keyData.Equals(Keys.Down)) {
                return true;
            } else {
                return base.IsInputKey(keyData);
            }
        }

        protected override void OnGotFocus(EventArgs e) {
            if (!State.Equals(PushButtonState.Pressed) && !State.Equals(PushButtonState.Disabled)) {
                State = PushButtonState.Default;
            }
        }

        protected override void OnKeyDown(KeyEventArgs kevent) {
            if (kevent.KeyCode.Equals(Keys.Down)) {
                ShowContextMenuStrip();
            } else if (kevent.KeyCode.Equals(Keys.Space) && kevent.Modifiers == Keys.None) {
                State = PushButtonState.Pressed;
            }

            base.OnKeyDown(kevent);
        }

        protected override void OnKeyUp(KeyEventArgs kevent) {
            if (kevent.KeyCode.Equals(Keys.Space)) {
                if (Control.MouseButtons == MouseButtons.None) {
                    State = PushButtonState.Normal;
                }
            }
            base.OnKeyUp(kevent);
        }

        /// <summary>
        /// Splitbutton recieves focus
        /// </summary>
        /// <param name="e">Focus arguments</param>
        protected override void OnLostFocus(EventArgs e) {
            if (!State.Equals(PushButtonState.Pressed) && !State.Equals(PushButtonState.Disabled)) {
                State = PushButtonState.Normal;
            }
        }

        /// <summary>
        /// SplitButton recieves Mouse Down
        /// </summary>
        /// <param name="e">Mouse down arguments</param>
        protected override void OnMouseDown(MouseEventArgs e) {
            if (dropDownRectangle.Contains(e.Location)) {
                ShowContextMenuStrip();
            } else {
                State = PushButtonState.Pressed;
            }
        }

        /// <summary>
        ///  Split button recieves mouse enter
        /// </summary>
        /// <param name="e">Mouse Enter Args</param>
        protected override void OnMouseEnter(EventArgs e) {
            if (!State.Equals(PushButtonState.Pressed) && !State.Equals(PushButtonState.Disabled)) {
                State = PushButtonState.Hot;
            }
        }

        /// <summary>
        /// Split Button recieves mouse Leave
        /// </summary>
        /// <param name="e">Arguments for MouseLeave</param>
        protected override void OnMouseLeave(EventArgs e) {
            if (!State.Equals(PushButtonState.Pressed) && !State.Equals(PushButtonState.Disabled)) {
                if (Focused) {
                    State = PushButtonState.Default;
                } else {
                    State = PushButtonState.Normal;
                }
            }
        }

        /// <summary>
        /// Occurs when the mouse is released up.
        /// </summary>
        /// <param name="mevent"></param>
        protected override void OnMouseUp(MouseEventArgs mevent) {
            if (ContextMenuStrip == null || !ContextMenuStrip.Visible) {
                SetButtonDrawState();
                if (Bounds.Contains(Parent.PointToClient(Cursor.Position)) && !dropDownRectangle.Contains(mevent.Location)) {
                    OnClick(new EventArgs());
                }
            }
        }

        /// <summary>
        /// Paints the split button.
        /// </summary>
        /// <param name="pevent"></param>
        protected override void OnPaint(PaintEventArgs pevent) {
            base.OnPaint(pevent);

            Graphics g = pevent.Graphics;
            Rectangle bounds = this.ClientRectangle;

            // draw the button background as according to the current state.
            if (State != PushButtonState.Pressed && IsDefault && !Application.RenderWithVisualStyles) {
                Rectangle backgroundBounds = bounds;
                backgroundBounds.Inflate(-1, -1);
                ButtonRenderer.DrawButton(g, backgroundBounds, State);

                // button renderer doesnt draw the black frame when themes are off =(
                g.DrawRectangle(SystemPens.WindowFrame, 0, 0, bounds.Width - 1, bounds.Height - 1);
            } else {
                ButtonRenderer.DrawButton(g, bounds, State);
            }

            // calculate the current dropdown rectangle.
            dropDownRectangle = new Rectangle(bounds.Right - PushButtonWidth - 1, BorderSize, PushButtonWidth, bounds.Height - BorderSize * 2);

            int internalBorder = BorderSize;
            Rectangle focusRect = new Rectangle(internalBorder, internalBorder, bounds.Width - dropDownRectangle.Width - internalBorder, bounds.Height - (internalBorder * 2));

            bool drawSplitLine = (State == PushButtonState.Hot || State == PushButtonState.Pressed || !Application.RenderWithVisualStyles);

            Point topLeftLinePosition = new Point();
            Point bottomRightLinePosition = new Point();

            if (RightToLeft == RightToLeft.Yes) {
                dropDownRectangle.X = bounds.Left + 1;
                focusRect.X = dropDownRectangle.Right;
                topLeftLinePosition.X = bounds.Left + PushButtonWidth;
                topLeftLinePosition.Y = BorderSize;
                bottomRightLinePosition.X = bounds.Left + PushButtonWidth;
                bottomRightLinePosition.Y = bounds.Bottom - BorderSize;
            } else {
                topLeftLinePosition.X = bounds.Right - PushButtonWidth;
                topLeftLinePosition.Y = BorderSize;
                bottomRightLinePosition.X = bounds.Right - PushButtonWidth;
                bottomRightLinePosition.Y = bounds.Bottom - BorderSize;
            }

            if (drawSplitLine) {
                Border3DStyle style = Border3DStyle.RaisedInner;
                ControlPaint.DrawBorder3D(g, topLeftLinePosition.X, topLeftLinePosition.Y, 0, bottomRightLinePosition.Y - topLeftLinePosition.Y, style);
            }
            // Draw an arrow in the correct location
            PaintArrow(g, dropDownRectangle);

            TextFormatFlags formatFlags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;

            if (!UseMnemonic) {
                formatFlags = formatFlags | TextFormatFlags.NoPrefix;
            } else if (!ShowKeyboardCues) {
                formatFlags = formatFlags | TextFormatFlags.HidePrefix;
            }

            if (!string.IsNullOrEmpty(this.Text)) {
                TextRenderer.DrawText(g, Text, Font, focusRect, SystemColors.ControlText, formatFlags);
            }
        }

        private static void PaintArrow(Graphics g, Rectangle dropDownRect) {
            Point middle = new Point(Convert.ToInt32(dropDownRect.Left + dropDownRect.Width / 2), Convert.ToInt32(dropDownRect.Top + dropDownRect.Height / 2));

            //if the width is odd - favor pushing it over one pixel right.
            middle.X += (dropDownRect.Width % 2);

            Point[] arrow = new Point[] { new Point(middle.X - 2, middle.Y - 1), new Point(middle.X + 3, middle.Y - 1), new Point(middle.X, middle.Y + 2) };

            g.FillPolygon(SystemBrushes.ControlText, arrow);
        }

        private void ContextMenuStrip_Closing(object sender, ToolStripDropDownClosingEventArgs e) {
            //b.Info.Log(traceSbtControls, "ContextMenuStrip_Closing", e.CloseReason.ToString());
            ContextMenuStrip cms = sender as ContextMenuStrip;

            if (NextCloseNotAllowed) {
                //  b.Info.Log(traceSbtControls, "NextCloseNotAllowed is active, menu is being prevent from closing");
                e.Cancel = true;
                return;
            }

            if (cms != null) {
                cms.Closing -= new ToolStripDropDownClosingEventHandler(ContextMenuStrip_Closing);
            }

            SetButtonDrawState();

            if (e.CloseReason == ToolStripDropDownCloseReason.AppClicked) {
                skipNextOpen = (dropDownRectangle.Contains(this.PointToClient(Cursor.Position)));
            }
        }

        private void SetButtonDrawState() {
            if (Bounds.Contains(Parent.PointToClient(Cursor.Position))) {
                State = PushButtonState.Hot;
            } else if (Focused) {
                State = PushButtonState.Default;
            } else {
                State = PushButtonState.Normal;
            }
        }

        private void ShowContextMenuStrip() {
            if (skipNextOpen) {
                // we were called because we're closing the context menu strip
                // when clicking the dropdown button.
                skipNextOpen = false;
                return;
            }

            if (OnBeforeSplitDropdownDisplayed != null) {
                OnBeforeSplitDropdownDisplayed(this, new EventArgs());
            }

            State = PushButtonState.Pressed;

            if (ContextMenuStrip != null) {
                ContextMenuStrip.Closing += new ToolStripDropDownClosingEventHandler(ContextMenuStrip_Closing);
                ContextMenuStrip.Show(this, new Point(0, Height), ToolStripDropDownDirection.BelowRight);
            }
        }

        private void tsmi_Click(object sender, EventArgs e) {
            //b.Info.Log(traceSbtControls, "Managed ToolsetMenuItem clicked, calling OnSplitDropdownItemClicked event handler");

            SbtEventArgs sbte = new SbtEventArgs() {
                SelectedItem = (ToolStripMenuItem)sender,
                SelectedItemText = ((ToolStripMenuItem)sender).Text,
                ClickInImageArea = IsImageClick
            };

            OnSplitDropdownItemClicked?.Invoke(this, sbte);

            if (!sbte.KeepPreventCloseActive) {
                NextCloseNotAllowed = false;
            } else {
                //b.Info.Log(traceSbtControls, "Menu will not close, NextCloseNotAllowed is still true");
            }
        }

        private void tsmi_MouseDown(object sender, MouseEventArgs e) {
            //b.Info.Log(traceSbtControls, "MouseDown occurs for managed toolstrip item.");
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;

            if ((clickedItem.Image != null) && ((clickedItem.DisplayStyle == ToolStripItemDisplayStyle.Image) || (clickedItem.DisplayStyle == ToolStripItemDisplayStyle.ImageAndText))) {
                // b.Verbose.Log("Image processing is active, attempting to resolve click location");
                BindingFlags getPropertyFlags = BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;
                PropertyInfo internalLayoutProperty = clickedItem.GetType().GetProperty("InternalLayout", getPropertyFlags);
                object internalLayoutValue = internalLayoutProperty.GetValue(clickedItem, null);
                PropertyInfo imageRectangleProperty = internalLayoutValue.GetType().GetProperty("ImageRectangle", getPropertyFlags);
                Rectangle imageRectangle = (Rectangle)imageRectangleProperty.GetValue(internalLayoutValue, null);

                IsImageClick = imageRectangle.Contains(e.Location);
                NextCloseNotAllowed = IsImageClick && PreventClose;
                // b.Info.Log(traceSbtControls, "LeavingMouseDown IsImageClick=" + IsImageClick.ToString() + "  NextCloseNotAllowed=" + NextCloseNotAllowed.ToString());
            }
        }
    }
}