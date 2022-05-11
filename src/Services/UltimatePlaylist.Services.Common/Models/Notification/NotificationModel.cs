#region Usings

using Newtonsoft.Json;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Notification
{
    [JsonObject("notification")]
    public class NotificationModel
    {
        [JsonProperty("body")]
        public string AlertBody { get; set; }

        [JsonProperty("title")]
        public string AlertTitle { get; set; }

        [JsonProperty("sound")]
        public string Sound { get; set; } = "default";

        [JsonProperty("click_action")]
        public string ClickAction { get; set; } = "ON_NOTIFICATION";
    }
}
