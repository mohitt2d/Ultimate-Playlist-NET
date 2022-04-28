#region Usings

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UltimatePlaylist.AdminApi.Models.CommonData;
using UltimatePlaylist.Common.Mvc.Attributes;
using UltimatePlaylist.Common.Mvc.Controllers;
using UltimatePlaylist.Services.Common.Interfaces.Song;
using static UltimatePlaylist.Common.Mvc.Consts.Consts;

#endregion

namespace UltimatePlaylist.AdminApi.Area.CommonData
{
    [Area("CommonData")]
    [Route("[controller]")]
    [ApiExplorerSettings(GroupName = AdminApiGroups.CommonData)]
    public class CommonDataController : BaseController
    {
        #region Private Members

        private readonly Lazy<IMapper> MapperProvider;
        private readonly Lazy<ISongGenreService> SongServiceProvider;

        #endregion

        #region Constructor(s)

        public CommonDataController(
            Lazy<IMapper> mapperProvider,
            Lazy<ISongGenreService> songServiceProvider)
        {
            MapperProvider = mapperProvider;
            SongServiceProvider = songServiceProvider;
        }

        #endregion

        #region Private Properites

        private IMapper Mapper => MapperProvider.Value;

        private ISongGenreService SongService => SongServiceProvider.Value;

        #endregion

        #region GET

        [HttpGet("music-genres")]
        [ProducesEnvelope(typeof(IList<SongGenresResponseModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMusicGenres()
        {
            return await SongService.Genres()
                .Map(result => Mapper.Map<IList<SongGenresResponseModel>>(result))
                .Finally(BuildEnvelopeResult);
        }

        #endregion
    }
}