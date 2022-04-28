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
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;
using UltimatePlaylist.Database.Infrastructure.Entities.Playlist;
using UltimatePlaylist.Database.Infrastructure.Entities.Playlist.Specifications;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Services.Common.Interfaces.Playlist;
using UltimatePlaylist.Services.Common.Interfaces.Schedule;
using UltimatePlaylist.Services.Common.Models.Playlist;
using UltimatePlaylist.Services.Common.Models.Schedule;

#endregion

namespace UltimatePlaylist.Services.Schedule
{
    public class ScheduleService : IScheduleService
    {
        #region Private members

        private readonly Lazy<IPlaylistService> PlaylistServiceProvider;

        private readonly Lazy<IRepository<PlaylistEntity>> PlaylistRepositoryProvider;

        private readonly Lazy<IMapper> MapperProvider;

        private readonly PlaylistConfig PlaylistConfig;

        #endregion

        #region Constructor(s)

        public ScheduleService(
            Lazy<IPlaylistService> playlistServiceProvider,
            Lazy<IRepository<PlaylistEntity>> playlistRepositoryProvider,
            Lazy<ILogger<ScheduleService>> loggerProvider,
            Lazy<IMapper> mapperProvider,
            IOptions<PlaylistConfig> playlistConfigOptions)
        {
            PlaylistServiceProvider = playlistServiceProvider;
            PlaylistRepositoryProvider = playlistRepositoryProvider;
            MapperProvider = mapperProvider;
            PlaylistConfig = playlistConfigOptions.Value;
        }

        #endregion

        #region Properties

        private IPlaylistService PlaylistService => PlaylistServiceProvider.Value;

        private IRepository<PlaylistEntity> PlaylistRepository => PlaylistRepositoryProvider.Value;

        private IMapper Mapper => MapperProvider.Value;

        #endregion

        #region Public Method(s)

        public async Task<Result<PlaylistsScheduleCalendarInfoReadServiceModel>> PlaylistsAsCalendarInfoAsync(DateTime calendarDate)
        {
            var firstDayOfMonth = new DateTime(calendarDate.Year, calendarDate.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            return await GetPlaylistsForMonthRangeAsync(firstDayOfMonth, lastDayOfMonth)
                .Bind(async playlists => await GeneratePlaylistsAsCalendarInfo(playlists, firstDayOfMonth, lastDayOfMonth));
        }

        public async Task<Result<AdminPlaylistReadServiceModel>> PlaylistsSongsAsync(Guid playlistExternalId)
        {
            return await PlaylistService.GetPlaylist(playlistExternalId);
        }

        public async Task<Result> AddSongToPlaylist(AddSongToPlaylistWriteServiceModel addSongToPlaylistWriteServiceModel)
        {
            return await PlaylistService.AddSongToPlaylistAsync(addSongToPlaylistWriteServiceModel);
        }

        public async Task<Result> RemoveSongFromPlaylistAsync(RemoveSongFromPlaylistWriteServiceModel removeSongFromPlaylistWriteServiceModel)
        {
            return await PlaylistService.RemoveSongFromPlaylistAsync(removeSongFromPlaylistWriteServiceModel);
        }

        public async Task<Result> RemoveAllSongsFromPlaylistAsync(Guid playlistExternalId)
        {
            return await PlaylistService.RemoveAllSongsFromPlaylistAsync(playlistExternalId);
        }

        #endregion

        #region Private Method(s)

        private async Task<Result<IReadOnlyList<PlaylistEntity>>> GetPlaylistsForMonthRangeAsync(
            DateTime firstDayOfMonth,
            DateTime lastDayOfMonth)
        {
            var playlists = await PlaylistRepository.ListAsync(new PlaylistSpecification()
                .ByMonthRange(PrepareStartDayByCalendarView(firstDayOfMonth), PrepareEndDayByCalendarView(lastDayOfMonth))
                .WithSongs());

            playlists.ToList().ForEach(s =>
            {
                s.PlaylistSongs = s.PlaylistSongs.Where(t => !t.IsDeleted).ToList();
            });

            return Result.Success(playlists);
        }

        private async Task<Result<PlaylistsScheduleCalendarInfoReadServiceModel>> GeneratePlaylistsAsCalendarInfo(
            IReadOnlyList<PlaylistEntity> playlistEntities,
            DateTime firstDayOfMonth,
            DateTime lastDayOfMonth)
        {
            var playlistsScheduleCalendarInfoReadServiceModel = new PlaylistsScheduleCalendarInfoReadServiceModel();
            playlistsScheduleCalendarInfoReadServiceModel.PlaylistsCalendarInfo = new List<PlaylistScheduleCalendarReadServiceModel>();

            var firstVisibledDayFromPreviousMonth = PrepareStartDayByCalendarView(firstDayOfMonth);
            var lastVisibledDayFromNextMonth = PrepareEndDayByCalendarView(lastDayOfMonth);
            var lastDayOfPreviousMonth = firstDayOfMonth.AddDays(-1);

            // Previous month visible days
            for (int i = firstVisibledDayFromPreviousMonth.Day; i <= lastDayOfPreviousMonth.Day; i++)
            {
                var result = await PrepareCalendarPlaylistsViewAsync(playlistEntities, lastDayOfPreviousMonth, i);

                if (result.IsFailure)
                {
                    return Result.Failure<PlaylistsScheduleCalendarInfoReadServiceModel>(result.Error);
                }

                playlistsScheduleCalendarInfoReadServiceModel.PlaylistsCalendarInfo.Add(result.Value);
            }

            // Current month visible days
            for (int i = 1; i <= lastDayOfMonth.Day; i++)
            {
                var result = await PrepareCalendarPlaylistsViewAsync(playlistEntities, lastDayOfMonth, i);

                if (result.IsFailure)
                {
                    return Result.Failure<PlaylistsScheduleCalendarInfoReadServiceModel>(result.Error);
                }

                playlistsScheduleCalendarInfoReadServiceModel.PlaylistsCalendarInfo.Add(result.Value);
            }

            // Next month visible days
            for (int i = 1; i <= lastVisibledDayFromNextMonth.Day; i++)
            {
                var result = await PrepareCalendarPlaylistsViewAsync(playlistEntities, lastVisibledDayFromNextMonth, i);

                if (result.IsFailure)
                {
                    return Result.Failure<PlaylistsScheduleCalendarInfoReadServiceModel>(result.Error);
                }

                playlistsScheduleCalendarInfoReadServiceModel.PlaylistsCalendarInfo.Add(result.Value);
            }

            return await Task.FromResult(playlistsScheduleCalendarInfoReadServiceModel);
        }

        private async Task<Result<PlaylistScheduleCalendarReadServiceModel>> PrepareCalendarPlaylistsViewAsync(
            IReadOnlyList<PlaylistEntity> playlistEntities,
            DateTime lastDayOfMonth,
            int day)
        {
            var playlistScheduleCalendarReadServiceModel = new PlaylistScheduleCalendarReadServiceModel();

            var dayDate = new DateTime(lastDayOfMonth.Year, lastDayOfMonth.Month, day).Add(PlaylistConfig.StartDateOffSet);

            var existPlaylist = playlistEntities.FirstOrDefault(s => s.StartDate.Date == dayDate.Date);

            if (existPlaylist != null)
            {
                playlistScheduleCalendarReadServiceModel.PlayListExternalId = existPlaylist.ExternalId;
                playlistScheduleCalendarReadServiceModel.PlaylistDate = existPlaylist.StartDate;
                playlistScheduleCalendarReadServiceModel.State = GetPlaylistStateBasedOnSongCount(existPlaylist.PlaylistSongs.Count);

                return playlistScheduleCalendarReadServiceModel;
            }

            return await AddNewPlaylistAsync(dayDate);
        }

        private PlaylistCompletionState GetPlaylistStateBasedOnSongCount(int songCount)
        {
            return songCount == PlaylistConfig.RequiredPlaylistSongsCount
                ? PlaylistCompletionState.Entire
                : songCount == 0
                    ? PlaylistCompletionState.Empty
                    : PlaylistCompletionState.NotFinished;
        }

        private async Task<Result<PlaylistScheduleCalendarReadServiceModel>> AddNewPlaylistAsync(
            DateTime startDate)
        {
            var addedPlaylist = await PlaylistRepository.AddAsync(new PlaylistEntity()
            {
                StartDate = startDate,
                IsFallback = false,
            });

            if (addedPlaylist != null)
            {
                var playlistScheduleCalendarReadServiceModel = new PlaylistScheduleCalendarReadServiceModel();

                playlistScheduleCalendarReadServiceModel.PlayListExternalId = addedPlaylist.ExternalId;
                playlistScheduleCalendarReadServiceModel.PlaylistDate = startDate;
                playlistScheduleCalendarReadServiceModel.State = PlaylistCompletionState.Empty;
                return playlistScheduleCalendarReadServiceModel;
            }

            return Result.Failure<PlaylistScheduleCalendarReadServiceModel>(ErrorMessages.ProblemWithInitializationPlaylistsInCurrentMonth);
        }

        private DateTime PrepareStartDayByCalendarView(DateTime startDate)
            => startDate.DayOfWeek switch
            {
                DayOfWeek.Sunday => startDate,
                DayOfWeek.Monday => startDate.AddDays(-1),
                DayOfWeek.Tuesday => startDate.AddDays(-2),
                DayOfWeek.Wednesday => startDate.AddDays(-3),
                DayOfWeek.Thursday => startDate.AddDays(-4),
                DayOfWeek.Friday => startDate.AddDays(-5),
                DayOfWeek.Saturday => startDate.AddDays(-6),
                _ => throw new Exception(ErrorType.NotSupportedDateFormat.ToString())
            };

        private DateTime PrepareEndDayByCalendarView(DateTime endDate)
            => endDate.DayOfWeek switch
            {
                DayOfWeek.Sunday => endDate.AddDays(6),
                DayOfWeek.Monday => endDate.AddDays(5),
                DayOfWeek.Tuesday => endDate.AddDays(4),
                DayOfWeek.Wednesday => endDate.AddDays(3),
                DayOfWeek.Thursday => endDate.AddDays(2),
                DayOfWeek.Friday => endDate.AddDays(1),
                DayOfWeek.Saturday => endDate,
                _ => throw new Exception(ErrorType.NotSupportedDateFormat.ToString())
            };

        #endregion
    }
}
