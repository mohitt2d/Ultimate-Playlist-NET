#region Usings

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Common.Mvc.Attributes;
using UltimatePlaylist.Common.Mvc.Controllers;
using UltimatePlaylist.MobileApi.Models.Song;
using UltimatePlaylist.Services.Common.Interfaces.Song;
using UltimatePlaylist.Services.Common.Models.Song;
using static UltimatePlaylist.Common.Mvc.Consts.Consts;

#endregion

namespace UltimatePlaylist.MobileApi.Area.Song
{
    [Area("Song")]
    [Route("[controller]")]
    [AuthorizeRole(UserRole.User)]
    [ApiExplorerSettings(GroupName = MobileApiGroups.User)]
    public class SongController : BaseControllerWithAuthentication
    {
        #region Private Members

        private readonly Lazy<IMapper> MapperProvider;

        private readonly Lazy<ISongRatingService> RatingServiceProvider;

        #endregion

        #region Constructor(s)

        public SongController(
            Lazy<IMapper> mapperProvider,
            Lazy<ISongRatingService> ratingServiceProvider)
        {
            MapperProvider = mapperProvider;
            RatingServiceProvider = ratingServiceProvider;
        }

        #endregion

        #region Private Properites

        private IMapper Mapper => MapperProvider.Value;

        private ISongRatingService RatingService => RatingServiceProvider.Value;

        #endregion

        #region POST

        [HttpPost("rate-song")]
        [ProducesEnvelope(typeof(RatingEarnedTicketsResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> RateSongAsync([FromBody] RateSongRequestModel rateSongRequestModel)
        {
            var mapped = Mapper.Map<RateSongWriteServiceModel>(rateSongRequestModel);

            return await RatingService.RateSongAsync(XUserExternalId, mapped)
               .Map(readServiceModel => Mapper.Map<RatingEarnedTicketsResponseModel>(readServiceModel))
               .Finally(BuildEnvelopeResult);
        }

        #endregion
    }
}
