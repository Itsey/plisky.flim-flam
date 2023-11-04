using System.Drawing;
using System.Windows.Forms;

namespace Plisky.FlimFlam { 

    /// <summary>
    /// Summary description for UIHelperRoutines.
    /// </summary>
    public abstract class UIHelperRoutines {

        // This returns the text of the found one which conveniently for us is the index
        internal static string FindSelectAndShow(ListView lvw, string match, bool caseSensitive) {
            string findThis = caseSensitive ? match.ToLower() : match;
            string inThis;

            foreach (ListViewItem lvi in lvw.Items) {
                inThis = lvi.Text;
                foreach (object o in lvi.SubItems) {
                    inThis += o.ToString(); ;
                }
                if (!caseSensitive) {
                    inThis = inThis.ToLower();
                }
                if (inThis.IndexOf(findThis) >= 0) {
                    // match.
                    lvw.SelectedItems.Clear();
                    lvw.EnsureVisible(lvi.Index);
                    lvw.Select();
                    lvi.Selected = true;
                    return lvi.Text;
                }
            } // End foreach lvi in the listview

            return null;
        }

        internal static Color GetRedGreenRangeByPercentile(int percentage) {
            if (percentage < 1) { percentage = 1; }
            if (percentage > 100) { percentage = 100; }

            percentage = (int)((double)percentage / 8.33);

            switch (percentage) {
                case 0:
                case 1: return Color.FromArgb(0, 255, 0);
                case 2: return Color.FromArgb(59, 240, 8);
                case 3: return Color.FromArgb(147, 217, 21);
                case 4: return Color.FromArgb(194, 205, 28);
                case 5: return Color.FromArgb(233, 194, 33);
                case 6: return Color.FromArgb(247, 189, 35);
                case 7: return Color.FromArgb(245, 172, 31);
                case 8: return Color.FromArgb(242, 151, 27);
                case 9: return Color.FromArgb(239, 130, 22);
                case 10: return Color.FromArgb(236, 104, 16);
                case 11: return Color.FromArgb(230, 67, 8);
                case 12: return Color.FromArgb(255, 0, 0);
            }

            return Color.Transparent;
        }

        // End FindSelectAndShow
    }
}