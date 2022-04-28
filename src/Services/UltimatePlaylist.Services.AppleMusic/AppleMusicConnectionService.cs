#region Usings

using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Database.Infrastructure.Entities.Dsp;
using UltimatePlaylist.Database.Infrastructure.Entities.Dsp.Specifications;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity.Specifications;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Services.Common.Interfaces.AppleMusic;
using UltimatePlaylist.Services.Common.Models.AppleMusic;

#endregion

namespace UltimatePlaylist.Services.AppleMusic
{
    public class AppleMusicConnectionService : IAppleMusicConnectionService
    {
        #region Private member

        private readonly Lazy<IAppleMusicTokenService> AppleMusicTokenServiceProvider;
        private readonly Lazy<IRepository<UserDspEntity>> UserDspReposiotryProvider;
        private readonly Lazy<IRepository<User>> UserReposiotryProvider;

        #endregion

        #region Constructor(s)

        public AppleMusicConnectionService(Lazy<IAppleMusicTokenService> appleMusicTokenServiceProvider, Lazy<IRepository<UserDspEntity>> userDspReposiotryProvider, Lazy<IRepository<User>> userReposiotryProvider)
        {
            AppleMusicTokenServiceProvider = appleMusicTokenServiceProvider;
            UserDspReposiotryProvider = userDspReposiotryProvider;
            UserReposiotryProvider = userReposiotryProvider;
        }

        #endregion

        #region Private Properties

        private IAppleMusicTokenService AppleMusicTokenService => AppleMusicTokenServiceProvider.Value;

        private IRepository<UserDspEntity> UserDspReposiotry => UserDspReposiotryProvider.Value;

        private IRepository<User> UserReposiotry => UserReposiotryProvider.Value;

        #endregion

        #region Public Methods

        public Result<DeveloperTokenReadServiceModel> GetDeveloperTokenAsync()
        {
            return Result.Success(new DeveloperTokenReadServiceModel() { DeveloperToken = AppleMusicTokenService.CreateAppleMusicToken() });
        }

        public async Task<Result> SaveUserTokenAsync(string userToken, Guid userExternalId)
        {
            var user = await UserReposiotry.FirstOrDefaultAsync(new UserSpecification().ByExternalId(userExternalId));

            return await Result.FailureIf(user is null, ErrorType.UserDoesNotExist.ToString())
                .Map(async () => await UserDspReposiotry.FirstOrDefaultAsync(new UserDspSpecification().ByUserExternalId(userExternalId).ByType(DspType.AppleMusic)))
                .TapIf(appleDsp => appleDsp is null, async appleDsp => await UserDspReposiotry.AddAsync(new UserDspEntity() { UserId = user.Id, Type = DspType.AppleMusic, AppleUserToken = userToken, IsActive = true }))
                .TapIf(appleDsp => appleDsp is not null, async appleDsp =>
                {
                    appleDsp.IsActive = true;
                    appleDsp.AppleUserToken = userToken;
                    await UserDspReposiotry.UpdateAndSaveAsync(appleDsp);
                });
        }

        public async Task<Result> RemoveUserTokenAsync(DspType type, Guid userExternalId)
        {
            var userDsp = await UserDspReposiotry.FirstOrDefaultAsync(new UserDspSpecification().ByUserExternalId(userExternalId).ByType(type));

            return await Result.Success()
                .TapIf(userDsp is not null, async () =>
                {
                    userDsp.IsActive = false;
                    userDsp.AppleUserToken = null;
                    await UserDspReposiotry.UpdateAndSaveAsync(userDsp);
                });
        }

        #endregion
    }
}
