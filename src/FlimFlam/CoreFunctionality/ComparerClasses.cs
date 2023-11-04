using System.Collections;
using System.Windows.Forms;

namespace Plisky.FlimFlam { 

    internal class SortListViewItemsByGIdx : IComparer {

        #region IComparer Members

        public int Compare(object x, object y) {
            ListViewItem castX = (ListViewItem)x;
            ListViewItem castY = (ListViewItem)y;

            //Bilge.Assert(castX.Tag != null, " The element comparer assumes that the tag contains the glboal index");
            //Bilge.Assert(castY.Tag != null, " The element comparer assumes that the tag contains the glboal index");

            return (int)(((long)castX.Tag) - ((long)castY.Tag));
        }

        #endregion IComparer Members
    }
}