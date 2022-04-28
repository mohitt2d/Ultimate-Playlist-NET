#region Usings

using UltimatePlaylist.Database.Infrastructure.Entities.Base;
using UltimatePlaylist.Database.Infrastructure.Entities.Song;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Playlist
{
    public class PlaylistSongEntity : BaseEntity
    {
        #region Service

        public long? SongId { get; set; }

        public long? PlaylistId { get; set; }

        #endregion

        #region Navigation properties

        public virtual PlaylistEntity Playlist { get; set; }

        public virtual SongEntity Song { get; set; }

        #endregion
    }
}
