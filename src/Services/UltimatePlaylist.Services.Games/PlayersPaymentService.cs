#region Usings

using AutoMapper;
using CSharpFunctionalExtensions;
using Hangfire;
using Microsoft.Extensions.Options;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Common.Filters.Models;
using UltimatePlaylist.Common.Models;
using UltimatePlaylist.Database.Infrastructure.Entities.Games;
using UltimatePlaylist.Database.Infrastructure.Entities.Games.Specifications;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Services.Common.Interfaces.Games;
using UltimatePlaylist.Services.Common.Models;
using UltimatePlaylist.Services.Common.Models.UserManagment;
using UltimatePlaylist.Services.Notification.Jobs;

#endregion

namespace UltimatePlaylist.Services.Games
{
    public class PlayersPaymentService : IPlayersPaymentService
    {
        #region Private field(s)

        private Lazy<IRepository<WinningEntity>> WinningRepositoryProvider;
        private Lazy<IMapper> MapperProvider;
        private readonly NotificationConfig NotificationConfig;

        #endregion

        #region Constructor(s)

        public PlayersPaymentService(
            Lazy<IRepository<WinningEntity>> winningRepositoryProvider,
            Lazy<IMapper> mapperProvider,
            IOptions<NotificationConfig> notificationConfig)
        {
            WinningRepositoryProvider = winningRepositoryProvider;
            MapperProvider = mapperProvider;
            NotificationConfig = notificationConfig.Value;
        }

        #endregion

        #region Private properties

        private IRepository<WinningEntity> WinningRepository => WinningRepositoryProvider.Value;

        private IMapper Mapper => MapperProvider.Value;

        #endregion

        #region Public method(s)

        public async Task<Result<WinningStatus>> ChangePaymentStatus(Guid userExternalId, WinningStatus winningStatus, Guid winningExternalId)
        {
            var specification = new WinningSpecification()
                .ByExternalId(winningExternalId)
                .ByUserExternalId(userExternalId)
                .WithUser();

            return await GetWinning(specification)
                .Check(winning => ValidateUser(winning.Winner))
                .Tap(winning => winning.Status = winningStatus)
                .Tap(async winning => await WinningRepository.UpdateAndSaveAsync(winning))
                .Tap(async winning => await SendNotification(winning))
                .Map(winning => winning.Status);
        }

        public async Task<Result<PaginatedReadServiceModel<PlayerPaymentManagementListItemReadServiceModel>>> GetWinnersList(
            Pagination pagination,
            PlayerPaymentManagementFilterReadServiceModel filter)
        {
            var specification = new WinningSpecification()
                .WithUser()
                .Search(pagination.SearchValue)
                .Filter(
                    filter.WinningStatus,
                    filter.GameType,
                    filter.Age?.Min(x => x.MinAge) ?? null,
                    filter.Age?.Max(x => x.MaxAge) ?? null,
                    filter.IsAgeVerified);

            var count = await WinningRepository.CountAsync(specification);

            return await GetWinningsList(pagination, specification)
                .Map(winnings => Mapper.Map<IReadOnlyList<PlayerPaymentManagementListItemReadServiceModel>>(winnings))
                .Map(winnings => new PaginatedReadServiceModel<PlayerPaymentManagementListItemReadServiceModel>(winnings, pagination, count));
        }

        public async Task<Result<IReadOnlyList<PlayerPaymentReadServiceModel>>> GetPlayerPaymentFileData()
        {
            var specification = new WinningSpecification()
                .WithUser()
                .WithGame();

            var result = await WinningRepository.ListAsync(specification);

            return Result.Success(result)
                .Map(winnings => MapWinningsAndOrder(winnings));
        }

        private IReadOnlyList<PlayerPaymentReadServiceModel> MapWinningsAndOrder(IReadOnlyList<WinningEntity> winnings)
        {
            var result = Mapper.Map<IEnumerable<PlayerPaymentReadServiceModel>>(winnings);
            return result.OrderByDescending(x => x.Prize).Select((x, i) =>
            {
                x.Number = ++i;
                return x;
            }).ToList();
        }

        #endregion

        #region Private method(s)

        private async Task<Result<WinningEntity>> GetWinning(WinningSpecification specification)
        {
            var winning = await WinningRepository.FirstOrDefaultAsync(specification);

            return Result.SuccessIf(winning != null, ErrorMessages.WinningNotFound)
                    .Map(() => winning);
        }

        private Result ValidateUser(User winner)
        {
            return Result.SuccessIf(winner.IsAgeVerified, ErrorMessages.UserAgeNeedToVerifiedToChangePaymentStatus);
        }

        private async Task<Result<IReadOnlyList<WinningEntity>>> GetWinningsList(Pagination pagination, WinningSpecification specification)
        {
            specification = specification
                .WithGame()
                .Pagination(pagination);

            var users = await WinningRepository.ListAsync(specification);

            return Result.Success(users);
        }

        private async Task SendNotification(WinningEntity winning)
        {
            if (!string.IsNullOrEmpty(winning.Winner.DeviceToken) && winning.Winner.IsNotificationEnabled)
            {
                if (winning.Status == WinningStatus.Paid)
                {
                    BackgroundJob.Enqueue<NotificationPlayerPaymentPaidJob>(p => p.RunNotificationsForPalyerPaymentStatusPaid(winning.Winner.Name, winning.Winner.DeviceToken));
                }

                if (winning.Status == WinningStatus.Flagged)
                {
                    BackgroundJob.Enqueue<NotificationPlayerPaymentFlaggedJob>(p => p.RunNotificationsForPalyerPaymentStatusFlagged(winning.Winner.Name, NotificationConfig.SupportEmailAddress, winning.Winner.DeviceToken));
                }
            }
        }

        #endregion
    }
}
