#region Usings

using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Services.Common.Models.AppleMusic;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.AppleMusic
{
    public interface IAppleMusicConnectionService
    {
        Result<DeveloperTokenReadServiceModel> GetDeveloperTokenAsync();

        Task<Result> SaveUserTokenAsync(string userToken, Guid userExternalId);

        Task<Result> RemoveUserTokenAsync(DspType type, Guid userExternalId);
    }
}
