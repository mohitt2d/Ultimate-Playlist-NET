#region Usings

using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using UltimatePlaylist.Services.Common.Models.Playlist;
using UltimatePlaylist.Services.Common.Models.Schedule;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Schedule
{
    public interface IScheduleService
    {
        Task<Result<PlaylistsScheduleCalendarInfoReadServiceModel>> PlaylistsAsCalendarInfoAsync(DateTime calendarDate);

        Task<Result<AdminPlaylistReadServiceModel>> PlaylistsSongsAsync(Guid playlistExternalId);

        Task<Result> AddSongToPlaylist(AddSongToPlaylistWriteServiceModel addSongToPlaylistWriteServiceModel);

        Task<Result> RemoveSongFromPlaylistAsync(RemoveSongFromPlaylistWriteServiceModel removeSongFromPlaylistWriteServiceModel);

        Task<Result> RemoveAllSongsFromPlaylistAsync(Guid playlistExternalId);
    }
}
