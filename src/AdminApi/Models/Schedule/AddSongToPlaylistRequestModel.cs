#region Usings

using System;

#endregion

namespace UltimatePlaylist.AdminApi.Models.Schedule
{
    public class AddSongToPlaylistRequestModel
    {
        public Guid? PlaylistExternalId { get; set; }

        public Guid? SongExternalId { get; set; }
    }
}
