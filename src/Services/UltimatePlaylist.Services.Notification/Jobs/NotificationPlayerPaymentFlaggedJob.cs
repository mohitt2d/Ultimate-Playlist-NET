#region Usings

using Microsoft.Extensions.Logging;
using UltimatePlaylist.Services.Common.Interfaces.Notification;
using UltimatePlaylist.Services.Common.Models.Notification;

#endregion

namespace UltimatePlaylist.Services.Notification.Jobs
{
    public class NotificationPlayerPaymentFlaggedJob
    {
        private const string Title = "Ultimate Playlist";
        private const string Message = "Please contact {0} Support for more information on your payment status";

        #region Private members

        private readonly Lazy<ILogger<NotificationPlayerPaymentFlaggedJob>> LoggerProvider;

        private readonly Lazy<INotificationService> NotificationServiceProvider;

        #endregion

        #region Constructor(s)

        public NotificationPlayerPaymentFlaggedJob(
            Lazy<ILogger<NotificationPlayerPaymentFlaggedJob>> loggerProvider,
            Lazy<INotificationService> notificationServiceProvider)
        {
            LoggerProvider = loggerProvider;
            NotificationServiceProvider = notificationServiceProvider;
        }

        #endregion

        #region Properties

        private ILogger<NotificationPlayerPaymentFlaggedJob> Logger => LoggerProvider.Value;

        private INotificationService NotificationService => NotificationServiceProvider.Value;

        #endregion

        #region Public methods

        public async Task RunNotificationsForPalyerPaymentStatusFlagged(string name, string email, string deviceToken)
        {
            try
            {
                await NotificationService.SendPushNotificationAsync(new NotificationRequestModel()
                {
                    DeviceToken = deviceToken,
                    Title = Title,
                    Message = string.Format(Message, email),
                    Recipient = name,
                });
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failure during sending notifications on player payment status change as flagged. Error: {ex.Message}");
            }
        }

        #endregion
    }
}
