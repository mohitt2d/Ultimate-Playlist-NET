#region Usings

using Newtonsoft.Json;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Webhook.Events
{
    public class EventGridEvent<TEventData> : EventGridEvent
        where TEventData : class
    {
        [JsonProperty("data")]
        public TEventData Data { get; set; }
    }
}
