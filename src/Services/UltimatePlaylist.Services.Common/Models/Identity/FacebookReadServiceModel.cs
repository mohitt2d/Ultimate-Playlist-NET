#region Usings

using Newtonsoft.Json;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Identity
{
    public class FacebookReadServiceModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("birthday")]
        public string Birthday { get; set; }
    }
}