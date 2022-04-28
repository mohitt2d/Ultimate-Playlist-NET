#region Usings

using System.Collections.Generic;
using UltimatePlaylist.Database.Infrastructure.Entities.Base;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Song
{
    public class GenreEntity : BaseEntity
    {
        #region Constructor(s)
        public GenreEntity()
        {
            SongGenres = new List<SongGenreEntity>();
        }

        #endregion

        public string Name { get; set; }

        #region Navigation properties

        public virtual ICollection<SongGenreEntity> SongGenres { get; set; }

        #endregion
    }
}
