#region Usings

using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Database.Infrastructure.Entities.Base;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Song
{
    public class SongGenreEntity : BaseEntity
    {
        #region Service

        public SongGenreType Type { get; set; }

        public long? SongId { get; set; }

        public long? GenreId { get; set; }

        #endregion

        #region Navigation properties

        public virtual GenreEntity Genre { get; set; }

        public virtual SongEntity Song { get; set; }

        #endregion
    }
}
