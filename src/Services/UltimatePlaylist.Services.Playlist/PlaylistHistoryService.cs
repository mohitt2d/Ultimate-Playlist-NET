#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Common.Extensions;
using UltimatePlaylist.Common.Models;
using UltimatePlaylist.Common.Mvc.Helpers;
using UltimatePlaylist.Database.Infrastructure.Entities.Playlist;
using UltimatePlaylist.Database.Infrastructure.Entities.Playlist.Specifications;
using UltimatePlaylist.Database.Infrastructure.Entities.UserSongHistory;
using UltimatePlaylist.Database.Infrastructure.Entities.UserSongHistory.Specifications;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Services.Common.Interfaces.Playlist;
using UltimatePlaylist.Services.Common.Models;
using UltimatePlaylist.Services.Common.Models.Playlist;

#endregion

namespace UltimatePlaylist.Services.Playlist
{
    public class PlaylistHistoryService : IPlaylistHistoryService
    {
        #region Private members

        private readonly Lazy<IReadOnlyRepository<UserPlaylistEntity>> UserPlaylistRepositoryProvider;

        private readonly Lazy<IReadOnlyRepository<UserSongHistoryEntity>> UserSongHistoryRepositoryProvider;

        private readonly Lazy<IMapper> MapperProvider;

        private readonly PlaylistConfig PlaylistConfig;

        #endregion

        #region Constructor(s)

        public PlaylistHistoryService(
            Lazy<IReadOnlyRepository<UserPlaylistEntity>> userPlaylistRepositoryProvider,
            Lazy<IReadOnlyRepository<UserSongHistoryEntity>> userSongHistoryRepositoryProvider,
            Lazy<IMapper> mapperProvider,
            IOptions<PlaylistConfig> playlistConfig)
        {
            UserPlaylistRepositoryProvider = userPlaylistRepositoryProvider;
            UserSongHistoryRepositoryProvider = userSongHistoryRepositoryProvider;
            MapperProvider = mapperProvider;
            PlaylistConfig = playlistConfig.Value;
        }

        #endregion

        #region Properties

        private IReadOnlyRepository<UserPlaylistEntity> UserPlaylistRepository => UserPlaylistRepositoryProvider.Value;

        private IReadOnlyRepository<UserSongHistoryEntity> UserSongHistoryRepository => UserSongHistoryRepositoryProvider.Value;

        private IMapper Mapper => MapperProvider.Value;

        #endregion

        #region Public Method(s)

        public async Task<Result<PlaylistHistroryPaginatedReadServiceModel>> GetUserPlaylistsAsync(
            Guid userExternalId,
            Pagination pagination,
            PlaylistHistoryWriteServiceModel playlistHistoryWriteServiceModel)
        {
            var specification = new UserPlaylistSpecification()
             .ByUserExternalId(userExternalId)
             .NotToday();

            if (playlistHistoryWriteServiceModel.SelectedDate.HasValue)
            {
                specification = specification
                    .ByDate(playlistHistoryWriteServiceModel.SelectedDate.Value);
            }

            var todayDate = DateTimeHelper.ToTodayUTCTimeForTimeZoneRelativeTime(PlaylistConfig.TimeZone);
            var currentDate = todayDate.Add(PlaylistConfig.StartDateOffSet);

            var nextPlaylistAvalible = currentDate.Add(PlaylistConfig.StartDateOffSet).AddDays(1);
            var count = await UserPlaylistRepository.CountAsync(specification);

            return await GetUserPlaylistsWithoutDspStatusAsync(userExternalId, pagination, playlistHistoryWriteServiceModel)
                .Bind(async plylistsHistory => await SynchronizeAddedToDspSongStatus(userExternalId, plylistsHistory))
                .Map(playlistHistory =>
                {
                    var serviceModel = new PlaylistHistroryPaginatedReadServiceModel(playlistHistory.ToList(), pagination, count);
                    serviceModel.NextPlaylistAvailable = nextPlaylistAvalible;
                    return serviceModel;
                });
        }

        #endregion

        #region Private Methods

        private async Task<Result<IList<PlaylistHistoryReadServiceModel>>> GetUserPlaylistsWithoutDspStatusAsync(
            Guid userExternalId,
            Pagination pagination,
            PlaylistHistoryWriteServiceModel playlistHistoryWriteServiceModel)
        {
            var specification = new UserPlaylistSpecification
               .Paged(pagination)
               .ByUserExternalId(userExternalId)
               .NotToday()
               .OrderByStartDateDescending()
               .WithSongs()
               .WithCover();

            if (playlistHistoryWriteServiceModel.SelectedDate.HasValue)
            {
                specification = specification
                    .ByDate(playlistHistoryWriteServiceModel.SelectedDate.Value);
            }

            var userPlaylists = await UserPlaylistRepository.ListAsync(specification);

            return Result.Success()
                .Map(() => Mapper.Map<IList<PlaylistHistoryReadServiceModel>>(userPlaylists));
        }

        private async Task<Result<IList<PlaylistHistoryReadServiceModel>>> SynchronizeAddedToDspSongStatus(
            Guid userExternalId,
            IList<PlaylistHistoryReadServiceModel> playlistsHistory)
        {
            var userSongsHistory = await UserSongHistoryRepository.ListAsync(new UserSongHistorySpecification()
                .ByUserExternalId(userExternalId)
                .WithSong());

            playlistsHistory.ForEach(s =>
            {
                s.Songs.ForEach(t =>
                {
                    var userSongHistory = userSongsHistory.FirstOrDefault(c => c.Song.ExternalId == t.ExternalId);

                    if (userSongHistory != null)
                    {
                        t.IsAddedToAppleMusic = userSongHistory.IsAddedToAppleMusic;
                        t.IsAddedToSpotify = userSongHistory.IsAddedToSpotify;
                    }
                });
            });

            return Result.Success(playlistsHistory);
        }
        #endregion

    }
}
