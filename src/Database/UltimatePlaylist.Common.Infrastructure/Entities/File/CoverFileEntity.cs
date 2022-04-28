#region Usings

using UltimatePlaylist.Database.Infrastructure.Entities.Song;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.File
{
    public class CoverFileEntity : BaseFileEntity
    {
        #region Service
        public long? SongId { get; set; }

        #endregion

        #region Navigation Properties

        public virtual SongEntity Song { get; set; }

        #endregion
    }
}
