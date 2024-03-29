﻿#region Usings

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UltimatePlaylist.AdminApi.Models.Song;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Common.Mvc.Attributes;
using UltimatePlaylist.Common.Mvc.Controllers;
using UltimatePlaylist.Common.Mvc.Paging;
using UltimatePlaylist.Services.Common.Interfaces.Song;
using UltimatePlaylist.Services.Common.Models.Song;
using static UltimatePlaylist.Common.Mvc.Consts.Consts;

#endregion

namespace UltimatePlaylist.AdminApi.Area.Admin
{
    [Area("Song")]
    [Route("[controller]")]
    [AuthorizeRole(UserRole.Administrator)]
    [ApiExplorerSettings(GroupName = AdminApiGroups.Administrator)]
    public class SongController : BaseControllerWithAuthentication
    {
        #region Private Members

        private readonly Lazy<IMapper> MapperProvider;

        private readonly Lazy<ISongService> SongServiceProvider;

        private readonly Lazy<ISongStatisticsService> SongStatisticsServiceProvider;

        #endregion

        #region Constructor(s)

        public SongController(
            Lazy<IMapper> mapperProvider,
            Lazy<ISongService> songServiceProvider,
            Lazy<ISongStatisticsService> songStatisticsServiceProvider)
        {
            MapperProvider = mapperProvider;
            SongServiceProvider = songServiceProvider;
            SongStatisticsServiceProvider = songStatisticsServiceProvider;
        }

        #endregion

        #region Private Properites

        private IMapper Mapper => MapperProvider.Value;

        private ISongService SongService => SongServiceProvider.Value;

        private ISongStatisticsService SongStatisticsService => SongStatisticsServiceProvider.Value;

        #endregion

        #region GET

        [HttpGet("songs-list")]
        [ProducesEnvelope(typeof(PaginatedResponse<SongResponseModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SongsListAsync()
        {
            return await SongStatisticsService.SongsListAsync(XPagination, new SongsAnalyticsFilterServiceModel())
               .Map(songs => Mapper.Map<PaginatedResponse<SongResponseModel>>(songs))
               .Finally(BuildEnvelopeResult);
        }

        #endregion

        #region POST

        [HttpPost("add-song")]
        [ProducesEmptyEnvelope(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddSongAsync([FromBody] AddSongRequestModel addSongRequestModel)
        {
            var mapped = Mapper.Map<AddSongWriteServiceModel>(addSongRequestModel);

            return await SongService.AddSongAsync(mapped)
               .Finally(BuildEnvelopeResult);
        }

        #endregion

        #region DELETE

        [HttpDelete("remove-song")]
        [ProducesEmptyEnvelope(StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveSongAsync([FromBody] RemoveSongRequestModel removeSongRequestModel)
        {
            var mapped = Mapper.Map<RemoveSongWriteServiceModel>(removeSongRequestModel);

            return await SongService.RemoveSongAsync(mapped)
               .Finally(BuildEnvelopeResult);
        }

        #endregion
    }
}