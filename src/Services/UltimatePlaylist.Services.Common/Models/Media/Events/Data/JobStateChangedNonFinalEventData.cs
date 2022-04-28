#region Usings

using Newtonsoft.Json;
using UltimatePlaylist.Common.Enums;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Media.Events.Data
{
    public class JobStateChangedNonFinalEventData
    {
        [JsonProperty("previousState")]
        public MediaServicesJobState PreviousState { get; set; }

        [JsonProperty("state")]
        public MediaServicesJobState State { get; set; }
    }
}
