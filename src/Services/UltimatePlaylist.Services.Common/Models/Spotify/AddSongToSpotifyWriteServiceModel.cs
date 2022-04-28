#region Usings

using System;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Spotify
{
    public class AddSongToSpotifyWriteServiceModel
    {
        public Guid SongExternalId { get; set; }

        public Guid PlaylistExternalId { get; set; }
    }
}
