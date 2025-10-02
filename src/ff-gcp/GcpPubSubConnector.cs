using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ff_gcp; 
internal class GcpPubSubConnector {

#if false
    private void BtnTest_Click(object sender, EventArgs e) {
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
                s = d.JsonPayload.Body.ToString();
            }
            IncomingMessageManager.Current.AddIncomingMessage(InternalSource.PubSubReciever, s, -1);

            return Task.FromResult(SubscriberClient.Reply.Ack);
            //await subscriber.AcknowledgeAsync(message.SystemProperties.Subscription, message.SystemProperties.MessageId);
        });
        lblAction.Text = "Subscribed To Pub Sub Event";
        MexCore.TheCore.MessageManager.AddTaskBasedImport(t);
        MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.PubSubEnable, UserMessageType.InformationMessage, "PubSuy - Connection Established.");
    }

       public class InnerDummy {
        public JsonElement Body { get; set; }
    }

    public class Dummy {
        public string InsertId { get; set; }
        public InnerDummy JsonPayload { get; set; }
        public Dictionary<string, string> Labels { get; set; }
        public string logName { get; set; }
        public string receiveTimestamp { get; set; }
        public JsonElement resource { get; set; }
        public string severity { get; set; }
        public string timestamp { get; set; }
    }

#endif

}
