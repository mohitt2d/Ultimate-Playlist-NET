#region Usings

using System;
using System.IO;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using UltimatePlaylist.Services.Common.Models.Files;
using UltimatePlaylist.Services.Common.Models.Profile;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Profile
{
    public interface IUserProfileService
    {
        Task<Result<UserProfileInfoReadServiceModel>> GetUserInfoAsync(Guid userExternalId);

        Task<Result<UserProfileInfoReadServiceModel>> UpdateUserProfileAsync(Guid userExternalId, EditUserProfileWriteServiceModel editUserProfileWriteServiceModel);

        Task<Result> AddOrUpdatePinAsync(Guid userExternalId, UserPinWriteServiceModel userPinWriteServiceModel);

        Task<Result> IsPinCorrectAsync(Guid userExternalId, UserPinWriteServiceModel userPinWriteServiceModel);

        Task<Result> DeactivateUserAsync(Guid userExternalId);

        Task<Result> RemovePinAsync(Guid userExternalId);

        Task<Result<FileReadServiceModel>> SetOrUpdateAvatarAsync(Guid userExternalId, Stream fileStream, string fileName);

        Task<Result> DeleteUserAvatarAsync(Guid userExternalId);
    }
}
