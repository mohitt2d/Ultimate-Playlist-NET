#region Usings

using Newtonsoft.Json;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Media.Events.Data
{
    public class OutputErrorDetail
    {
        [JsonProperty("code")]
        public string DetailedCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
