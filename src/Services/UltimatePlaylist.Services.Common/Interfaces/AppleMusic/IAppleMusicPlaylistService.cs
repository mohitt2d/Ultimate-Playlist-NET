#region Usings

using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using UltimatePlaylist.Services.Common.Models.AppleMusic.Responses;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.AppleMusic
{
    public interface IAppleMusicPlaylistService
    {
        public Task<Result<AppleMusicPlaylistDataResponseModel>> CreateOrRestorePlaylistAsync(Guid userExternalId);
    }
}
