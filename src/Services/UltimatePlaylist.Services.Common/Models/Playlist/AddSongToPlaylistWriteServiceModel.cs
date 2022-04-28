#region Usings

using System;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Playlist
{
    public class AddSongToPlaylistWriteServiceModel
    {
        public Guid PlaylistExternalId { get; set; }

        public Guid SongExternalId { get; set; }
    }
}
