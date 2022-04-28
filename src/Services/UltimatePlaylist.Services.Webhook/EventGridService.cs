#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Services.Common.Interfaces.Webhook;
using UltimatePlaylist.Services.Common.Models.Webhook.Events;

#endregion

namespace UltimatePlaylist.Services.Webhook
{
    public class EventGridService : IEventGridService
    {
        #region Private fields

        private readonly Lazy<IEventGridEventKeyValidatorService> EventGridEventKeyValidatorServiceProvider;
        private readonly Lazy<IEventGridEventHeadersValidatorService> EventGridEventHeadersValidatorServiceProvider;
        private readonly Lazy<IEventGridEventArrayReaderService> EventGridEventArrayReaderServiceProvider;
        private readonly Lazy<IHttpContextAccessor> HttpContextAccessorProvider;
        private readonly Lazy<ILogger<EventGridService>> LoggerProvider;
        private readonly Lazy<IOptions<AzureEventGridConfig>> AzureEventGridConfigProvider;
        private readonly Lazy<IServiceProvider> ServiceProviderLazy;

        #endregion

        #region Constructor(s)

        public EventGridService(
            Lazy<IEventGridEventKeyValidatorService> eventGridEventKeyValidatorServiceProvider,
            Lazy<IEventGridEventHeadersValidatorService> eventGridEventHeadersValidatorServiceProvider,
            Lazy<IEventGridEventArrayReaderService> eventGridEventArrayReaderServiceProvider,
            Lazy<IHttpContextAccessor> httpContextAccessorProvider,
            Lazy<ILogger<EventGridService>> loggerProvider,
            Lazy<IOptions<AzureEventGridConfig>> azureEventGridConfigProvider,
            Lazy<IServiceProvider> serviceProviderLazy)
        {
            EventGridEventKeyValidatorServiceProvider = eventGridEventKeyValidatorServiceProvider;
            EventGridEventHeadersValidatorServiceProvider = eventGridEventHeadersValidatorServiceProvider;
            EventGridEventArrayReaderServiceProvider = eventGridEventArrayReaderServiceProvider;
            HttpContextAccessorProvider = httpContextAccessorProvider;
            LoggerProvider = loggerProvider;
            AzureEventGridConfigProvider = azureEventGridConfigProvider;
            ServiceProviderLazy = serviceProviderLazy;
        }

        #endregion

        #region Properties

        private IEventGridEventKeyValidatorService EventGridEventKeyValidator => EventGridEventKeyValidatorServiceProvider.Value;

        private IEventGridEventHeadersValidatorService EventGridEventHeadersValidator => EventGridEventHeadersValidatorServiceProvider.Value;

        private IEventGridEventArrayReaderService EventGridEventArrayReader => EventGridEventArrayReaderServiceProvider.Value;

        private IHttpContextAccessor HttpContextAccessor => HttpContextAccessorProvider.Value;

        private ILogger<EventGridService> Logger => LoggerProvider.Value;

        private AzureEventGridConfig AzureEventGridConfig => AzureEventGridConfigProvider.Value.Value;

        private IServiceProvider ServiceProvider => ServiceProviderLazy.Value;

        #endregion

        #region Public methods

        public async Task<Result> ProcessAsync(string key)
        {
            var httpContext = HttpContextAccessor.HttpContext;
            if (httpContext is null)
            {
                throw new InvalidOperationException("Cannot obtain HTTP context when there is no associated HTTP request.");
            }

            return await Result.Success()
                .Bind(() => EventGridEventHeadersValidator.Validate(httpContext.Request.Headers))
                .Map(async () => await ReadRequestBodyAsync(httpContext.Request.Body))
                .Bind(jsonBody => EventGridEventArrayReader.ReadArray(jsonBody))
                .Bind(jsonArray => ValidateArrayCount(jsonArray))
                .Bind(async jsonArray => await HandleEvents(jsonArray));
        }

        #endregion

        #region Private methods

        private async Task<string> ReadRequestBodyAsync(Stream stream)
        {
            using var streamReader = new StreamReader(stream);

            return await streamReader.ReadToEndAsync();
        }

        private Result<JArray> ValidateArrayCount(JArray jsonArray)
        {
            var maxEventsInBatch = AzureEventGridConfig.MaxEventsInBatch;

            return Result.FailureIf(jsonArray.Count > maxEventsInBatch, jsonArray, ErrorType.TooManyEventsInBatch.ToString())
                .OnFailure(() => Logger.LogWarning(
                    "Incoming Event Grid event have {0} events in batch, while allowed maximum is {1}.",
                    jsonArray.Count,
                    maxEventsInBatch));
        }

        private async Task<Result> HandleEvents(JArray jsonArray)
        {
            var results = new List<Result>();

            foreach (var jsonToken in jsonArray)
            {
                var result = await GetEventGridEventType(jsonToken)
                    .Bind(async eventGridEventType => await HandleEvent(jsonToken, eventGridEventType));

                results.Add(result);
            }

            return Result.Combine(results);
        }

        private Result<EventGridEventType> GetEventGridEventType(JToken jsonToken)
        {
            try
            {
                var @event = jsonToken.ToObject<EventGridEvent>();

                return Result.Success(@event.EventType);
            }
            catch (JsonException)
            {
                return Result.Failure<EventGridEventType>(ErrorType.UnsupportedOrMissingEventType.ToString());
            }
        }

        private async Task<Result> HandleEvent(JToken jsonToken, EventGridEventType eventGridEventType)
        {
            using var scope = ServiceProvider.CreateScope();

            var handlerFactory = scope.ServiceProvider.GetRequiredService<IEventGridEventHandlerFactoryService>();
            var handler = handlerFactory.CreateHandler(eventGridEventType);

            return await handler.HandleAsync(jsonToken);
        }

        #endregion
    }
}
