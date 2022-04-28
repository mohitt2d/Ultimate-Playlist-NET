#region Usings

using System;
using System.Collections.Generic;
using UltimatePlaylist.Database.Infrastructure.Specifications;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.UserSongHistory.Specifications
{
    public class UserSongHistorySpecification : BaseSpecification<UserSongHistoryEntity>
    {
        #region Constructor(s)

        public UserSongHistorySpecification(bool includeDeleted = false)
        {
            if (!includeDeleted)
            {
                AddCriteria(s => !s.IsDeleted);
            }
        }

        #endregion

        #region Filters

        public UserSongHistorySpecification ByUserExternalId(Guid userExternalId)
        {
            AddCriteria(s => s.User.ExternalId == userExternalId);

            return this;
        }

        public UserSongHistorySpecification BySongExternalId(Guid songExternalId)
        {
            AddCriteria(s => s.Song.ExternalId == songExternalId);

            return this;
        }

        public UserSongHistorySpecification BySongIds(ICollection<long> songsIds)
        {
            AddCriteria(s => songsIds.Contains(s.Song.Id));

            return this;
        }

        #endregion

        #region Ordering

        public UserSongHistorySpecification OrderByCreatedDescending()
        {
            ApplyOrderByDescending(s => s.Created);

            return this;
        }

        #endregion

        #region Include

        public UserSongHistorySpecification WithUser()
        {
            AddInclude(s => s.User);

            return this;
        }

        public UserSongHistorySpecification WithSong()
        {
            AddInclude(s => s.Song);

            return this;
        }

        #endregion
    }
}
