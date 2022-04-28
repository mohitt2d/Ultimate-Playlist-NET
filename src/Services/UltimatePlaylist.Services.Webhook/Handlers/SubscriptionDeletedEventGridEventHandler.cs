#region Usings

using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Services.Common.Interfaces.Webhook;
using UltimatePlaylist.Services.Common.Models.Webhook.Events;
using UltimatePlaylist.Services.Common.Models.Webhook.Events.Data;
using UltimatePlaylist.Services.Webhook.Handlers.Abstractions;

#endregion

namespace UltimatePlaylist.Services.Webhook.Handlers
{
    public class SubscriptionDeletedEventGridEventHandler : BaseEventGridEventHandler<SubscriptionDeletedEventData>
    {
        #region Private fields

        private readonly Lazy<IEventGridSubscriptionIdParserService> EventGridSubscriptionIdParserServiceProvider;

        #endregion

        #region Constructor(s)

        public SubscriptionDeletedEventGridEventHandler(
            Lazy<IEventGridSubscriptionIdParserService> eventGridSubscriptionIdParserServiceProvider,
            Lazy<IEventGridEventDeserializerService> eventGridEventDeserializerProvider,
            Lazy<IEventGridEventTypeValidatorService> eventGridEventTypeValidatorServiceProvider,
            ILogger<SubscriptionDeletedEventGridEventHandler> logger)
            : base(eventGridEventDeserializerProvider, eventGridEventTypeValidatorServiceProvider, logger)
        {
            EventGridSubscriptionIdParserServiceProvider = eventGridSubscriptionIdParserServiceProvider;
        }

        #endregion

        #region Properties

        public override EventGridEventType EventGridEventType => EventGridEventType.EventGridSubscriptionDeleted;

        private IEventGridSubscriptionIdParserService EventGridSubscriptionIdParser => EventGridSubscriptionIdParserServiceProvider.Value;

        #endregion

        #region Protected methods

        protected override Task<Result> HandleEventAsync(EventGridEvent<SubscriptionDeletedEventData> eventGridEvent)
        {
            var subscriptionId = EventGridSubscriptionIdParser.Parse(eventGridEvent.Data);

            Logger.LogInformation("Event Grid subscription {0} deleted.", subscriptionId);

            return Task.FromResult(Result.Success());
        }

        #endregion
    }
}
