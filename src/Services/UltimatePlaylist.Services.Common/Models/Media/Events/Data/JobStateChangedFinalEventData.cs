#region Usings

using System.Collections.Generic;
using Newtonsoft.Json;
using UltimatePlaylist.Common.Enums;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Media.Events.Data
{
    public class JobStateChangedFinalEventData
    {
        public JobStateChangedFinalEventData()
        {
            Outputs = new List<JobStateChangedFinalEventDataOutput>();
        }

        [JsonProperty("outputs")]
        public IList<JobStateChangedFinalEventDataOutput> Outputs { get; set; }

        [JsonProperty("previousState")]
        public MediaServicesJobState PreviousState { get; set; }

        [JsonProperty("state")]
        public MediaServicesJobState State { get; set; }
    }
}
