#region Using

using System;

#endregion

namespace UltimatePlaylist.MobileApi.Models.Song
{
    public class RateSongRequestModel
    {
        public Guid? ExternalId { get; set; }

        public Guid? PlaylistExternalId { get; set; }

        public int? Rating { get; set; }
    }
}
