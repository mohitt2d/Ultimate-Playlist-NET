#region Usings

using UltimatePlaylist.Services.Common.Models.Notification;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Notification
{
    public interface INotificationService
    {
        public Task SendPushNotificationAsync(NotificationRequestModel notificationRequest);
    }
}
