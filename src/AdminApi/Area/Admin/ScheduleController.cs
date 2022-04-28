#region Usings

using System;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UltimatePlaylist.AdminApi.Models.Schedule;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Common.Mvc.Attributes;
using UltimatePlaylist.Common.Mvc.Controllers;
using UltimatePlaylist.Services.Common.Interfaces.Playlist;
using UltimatePlaylist.Services.Common.Interfaces.Schedule;
using UltimatePlaylist.Services.Common.Models.Playlist;
using UltimatePlaylist.Services.Common.Models.Schedule;
using static UltimatePlaylist.Common.Mvc.Consts.Consts;

#endregion

namespace UltimatePlaylist.AdminApi.Area.Admin
{
    [Area("Schedule")]
    [Route("[controller]")]
    [AuthorizeRole(UserRole.Administrator)]
    [ApiExplorerSettings(GroupName = AdminApiGroups.Administrator)]
    public class ScheduleController : BaseControllerWithAuthentication
    {
        #region Private Members

        private readonly Lazy<IMapper> MapperProvider;

        private readonly Lazy<IScheduleService> ScheduleServiceProvider;

        #endregion

        #region Constructor(s)

        public ScheduleController(
            Lazy<IMapper> mapperProvider,
            Lazy<IScheduleService> scheduleServiceProvider)
        {
            MapperProvider = mapperProvider;
            ScheduleServiceProvider = scheduleServiceProvider;
        }

        #endregion

        #region Private Properites

        private IMapper Mapper => MapperProvider.Value;

        private IScheduleService ScheduleService => ScheduleServiceProvider.Value;

        #endregion

        #region GET

        [HttpGet("playlists-as-calendar")]
        [ProducesEnvelope(typeof(PlaylistsScheduleCalendarInfoResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPlaylistsAsCalendarDataAsync([FromQuery] CalendarDataRequestModel calendarDataRequestModel)
        {
            return await ScheduleService.PlaylistsAsCalendarInfoAsync(calendarDataRequestModel.CalendarDateTimeStamp.Value)
               .Map(playlists => Mapper.Map<PlaylistsScheduleCalendarInfoResponseModel>(playlists))
               .Finally(BuildEnvelopeResult);
        }

        [HttpGet("playlist-songs")]
        [ProducesEnvelope(typeof(PlaylistsResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPlaylistsSongsAsync([FromQuery] PlaylistSongsRequestModel playlistSongsRequestModel)
        {
            return await ScheduleService.PlaylistsSongsAsync(playlistSongsRequestModel.PlaylistExternalId.Value)
               .Map(playlist => Mapper.Map<PlaylistsResponseModel>(playlist))
               .Finally(BuildEnvelopeResult);
        }

        #endregion

        #region POST

        [HttpPost("add-song-to-playlist")]
        [ProducesEmptyEnvelope(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddSongToPlaylistAsync([FromBody] AddSongToPlaylistRequestModel addSongToPlaylistRequestModel)
        {
            var mapped = Mapper.Map<AddSongToPlaylistWriteServiceModel>(addSongToPlaylistRequestModel);

            return await ScheduleService.AddSongToPlaylist(mapped)
               .Finally(BuildEnvelopeResult);
        }

        #endregion

        #region DELETE

        [HttpDelete("remove-song-from-playlist")]
        [ProducesEmptyEnvelope(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPlaylistsSongsAsync([FromBody] RemoveSongFromPlaylistRequestModel removeSongFromPlaylistRequestModel)
        {
            var mapped = Mapper.Map<RemoveSongFromPlaylistWriteServiceModel>(removeSongFromPlaylistRequestModel);

            return await ScheduleService.RemoveSongFromPlaylistAsync(mapped)
               .Finally(BuildEnvelopeResult);
        }

        [HttpDelete("remove-all-songs-from-playlist")]
        [ProducesEmptyEnvelope(StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveAllSongsFromPlaylistAsync([FromBody] PlaylistBaseRequestModel playlistBaseRequestModel)
        {
            var mapped = Mapper.Map<PlaylistBaseWriteServiceModel>(playlistBaseRequestModel);

            return await ScheduleService.RemoveAllSongsFromPlaylistAsync(mapped.ExternalId)
               .Finally(BuildEnvelopeResult);
        }

        #endregion
    }
}
