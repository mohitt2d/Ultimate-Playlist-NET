#region Usings

using Newtonsoft.Json;
using UltimatePlaylist.Common.Enums;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Media.Events.Data
{
    public class JobStateChangedFinalEventDataOutput
    {
        [JsonProperty("assetName")]
        public string AssetName { get; set; }

        [JsonProperty("error")]
        public OutputError Error { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("@odatatype")]
        public string ODataType { get; set; }

        [JsonProperty("progress")]
        public int Progress { get; set; }

        [JsonProperty("state")]
        public MediaServicesJobState State { get; set; }
    }
}
