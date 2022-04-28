#region Usings

using System.Net.Http.Headers;
using System.Net.Http.Json;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Services.Common.Interfaces.AppleMusic;
using UltimatePlaylist.Services.Common.Models.AppleMusic;

#endregion

namespace UltimatePlaylist.Services.AppleMusic
{
    public abstract class AppleMusicBaseClient
    {
        #region Consts

        private const string UserTokenHeaderName = "Music-User-Token";
        private const string LimitQueryStringKey = "limit";
        private const string OffsetQueryStringKey = "offset";
        private const string LocaleQueryStringKey = "l";

        #endregion

        #region Private Members

        private readonly Lazy<IAppleMusicTokenService> AppleMusicTokenServiceProvider;

        private readonly Lazy<IAppleMusicConnectionService> AppleMusicConnectionServiceProvider;

        private readonly Lazy<ILogger<AppleMusicBaseClient>> LoggerProvider;

        #endregion

        #region Constructor(s)

        protected AppleMusicBaseClient(
            IHttpClientFactory clientFactory,
            Lazy<IAppleMusicTokenService> appleMusicTokenServiceProvider,
            Lazy<IAppleMusicConnectionService> appleMusicConnectionServiceProvider,
            Lazy<ILogger<AppleMusicBaseClient>> loggerProvider)
        {
            AppleMusicTokenServiceProvider = appleMusicTokenServiceProvider;
            AppleMusicConnectionServiceProvider = appleMusicConnectionServiceProvider;
            LoggerProvider = loggerProvider;

            HttpClient = clientFactory.CreateClient(Config.AppleHttpClient);
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", AppleMusicTokenService.CreateAppleMusicToken());
        }

        #endregion

        #region Protected Properties

        protected HttpClient HttpClient { get; }

        protected IAppleMusicTokenService AppleMusicTokenService => AppleMusicTokenServiceProvider.Value;

        protected IAppleMusicConnectionService AppleMusicConnectionService => AppleMusicConnectionServiceProvider.Value;

        protected ILogger<AppleMusicBaseClient> Logger => LoggerProvider.Value;

        #endregion

        #region Protected Methods

        protected Result SetUserTokenHeader(string userToken)
        {
            return Result.FailureIf(string.IsNullOrWhiteSpace(userToken), ErrorType.UserTokenDoesNotExist.ToString())
                .TapIf(!HttpClient.DefaultRequestHeaders.Any(i => i.Key == UserTokenHeaderName), () => HttpClient.DefaultRequestHeaders.Add(UserTokenHeaderName, userToken));
        }

        protected async Task<Result<TResponse>> Get<TResponse>(string requestUri, Guid userExternalId, IDictionary<string, string> queryStringParameters = null, AppleMusicPageOptions pageOptions = null, string locale = null, CancellationToken cancellationToken = default)
        {
            queryStringParameters ??= new Dictionary<string, string>();

            Result.Success()
                .TapIf(pageOptions?.Limit is not null, () => queryStringParameters.Add(LimitQueryStringKey, pageOptions.Limit.Value.ToString()))
                .TapIf(pageOptions?.Offset is not null, () => queryStringParameters.Add(OffsetQueryStringKey, pageOptions.Offset.Value.ToString()))
                .TapIf(locale is not null, () => queryStringParameters.Add(LocaleQueryStringKey, locale))
                .TapIf(queryStringParameters.Any(), () => requestUri = QueryHelpers.AddQueryString(requestUri, queryStringParameters));

            return await GetResponse<TResponse>(requestUri, userExternalId, cancellationToken);
        }

        protected async Task<Result<TResponse>> Post<TRequest, TResponse>(string enpoint, TRequest request, Guid userExternalId, CancellationToken cancellationToken = default)
        {
            try
            {
                Logger.LogInformation($"Request for Apple music POST {enpoint}");

                var result = await HttpClient.PostAsJsonAsync(enpoint, request, cancellationToken);

                Logger.LogInformation($"Result for Apple music POST {enpoint}.\n{result}");

                return await AppleMusicValidateAndCompensateResponse(enpoint, userExternalId, result)
                    .Map(async () => await result.Content.ReadAsStringAsync())
                    .Map(resultContent => JsonConvert.DeserializeObject<TResponse>(resultContent));
            }
            catch (Exception exception)
            {
                Logger.LogWarning($"Result for POST {enpoint}.\n{exception.Message}");

                return Result.Failure<TResponse>(ErrorType.DSPConnectionError.ToString());
            }
        }

        #endregion

        #region Private Methods

        private async Task<Result<TResponse>> GetResponse<TResponse>(string requestUri, Guid userExternalId, CancellationToken cancellationToken = default)
        {
            try
            {
                Logger.LogInformation($"Request for Apple music GET {requestUri}");

                var result = await HttpClient.GetAsync(requestUri, cancellationToken);

                Logger.LogInformation($"Result for Apple music GET {requestUri}.\n{result}");

                return await AppleMusicValidateAndCompensateResponse(requestUri, userExternalId, result)
                    .Map(async () => await result.Content.ReadAsStringAsync())
                    .Map(resultContent => JsonConvert.DeserializeObject<TResponse>(resultContent));
            }
            catch (Exception exception)
            {
                Logger.LogWarning($"Result for GET {requestUri}.\n{exception.Message}");

                return Result.Failure<TResponse>(ErrorType.DSPConnectionError.ToString());
            }
        }

        private async Task<Result> AppleMusicValidateAndCompensateResponse(string requestUri, Guid userExternalId, HttpResponseMessage httpResponse)
        {
            if (httpResponse.IsSuccessStatusCode)
            {
                return Result.Success();
            }

            Logger.LogWarning($"Result for GET {requestUri}.\n StatusCode: {httpResponse.StatusCode}, Response: {await httpResponse.Content.ReadAsStringAsync()}");

            if (httpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                await AppleMusicConnectionService.RemoveUserTokenAsync(DspType.AppleMusic, userExternalId);
                return Result.Failure(ErrorMessages.DisconnectedFromAppleMusic);
            }

            return Result.Failure(ErrorType.DSPConnectionError.ToString());
        }

        #endregion
    }
}
