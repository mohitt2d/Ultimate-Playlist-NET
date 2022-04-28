#region Usings

using System;
using System.Collections.Generic;
using UltimatePlaylist.Database.Infrastructure.Entities.Base;
using UltimatePlaylist.Database.Infrastructure.Entities.Ticket;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Playlist
{
    public class PlaylistEntity : BaseEntity
    {
        #region Constructor(s)

        public PlaylistEntity()
        {
            PlaylistSongs = new List<PlaylistSongEntity>();
        }

        #endregion

        public DateTime StartDate { get; set; }

        public bool IsFallback { get; set; }

        #region Navigation properties

        public virtual ICollection<PlaylistSongEntity> PlaylistSongs { get; set; }

        #endregion
    }
}
