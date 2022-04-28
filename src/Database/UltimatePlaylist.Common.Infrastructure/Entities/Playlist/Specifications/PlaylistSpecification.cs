#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimatePlaylist.Database.Infrastructure.Entities.Song;
using UltimatePlaylist.Database.Infrastructure.Specifications;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Playlist.Specifications
{
    public class PlaylistSpecification : BaseSpecification<PlaylistEntity>
    {
        #region Constructor(s)

        public PlaylistSpecification(bool includeDeleted = false)
        {
            if (!includeDeleted)
            {
                AddCriteria(c => !c.IsDeleted);
            }
        }

        #endregion

        #region Filters

        public PlaylistSpecification ByExternalId(Guid externalId)
        {
            AddCriteria(c => externalId.Equals(c.ExternalId));

            return this;
        }

        public PlaylistSpecification ByExternalIds(IList<Guid> externalIds)
        {
            AddCriteria(c => externalIds.Contains(c.ExternalId));

            return this;
        }

        public PlaylistSpecification ByDate(DateTime date)
        {
            AddCriteria(c => c.StartDate.Date.Equals(date.Date));

            return this;
        }

        public PlaylistSpecification ByNotEmptyPlaylist()
        {
            AddCriteria(c => c.PlaylistSongs.Count() > 0);

            return this;
        }

        public PlaylistSpecification ByIsFallback()
        {
            AddCriteria(c => c.IsFallback);

            return this;
        }

        public PlaylistSpecification ByMonthRange(DateTime monthStart, DateTime monthEnd)
        {
            AddCriteria(c => c.StartDate >= monthStart && c.StartDate <= monthEnd);

            return this;
        }

        #endregion

        #region Ordering

        public PlaylistSpecification OrderByCreatedDescending()
        {
            ApplyOrderByDescending(s => s.Created);

            return this;
        }

        #endregion

        #region Include

        public PlaylistSpecification WithSongs()
        {
            AddInclude(c => c.PlaylistSongs);
            AddInclude($"{nameof(PlaylistEntity.PlaylistSongs)}.{nameof(PlaylistSongEntity.Song)}");

            return this;
        }

        public PlaylistSpecification WithCover()
        {
            AddInclude(c => c.PlaylistSongs);
            AddInclude($"{nameof(PlaylistEntity.PlaylistSongs)}.{nameof(PlaylistSongEntity.Song)}.{nameof(SongEntity.CoverFile)}");

            return this;
        }

        #endregion
    }
}
