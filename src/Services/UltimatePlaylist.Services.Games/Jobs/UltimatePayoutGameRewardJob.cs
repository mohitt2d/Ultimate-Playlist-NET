#region Usings

using Microsoft.Extensions.Options;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Common.Mvc.Helpers;
using UltimatePlaylist.Database.Infrastructure.Entities.Games;
using UltimatePlaylist.Database.Infrastructure.Entities.Games.Specifications;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;

#endregion

namespace UltimatePlaylist.Services.Games.Jobs
{
    public class UltimatePayoutGameRewardJob
    {
        #region Private members

        private readonly Lazy<IRepository<UltimatePayoutEntity>> UltimatePayoutRepositoryProvider;

        private readonly GamesConfig GamesConfig;

        private readonly PlaylistConfig PlaylistConfig;

        #endregion

        #region Constructor(s)

        public UltimatePayoutGameRewardJob(
            Lazy<IRepository<UltimatePayoutEntity>> ultimatePayoutRepositoryProvider,
            IOptions<GamesConfig> gamesOptions,
            IOptions<PlaylistConfig> playlistConfig)
        {
            UltimatePayoutRepositoryProvider = ultimatePayoutRepositoryProvider;
            GamesConfig = gamesOptions.Value;
            PlaylistConfig = playlistConfig.Value;
        }

        #endregion

        #region Properties

        private IRepository<UltimatePayoutEntity> UltimatePayoutRepository => UltimatePayoutRepositoryProvider.Value;

        #endregion

        #region Public methods

        public async Task RunUltimatePayoutGameReward()
        {
            var todayDate = DateTimeHelper.ToTodayUTCTimeForTimeZoneRelativeTime(PlaylistConfig.TimeZone);
            var currentGameDate = todayDate.Add(PlaylistConfig.StartDateOffSet).AddDays(1);
            var previousMonthDate = currentGameDate.AddMonths(-1);
            var comparisonDate = new DateTime(previousMonthDate.Year, previousMonthDate.Month, 1);

            var previousMonthGames = await UltimatePayoutRepository.ListAsync(
                 new UltimatePayoutSpecification()
                 .ByGameDateRange(comparisonDate, comparisonDate.AddMonths(1).AddDays(-1))
                 .WithWinners()
                 .ByIsFinished(true));

            if (!previousMonthGames.SelectMany(i => i.Winnings).Any())
            {
                var currentGame = await UltimatePayoutRepository.FirstOrDefaultAsync(
                 new UltimatePayoutSpecification()
                 .ByGameDate(currentGameDate)
                 .ByIsFinished(false));

                if (currentGame is null)
                {
                    await UltimatePayoutRepository.AddAsync(new UltimatePayoutEntity()
                    {
                        GameDate = currentGameDate,
                        Reward = (GamesConfig.UltimateBaseReward != 0) ? GamesConfig.UltimateBaseReward : 20000
                    }); ;
                }
                else
                {
                    currentGame.Reward += GamesConfig.UltimateIncrementReward;
                    await UltimatePayoutRepository.UpdateAndSaveAsync(currentGame);
                }
            }
        }

        #endregion
    }
}
