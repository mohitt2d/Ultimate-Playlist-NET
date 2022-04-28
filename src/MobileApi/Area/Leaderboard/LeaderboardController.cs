#region Usings

using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Common.Mvc.Attributes;
using UltimatePlaylist.Common.Mvc.Controllers;
using UltimatePlaylist.MobileApi.Models.Leaderboard;
using UltimatePlaylist.Services.Common.Interfaces.Leaderboard;
using static UltimatePlaylist.Common.Mvc.Consts.Consts;

#endregion

namespace UltimatePlaylist.MobileApi.Area.Leaderboard
{
    [Area("Leaderboard")]
    [Route("[controller]")]
    [AuthorizeRole(UserRole.User)]
    [ApiExplorerSettings(GroupName = MobileApiGroups.User)]
    public class LeaderboardController : BaseControllerWithAuthentication
    {
        #region Private Members

        private readonly Lazy<IMapper> MapperProvider;
        private readonly Lazy<ILeaderboardService> LeaderboardServiceProvider;

        #endregion

        #region Constructor(s)

        public LeaderboardController(
            Lazy<IMapper> mapperProvider,
            Lazy<ILeaderboardService> leaderboardServiceProvider)
        {
            MapperProvider = mapperProvider;
            LeaderboardServiceProvider = leaderboardServiceProvider;
        }

        #endregion

        #region Private Properites

        private IMapper Mapper => MapperProvider.Value;

        private ILeaderboardService LeaderboardService => LeaderboardServiceProvider.Value;

        #endregion

        #region GET

        [HttpGet("info")]
        [ProducesEnvelope(typeof(LeaderboardResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLeaderboardInfoAsync()
        {
            return await LeaderboardService.GetLeaderboardInfoAsync(XUserExternalId)
               .Map(leaderboardInfo => Mapper.Map<LeaderboardResponseModel>(leaderboardInfo))
               .Finally(BuildEnvelopeResult);
        }

        #endregion
    }
}
