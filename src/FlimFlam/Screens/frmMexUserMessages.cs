using System.Reflection;
using System.Windows.Forms;

namespace Plisky.FlimFlam { 

    public partial class frmMexUserMessages : Form {

        public frmMexUserMessages() {
            InitializeComponent();
            lblMexVersionInformation.Text = "Mex Version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public void InitialiseFromMessageStore() {
            txtMostRecentMessage.Text = MexCore.TheCore.ViewManager.LastUserNotificationMessage;
            MexCore.TheCore.ViewManager.RenderUserNotificationLog(lbxUserMessages);
        }
    }
}