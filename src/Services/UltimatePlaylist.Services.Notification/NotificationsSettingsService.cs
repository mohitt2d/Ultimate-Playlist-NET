#region Usings

using CSharpFunctionalExtensions;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity.Specifications;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Services.Common.Interfaces.Notification;

#endregion

namespace UltimatePlaylist.Services.Notification
{
    public class NotificationsSettingsService : INotificationsSettingsService
    {
        #region Private members

        private readonly Lazy<IRepository<User>> UserRepositoryProvider;

        #endregion

        #region Constructor(s)

        public NotificationsSettingsService(
            Lazy<IRepository<User>> userRepositoryProvider)
        {
            UserRepositoryProvider = userRepositoryProvider;
        }

        #endregion

        #region Properties

        private IRepository<User> UserRepository => UserRepositoryProvider.Value;

        #endregion

        #region Public Methods

        public async Task<Result> SetDeviceToken(string deviceToken, Guid userExternalId)
        {
            var user = await UserRepository.FirstOrDefaultAsync(new UserSpecification().ByExternalId(userExternalId));
            return await Result.FailureIf(user is null, user, ErrorMessages.UserDoesNotExist.ToString())
                .TapIf(user => string.IsNullOrEmpty(user.DeviceToken) && user.ShouldNotificationBeEnabled, user => user.IsNotificationEnabled = true)
                .Tap(user => user.DeviceToken = deviceToken)
                .Tap(user => user.ShouldNotificationBeEnabled = false)
                .Tap(async () => await UserRepository.UpdateAndSaveAsync(user));
        }

        public async Task<Result> RemoveDeviceToken(Guid userExternalId)
        {
            var user = await UserRepository.FirstOrDefaultAsync(new UserSpecification().ByExternalId(userExternalId));
            return await Result.FailureIf(user is null, user, ErrorMessages.UserDoesNotExist.ToString())
                .Tap(user => user.DeviceToken = null)
                .Tap(async () => await UserRepository.UpdateAndSaveAsync(user));
        }

        public async Task<Result> UpdateEnableStatusAsync(Guid userExternalId, bool isEnabled)
        {
            var user = await UserRepository.FirstOrDefaultAsync(new UserSpecification().ByExternalId(userExternalId));
            user.IsNotificationEnabled = isEnabled;

            await UserRepository.UpdateAndSaveAsync(user);

            return Result.Success();
        }
        #endregion
    }
}
