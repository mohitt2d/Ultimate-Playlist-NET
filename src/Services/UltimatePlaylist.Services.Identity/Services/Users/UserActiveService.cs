#region Usings

using CSharpFunctionalExtensions;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.Common.Exceptions;
using UltimatePlaylist.Common.Mvc.Interface;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity.Specifications;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Services.Common.Interfaces.Identity;

#endregion

namespace UltimatePlaylist.Services.Identity.Services.Users
{
    public class UserActiveService : IUserActiveService
    {
        #region Private field(s)

        private readonly Lazy<IReadOnlyRepository<User>> UserReadRepositoryProvider;
        private readonly Lazy<IUserBlacklistTokenStore> UserBlacklistTokenStoreProvider;
        private readonly Lazy<IUserActiveStore> UserActiveStoreProvider;

        #endregion

        #region Constructor(s)

        public UserActiveService(
            Lazy<IReadOnlyRepository<User>> userReadRepositoryProvider,
            Lazy<IUserBlacklistTokenStore> userBlacklistTokenStoreProvider,
            Lazy<IUserActiveStore> userActiveStoreProvider)
        {
            UserReadRepositoryProvider = userReadRepositoryProvider;
            UserBlacklistTokenStoreProvider = userBlacklistTokenStoreProvider;
            UserActiveStoreProvider = userActiveStoreProvider;
        }

        #endregion

        #region Private properties

        private IReadOnlyRepository<User> UserReadRepository => UserReadRepositoryProvider.Value;

        private IUserBlacklistTokenStore UserBlacklistTokenStore => UserBlacklistTokenStoreProvider.Value;

        private IUserActiveStore UserActiveStore => UserActiveStoreProvider.Value;

        #endregion

        public void UpdateActiveStatusInStore(Guid userExternalId, bool isActive)
        {
            UserActiveStore.Set(userExternalId, isActive);
        }

        public async Task CheckActiveStatusAndSetTokenOnBlacklist(Guid userExternalId, string token)
        {
            if (!UserActiveStore.TryGet(userExternalId, out bool isActive))
            {
                isActive = (await GetUser(userExternalId)).IsActive;
                UserActiveStore.Set(userExternalId, isActive);
            }

            if (!isActive)
            {
                UserBlacklistTokenStore.Set(userExternalId, token);
            }
        }

        private async Task<User> GetUser(Guid userExternalId)
        {
            return await UserReadRepository.FirstOrDefaultAsync(new UserSpecification().ByExternalId(userExternalId))
                ?? throw new NotFoundException(ErrorMessages.UserDoesNotExist);
        }
    }
}
