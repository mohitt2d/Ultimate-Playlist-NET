#region Usings

using Microsoft.Extensions.Logging;
using UltimatePlaylist.Services.Common.Interfaces.Notification;
using UltimatePlaylist.Services.Common.Models.Notification;

#endregion

namespace UltimatePlaylist.Services.Notification.Jobs
{
    public class NotificationPlayerPaymentPaidJob
    {
        private const string Title = "Ultimate Playlist";
        private const string Message = "You have successfully been paid, please allow 3-5 days for the payment to process";

        #region Private members

        private readonly Lazy<ILogger<NotificationPlayerPaymentPaidJob>> LoggerProvider;

        private readonly Lazy<INotificationService> NotificationServiceProvider;

        #endregion

        #region Constructor(s)

        public NotificationPlayerPaymentPaidJob(
            Lazy<ILogger<NotificationPlayerPaymentPaidJob>> loggerProvider,
            Lazy<INotificationService> notificationServiceProvider)
        {
            LoggerProvider = loggerProvider;
            NotificationServiceProvider = notificationServiceProvider;
        }

        #endregion

        #region Properties

        private ILogger<NotificationPlayerPaymentPaidJob> Logger => LoggerProvider.Value;

        private INotificationService NotificationService => NotificationServiceProvider.Value;

        #endregion

        #region Public methods

        public async Task RunNotificationsForPalyerPaymentStatusPaid(string name, string deviceToken)
        {
            try
            {
                await NotificationService.SendPushNotificationAsync(new NotificationRequestModel()
                {
                    DeviceToken = deviceToken,
                    Title = Title,
                    Message = Message,
                    Recipient = name,
                });
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failure during sending notifications on player payment status change as paid. Error: {ex.Message}");
            }
        }

        #endregion
    }
}
