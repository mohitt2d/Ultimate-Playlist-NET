#region Usings

using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimatePlaylist.Common.Mvc.Attributes;
using UltimatePlaylist.Common.Mvc.Controllers;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.MobileApi.Models.Notification;
using UltimatePlaylist.Services.Common.Interfaces.Games;
using UltimatePlaylist.Services.Common.Interfaces.Notification;
using UltimatePlaylist.Services.Notification.Jobs;
using static UltimatePlaylist.Common.Mvc.Consts.Consts;

#endregion

namespace UltimatePlaylist.MobileApi.Area.Notification
{
    [Area("Notification")]
    [Route("[controller]")]
    [ApiExplorerSettings(GroupName = MobileApiGroups.User)]
    public class NotificationController : BaseControllerWithAuthentication
    {
        #region Private Members

        private readonly Lazy<IMapper> MapperProvider;
        private readonly Lazy<INotificationsSettingsService> NotificationsSettingsServiceProvider;
        private readonly Lazy<ILogger<NotificationAfterGamesJob>> LoggingProvider;
        private readonly Lazy<IReadOnlyRepository<Database.Infrastructure.Entities.Identity.User>> UserRepositoryProvider;
        private readonly Lazy<INotificationService> NotificationServiceProvider;
        private readonly Lazy<IGamesWinningCollectionService> GamesWinningCollectionServiceProvider;
        private readonly Lazy<ILogger<NotificationReminderJob>> ReminderLoggerProvider;

        #endregion

        #region Constructor(s)

        public NotificationController(
            Lazy<IMapper> mapperProvider,
            Lazy<INotificationsSettingsService> notificationsSettingsServiceProvider,
            Lazy<ILogger<NotificationAfterGamesJob>> loggerProvider,
            Lazy<IReadOnlyRepository<Database.Infrastructure.Entities.Identity.User>> userRepositoryProvider,
            Lazy<INotificationService> notificationServiceProvider,
            Lazy<IGamesWinningCollectionService> gamesWinningCollectionServiceProvider,
            Lazy<ILogger<NotificationReminderJob>> reminderLoggerProvider)
        {
            MapperProvider = mapperProvider;
            NotificationsSettingsServiceProvider = notificationsSettingsServiceProvider;
            LoggingProvider = loggerProvider;
            UserRepositoryProvider = userRepositoryProvider;
            NotificationServiceProvider = notificationServiceProvider;
            GamesWinningCollectionServiceProvider = gamesWinningCollectionServiceProvider;
            ReminderLoggerProvider = reminderLoggerProvider;
        }

        #endregion

        #region Private Properites

        private IMapper Mapper => MapperProvider.Value;

        private INotificationsSettingsService NotificationsSettingsService => NotificationsSettingsServiceProvider.Value;

        #endregion

        #region POST

        [HttpPost("device-token")]
        [ProducesEmptyEnvelope(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateDeviceToken([FromBody] DeviceTokenRequestModel requestModel)
        {
            return await NotificationsSettingsService.SetDeviceToken(requestModel.DeviceToken, XUserExternalId)
               .Finally(BuildEnvelopeResult);
        }

        [HttpPost("logout")]
        [ProducesEmptyEnvelope(StatusCodes.Status200OK)]
        public async Task<IActionResult> Logout()
        {
            return await NotificationsSettingsService.RemoveDeviceToken(XUserExternalId)
                .Finally(BuildEnvelopeResult);
        }

        [HttpPost("update-enable-status")]
        [ProducesEmptyEnvelope(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateEnableStatus([FromBody] NotificationEnableRequestModel notificationEnableRequestModel)
        {
            return await NotificationsSettingsService.UpdateEnableStatusAsync(XUserExternalId, notificationEnableRequestModel.IsEnabled)
                .Finally(BuildEnvelopeResult);
        }

        #endregion

        [HttpGet("run-after-game-notification-manually")]
        public async Task RunNotificationsAfterGamesManuallyAsync()
        {
            var notificationsJob = new NotificationAfterGamesJob(LoggingProvider, UserRepositoryProvider,
                NotificationServiceProvider, GamesWinningCollectionServiceProvider);
            await notificationsJob.RunNotificationsAfterGame();
        }
        [HttpGet("run-reminder-notification-manually")]
        public async Task RunReminderNotificationsManuallyAsync()
        {
            var notificationsJob = new NotificationReminderJob(ReminderLoggerProvider, UserRepositoryProvider,
                NotificationServiceProvider);
            await notificationsJob.RunReminderNotifications();
        }
    }
}
