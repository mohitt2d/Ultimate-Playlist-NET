#region Usings

using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Common.Mvc.Attributes;
using UltimatePlaylist.Common.Mvc.Controllers;
using UltimatePlaylist.MobileApi.Models.Analytics;
using UltimatePlaylist.Services.Common.Interfaces.Song;
using UltimatePlaylist.Services.Common.Models.Analytics;
using static UltimatePlaylist.Common.Mvc.Consts.Consts;

#endregion

namespace UltimatePlaylist.MobileApi.Area.Analitycs
{
    [Area("Analytics")]
    [Route("Analytics")]
    [AuthorizeRole(UserRole.User)]
    [ApiExplorerSettings(GroupName = MobileApiGroups.User)]
    public class AnalyticsController : BaseControllerWithAuthentication
    {
        #region Private Members

        private readonly Lazy<IMapper> MapperProvider;
        private readonly Lazy<IAnalyticsService> AnalyticsServiceProvider;

        #endregion

        #region Constructor(s)

        public AnalyticsController(
            Lazy<IMapper> mapperProvider,
            Lazy<IAnalyticsService> analyticsServiceProvider)
        {
            MapperProvider = mapperProvider;
            AnalyticsServiceProvider = analyticsServiceProvider;
        }

        #endregion

        #region Private Properites

        private IMapper Mapper => MapperProvider.Value;

        private IAnalyticsService AnalyticsService => AnalyticsServiceProvider.Value;

        #endregion

        #region POST

        [HttpPost("event")]
        [ProducesEnvelope(typeof(AnalyticsLastEarnedTicketsResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> SaveAnalitycsData([FromBody] SaveAnalyticsDataRequestModel saveAnalyticsDataRequestModel)
        {
            var mapped = Mapper.Map<SaveAnalyticsDataWriteServiceModel>(saveAnalyticsDataRequestModel);

            return await AnalyticsService.SaveAnalitycsDataAsync(XUserExternalId, mapped)
               .Map(analyticsLastEarnedTicketsReadServiceModel => Mapper.Map<AnalyticsLastEarnedTicketsResponseModel>(analyticsLastEarnedTicketsReadServiceModel))
               .Finally(BuildEnvelopeResult);
        }

        #endregion
    }
}
