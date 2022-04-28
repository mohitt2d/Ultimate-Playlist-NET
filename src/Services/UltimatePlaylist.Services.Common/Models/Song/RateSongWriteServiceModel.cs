#region Usings

using System;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Song
{
    public class RateSongWriteServiceModel
    {
        public Guid ExternalId { get; set; }

        public Guid PlaylistExternalId { get; set; }

        public int Rating { get; set; }
    }
}
