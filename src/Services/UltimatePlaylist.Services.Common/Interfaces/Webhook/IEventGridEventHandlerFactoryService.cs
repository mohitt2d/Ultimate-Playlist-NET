#region Usings

using UltimatePlaylist.Common.Enums;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Webhook
{
    public interface IEventGridEventHandlerFactoryService
    {
        IEventGridEventHandler CreateHandler(EventGridEventType eventGridEventType);
    }
}
