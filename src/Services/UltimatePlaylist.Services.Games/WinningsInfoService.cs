#region Usings

using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Common.Models;
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

        private readonly Lazy<IRepository<UltimatePayoutEntity>> UltimatePayoutRepositoryProvider;

        private readonly Lazy<IRepository<DailyCashDrawingEntity>> DailyCashDrawingRepositoryProvider;

        private readonly Lazy<IRepository<UserLotteryEntryEntity>> UserLotteryEntryRepositoryProvider;

        private readonly Lazy<IUltimatePayoutGameService> UltimatePayoutGameServiceProvider;

        private readonly PlaylistConfig PlaylistConfig;

        #endregion

        #region Constructor(s)

        public WinningsInfoService(
            Lazy<IMapper> mapperProvider,
            Lazy<IRepository<WinningEntity>> winningRepositoryProvider,
            Lazy<IRepository<UltimatePayoutEntity>> ultimatePayoutRepositoryProvider,
            Lazy<IRepository<DailyCashDrawingEntity>> dailyCashDrawingRepositoryProvider,
            Lazy<IRepository<UserLotteryEntryEntity>> userLotteryEntryRepositoryProvider,
            Lazy<IUltimatePayoutGameService> ultimatePayoutGameServiceProvider,
            IOptions<PlaylistConfig> playlistConfig)
        {
            MapperProvider = mapperProvider;
            WinningRepositoryProvider = winningRepositoryProvider;
            UltimatePayoutRepositoryProvider = ultimatePayoutRepositoryProvider;
            DailyCashDrawingRepositoryProvider = dailyCashDrawingRepositoryProvider;
            UserLotteryEntryRepositoryProvider = userLotteryEntryRepositoryProvider;
            UltimatePayoutGameServiceProvider = ultimatePayoutGameServiceProvider;
            PlaylistConfig = playlistConfig.Value;
        }

        #endregion

        #region Properties

        private IMapper Mapper => MapperProvider.Value;

        private IRepository<WinningEntity> WinningRepository => WinningRepositoryProvider.Value;
        private IRepository<UltimatePayoutEntity> UltimatePayoutRepository => UltimatePayoutRepositoryProvider.Value;

        private IRepository<DailyCashDrawingEntity> DailyCashDrawingRepository => DailyCashDrawingRepositoryProvider.Value;
        private IRepository<UserLotteryEntryEntity> UserLotteryEntryRepository => UserLotteryEntryRepositoryProvider.Value;

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

        public async Task<Result<List<DailyCashWinnerResponseModel>>> GetDailyWinnersAsync(int pageSize = 10, int pageNumber = 1)
        {
            var dailyCashWinners = new List<DailyCashWinnerResponseModel>();
            var winnings = await WinningRepository.ListAsync(
                new WinningSpecification()
                .WithGame()
                .WithUser()
                .Pagination(new Pagination(pageSize: pageSize, pageNumber: pageNumber, searchValue: String.Empty, orderBy: "created", desc: true)));
            dailyCashWinners = Mapper.Map<List<DailyCashWinnerResponseModel>>(winnings);

            return Result.Success(dailyCashWinners);
        }

        public async Task<Result<List<JackpotWinnersAndNumbersResponseModel>>> GetUltimatePayoutInfoPublicAsync(int pageSize = 10, int pageNumber = 1)
        {
            var response = new List<JackpotWinnersAndNumbersResponseModel>();
            var results = await UltimatePayoutRepository.ListAsync(
                new UltimatePayoutSpecification()
                .OrderByCreated(true)
                .WithWinners()
                .Pagination(new Pagination(pageSize: pageSize, pageNumber: pageNumber))
                .ByIsFinished(true));

            response = Mapper.Map<List<JackpotWinnersAndNumbersResponseModel>>(results);

            return Result.Success(response);
        }

        public async Task<Result<List<WinningHistoryReadServicModel>>> GetWinningHistory(Guid userExternalId)
        {
            var responseDaily = new List<WinningHistoryReadServicModel>();
            var winningHistory = await WinningRepository.ListAsync(
                new WinningSpecification()
                .ByUserExternalId(userExternalId)
                .WithGame()
                );
            responseDaily = Mapper.Map<List<WinningHistoryReadServicModel>>(winningHistory);

            /*var responsePayout = new List<WinningHistoryReadServicModel>();
            var payoutHistory = await UserLotteryEntryRepository.ListAsync(
                new UserLotteryEntryEntitySpecification()
                .ByUserId(userExternalId)
                .WithGame()
               );

            responsePayout = Mapper.Map<List<WinningHistoryReadServicModel>>(payoutHistory);

            responseDaily.AddRange(responsePayout);*/

            var response = responseDaily.OrderByDescending(x => x.Date).ToList();
            return Result.Success(response);
        }
    }
}