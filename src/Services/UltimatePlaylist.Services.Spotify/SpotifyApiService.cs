#region Usings

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Common.Mvc.Exceptions;
using UltimatePlaylist.Services.Common.Interfaces.Spotify;
using UltimatePlaylist.Services.Common.Models.Spotify;
using UltimatePlaylist.Services.Common.Models.Spotify.Request;
using UltimatePlaylist.Services.Common.Models.Spotify.Response;

#endregion

namespace UltimatePlaylist.Services.Spotify
{
    public class SpotifyApiService : ISpotifyApiService
    {
        #region Private members

        private const string UserProfile = "me";

        private const string Users = "users";

        private const string Playlists = "playlists";

        private const string Tracks = "tracks";

        private readonly Lazy<ILogger<SpotifyApiService>> LoggerProvider;

        private readonly SpotifyConfig SpotifyConfig;

        private readonly DSPConfig DSPConfig;

        private readonly Lazy<IHttpClientFactory> HttpClientFactoryProvider;

        private readonly Lazy<IMapper> MapperProvider;

        #endregion

        #region Constructor(s)

        public SpotifyApiService(
            Lazy<ILogger<SpotifyApiService>> loggerProvider,
            IOptions<SpotifyConfig> spotifyConfigOptions,
            IOptions<DSPConfig> dspConfigOptions,
            Lazy<IHttpClientFactory> httpClientFactoryProvider,
            Lazy<IMapper> mapperProvider)
        {
            LoggerProvider = loggerProvider;
            SpotifyConfig = spotifyConfigOptions.Value;
            DSPConfig = dspConfigOptions.Value;
            HttpClientFactoryProvider = httpClientFactoryProvider;
            MapperProvider = mapperProvider;
        }

        #endregion

        #region Properties

        private ILogger<SpotifyApiService> Logger => LoggerProvider.Value;

        private IHttpClientFactory HttpClientFactory => HttpClientFactoryProvider.Value;

        private IFlurlClient FlurlClient => new FlurlClient(HttpClientFactory.CreateClient("Spotify"));

        private IMapper Mapper => MapperProvider.Value;

        #endregion

        #region Public Method(s)

        public async Task<Result<string>> GetSpotifyUserIdentity(string accessToken)
        {
            try
            {
                Logger.LogInformation("Request for user Spotify identity");

                var result = await SpotifyConfig.ApiUrl
                    .WithHeader("Authorization", $"Bearer {accessToken}")
                    .AppendPathSegment(UserProfile)
                    .WithClient(FlurlClient)
                    .GetJsonAsync<SpotifyUserProfileResponseModel>();

                Logger.LogInformation($"Returned user spotify profile.\n{result}");

                return Result.SuccessIf(result != null && !string.IsNullOrEmpty(result.Id), result.Id, ErrorType.FailedToFetchSpotifyUserProfile.ToString());
            }
            catch (FlurlHttpException exception)
            {
                Logger.LogWarning($"Spotify call failed to obtain user spotify profile. \n StatusCode: {exception.StatusCode}, Response: {await exception.GetResponseStringAsync()}");

                if (exception.StatusCode == StatusCodes.Status403Forbidden)
                {
                    throw new BadRequestException(ErrorMessages.DisconnectedFromSpotify);
                }

                return Result.Failure<string>(ErrorType.FailedToFetchSpotifyUserProfile.ToString());
            }
        }

        public async Task<Result<SpotifyPlaylistsResponseModel>> FetchUserPlaylists(
            string accessToken)
        {
            try
            {
                Logger.LogInformation("Request for fetching user Spotify playlists.");

                var result = await SpotifyConfig.ApiUrl
                    .WithHeader("Authorization", $"Bearer {accessToken}")
                    .AppendPathSegment(UserProfile)
                    .AppendPathSegment(Playlists)
                    .WithClient(FlurlClient)
                    .GetJsonAsync<SpotifyPlaylistsResponseModel>();

                Logger.LogInformation($"Returned user spotify playlists.\n{result}");

                return Result.SuccessIf(result != null && result.Items.Count > 0, result, ErrorType.FailedToFetchUserPlaylists.ToString());
            }
            catch (FlurlHttpException exception)
            {
                Logger.LogWarning($"Spotify call failed to get user playlist. \n StatusCode: {exception.StatusCode}, Response: {await exception.GetResponseStringAsync()}");

                if (exception.StatusCode == StatusCodes.Status403Forbidden)
                {
                    throw new BadRequestException(ErrorMessages.DisconnectedFromSpotify);
                }

                return Result.Failure<SpotifyPlaylistsResponseModel>(ErrorType.FailedToFetchUserPlaylists.ToString());
            }
        }

        public async Task<Result<CreatePlaylistResponseModel>> CreatePlaylist(
            string accessToken,
            string userSpotifyIdentity)
        {
            try
            {
                var requestModel = new CreatePlaylistRequestModel()
                {
                    Name = DSPConfig.DefaultPlayListName,
                    Description = DSPConfig.DefaultPlayListDescription,
                    Public = true,
                };

                Logger.LogInformation($"Request for creating user Spotify playlist with request.\n{requestModel}");

                var result = await SpotifyConfig.ApiUrl
                    .WithHeader("Authorization", $"Bearer {accessToken}")
                    .AppendPathSegment(Users)
                    .AppendPathSegment(userSpotifyIdentity)
                    .AppendPathSegment(Playlists)
                    .WithClient(FlurlClient)
                    .PostJsonAsync(new
                    {
                        name = DSPConfig.DefaultPlayListName,
                        description = DSPConfig.DefaultPlayListDescription,
                    })
                    .ReceiveJson<CreatePlaylistResponseModel>();

                Logger.LogInformation($"Returned created playlist for Spotify user.\n{result}");

                return Result.SuccessIf(result != null && !string.IsNullOrEmpty(result.Id), result, ErrorType.FailedToCreatePlaylist.ToString());
            }
            catch (FlurlHttpException exception)
            {
                Logger.LogWarning($"Spotify call failed to create playlist. \n StatusCode: {exception.StatusCode}, Response: {await exception.GetResponseStringAsync()}");

                if (exception.StatusCode == StatusCodes.Status403Forbidden)
                {
                    throw new BadRequestException(ErrorMessages.DisconnectedFromSpotify);
                }

                return Result.Failure<CreatePlaylistResponseModel>(ErrorType.FailedToCreatePlaylist.ToString());
            }
        }

        public async Task<Result> AddSongToPlaylistAsync(
            string accessToken,
            string playlistSpotifyId,
            string songSpotifyId)
        {
            try
            {
                Logger.LogInformation($"Request for adding song to user Spotify playlist with song Spotify ID.\n{playlistSpotifyId}");

                var result = await SpotifyConfig.ApiUrl
                    .WithHeader("Authorization", $"Bearer {accessToken}")
                    .AppendPathSegment(Playlists)
                    .AppendPathSegment(playlistSpotifyId)
                    .AppendPathSegment(Tracks)
                    .WithClient(FlurlClient)
                    .PostJsonAsync(new
                    {
                        uris = new List<string>() { $"spotify:track:{songSpotifyId}" },
                    })
                    .ReceiveJson<object>();

                Logger.LogInformation($"Returned added song to user Spotify playlist.\n{result}");

                return Result.SuccessIf(result != null, result, ErrorType.FailedToAddSongToPlaylist.ToString());
            }
            catch (FlurlHttpException exception)
            {
                Logger.LogWarning($"Spotify call failed to add song to playlist. \n StatusCode: {exception.StatusCode}, Response: {await exception.GetResponseStringAsync()}");

                if (exception.StatusCode == StatusCodes.Status403Forbidden)
                {
                    throw new BadRequestException(ErrorMessages.DisconnectedFromSpotify);
                }

                return Result.Failure<CreatePlaylistResponseModel>(ErrorType.FailedToAddSongToPlaylist.ToString());
            }
        }


        public SpotifyClientConfigurationReadServiceModel GetSpotifyConfiguration() => new SpotifyClientConfigurationReadServiceModel()
        {
            ClientId = SpotifyConfig.ClientId,
        };

        #endregion
    }
}