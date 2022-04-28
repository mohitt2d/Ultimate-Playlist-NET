#region Usings

using Newtonsoft.Json;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Webhook.Events.Data
{
    public class SubscriptionDeletedEventData
    {
        [JsonProperty("eventSubscriptionId")]
        public string EventSubscriptionId { get; set; }
    }
}
