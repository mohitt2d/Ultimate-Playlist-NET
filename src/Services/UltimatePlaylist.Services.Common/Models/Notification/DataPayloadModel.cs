#region Usings

using Newtonsoft.Json;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Notification
{
    [JsonObject("data")]
    public class DataPayloadModel
    {
        [JsonProperty("message")]
        public object Message { get; set; }
    }
}
