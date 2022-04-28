#region Usings

using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Common.Mvc.Exceptions;
using UltimatePlaylist.Database.Infrastructure.Entities.Dsp;
using UltimatePlaylist.Database.Infrastructure.Entities.Dsp.Specifications;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Services.Common.AppleMusic.Enums;
using UltimatePlaylist.Services.Common.Interfaces.AppleMusic;
using UltimatePlaylist.Services.Common.Interfaces.AppleMusic.Client;
using UltimatePlaylist.Services.Common.Models.AppleMusic;
using UltimatePlaylist.Services.Common.Models.AppleMusic.Request;
using UltimatePlaylist.Services.Common.Models.AppleMusic.Responses;

#endregion

namespace UltimatePlaylist.Services.AppleMusic
{
    public class AppleMusicPlaylistService : IAppleMusicPlaylistService
    {
        #region Private members

        private const int AppleMusicPageLimit = 25;

        private readonly Lazy<IAppleMusicPlaylistClientService> AppleMusicPlaylistClientServiceProvider;

        private readonly DSPConfig DSPConfig;

        private readonly Lazy<IRepository<UserDspEntity>> UserDspsRepositoryProvider;

        #endregion

        #region Constructor(s)

        public AppleMusicPlaylistService(
            Lazy<IAppleMusicPlaylistClientService> appleMusicPlaylistClientServiceProvider,
            IOptions<DSPConfig> dspConfigOptions,
            Lazy<IRepository<UserDspEntity>> userDspsRepositoryProvider)
        {
            AppleMusicPlaylistClientServiceProvider = appleMusicPlaylistClientServiceProvider;
            DSPConfig = dspConfigOptions.Value;
            UserDspsRepositoryProvider = userDspsRepositoryProvider;
        }

        #endregion

        #region Properties

        private IAppleMusicPlaylistClientService AppleMusicPlaylistClientService => AppleMusicPlaylistClientServiceProvider.Value;

        private IRepository<UserDspEntity> UserDspsRepository => UserDspsRepositoryProvider.Value;

        #endregion

        #region Public methods

        public async Task<Result<AppleMusicPlaylistDataResponseModel>> CreateOrRestorePlaylistAsync(Guid userExternalId)
        {
            UserDspEntity userDspEntity = default;

            var test = await GetUserDsp(userExternalId);

            return await GetUserDsp(userExternalId)
                .Tap(userDsp => userDspEntity = userDsp)
                .Bind(async userDsp => await CheckIfPlaylistExists(userDsp.AppleUserToken, userExternalId)
                    .OnFailureCompensate(async error => await CreatePlaylistAsync(userDspEntity.AppleUserToken, userExternalId)))
                .Tap(result => userDspEntity.UserPlaylistId = result.Id)
                .Check(async result => await SaveUserAppleMusicPlaylist(userDspEntity));
        }

        #endregion

        #region Private methods

        private async Task<Result<AppleMusicPlaylistDataResponseModel>> CreatePlaylistAsync(
           string userToken,
           Guid userExternalId)
        {
            var appleMusicCreatePlaylistRequestModel = new AppleMusicCreatePlaylistRequestModel()
            {
                Attributes = new AppleMusicPlayListAttributesRequestModel()
                {
                    Name = DSPConfig.DefaultPlayListName,
                    Description = DSPConfig.DefaultPlayListDescription,
                },
                Relationships = new AppleMusicRelationshipsRequestModel()
                {
                    Tracks = new AppleMusicTracksRequestModel()
                    {
                        Data = new List<AppleMusicSongRequestModel>(),
                    },
                },
            };

            var result = await AppleMusicPlaylistClientService.CreateNewLibraryResources<AppleMusicPlaylistDataResponseModel>(
                userToken,
                userExternalId,
                AppleMusicResurceType.Playlists,
                appleMusicCreatePlaylistRequestModel);

            return result;
        }

        private async Task<Result<AppleMusicPlaylistDataResponseModel>> CheckIfPlaylistExists(string appleUserToken, Guid userExternalId)
        {
            AppleMusicPlaylistDataResponseModel ultimatePlaylist = default;
            Result<AppleDataResponseRoot<AppleMusicPlaylistDataResponseModel>> result = default;
            var offset = 0;

            do
            {
                result = await AppleMusicPlaylistClientService.FetchPlaylistsAsync(appleUserToken, userExternalId, new AppleMusicPageOptions()
                {
                    Limit = AppleMusicPageLimit,
                    Offset = offset,
                });

                if (result.IsFailure)
                {
                    throw new BadRequestException(result.Error);
                }

                ultimatePlaylist = result.Value.Data.FirstOrDefault(s => s.Attributes.Name == DSPConfig.DefaultPlayListName);
                offset += AppleMusicPageLimit;
            }
            while (ultimatePlaylist is null && result.Value.Data.Count > 0);

            return Result.SuccessIf(ultimatePlaylist is not null, ultimatePlaylist, ErrorType.PlaylistNotFound.ToString());
        }

        private async Task<Result> SaveUserAppleMusicPlaylist(UserDspEntity userDspEntity)
        {
            var updated = await UserDspsRepository.UpdateAndSaveAsync(userDspEntity);
            return Result.SuccessIf(updated != null && !string.IsNullOrEmpty(updated.UserPlaylistId), ErrorType.CannotAddSongToAppleMusicPlaylist.ToString());
        }

        private async Task<Result<UserDspEntity>> GetUserDsp(Guid userExternalId)
        {
            var userDsp = await UserDspsRepository.FirstOrDefaultAsync(new UserDspSpecification()
                .ByUserExternalId(userExternalId)
                .ByType(DspType.AppleMusic)
                .ByActive()
                .WithUser()
                .OrderByCreatedDescending());

            return Result.SuccessIf(userDsp?.AppleUserToken != null && userDsp.IsActive, userDsp, ErrorMessages.DisconnectedFromAppleMusic);
        }

        #endregion
    }
}
