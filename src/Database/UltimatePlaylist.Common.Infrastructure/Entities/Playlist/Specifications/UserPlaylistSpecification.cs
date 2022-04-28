#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using UltimatePlaylist.Common.Models;
using UltimatePlaylist.Database.Infrastructure.Entities.Song;
using UltimatePlaylist.Database.Infrastructure.Specifications;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Playlist.Specifications
{
    public class UserPlaylistSpecification : BaseSpecification<UserPlaylistEntity>
    {
        #region Constructor(s)

        public UserPlaylistSpecification(bool includeDeleted = false)
        {
            if (!includeDeleted)
            {
                AddCriteria(c => !c.IsDeleted);
            }
        }

        #endregion

        #region Filters

        public UserPlaylistSpecification ByExternalId(Guid externalId)
        {
            AddCriteria(c => externalId.Equals(c.ExternalId));

            return this;
        }

        public UserPlaylistSpecification ByDate(DateTime date)
        {
            AddCriteria(c => c.StartDate.Date <= date.Date);

            return this;
        }

        public UserPlaylistSpecification BySpecificDate(DateTime date)
        {
            AddCriteria(c => c.StartDate.Date.Equals(date.Date));

            return this;
        }

        public UserPlaylistSpecification ByNotEmptyPlaylist()
        {
            AddCriteria(c => c.UserPlaylistSongs.Count() > 0);

            return this;
        }

        public UserPlaylistSpecification ByUserExternalId(Guid userExternalId)
        {
            AddCriteria(c => c.User.ExternalId == userExternalId);

            return this;
        }

        public UserPlaylistSpecification NotToday()
        {
            AddCriteria(c => c.StartDate.Date != DateTime.UtcNow.Date);

            return this;
        }

        #endregion

        #region Ordering

        public UserPlaylistSpecification OrderByCreatedDescending()
        {
            ApplyOrderByDescending(s => s.Created);

            return this;
        }

        public UserPlaylistSpecification OrderByStartDateDescending()
        {
            ApplyOrderByDescending(s => s.StartDate);

            return this;
        }

        #endregion

        #region Include

        public UserPlaylistSpecification WithSongs()
        {
            AddInclude(c => c.UserPlaylistSongs);
            AddInclude($"{nameof(UserPlaylistEntity.UserPlaylistSongs)}.{nameof(UserPlaylistSongEntity.Song)}");

            return this;
        }

        public UserPlaylistSpecification WithCover()
        {
            AddInclude(c => c.UserPlaylistSongs);
            AddInclude($"{nameof(UserPlaylistEntity.UserPlaylistSongs)}.{nameof(UserPlaylistSongEntity.Song)}.{nameof(SongEntity.CoverFile)}");

            return this;
        }

        #endregion

        #region Pagination

        public class Paged : UserPlaylistSpecification
        {
            #region Constructor(s)

            public Paged(Pagination pagination = null)
            {
                if (pagination == null)
                {
                    ApplyOrderBy(c => c.Created, true);
                }
                else
                {
                    ApplyOrderBy(pagination);
                    ApplyPaging(pagination);
                }
            }

            private void ApplyOrderBy(Pagination pagination)
            {
                if (string.IsNullOrEmpty(pagination.OrderBy))
                {
                    return;
                }

                switch (pagination.OrderBy)
                {
                    case "created":
                        ApplyOrderBy(p => p.Created, pagination.Descending);
                        break;
                }
            }

            #endregion
        }

        #endregion
    }
}
