#region Usings

using UltimatePlaylist.Services.Common.Models.Webhook.Events.Data;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Webhook
{
    public interface IEventGridSubscriptionIdParserService
    {
        string Parse(SubscriptionDeletedEventData subscriptionDeletedEventData);
    }
}
