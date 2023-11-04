namespace OldFlimflam.Screens {
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Flurl;
    using Flurl.Http;
    using Google.Cloud.PubSub.V1;
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

        private void btnAdd_Click(object sender, EventArgs e) {
            string uriToUse = FullGetUri;
            bool poll = PollForUpdates;

            var jtlf = new Job_AddHttpPollGatherer(uriToUse, poll);
            MexCore.TheCore.WorkManager.AddJob(jtlf);
        }

        private async void btnExecute_Click(object sender, EventArgs e) {
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

        private void btnQcGetTrace_Click(object sender, EventArgs e) {
            txtFullUri.Text = "/bilge-api/ondemand/get/";
        }

        private void btnQcSetTraceOn_Click(object sender, EventArgs e) {
            txtFullUri.Text = "/bilge-api/ondemand/set/";
        }

        private void btnSave_Click(object sender, EventArgs e) {
        }

        private void btnTest_Click(object sender, EventArgs e) {
            string subName = txtSubscription.Text;
            string project = txtGoogleProjectId.Text;

            var s = new SubscriptionName(project, subName);
            var subscriber = SubscriberClient.Create(s);

            var t = subscriber.StartAsync((PubsubMessage message, CancellationToken cancel) => {
                // Process the message
                // Console.WriteLine(message.Data.ToStringUtf8());
                // Send ACK to acknowledge that message has been received and processed

                string s = message.Data.ToStringUtf8();

                // Get the jsonpayload property from the string s into a new string
                var d = JsonSerializer.Deserialize<Dummy>(s);
                if (d != null) {
                    s = d.jsonPayload.body.ToString();
                }
                IncomingMessageManager.Current.AddIncomingMessage(InternalSource.PubSubReciever, s, -1);

                return Task.FromResult(SubscriberClient.Reply.Ack);
                //await subscriber.AcknowledgeAsync(message.SystemProperties.Subscription, message.SystemProperties.MessageId);
            });
            lblAction.Text = "Subscribed To Pub Sub Event";
            MexCore.TheCore.MessageManager.AddTaskBasedImport(t);
            MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.PubSubEnable, UserMessageType.InformationMessage, "PubSuy - Connection Established.");
        }

        private void chkPollForUpdates_CheckedChanged(object sender, EventArgs e) {
            PollForUpdates = chkPollForUpdates.Checked;
        }

        private void label4_Click(object sender, EventArgs e) {
        }

        private void txtUri_TextChanged(object sender, EventArgs e) {
            CallableURI = string.Format($"http://{txtUri.Text}:{txtPort.Text}");
            txtUriBase.Text = CallableURI;
        }
    }

    public class InnerDummy {
        public JsonElement body { get; set; }
    }

    public class Dummy {
        public string insertId { get; set; }
        public InnerDummy jsonPayload { get; set; }
        public Dictionary<string, string> labels { get; set; }
        public string logName { get; set; }
        public string receiveTimestamp { get; set; }
        public JsonElement resource { get; set; }
        public string severity { get; set; }
        public string timestamp { get; set; }
    }
}