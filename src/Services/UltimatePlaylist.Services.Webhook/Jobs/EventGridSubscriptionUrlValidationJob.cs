#region Usings

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

#endregion

namespace UltimatePlaylist.Services.Webhook.Jobs
{
    public class EventGridSubscriptionUrlValidationJob
    {
        #region Private methods

        private readonly Lazy<IHttpClientFactory> HttpClientFactoryProvider;
        private readonly Lazy<ILogger<EventGridSubscriptionUrlValidationJob>> LoggerProvider;

        #endregion

        #region Constructor(s)
        public EventGridSubscriptionUrlValidationJob(
            Lazy<IHttpClientFactory> httpClientFactoryProvider,
            Lazy<ILogger<EventGridSubscriptionUrlValidationJob>> loggerProvider)
        {
            HttpClientFactoryProvider = httpClientFactoryProvider;
            LoggerProvider = loggerProvider;
        }

        #endregion

        #region Properties

        private IHttpClientFactory HttpClientFactory => HttpClientFactoryProvider.Value;

        private ILogger<EventGridSubscriptionUrlValidationJob> Logger => LoggerProvider.Value;

        #endregion

        #region Public methods

        public async Task RunAsync(Uri requestUri)
        {
            var client = HttpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

            Logger.LogDebug("Sending HTTP GET to Event Grid subscription validation url {0}.", requestUri);

            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogWarning(
                    "Event Grid subscription validation url {0} responded with unsuccessful status code {1}.",
                    requestUri,
                    response.StatusCode);
            }
            else
            {
                Logger.LogDebug(
                    "Event Grid subscription validation url {0} responded with successful status code {1}.",
                    requestUri,
                    response.StatusCode);
            }
        }

        #endregion
    }
}
