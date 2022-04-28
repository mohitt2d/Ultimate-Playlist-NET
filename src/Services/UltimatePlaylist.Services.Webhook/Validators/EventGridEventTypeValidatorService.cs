#region Usings

using CSharpFunctionalExtensions;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Services.Common.Interfaces.Webhook;
using UltimatePlaylist.Services.Common.Models.Webhook.Events;

#endregion

namespace UltimatePlaylist.Services.Webhook.Validators
{
    public class EventGridEventTypeValidatorService : IEventGridEventTypeValidatorService
    {
        #region Public methods

        public Result ValidateEventType<TEventData>(
            EventGridEvent<TEventData> eventGridEvent,
            EventGridEventType exprectedEventType)
                where TEventData : class
        {
            return Result.SuccessIf(
                exprectedEventType == eventGridEvent.EventType,
                ErrorType.InvalidEventType.ToString());
        }

        #endregion
    }
}
