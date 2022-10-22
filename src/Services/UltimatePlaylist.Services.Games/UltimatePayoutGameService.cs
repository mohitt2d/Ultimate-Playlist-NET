#region Usings

using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Common.Mvc.Helpers;
using UltimatePlaylist.Database.Infrastructure.Entities.Games;
using UltimatePlaylist.Database.Infrastructure.Entities.Games.Specifications;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity.Specifications;
using UltimatePlaylist.Database.Infrastructure.Entities.Ticket;
using UltimatePlaylist.Database.Infrastructure.Entities.Ticket.Specifications;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Services.Common.Interfaces.Games;
using UltimatePlaylist.Services.Common.Models.Games;

#endregion

namespace UltimatePlaylist.Services.Games
{
    public class UltimatePayoutGameService : IUltimatePayoutGameService
    {
        #region Private members

        private readonly Lazy<IUltimateWinningClaimService> UltimateWinningClaimServiceProvider;

        private readonly Lazy<IRepository<UltimatePayoutEntity>> UltimatePayoutRepositoryProvider;

        private readonly Lazy<IRepository<UserLotteryEntryEntity>> UserLoteryRepositoryProvider;

        private readonly Lazy<IRepository<TicketEntity>> TicketEntityRepositoryProvider;

        private readonly Lazy<IRepository<User>> UserRepositoryProvider;

        private readonly PlaylistConfig PlaylistConfig;

        private readonly GamesConfig GamesConfig;

        #endregion

        #region Constructor(s)

        public UltimatePayoutGameService(
            Lazy<IRepository<UltimatePayoutEntity>> ultimatePayoutRepositoryProvider,
            Lazy<IRepository<UserLotteryEntryEntity>> userLoteryRepositoryProvider,
            Lazy<IRepository<TicketEntity>> ticketEntityRepositoryProvider,
            Lazy<IRepository<User>> userRepositoryProvider,
            IOptions<PlaylistConfig> playlistConfig,
            IOptions<GamesConfig> gamesConfig,
            Lazy<IUltimateWinningClaimService> ultimateWinningClaimServiceProvider)
        {
            UltimatePayoutRepositoryProvider = ultimatePayoutRepositoryProvider;
            UserLoteryRepositoryProvider = userLoteryRepositoryProvider;
            TicketEntityRepositoryProvider = ticketEntityRepositoryProvider;
            UserRepositoryProvider = userRepositoryProvider;
            PlaylistConfig = playlistConfig.Value;
            GamesConfig = gamesConfig.Value;
            UltimateWinningClaimServiceProvider = ultimateWinningClaimServiceProvider;
        }

        #endregion

        #region Properties

        private IUltimateWinningClaimService UltimateWinningClaimService => UltimateWinningClaimServiceProvider.Value;

        private IRepository<UltimatePayoutEntity> UltimatePayoutRepository => UltimatePayoutRepositoryProvider.Value;

        private IRepository<UserLotteryEntryEntity> UserLoteryRepository => UserLoteryRepositoryProvider.Value;

        private IRepository<TicketEntity> TicketEntityRepository => TicketEntityRepositoryProvider.Value;

        private IRepository<User> UserRepository => UserRepositoryProvider.Value;

        #endregion

        #region Public Methods

        public async Task<Result<UltimatePayoutReadServiceModel>> CheckNewestGame(Guid userExternalId)
        {
            var todayDate = DateTimeHelper.ToTodayUTCTimeForTimeZoneRelativeTime(PlaylistConfig.TimeZone);
            var currentGameDate = todayDate.Add(PlaylistConfig.StartDateOffSet);

            var nextGame = await GetOrPrepUltimatePayoutGame(currentGameDate, false);

            return await Result.SuccessIf(nextGame.IsFinished, ErrorMessages.GameNotYetFinished)
                .Bind(async () => await GetUltimatePayoutInfoAsync(userExternalId));
        }

        public async Task<Result<UltimatePayoutReadServiceModel>> ClaimWinningsAsync(Guid userExternalId)
        {
            await UltimateWinningClaimService.Set(userExternalId);
            return await GetUltimatePayoutInfoAsync(userExternalId);
        }

        public async Task<Result<UltimatePayoutReadServiceModel>> GetUltimatePayoutInfoAsync(Guid userExternalId)
        {
            var todayDate = DateTimeHelper.ToTodayUTCTimeForTimeZoneRelativeTime(PlaylistConfig.TimeZone);
            var currentDate = todayDate.Add(PlaylistConfig.StartDateOffSet);

            var now = DateTimeHelper.ToUTCTimeForTimeZoneRelativeTime(DateTime.UtcNow, PlaylistConfig.TimeZone);
            var nextDate = (now < currentDate) ? currentDate : currentDate.AddDays(1);

            var lastGame = await GetOrPrepUltimatePayoutGame(currentDate, true);
            var nextGame = await GetOrPrepUltimatePayoutGame(nextDate, false);

            var userTickets = await TicketEntityRepository.CountAsync(
                new TicketSpecification()
                .OnlyNotUsed()
                .ByType(TicketType.Jackpot)
                .ByUserExternalIdUsingPlaylistRelation(userExternalId));

            var userNumbers = await UserLoteryRepository.ListAsync(
                new UserLotteryEntryEntitySpecification()
                .WithUser()
                .ByUserId(userExternalId)
                .ByGameId(nextGame.Id));

            var previousUserChosenNumbers = await UserLoteryRepository.ListAsync(
                new UserLotteryEntryEntitySpecification()
                .WithUser()
                .ByUserId(userExternalId)
                .ByGameId(lastGame.Id));

            UltimatePayoutWinnerReadServiceModel winner = default;
            var firstWinner = lastGame.Winnings.FirstOrDefault();
            var winnerClaim = await UltimateWinningClaimService.Get(userExternalId);

            if (winnerClaim is null && firstWinner?.Winner is not null)
            {
                winner = new UltimatePayoutWinnerReadServiceModel()
                {
                    IsCurrentUser = firstWinner.Winner.ExternalId.Equals(userExternalId),
                    ExternalId = firstWinner.Winner.ExternalId,
                    WinnerFullName = $"{firstWinner.Winner.Name} {firstWinner.Winner.LastName}",
                    WinnerUsername = $"{firstWinner.Winner.UserName}",
                    Amount = firstWinner.Amount,
                    WinnerAvatarUrl = firstWinner.Winner.AvatarFile?.Url,
                };
            }
            var timeDiff = Convert.ToInt32(Math.Floor((nextGame.GameDate - now).TotalSeconds));

            return Result.Success(new UltimatePayoutReadServiceModel()
            {
                NextUltimateDate = timeDiff,
                TicketsCount = userTickets,
                NextUltimatePrize = lastGame.Reward,
                UltimatePayoutUserNumbers = userNumbers.Select(c => new int[] { c.FirstNumber, c.SecondNumber, c.ThirdNumber, c.FourthNumber, c.FifthNumber, c.SixthNumber }).ToList(),
                UltimatePayoutYesterdayChosenUserNumbers = previousUserChosenNumbers.Select(c => new int[] { c.FirstNumber, c.SecondNumber, c.ThirdNumber, c.FourthNumber, c.FifthNumber, c.SixthNumber }).ToList(),
                UltimatePayoutWinner = winner,
                UltimatePayoutWinningNumbers = new int[]
                    {
                        lastGame.FirstNumber,
                        lastGame.SecondNumber,
                        lastGame.ThirdNumber,
                        lastGame.FourthNumber,
                        lastGame.FifthNumber,
                        lastGame.SixthNumber,
                    },
            });
        }

        public async Task<Result<UltimatePayoutReadServiceModel>> PostUltimatePayoutInfoAsync(Guid userExternalId, List<int> ultimatePayoutNumbers)
        {
            var user = await UserRepository.FirstOrDefaultAsync(
                new UserSpecification()
                .ByExternalId(userExternalId));
            var todayDate = DateTimeHelper.ToTodayUTCTimeForTimeZoneRelativeTime(PlaylistConfig.TimeZone);
            var currentDate = todayDate.Add(PlaylistConfig.StartDateOffSet);

            var now = DateTimeHelper.ToUTCTimeForTimeZoneRelativeTime(DateTime.UtcNow, PlaylistConfig.TimeZone);
            var nextDate = (now < currentDate) ? currentDate : currentDate.AddDays(1);

            var nextGame = await GetOrPrepUltimatePayoutGame(nextDate, false);

            return await Result.FailureIf(user is null, ErrorType.UserDoesNotExist.ToString())
                .Bind(async () => await CheckTicketAsycn(userExternalId))
                .Tap(async ticket => await UserLoteryRepository.AddAsync(
                new UserLotteryEntryEntity()
                    {
                        FirstNumber = ultimatePayoutNumbers[0],
                        SecondNumber = ultimatePayoutNumbers[1],
                        ThirdNumber = ultimatePayoutNumbers[2],
                        FourthNumber = ultimatePayoutNumbers[3],
                        FifthNumber = ultimatePayoutNumbers[4],
                        SixthNumber = ultimatePayoutNumbers[5],
                        GameId = nextGame.Id,
                        UserId = user.Id,
                    }))
                 .Tap(async ticket =>
                 {
                     ticket.IsUsed = true;
                     await TicketEntityRepository.UpdateAndSaveAsync(ticket);
                 })
                .Bind(async ticket => await GetUltimatePayoutInfoAsync(userExternalId));
        }

        #endregion

        #region Private methods

        private async Task<UltimatePayoutEntity> GetOrPrepUltimatePayoutGame(DateTime gameDate, bool isFinished)
        {
            var nextGame = await UltimatePayoutRepository.FirstOrDefaultAsync(
                new UltimatePayoutSpecification()
                .ByGameDate(gameDate)
                .WithWinners()
                .OrderByCreated(true)
                .ByIsFinished(isFinished));

            if (nextGame is null)
            {
                nextGame = await UltimatePayoutRepository.AddAsync(new UltimatePayoutEntity()
                {
                    GameDate = gameDate,
                    Reward = (GamesConfig.UltimateBaseReward != 0) ? GamesConfig.UltimateBaseReward : 20000,//2022-10-21 : 20000
                });
            }

            return nextGame;
        }

        private async Task<Result<TicketEntity>> CheckTicketAsycn(Guid userExternalId)
        {
            var userLotteryTicket = await TicketEntityRepository.FirstOrDefaultAsync(
              new TicketSpecification()
              .WithUserByPlaylist()
              .ByType(TicketType.Jackpot)
              .ByUserExternalIdUsingPlaylistRelation(userExternalId)
              .OnlyNotUsed());

            return Result.FailureIf(userLotteryTicket is null, userLotteryTicket, ErrorMessages.UserHasNoTickets);
        }

        #endregion
    }
}