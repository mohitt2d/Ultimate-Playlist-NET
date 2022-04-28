#region Usings

using System;
using Newtonsoft.Json;
using UltimatePlaylist.Common.Enums;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Webhook.Events
{
    public class EventGridEvent
    {
        [JsonProperty("dataVersion")]
        public string DataVersion { get; set; }

        [JsonProperty("eventTime")]
        public DateTime EventTime { get; set; }

        [JsonProperty("eventType")]
        public EventGridEventType EventType { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("metadataVersion")]
        public string MetadataVersion { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("topic")]
        public string Topic { get; set; }
    }
}
