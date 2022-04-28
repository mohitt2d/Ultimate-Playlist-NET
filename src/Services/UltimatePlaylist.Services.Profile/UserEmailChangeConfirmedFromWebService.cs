#region Usings

using UltimatePlaylist.Common.Const;
using UltimatePlaylist.Common.Mvc.Exceptions;
using UltimatePlaylist.Common.Mvc.Interface;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity.Specifications;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;

#endregion

namespace UltimatePlaylist.Services.Personalization
{
    public class UserEmailChangeConfirmedFromWebService : IUserEmailChangeConfirmedFromWebService
    {
        #region Private members

        private readonly Lazy<IRepository<User>> UserRepositoryProvider;

        private readonly Lazy<IUserBlacklistTokenStore> UserBlacklistTokenStoreProvider;

        #endregion

        public UserEmailChangeConfirmedFromWebService(
            Lazy<IRepository<User>> userRepositoryProvider,
            Lazy<IUserBlacklistTokenStore> userBlacklistTokenStoreProvider)
        {
            UserRepositoryProvider = userRepositoryProvider;
            UserBlacklistTokenStoreProvider = userBlacklistTokenStoreProvider;
        }

        #region Properties

        private IRepository<User> UserRepository => UserRepositoryProvider.Value;

        private IUserBlacklistTokenStore UserBlacklistTokenStore => UserBlacklistTokenStoreProvider.Value;

        #endregion

        #region Public Methods

        public async Task CheckIfUserShouldBeLogOut(Guid userExternalId, string userToken)
        {
            var user = await UserRepository.FirstOrDefaultAsync(new UserSpecification().ByExternalId(userExternalId));
            if (user != null && user.IsEmailChangeConfirmedFromWeb)
            {
                UserBlacklistTokenStore.Set(userExternalId, userToken);
                user.RefreshTokenHash = null;
                await UserRepository.UpdateAndSaveAsync(user);
                throw new UnauthorizedAccessException(ErrorMessages.LogoutUserInMobileApps);
            }
        }

        #endregion
    }
}
