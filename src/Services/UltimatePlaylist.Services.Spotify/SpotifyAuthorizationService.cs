#region Usings

using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Services.Common.Interfaces.Spotify;
using UltimatePlaylist.Services.Common.Models.Spotify;
using UltimatePlaylist.Services.Common.Models.Spotify.Response;

#endregion

namespace UltimatePlaylist.Services.Spotify
{
    public class SpotifyAuthorizationService : ISpotifyAuthorizationService
    {
        #region Private members

        private const string Token = "token";

        private const string UserProfile = "v1/me";

        private readonly Lazy<ILogger<SpotifyAuthorizationService>> LoggerProvider;

        private readonly SpotifyConfig SpotifyConfig;

        private readonly Lazy<IHttpClientFactory> HttpClientFactoryProvider;

        private readonly Lazy<IMapper> MapperProvider;

        #endregion

        #region Constructor(s)

        public SpotifyAuthorizationService(
            Lazy<ILogger<SpotifyAuthorizationService>> loggerProvider,
            IOptions<SpotifyConfig> spotifyConfigOptions,
            Lazy<IHttpClientFactory> httpClientFactoryProvider,
            Lazy<IMapper> mapperProvider)
        {
            LoggerProvider = loggerProvider;
            SpotifyConfig = spotifyConfigOptions.Value;
            HttpClientFactoryProvider = httpClientFactoryProvider;
            MapperProvider = mapperProvider;
        }

        #endregion

        #region Properties

        private ILogger<SpotifyAuthorizationService> Logger => LoggerProvider.Value;

        private IHttpClientFactory HttpClientFactory => HttpClientFactoryProvider.Value;

        private IFlurlClient FlurlClient => new FlurlClient(HttpClientFactory.CreateClient("Spotify"));

        private IMapper Mapper => MapperProvider.Value;

        #endregion

        #region Public Method(s)

        public async Task<Result<SpotifyAuthorizationReadServiceModel>> ReceiveSpotifyTokens(SpotifyAuthorizationWriteServiceModel spotifyAuthorizationWriteServiceModel)
        {
            try
            {
                var base64Authorization = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{SpotifyConfig.ClientId}:{SpotifyConfig.ClientSecret}"));

                var result = await SpotifyConfig.AuthorizationUrl
                    .WithHeader("Authorization", $"Basic {base64Authorization}")
                    .WithHeader("Content-Type", "application/x-www-form-urlencoded")
                    .AppendPathSegment(Token)
                    .WithClient(FlurlClient)
                    .PostUrlEncodedAsync(new
                    {
                        grant_type = "authorization_code",
                        code = spotifyAuthorizationWriteServiceModel.Code,
                        redirect_uri = SpotifyConfig.RedirectUri,
                    })
                    .ReceiveJson<SpotifyAuthorizationResponseModel>();

                return Result.SuccessIf(result != null && !string.IsNullOrEmpty(result.Refresh_token), ErrorType.FailedToAuthorizeSpotify.ToString())
                    .Map(() => Mapper.Map<SpotifyAuthorizationReadServiceModel>(result));
            }
            catch (FlurlHttpException e)
            {
                Logger.LogInformation($"Failed to receive Spotify token.\n{e.Message}");

                return Result.Failure<SpotifyAuthorizationReadServiceModel>(ErrorType.FailedToAuthorizeSpotify.ToString());
            }
        }

        public async Task<Result<SpotifyAuthorizationReadServiceModel>> RefreshSpotifyTokens(string refreshToken)
        {
            try
            {
                var base64Authorization = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{SpotifyConfig.ClientId}:{SpotifyConfig.ClientSecret}"));

                var result = await SpotifyConfig.AuthorizationUrl
                    .WithHeader("Authorization", $"Basic {base64Authorization}")
                    .WithHeader("Content-Type", "application/x-www-form-urlencoded")
                    .AppendPathSegment(Token)
                    .WithClient(FlurlClient)
                    .PostUrlEncodedAsync(new
                    {
                        grant_type = "refresh_token",
                        refresh_token = refreshToken,
                    })
                    .ReceiveJson<SpotifyAuthorizationResponseModel>();

                return Result.SuccessIf(result != null && !string.IsNullOrEmpty(result.Access_token), ErrorType.FailedToRefreshSpotifyAccessToken.ToString())
                    .Map(() => Mapper.Map<SpotifyAuthorizationReadServiceModel>(result));
            }
            catch (FlurlHttpException e)
            {
                Logger.LogInformation($"Failed to refresh Spotify token.\n{e.Message}");

                return Result.Failure<SpotifyAuthorizationReadServiceModel>(ErrorType.FailedToRefreshSpotifyAccessToken.ToString());
            }
        }

        #endregion
    }
}
