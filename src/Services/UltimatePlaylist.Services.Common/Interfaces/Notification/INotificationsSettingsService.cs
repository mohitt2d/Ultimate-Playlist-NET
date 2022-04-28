#region Usings

using CSharpFunctionalExtensions;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Notification
{
    public interface INotificationsSettingsService
    {
        public Task<Result> SetDeviceToken(string deviceToken, Guid userExternalId);

        public Task<Result> RemoveDeviceToken(Guid userExternalId);

        public Task<Result> UpdateEnableStatusAsync(Guid userExternalId, bool isEnabled);
    }
}
