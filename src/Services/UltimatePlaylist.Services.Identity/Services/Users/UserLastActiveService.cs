#region Usings

using CSharpFunctionalExtensions;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.Common.Exceptions;
using UltimatePlaylist.Common.Mvc.Interface;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity.Specifications;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;

#endregion

namespace UltimatePlaylist.Services.Identity.Services.Users
{
    public class UserLastActiveService : IUserLastActiveService
    {
        #region Private field(s)

        private readonly Lazy<IRepository<User>> UserRepositoryProvider;

        #endregion

        #region Constructor(s)

        public UserLastActiveService(Lazy<IRepository<User>> userRepositoryProvider)
        {
            UserRepositoryProvider = userRepositoryProvider;
        }

        #endregion

        #region Private properties

        private IRepository<User> UserRepository => UserRepositoryProvider.Value;

        #endregion

        #region Public method(s)

        public async Task SetLastActiveToUtcNow(Guid userExternalId)
        {
            await GetUser(userExternalId)
                .Tap(user => user.LastActive = DateTime.UtcNow)
                .Tap(user => UserRepository.UpdateAndSaveAsync(user));
        }

        #endregion

        #region Private method(s)

        private async Task<Result<User>> GetUser(Guid userExternalId)
        {
            var user = await UserRepository.FirstOrDefaultAsync(new UserSpecification().ByExternalId(userExternalId));

            return Result.SuccessIf(user != null, ErrorMessages.UserDoesNotExist)
                .Map(() => user);
        }

        #endregion
    }
}
