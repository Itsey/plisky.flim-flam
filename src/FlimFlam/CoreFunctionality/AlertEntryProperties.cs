namespace Plisky.FlimFlam {
    using System;
    using System.Text.Json.Serialization;

    public class AlertEntryProperties : SupportingMessageData {

        [JsonPropertyName("alert-name")]
        public string AlertName { get; set; }

        [JsonPropertyName("app-name")]
        public string AppName { get; set; }

        [JsonPropertyName("machine-name")]
        public string MachineName { get; set; }

        [JsonPropertyName("onlineAt")]
        public string OnlineAt { get; set; }

        [JsonPropertyName("app-ver")]
        public string Version { get; set; }

        internal override string RevertToDebugMessage(string currentDebugMessage) {
            throw new NotImplementedException();
        }

        internal override string RevertToMoreInfo(string currentMoreInfo) {
            throw new NotImplementedException();
        }
    }
}