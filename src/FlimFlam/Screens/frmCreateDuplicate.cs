using System.Windows.Forms;

namespace Plisky.FlimFlam { 

    /// <summary>
    /// Form displays a confirmation dialog to the user to determine whether they are sure that they wish to duplicate the current
    /// range selected out into a new application.
    /// </summary>
    internal partial class frmCreateDuplicate : Form {

        internal frmCreateDuplicate() {
            InitializeComponent();
        }

        internal void DescribeWhatsHappening(string appDescription, long lowRange, long hiRange) {
            lblIdentifyAppRange.Text = string.Format("You are duplicating application {0} from low index {1} to high index {2}.", appDescription, lowRange, hiRange);
            txtNewDupeName.Text = "Partial Duplicate of " + appDescription;
        }

        internal void DescribeWhatsHappening(string appDescription) {
            lblIdentifyAppRange.Text = string.Format("You are duplicating application {0}.", appDescription);
            txtNewDupeName.Text = "Duplicate of " + appDescription;
        }

        internal string GetNameForDupe() {
            return txtNewDupeName.Text;
        }
    }
}