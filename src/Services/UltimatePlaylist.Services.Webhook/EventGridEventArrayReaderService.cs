#region Usings

using System;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using UltimatePlaylist.Services.Common.Interfaces.Webhook;

#endregion

namespace UltimatePlaylist.Services.Webhook
{
    public class EventGridEventArrayReaderService : IEventGridEventArrayReaderService
    {
        #region Private fields

        private readonly Lazy<ILogger<EventGridEventArrayReaderService>> LoggerProvider;

        #endregion

        #region Constructor(s)

        public EventGridEventArrayReaderService(
            Lazy<ILogger<EventGridEventArrayReaderService>> loggerProvider)
        {
            LoggerProvider = loggerProvider;
        }

        #endregion

        #region Properties

        private ILogger<EventGridEventArrayReaderService> Logger => LoggerProvider.Value;

        #endregion

        #region Public methods

        public Result<JArray> ReadArray(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                Logger.LogWarning(
                    "Failed to parse Event Grid events into JSON array. " +
                    "Input string was null, empty or contained only whitespace characters.");

                return Result.Failure<JArray>("Request body cannot be null, empty or contain only whitespace characters.");
            }

            try
            {
                return JArray.Parse(json);
            }
            catch (Exception exception)
            {
                Logger.LogWarning(exception, $"Failed to parse Event Grid events into JSON array. Full JSON:\n{json}");

                return Result.Failure<JArray>("Unable to parse events into JSON array.");
            }
        }

        #endregion
    }
}
