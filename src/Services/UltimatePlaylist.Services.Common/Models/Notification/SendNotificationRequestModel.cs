#region Usings

using Newtonsoft.Json;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Notification
{
    public class SendNotificationRequestModel
    {
        #region Contructor(s)

        public SendNotificationRequestModel(string recipient, string title, string body, object message, string priority = "High")
        {
            Recipient = recipient;
            Priority = priority;
            Data = new DataPayloadModel
            {
                Message = message,
            };
            Notification = new NotificationModel
            {
                AlertTitle = title,
                AlertBody = body,
            };
        }

        #endregion

        [JsonProperty("to")]
        public string Recipient { get; set; }

        [JsonProperty("priority")]
        public string Priority { get; set; }

        [JsonProperty("data")]
        public DataPayloadModel Data { get; set; }

        [JsonProperty("notification")]
        public NotificationModel Notification { get; set; }
    }
}
