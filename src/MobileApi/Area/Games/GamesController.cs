#region usings

using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Common.Mvc.Attributes;
using UltimatePlaylist.Common.Mvc.Controllers;
using UltimatePlaylist.MobileApi.Models.Games;
using UltimatePlaylist.Services.Common.Interfaces.Games;
using static UltimatePlaylist.Common.Mvc.Consts.Consts;

#endregion

namespace UltimatePlaylist.MobileApi.Area.Dsp
{
    [Area("Games")]
    [Route("[controller]")]
    [AuthorizeRole(UserRole.User)]
    [ApiExplorerSettings(GroupName = MobileApiGroups.User)]
    public class GamesController : BaseControllerWithAuthentication
    {
        #region Private Members

        private readonly Lazy<IMapper> MapperProvider;
        private readonly Lazy<IWinningsInfoService> WinningsServiceProvider;
        private readonly Lazy<IGamesInfoService> GamesInfoServiceProvider;
        private readonly Lazy<IUltimatePayoutGameService> UltimatePayoutGameServiceProvider;

        #endregion

        #region Constructor(s)

        public GamesController(
            Lazy<IMapper> mapperProvider,
            Lazy<IWinningsInfoService> winningsServiceProvider,
            Lazy<IGamesInfoService> gamesInfoServiceProvider,
            Lazy<IUltimatePayoutGameService> ultimatePayoutGameServiceProvider)
        {
            MapperProvider = mapperProvider;
            WinningsServiceProvider = winningsServiceProvider;
            GamesInfoServiceProvider = gamesInfoServiceProvider;
            UltimatePayoutGameServiceProvider = ultimatePayoutGameServiceProvider;
        }

        #endregion

        #region Private Properites

        private IMapper Mapper => MapperProvider.Value;

        private IWinningsInfoService WinningsService => WinningsServiceProvider.Value;

        private IGamesInfoService GamesInfoService => GamesInfoServiceProvider.Value;

        private IUltimatePayoutGameService UltimatePayoutGameService => UltimatePayoutGameServiceProvider.Value;

        #endregion

        #region GET

        [HttpGet("")]
        [ProducesEnvelope(typeof(GamesInfoResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetGames()
        {
            return await GamesInfoService.GetGamesInfoAsync(XUserExternalId)
               .Map(result => Mapper.Map<GamesInfoResponseModel>(result))
               .Finally(BuildEnvelopeResult);
        }

        [HttpGet("winners")]
        [ProducesEnvelope(typeof(WinnersResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWinnersList()
        {
            return await WinningsService.GetWinnersListAsync(XUserExternalId)
               .Map(result => Mapper.Map<WinnersResponseModel>(result))
               .Finally(BuildEnvelopeResult);
        }

        [HttpGet("check-newest-game")]
        [ProducesEnvelope(typeof(GamesInfoResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> CheckNewestGame()
        {
            return await GamesInfoService.CheckNewestGame(XUserExternalId)
               .Map(result => Mapper.Map<GamesInfoResponseModel>(result))
               .Finally(BuildEnvelopeResult);
        }

        [HttpGet("ultimate-payout")]
        [ProducesEnvelope(typeof(UltimatePayoutResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUltimatePayout()
        {
            return await UltimatePayoutGameService.GetUltimatePayoutInfoAsync(XUserExternalId)
               .Map(result => Mapper.Map<UltimatePayoutResponseModel>(result))
               .Finally(BuildEnvelopeResult);
        }

        [HttpGet("ultimate/check-newest-game")]
        [ProducesEnvelope(typeof(UltimatePayoutResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> CheckltimatePayout()
        {
            return await UltimatePayoutGameService.CheckNewestGame(XUserExternalId)
               .Map(result => Mapper.Map<UltimatePayoutResponseModel>(result))
               .Finally(BuildEnvelopeResult);
        }

        #endregion

        #region POST

        [HttpPost("ultimate/claim-winnings")]
        [ProducesEnvelope(typeof(UltimatePayoutResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> ClaimWinningsForUltimate()
        {
            return await UltimatePayoutGameService.ClaimWinningsAsync(XUserExternalId)
               .Map(result => Mapper.Map<UltimatePayoutResponseModel>(result))
               .Finally(BuildEnvelopeResult);
        }

        [HttpPost("claim-winnings")]
        [ProducesEnvelope(typeof(GamesInfoResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> ClaimWinnings()
        {
            return await GamesInfoService.ClaimWinningsAsync(XUserExternalId)
                .Map(result => Mapper.Map<GamesInfoResponseModel>(result))
               .Finally(BuildEnvelopeResult);
        }

        [HttpPost("ultimate-payout")]
        [ProducesEnvelope(typeof(UltimatePayoutResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> PostNumbersAsync(UltimatePayoutRequestModel requestModel)
        {
            return await UltimatePayoutGameService.PostUltimatePayoutInfoAsync(XUserExternalId, requestModel.UltimatePayoutNumbers)
               .Map(result => Mapper.Map<UltimatePayoutResponseModel>(result))
               .Finally(BuildEnvelopeResult);
        }

        #endregion
    }
}