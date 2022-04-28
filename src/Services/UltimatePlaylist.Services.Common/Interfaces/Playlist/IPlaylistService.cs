#region Usings

using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using UltimatePlaylist.Services.Common.Models.Playlist;
using UltimatePlaylist.Services.Common.Models.Schedule;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Playlist
{
    public interface IPlaylistService
    {
        Task<Result<PlaylistReadServiceModel>> GetTodaysPlaylist(Guid userExternalId);

        Task<Result<AdminPlaylistReadServiceModel>> GetPlaylist(Guid playlistExternalId);

        Task<Result> AddSongToPlaylistAsync(AddSongToPlaylistWriteServiceModel addSongToPlaylistWriteServiceModel);

        Task<Result> RemoveSongFromPlaylistAsync(RemoveSongFromPlaylistWriteServiceModel removeSongFromPlaylistWriteServiceModel);

        Task<Result> RemoveAllSongsFromPlaylistAsync(Guid playlistExternalId);
    }
}
