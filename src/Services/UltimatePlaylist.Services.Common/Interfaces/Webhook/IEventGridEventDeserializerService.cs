#region Usings

using Newtonsoft.Json.Linq;
using UltimatePlaylist.Services.Common.Models.Webhook.Events;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Webhook
{
    public interface IEventGridEventDeserializerService
    {
        EventGridEvent<TEventData> DeserializeToEventDataModel<TEventData>(JToken jsonToken)
            where TEventData : class;
    }
}
