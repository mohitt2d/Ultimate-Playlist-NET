#region Usings

using System;
using UltimatePlaylist.Database.Infrastructure.Specifications;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Playlist.Specifications
{
    public class PlaylistSongSpecification : BaseSpecification<PlaylistSongEntity>
    {
        #region Constructor(s)

        public PlaylistSongSpecification(bool includeDeleted = false)
        {
            if (!includeDeleted)
            {
                AddCriteria(c => !c.IsDeleted);
            }
        }

        #endregion

        #region Filters

        public PlaylistSongSpecification ByPlaylistExternalId(Guid playlistExternalId)
        {
            AddCriteria(c => playlistExternalId.Equals(c.Playlist.ExternalId));

            return this;
        }

        public PlaylistSongSpecification BySongExternalId(Guid songExternalId)
        {
            AddCriteria(c => songExternalId.Equals(c.Song.ExternalId));

            return this;
        }

        public PlaylistSongSpecification ByNewerThanTodayDate()
        {
            AddCriteria(c => c.Playlist.StartDate > DateTime.UtcNow.Date);

            return this;
        }

        #endregion

        #region Include

        public PlaylistSongSpecification WithPlaylist()
        {
            AddInclude(c => c.Playlist);

            return this;
        }

        public PlaylistSongSpecification WithSong()
        {
            AddInclude(c => c.Song);

            return this;
        }

        #endregion
    }
}
