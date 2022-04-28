#region Usings

using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using UltimatePlaylist.Services.Common.AppleMusic.Enums;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Client.AppleMusic
{
    public interface IAppleMusicSongClientService
    {
        public Task<Result> AddSongToPlaylistAsync(string userToken, AppleMusicResurceType libraryResource, string playlistId, string appleMusicSongId, Guid userExternalId);
    }
}
