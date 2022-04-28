#region Usings

using System.Collections.Generic;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Spotify.Response
{
    public class SpotifyUserProfileResponseModel
    {
        public string DisplayName { get; set; }

        public string Href { get; set; }

        public string Id { get; set; }

        public string Type { get; set; }

        public string Uri { get; set; }
    }
}
