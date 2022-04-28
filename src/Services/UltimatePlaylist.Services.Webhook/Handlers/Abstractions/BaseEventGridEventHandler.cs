#region Usings

using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Services.Common.Interfaces.Webhook;
using UltimatePlaylist.Services.Common.Models.Webhook.Events;

#endregion

namespace UltimatePlaylist.Services.Webhook.Handlers.Abstractions
{
    public abstract class BaseEventGridEventHandler<TEventData>
        : IEventGridEventHandler<TEventData>, IEventGridEventHandler
        where TEventData : class
    {
        #region Private fields

        private readonly Lazy<IEventGridEventDeserializerService> EventGridEventDeserializerProvider;
        private readonly Lazy<IEventGridEventTypeValidatorService> EventGridEventTypeValidatorServiceProvider;

        #endregion

        #region Constructor(s)

        protected BaseEventGridEventHandler(
            Lazy<IEventGridEventDeserializerService> eventGridEventDeserializerProvider,
            Lazy<IEventGridEventTypeValidatorService> eventGridEventTypeValidatorServiceProvider,
            ILogger logger)
        {
            EventGridEventDeserializerProvider = eventGridEventDeserializerProvider;
            EventGridEventTypeValidatorServiceProvider = eventGridEventTypeValidatorServiceProvider;
            Logger = logger;
        }

        #endregion

        #region Properties

        public abstract EventGridEventType EventGridEventType { get; }

        protected ILogger Logger { get; }

        private IEventGridEventDeserializerService EventGridEventDeserializer => EventGridEventDeserializerProvider.Value;

        private IEventGridEventTypeValidatorService EventGridEventTypeValidator => EventGridEventTypeValidatorServiceProvider.Value;

        #endregion

        #region Public methods

        public async Task<Result> HandleAsync(JToken jsonToken)
        {
            var deserializedEvent = EventGridEventDeserializer.DeserializeToEventDataModel<TEventData>(jsonToken);

            return await EventGridEventTypeValidator.ValidateEventType(deserializedEvent, EventGridEventType)
                .OnFailure(() => Logger.LogWarning(
                    "Validation failed for event type. " +
                    "Expected type is {0}, but was {1}. " +
                    "Ensure that Webhook configuration is correct.",
                    EventGridEventType,
                    deserializedEvent.EventType))
                .Tap(() => Logger.LogInformation("Executing Event Grid handler for {0}.", EventGridEventType))
                .Bind(async () => await HandleEventAsync(deserializedEvent));
        }

        #endregion

        #region Protected methods

        protected abstract Task<Result> HandleEventAsync(EventGridEvent<TEventData> eventGridEvent);

        #endregion
    }
}
