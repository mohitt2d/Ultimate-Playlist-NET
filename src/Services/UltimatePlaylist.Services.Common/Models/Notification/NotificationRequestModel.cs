namespace UltimatePlaylist.Services.Common.Models.Notification
{
    public class NotificationRequestModel
    {
        public string DeviceToken { get; set; }

        public string Recipient { get; set; }

        public string Message { get; set; }

        public string Title { get; set; }

        public NotificationData NotificationData { get; set; }
    }
}
