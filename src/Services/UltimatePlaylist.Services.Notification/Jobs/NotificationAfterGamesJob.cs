#region Usings

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
    public class NotificationAfterGamesJob
    {
        private const string Title = "Ultimate Playlist";
        private const string Message = "Today’s $20,000 Jackpot drawing results are in. Also, check now to see if you were one of todays lucky cash prize winners!";

        #region Private members

        private readonly Lazy<ILogger<NotificationAfterGamesJob>> LoggerProvider;

        private readonly Lazy<IReadOnlyRepository<User>> UserRepositoryProvider;

        private readonly Lazy<INotificationService> NotificationServiceProvider;

        private readonly Lazy<IGamesWinningCollectionService> GamesWinningCollectionServiceProvider;

        #endregion

        #region Constructor(s)

        public NotificationAfterGamesJob(
            Lazy<ILogger<NotificationAfterGamesJob>> loggerProvider,
            Lazy<IReadOnlyRepository<User>> userRepositoryProvider,
            Lazy<INotificationService> notificationServiceProvider,
            Lazy<IGamesWinningCollectionService> gamesWinningCollectionServiceProvider)
        {
            LoggerProvider = loggerProvider;
            UserRepositoryProvider = userRepositoryProvider;
            NotificationServiceProvider = notificationServiceProvider;
            GamesWinningCollectionServiceProvider = gamesWinningCollectionServiceProvider;
        }

        #endregion

        #region Properties

        private ILogger<NotificationAfterGamesJob> Logger => LoggerProvider.Value;

        private IReadOnlyRepository<User> UserRepository => UserRepositoryProvider.Value;

        private INotificationService NotificationService => NotificationServiceProvider.Value;

        private IGamesWinningCollectionService GamesWinningCollectionService => GamesWinningCollectionServiceProvider.Value;

        #endregion

        #region Public methods

        public async Task RunNotificationsAfterGame()
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
                        var isUnclaimed = await GamesWinningCollectionService.Get(user.ExternalId);
                        if (!usersSentTo.Contains(user.Id) && isUnclaimed is null)
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
                Logger.LogError($"Failure during sending notifications after games. Error: {ex.Message}");
            }
        }

        #endregion
    }
}
