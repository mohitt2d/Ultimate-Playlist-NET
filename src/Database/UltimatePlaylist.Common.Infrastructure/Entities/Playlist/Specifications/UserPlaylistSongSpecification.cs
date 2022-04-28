#region Usings

using System;
using UltimatePlaylist.Database.Infrastructure.Specifications;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Playlist.Specifications
{
    public class UserPlaylistSongSpecification : BaseSpecification<UserPlaylistSongEntity>
    {
        #region Constructor(s)

        public UserPlaylistSongSpecification(bool includeDeleted = false)
        {
            if (!includeDeleted)
            {
                AddCriteria(c => !c.IsDeleted);
            }
        }

        #endregion

        #region Filters

        public UserPlaylistSongSpecification ByUserPlaylistExternalId(Guid userPlaylistExternalId)
        {
            AddCriteria(c => userPlaylistExternalId.Equals(c.UserPlaylist.ExternalId));

            return this;
        }

        public UserPlaylistSongSpecification ByPlaylistExternalId(Guid playlistExternalId)
        {
            AddCriteria(c => playlistExternalId.Equals(c.UserPlaylist.ExternalId));

            return this;
        }

        public UserPlaylistSongSpecification BySongExternalId(Guid songExternalId)
        {
            AddCriteria(c => songExternalId.Equals(c.Song.ExternalId));

            return this;
        }

        public UserPlaylistSongSpecification ByUserExternalId(Guid userExternalId)
        {
            AddCriteria(c => userExternalId.Equals(c.UserPlaylist.User.ExternalId));

            return this;
        }

        public UserPlaylistSongSpecification ByStartDate(DateTime date)
        {
            AddCriteria(c => c.UserPlaylist.StartDate.Equals(date.Date));

            return this;
        }

        public UserPlaylistSongSpecification OnlySkipped()
        {
            AddCriteria(c => c.IsSkipped);

            return this;
        }

        public UserPlaylistSongSpecification ByNewerThanDate(DateTime date)
        {
            AddCriteria(c => c.SkipDate >= date);

            return this;
        }

        #endregion

        #region Include

        public UserPlaylistSongSpecification WithSong()
        {
            AddInclude(c => c.Song);

            return this;
        }

        public UserPlaylistSongSpecification WithPlaylist()
        {
            AddInclude(c => c.UserPlaylist);
            AddInclude(c => c.UserPlaylist.User);

            return this;
        }

        #endregion
    }
}
