#region usings

using Microsoft.Extensions.Logging;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.Common.Models;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity.Specifications;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Services.Common.Interfaces.Games;
using UltimatePlaylist.Services.Common.Interfaces.Notification;
using UltimatePlaylist.Services.Common.Models.Notification;

#endregion

namespace UltimatePlaylist.Services.Notification.Jobs
{
    public class NotificationReminderJob
    {
        private const string Title = "Ultimate Playlist";
        private const string Message = "You still have time to win up to $20k in the Ultimate Payout jackpot! Earn entries by listening to all 40 songs!";

        #region Private members

        private readonly Lazy<ILogger<NotificationBeforeGamesJob>> LoggerProvider;

        private readonly Lazy<IReadOnlyRepository<User>> UserRepositoryProvider;

        private readonly Lazy<INotificationService> NotificationServiceProvider;

        #endregion

        #region Constructor(s)

        public NotificationReminderJob(
            Lazy<ILogger<NotificationBeforeGamesJob>> loggerProvider,
            Lazy<IReadOnlyRepository<User>> userRepositoryProvider,
            Lazy<INotificationService> notificationServiceProvider,
            Lazy<IGamesWinningCollectionService> gamesWinningCollectionServiceProvider)
        {
            LoggerProvider = loggerProvider;
            UserRepositoryProvider = userRepositoryProvider;
            NotificationServiceProvider = notificationServiceProvider;
        }

        #endregion

        #region Properties

        private ILogger<NotificationBeforeGamesJob> Logger => LoggerProvider.Value;

        private IReadOnlyRepository<User> UserRepository => UserRepositoryProvider.Value;

        private INotificationService NotificationService => NotificationServiceProvider.Value;

        #endregion

        #region Public methods

        public async Task RunNotificationsBeforeGame()
        {
            try
            {
                var userCount = await UserRepository.CountAsync(
                    new UserSpecification()
                    .WithRoles()
                    .OnlyUsers()
                    .HasDeviceToken()
                    .IsNotificationEnabled());

                var totalPages = (int)Math.Ceiling((double)userCount / NotificationConst.PageSize);
                var usersSentTo = new HashSet<long>();

                for (var page = 1; page <= totalPages; ++page)
                {
                    var pagination = new Pagination(NotificationConst.PageSize, page, string.Empty, "created", true);
                    var users = await UserRepository.ListAsync(
                        new UserSpecification()
                        .Pagination(pagination)
                        .WithRoles()
                        .OnlyUsers()
                        .HasDeviceToken()
                        .IsNotificationEnabled());

                    foreach (var user in users)
                    {
                        if (!usersSentTo.Contains(user.Id))
                        {
                            await NotificationService.SendPushNotificationAsync(new NotificationRequestModel()
                            {
                                DeviceToken = user.DeviceToken,
                                Title = Title,
                                Message = Message,
                                Recipient = user.Name,
                            });

                            usersSentTo.Add(user.Id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failure during sending reminder notifications. Error: {ex.Message}");
            }
        }

        private async Task<IReadOnlyList<User>> GetUserForNotifications()
        {
            return await UserRepository.ListAsync(new UserSpecification()
                .OnlyUsers()
                .HasDeviceToken());
        }

        #endregion
    }
}
