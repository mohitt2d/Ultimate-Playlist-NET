#region Usings

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Common.Mvc.Attributes;
using UltimatePlaylist.Common.Mvc.Controllers;
using UltimatePlaylist.Common.Mvc.Paging;
using UltimatePlaylist.MobileApi.Models.Playlist;
using UltimatePlaylist.Services.Common.Interfaces.Playlist;
using UltimatePlaylist.Services.Common.Models.Playlist;
using static UltimatePlaylist.Common.Mvc.Consts.Consts;

#endregion

namespace UltimatePlaylist.MobileApi.Area.Playlist
{
    [Area("Playlist")]
    [Route("[controller]")]
    [AuthorizeRole(UserRole.User)]
    [ApiExplorerSettings(GroupName = MobileApiGroups.User)]
    public class PlaylistController : BaseControllerWithAuthentication
    {
        #region Private Members

        private readonly Lazy<IMapper> MapperProvider;
        private readonly Lazy<IPlaylistService> PlaylistServiceProvider;
        private readonly Lazy<IPlaylistHistoryService> PlaylistHistoryServiceProvider;

        #endregion

        #region Constructor(s)

        public PlaylistController(
            Lazy<IMapper> mapperProvider,
            Lazy<IPlaylistService> playlistServiceProvider,
            Lazy<IPlaylistHistoryService> playlistHistoryServiceProvider)
        {
            MapperProvider = mapperProvider;
            PlaylistServiceProvider = playlistServiceProvider;
            PlaylistHistoryServiceProvider = playlistHistoryServiceProvider;
        }

        #endregion

        #region Private Properites

        private IMapper Mapper => MapperProvider.Value;

        private IPlaylistService PlaylistService => PlaylistServiceProvider.Value;

        private IPlaylistHistoryService PlaylistHistoryService => PlaylistHistoryServiceProvider.Value;

        #endregion

        #region GET

        [HttpGet("today")]
        [ProducesEnvelope(typeof(PlaylistResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTodaysPlaylist()
        {
            return await PlaylistService.GetTodaysPlaylist(XUserExternalId)
               .Map(result => Mapper.Map<PlaylistResponseModel>(result))
               .Finally(BuildEnvelopeResult);
        }

        [HttpGet("history")]
        [ProducesEnvelope(typeof(PlaylistHistroryPaginatedResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserPlaylistHistory([FromQuery] PlaylistHistoryRequestModel playlistHistoryRequestModel)
        {
            var mapped = Mapper.Map<PlaylistHistoryWriteServiceModel>(playlistHistoryRequestModel);

            return await PlaylistHistoryService.GetUserPlaylistsAsync(XUserExternalId, XPagination, mapped)
               .Map(result => new PlaylistHistroryPaginatedResponseModel(Mapper.Map<IList<PlaylistHistoryResponseModel>>(result.Items), result.TotalCount, result.Pagination, result.NextPlaylistAvailable))
               .Finally(BuildEnvelopeResult);
        }

        #endregion
    }
}
