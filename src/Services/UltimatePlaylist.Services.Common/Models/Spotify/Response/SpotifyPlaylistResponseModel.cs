#region Usings

using System.Collections.Generic;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Spotify.Response
{
    public class SpotifyPlaylistResponseModel
    {
        public bool Collaborative { get; set; }

        public string Description { get; set; }

        public ExternalUrlsResponseModel ExternalUrls { get; set; }

        public string Href { get; set; }

        public string Id { get; set; }

        public List<object> Images { get; set; }

        public string Name { get; set; }

        public OwnerResponseModel Owner { get; set; }

        public object PrimaryColor { get; set; }

        public bool Public { get; set; }

        public string SnapshotId { get; set; }

        public TracksResponseModel Tracks { get; set; }

        public string Type { get; set; }

        public string Uri { get; set; }
    }
}
