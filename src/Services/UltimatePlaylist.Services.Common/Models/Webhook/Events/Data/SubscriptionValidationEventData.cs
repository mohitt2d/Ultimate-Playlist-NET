#region Usings

using Newtonsoft.Json;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Webhook.Events.Data
{
    public class SubscriptionValidationEventData
    {
        [JsonProperty("validationCode")]
        public string ValidationCode { get; set; }

        [JsonProperty("validationUrl")]
        public string ValidationUrl { get; set; }
    }
}
