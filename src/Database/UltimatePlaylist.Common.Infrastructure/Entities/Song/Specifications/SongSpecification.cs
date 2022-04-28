#region Usings

using System;
using System.Linq;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Common.Models;
using UltimatePlaylist.Database.Infrastructure.Specifications;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Song.Specifications
{
    public class SongSpecification : BaseSpecification<SongEntity>
    {
        #region Constructor(s)

        public SongSpecification(bool includeDeleted = false)
        {
            if (!includeDeleted)
            {
                AddCriteria(c => !c.IsDeleted);
            }
        }

        #endregion

        #region Filters

        public SongSpecification ByExternalId(Guid songExternalId)
        {
            AddCriteria(c => c.ExternalId == songExternalId);

            return this;
        }

        public SongSpecification ByPlaylistId(long playlistId)
        {
            AddCriteria(c => c.PlaylistSongs.Any(p => p.PlaylistId == playlistId));

            return this;
        }

        public SongSpecification ByPlaylistExternalId(Guid playlistExternalId)
        {
            AddCriteria(c => c.UserPlaylistSongs.Any(p => p.UserPlaylist.ExternalId == playlistExternalId));

            return this;
        }

        #endregion

        #region Include

        public SongSpecification WithFullInfo()
        {
            WithDSPs();
            WithCover();
            WithAudioFile();
            WithGenres();
            WithSocialMedias();

            return this;
        }

        public SongSpecification WithDSPs()
        {
            AddInclude(c => c.SongDSPs);

            return this;
        }

        public SongSpecification WithCover()
        {
            AddInclude(c => c.CoverFile);

            return this;
        }

        public SongSpecification WithAudioFile()
        {
            AddInclude(c => c.AudioFile);

            return this;
        }

        public SongSpecification WithGenres()
        {
            AddInclude(c => c.SongGenres);
            AddInclude($"{nameof(SongEntity.SongGenres)}.{nameof(SongGenreEntity.Genre)}");

            return this;
        }

        public SongSpecification WithSocialMedias()
        {
            AddInclude(c => c.SongSocialMedias);

            return this;
        }

        public SongSpecification WithUserPlaylistSongs()
        {
            AddInclude(c => c.UserPlaylistSongs);

            return this;
        }

        public SongSpecification Search(string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                var isGuid = Guid.TryParse(search, out var searchGuid);
                AddCriteria(x => x.Artist.Contains(search)
                                 || x.Title.Contains(search)
                                 || x.Album.Contains(search)
                                 || x.SongGenres.Any(t => t.Genre.Name.Contains(search))
                                 || (isGuid && x.ExternalId.Equals(searchGuid)));
            }

            return this;
        }

        #endregion

        #region Pagination

        public class Paged : SongSpecification
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
                    case "album":
                        ApplyOrderBy(p => p.Album, pagination.Descending);
                        break;
                    case "title":
                        ApplyOrderBy(p => p.Title, pagination.Descending);
                        break;
                    case "plays":
                        ApplyOrderBy(p => p.UserSongHistory.Count, pagination.Descending);
                        break;
                    case "adds":
                        ApplyOrderBy(p => p.UserSongHistory.Where(s => s.IsAddedToSpotify).Count(), pagination.Descending);
                        break;
                    case "genre":
                        ApplyOrderBy(p => p.SongGenres.Where(s => s.Type == SongGenreType.Primary).FirstOrDefault().Genre.Name, pagination.Descending);
                        break;
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
