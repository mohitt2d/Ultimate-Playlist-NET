#region Usings

using System;
using System.Collections.Generic;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Database.Infrastructure.Entities.Base;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Playlist
{
    public class UserPlaylistEntity : BaseEntity
    {
        #region Constructor(s)
        public UserPlaylistEntity()
        {
            UserPlaylistSongs = new List<UserPlaylistSongEntity>();
        }

        #endregion

        public DateTime StartDate { get; set; }

        public PlaylistState State { get; set; }

        public long? UserId { get; set; }

        #region Navigation properties

        public virtual User User { get; set; }

        public virtual ICollection<UserPlaylistSongEntity> UserPlaylistSongs { get; set; }

        #endregion
    }
}
