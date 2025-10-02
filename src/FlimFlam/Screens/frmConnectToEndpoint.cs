namespace OldFlimflam.Screens {
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Flurl;
    using Flurl.Http;
    using Plisky.Diagnostics.FlimFlam;
    using Plisky.Flimflam;
    using Plisky.FlimFlam;

    public partial class frmConnectToEndpoint : Form {

        public frmConnectToEndpoint() {
            InitializeComponent();

            txtGoogleProjectId.Text = MexCore.TheCore.Options.UserDefaults["pubsub-projectid"];
            txtSubscription.Text = MexCore.TheCore.Options.UserDefaults["pubsub-sinkname"];
        }

        public string CallableURI { get; set; }

        public string FullGetUri {
            get {
                return CallableURI + "/bilge-api/ondemand/get/";
            }
        }

        public bool PollForUpdates { get; set; }

        private void BtnAdd_Click(object sender, EventArgs e) {
            string uriToUse = FullGetUri;
            bool poll = PollForUpdates;

            var jtlf = new Job_AddHttpPollGatherer(uriToUse, poll);
            MexCore.TheCore.WorkManager.AddJob(jtlf);
        }

        private async void BtnExecute_Click(object sender, EventArgs e) {
            Url u = txtUriBase.Text + txtFullUri.Text;
            var res = await u.AllowAnyHttpStatus().GetAsync();
            lbxResults.Items.Clear();
            if (res.ResponseMessage.StatusCode == System.Net.HttpStatusCode.OK) {
                string s = await res.ResponseMessage.Content.ReadAsStringAsync();
                lbxResults.Items.AddRange(s.Split('\n'));
            } else {
                _ = lbxResults.Items.Add($"ERROR :  {res.ResponseMessage.StatusCode}");
            }
        }

        private void BtnQcGetTrace_Click(object sender, EventArgs e) {
            txtFullUri.Text = "/bilge-api/ondemand/get/";
        }

        private void BtnQcSetTraceOn_Click(object sender, EventArgs e) {
            txtFullUri.Text = "/bilge-api/ondemand/set/";
        }


      

        private void ChkPollForUpdates_CheckedChanged(object sender, EventArgs e) {
            PollForUpdates = chkPollForUpdates.Checked;
        }


        private void TxtUri_TextChanged(object sender, EventArgs e) {
            CallableURI = string.Format($"http://{txtUri.Text}:{txtPort.Text}");
            txtUriBase.Text = CallableURI;
        }
    }

 
}