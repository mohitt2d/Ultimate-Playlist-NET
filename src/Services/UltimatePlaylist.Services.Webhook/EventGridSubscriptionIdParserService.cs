#region Usings

using System.Linq;
using UltimatePlaylist.Services.Common.Interfaces.Webhook;
using UltimatePlaylist.Services.Common.Models.Webhook.Events.Data;

#endregion

namespace UltimatePlaylist.Services.Webhook
{
    public class EventGridSubscriptionIdParserService : IEventGridSubscriptionIdParserService
    {
        public string Parse(SubscriptionDeletedEventData subscriptionDeletedEventData)
        {
            return subscriptionDeletedEventData.EventSubscriptionId.Split('/').Last();
        }
    }
}
