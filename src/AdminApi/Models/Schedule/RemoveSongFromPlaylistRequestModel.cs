#region Usings

using System;

#endregion

namespace UltimatePlaylist.AdminApi.Models.Schedule
{
    public class RemoveSongFromPlaylistRequestModel : PlaylistBaseRequestModel
    {
        public Guid? SongExternalId { get; set; }
    }
}
