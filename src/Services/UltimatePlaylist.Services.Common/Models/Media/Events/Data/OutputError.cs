#region Usings

using System.Collections.Generic;
using Newtonsoft.Json;
using UltimatePlaylist.Common.Enums;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Media.Events.Data
{
    public class OutputError
    {
        public OutputError()
        {
            Details = new List<OutputErrorDetail>();
        }

        [JsonProperty("category")]
        public MediaServicesJobErrorCategoryType CategoryType { get; set; }

        [JsonProperty("code")]
        public MediaServicesJobErrorCode Code { get; set; }

        [JsonProperty("details")]
        public IList<OutputErrorDetail> Details { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("retry")]
        public MediaServicesJobRetryPolicyType RetryPolicyType { get; set; }
    }
}
