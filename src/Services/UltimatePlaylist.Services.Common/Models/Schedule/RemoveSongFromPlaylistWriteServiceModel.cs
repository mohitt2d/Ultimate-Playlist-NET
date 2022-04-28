#region Usings

using System;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Schedule
{
    public class RemoveSongFromPlaylistWriteServiceModel : PlaylistBaseWriteServiceModel
    {
        public Guid SongExternalId { get; set; }
    }
}
