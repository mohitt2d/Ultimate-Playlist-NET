#region Usings

using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Common.Mvc.Helpers;
using UltimatePlaylist.Database.Infrastructure.Entities.Games;
using UltimatePlaylist.Database.Infrastructure.Entities.Games.Specifications;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Services.Common.Interfaces.Games;
using UltimatePlaylist.Services.Common.Models.Games;

#endregion

namespace UltimatePlaylist.Services.Games
{
    public class WinningsInfoService : IWinningsInfoService
    {
        #region Private members

        private readonly Lazy<IMapper> MapperProvider;

        private readonly Lazy<IRepository<WinningEntity>> WinningRepositoryProvider;

        private readonly Lazy<IRepository<DailyCashDrawingEntity>> DailyCashDrawingRepositoryProvider;

        private readonly Lazy<IUltimatePayoutGameService> UltimatePayoutGameServiceProvider;

        private readonly PlaylistConfig PlaylistConfig;

        #endregion

        #region Constructor(s)

        public WinningsInfoService(
            Lazy<IMapper> mapperProvider,
            Lazy<IRepository<WinningEntity>> winningRepositoryProvider,
            Lazy<IRepository<DailyCashDrawingEntity>> dailyCashDrawingRepositoryProvider,
            Lazy<IUltimatePayoutGameService> ultimatePayoutGameServiceProvider,
            IOptions<PlaylistConfig> playlistConfig)
        {
            MapperProvider = mapperProvider;
            WinningRepositoryProvider = winningRepositoryProvider;
            DailyCashDrawingRepositoryProvider = dailyCashDrawingRepositoryProvider;
            UltimatePayoutGameServiceProvider = ultimatePayoutGameServiceProvider;
            PlaylistConfig = playlistConfig.Value;
        }

        #endregion

        #region Properties

        private IMapper Mapper => MapperProvider.Value;

        private IRepository<WinningEntity> WinningRepository => WinningRepositoryProvider.Value;

        private IRepository<DailyCashDrawingEntity> DailyCashDrawingRepository => DailyCashDrawingRepositoryProvider.Value;

        private IUltimatePayoutGameService UltimatePayoutGameService => UltimatePayoutGameServiceProvider.Value;

        #endregion

        public async Task<Result<WinnersReadServiceModel>> GetWinnersListAsync(Guid userExternalId)
        {
            var todayDate = DateTimeHelper.ToTodayUTCTimeForTimeZoneRelativeTime(PlaylistConfig.TimeZone);
            var currentGameDate = todayDate.Add(PlaylistConfig.StartDateOffSet);

            var lastDailyDrawingGame = await DailyCashDrawingRepository.FirstOrDefaultAsync(
                new DailyCashDrawingSpecification()
                .OrderByCreated(true));

            var dailyCashWinners = new List<WinnerProfileReadServiceModel>();
            if (lastDailyDrawingGame is not null)
            {
                var winnings = await WinningRepository.ListAsync(
                    new WinningSpecification()
                    .ByGameId(lastDailyDrawingGame.Id)
                    .WithUser());
                dailyCashWinners = Mapper.Map<List<WinnerProfileReadServiceModel>>(winnings);
            }

            return await UltimatePayoutGameService.GetUltimatePayoutInfoAsync(userExternalId)
                .Map(ultimateInfo => new WinnersReadServiceModel()
                {
                    DailyCashDrawingsWinners = dailyCashWinners,
                    DateTimestamp = currentGameDate.AddDays(-1),
                    UltimatePayoutUserNumbers = ultimateInfo.UltimatePayoutUserNumbers,
                    UltimatePayoutWinner = Mapper.Map<WinnerProfileReadServiceModel>(ultimateInfo.UltimatePayoutWinner),
                    UltimatePayoutWinningNumbers = ultimateInfo.UltimatePayoutWinningNumbers,
                });
        }
    }
}
