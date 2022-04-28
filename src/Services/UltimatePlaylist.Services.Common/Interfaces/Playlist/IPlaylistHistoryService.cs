#region Usings

using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using UltimatePlaylist.Common.Models;
using UltimatePlaylist.Services.Common.Models;
using UltimatePlaylist.Services.Common.Models.Playlist;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Playlist
{
    public interface IPlaylistHistoryService
    {
        Task<Result<PlaylistHistroryPaginatedReadServiceModel>> GetUserPlaylistsAsync(
            Guid userExternalId,
            Pagination pagination,
            PlaylistHistoryWriteServiceModel playlistHistoryWriteServiceModel);
    }
}
