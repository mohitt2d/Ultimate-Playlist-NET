#region Usings

using CSharpFunctionalExtensions;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Services.Common.Models.Webhook.Events;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Webhook
{
    public interface IEventGridEventTypeValidatorService
    {
        Result ValidateEventType<TEventData>(
            EventGridEvent<TEventData> eventGridEvent,
            EventGridEventType exprectedEventType)
                where TEventData : class;
    }
}
