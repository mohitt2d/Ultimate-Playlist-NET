#region Usings

using System;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using UltimatePlaylist.Services.Common.Interfaces.Webhook;
using UltimatePlaylist.Services.Common.Models.Webhook.Events;

#endregion

namespace UltimatePlaylist.Services.Webhook
{
    public class EventGridEventDeserializerService : IEventGridEventDeserializerService
    {
        #region Private fields

        private readonly Lazy<ILogger<EventGridEventDeserializerService>> LoggerProvider;

        private readonly JsonSerializerSettings JsonSerializerSettings;
        private readonly JsonSerializer JsonSerializer;

        #endregion

        #region Constructor(s)

        public EventGridEventDeserializerService(
            Lazy<ILogger<EventGridEventDeserializerService>> loggerProvider)
        {
            LoggerProvider = loggerProvider;

            JsonSerializerSettings = new JsonSerializerSettings();
            JsonSerializerSettings.Converters.Add(new StringEnumConverter());
            JsonSerializer = JsonSerializer.Create(JsonSerializerSettings);
        }

        #endregion

        #region Propeties

        private ILogger<EventGridEventDeserializerService> Logger => LoggerProvider.Value;

        #endregion

        #region Public methods

        public EventGridEvent<TEventData> DeserializeToEventDataModel<TEventData>(JToken jsonToken)
            where TEventData : class
        {
            try
            {
                return jsonToken.ToObject<EventGridEvent<TEventData>>(JsonSerializer);
            }
            catch (JsonException exception)
            {
                Logger.LogError(
                    exception,
                    "Failed to deserialize Event Grid event to type {0} of {1}.",
                    nameof(EventGridEvent),
                    typeof(TEventData).Name);

                throw;
            }
        }

        #endregion
    }
}
