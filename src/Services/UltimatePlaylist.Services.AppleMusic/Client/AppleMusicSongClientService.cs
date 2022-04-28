#region Usings

using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Common.Extensions;
using UltimatePlaylist.Services.Common.AppleMusic.Enums;
using UltimatePlaylist.Services.Common.Interfaces.AppleMusic;
using UltimatePlaylist.Services.Common.Interfaces.Client.AppleMusic;
using UltimatePlaylist.Services.Common.Models.AppleMusic.Request;

#endregion

namespace UltimatePlaylist.Services.AppleMusic
{
    public class AppleMusicSongClientService : AppleMusicBaseClient, IAppleMusicSongClientService
    {
        #region Consts

        private const string BaseRequestUri = "me/library";

        #endregion

        #region Constructor(s)

        public AppleMusicSongClientService(
            IHttpClientFactory clientFactory,
            Lazy<IAppleMusicTokenService> appleMusicTokenServiceProvider,
            Lazy<IAppleMusicConnectionService> appleMusicConnectionServiceProvider,
            Lazy<ILogger<AppleMusicBaseClient>> loggerBaseClassProvider)
            : base(clientFactory, appleMusicTokenServiceProvider, appleMusicConnectionServiceProvider, loggerBaseClassProvider)
        {
        }

        #endregion

        #region Public Methods

        public async Task<Result> AddSongToPlaylistAsync(
            string userToken,
            AppleMusicResurceType libraryResource,
            string playlistId,
            string appleMusicSongId,
            Guid userExternalId)
        {
            var setTokenResult = SetUserTokenHeader(userToken);
            if (setTokenResult.IsFailure)
            {
                return Result.Failure(ErrorType.AppleMusicSetUserTokenFailure.ToString());
            }

            var appleMusicTracksRequestModel = new AppleMusicTracksRequestModel()
            {
                Data = new List<AppleMusicSongRequestModel>()
                    {
                        new AppleMusicSongRequestModel()
                        {
                            Id = appleMusicSongId,
                            Type = "songs",
                        },
                    },
            };

            Logger.LogInformation("Request for adding song to user Apple Music playlist.");
            var result = await Post<AppleMusicTracksRequestModel, string>($"{BaseRequestUri}/{libraryResource.GetValue()}/{playlistId}/tracks", appleMusicTracksRequestModel, userExternalId);

            if (result.IsFailure)
            {
                Logger.LogInformation($"Request for adding song to user Apple Music playlist failed.");
                return Result.Failure(ErrorType.CannotAddSongToAppleMusicPlaylist.ToString());
            }

            Logger.LogInformation("Added song to user Apple Music playlist successfully.");
            return Result.Success();
        }

        #endregion
    }
}