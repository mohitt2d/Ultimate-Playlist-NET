#region Usings

using System.Collections.Generic;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Spotify.Response
{
    public class TracksResponseModel
    {
        public string Href { get; set; }

        public List<object> Items { get; set; }

        public int Limit { get; set; }

        public object Next { get; set; }

        public int Offset { get; set; }

        public object Previous { get; set; }

        public int Total { get; set; }
    }
}
