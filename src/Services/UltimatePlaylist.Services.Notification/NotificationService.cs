#region Usings

using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Services.Common.Interfaces.Notification;
using UltimatePlaylist.Services.Common.Models.Notification;

#endregion

namespace UltimatePlaylist.Services.Notification
{
    public class NotificationService : INotificationService
    {
        #region Private Members

        private readonly Lazy<ILogger<NotificationService>> LoggerProvider;

        #endregion

        #region Contructor(s)

        public NotificationService(
            Lazy<ILogger<NotificationService>> loggerProvider,
            IOptions<FirebaseConfig> firebaseConfig)
        {
            LoggerProvider = loggerProvider;
            FirebaseConfig = firebaseConfig.Value;
        }

        #endregion

        #region Properties

        private FirebaseConfig FirebaseConfig { get; }

        private ILogger<NotificationService> Logger => LoggerProvider.Value;

        #endregion

        #region Public Methods

        public async Task SendPushNotificationAsync(NotificationRequestModel notificationRequest)
        {
            if (string.IsNullOrEmpty(notificationRequest.DeviceToken))
            {
                return;
            }

            var notification = new SendNotificationRequestModel(
                    notificationRequest.DeviceToken,
                    notificationRequest.Title,
                    notificationRequest.Message,
                    notificationRequest.NotificationData);

            var notificationJson = JsonConvert.SerializeObject(notification);
            var wasSuccessful = false;

            var request = new HttpRequestMessage(HttpMethod.Post, FirebaseConfig.NotificationUrl);

            request.Headers.TryAddWithoutValidation("Authorization", $"key={FirebaseConfig.ServerKey}");
            request.Content = new StringContent(notificationJson, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var result = await client.SendAsync(request);
                wasSuccessful = result.IsSuccessStatusCode;
            }

            if (wasSuccessful)
            {
                Logger.LogInformation($"Notification was sent to user: {notification.Recipient}");
            }
            else
            {
                Logger.LogError($"Notification to user: {notification.Recipient} Failed.");
            }
        }

        #endregion
    }
}