#region Usings

using System;

#endregion

namespace UltimatePlaylist.MobileApi.Models.Dsp.Spotify
{
    public class AddSongToSpotifyRequestModel
    {
        public Guid? SongExternalId { get; set; }

        public Guid? PlaylistExternalId { get; set; }
    }
}
