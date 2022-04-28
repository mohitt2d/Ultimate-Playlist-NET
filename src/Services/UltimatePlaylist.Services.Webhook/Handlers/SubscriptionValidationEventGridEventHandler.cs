#region Usings

using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Hangfire;
using Microsoft.Extensions.Logging;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Services.Common.Interfaces.Webhook;
using UltimatePlaylist.Services.Common.Models.Webhook.Events;
using UltimatePlaylist.Services.Common.Models.Webhook.Events.Data;
using UltimatePlaylist.Services.Webhook.Handlers.Abstractions;
using UltimatePlaylist.Services.Webhook.Jobs;

#endregion

namespace UltimatePlaylist.Services.Webhook.Handlers
{
    public class SubscriptionValidationEventGridEventHandler : BaseEventGridEventHandler<SubscriptionValidationEventData>
    {
        #region Private fields

        private readonly Lazy<IBackgroundJobClient> BackgroundJobClientProvider;

        #endregion

        #region Constructor(s)

        public SubscriptionValidationEventGridEventHandler(
            Lazy<IBackgroundJobClient> backgroundJobClientProvider,
            Lazy<IEventGridEventDeserializerService> eventGridEventDeserializerProvider,
            Lazy<IEventGridEventTypeValidatorService> eventGridEventTypeValidatorServiceProvider,
            ILogger<SubscriptionValidationEventGridEventHandler> logger)
            : base(eventGridEventDeserializerProvider, eventGridEventTypeValidatorServiceProvider, logger)
        {
            BackgroundJobClientProvider = backgroundJobClientProvider;
        }

        #endregion

        #region Properties

        public override EventGridEventType EventGridEventType => EventGridEventType.EventGridSubscriptionValidation;

        private IBackgroundJobClient BackgroundJobClient => BackgroundJobClientProvider.Value;

        #endregion

        #region Protected methods

        protected override Task<Result> HandleEventAsync(EventGridEvent<SubscriptionValidationEventData> eventGridEvent)
        {
            var uriResult = CreateRequestUri(eventGridEvent.Data.ValidationUrl);
            if (uriResult.IsFailure)
            {
                return Task.FromResult<Result>(uriResult);
            }
            else
            {
                BackgroundJobClient.Schedule<EventGridSubscriptionUrlValidationJob>(
                    job => job.RunAsync(uriResult.Value),
                    delay: TimeSpan.FromMinutes(1));

                return Task.FromResult(Result.Success());
            }
        }

        #endregion

        #region Private methods

        private Result<Uri> CreateRequestUri(string validationUrl)
        {
            return Result.SuccessIf(
                Uri.TryCreate(validationUrl, UriKind.Absolute, out var resultUri),
                resultUri,
                ErrorType.InvalidSubscriptionValidationUrl.ToString());
        }

        #endregion
    }
}
