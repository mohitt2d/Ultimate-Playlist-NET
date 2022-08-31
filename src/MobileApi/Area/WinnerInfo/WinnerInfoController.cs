using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using CSharpFunctionalExtensions;
using UltimatePlaylist.Common.Mvc.Attributes;
using UltimatePlaylist.Common.Mvc.Controllers;
using UltimatePlaylist.Services.Common.Interfaces.Games;
using UltimatePlaylist.Services.Common.Models.Games;


namespace UltimatePlaylist.MobileApi.Area.WinnerInfo
{
    [Area("WinnerInfo")]
    [Route("[controller]")]
    [ApiController]
    public class WinnerInfoController : BaseController
    {
        #region Private Members

        private readonly Lazy<IMapper> MapperProvider;
        private readonly Lazy<IWinningsInfoService> WinningsServiceProvider;

        #endregion


        #region Constructor(s)

        public WinnerInfoController(
            Lazy<IMapper> mapperProvider,
            Lazy<IWinningsInfoService> winningsServiceProvider)
        {
            MapperProvider = mapperProvider;
            WinningsServiceProvider = winningsServiceProvider;
        }

        #endregion

        #region Private Properites

        private IMapper Mapper => MapperProvider.Value;

        private IWinningsInfoService WinningsService => WinningsServiceProvider.Value;


        #endregion

        [HttpGet("daily")]
        [ProducesEnvelope(typeof(List<WinnerProfileReadServiceModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWinners()
        {

            return await WinningsService.GetDailyWinnersAsync()
               .Finally(BuildEnvelopeResult);
        }

        [HttpGet("daily/{pageSize}/{pageNumber}")]
        [ProducesEnvelope(typeof(List<WinnerProfileReadServiceModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWinnersPaging(int pageSize, int pageNumber)
        {
            return await WinningsService.GetDailyWinnersAsync(pageSize: pageSize, pageNumber: pageNumber)
               .Finally(BuildEnvelopeResult);
        }

        [HttpGet("jackpot")]
        [ProducesEnvelope(typeof(List<JackpotWinnersAndNumbersResponseModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetJackpot()
        {
            return await WinningsService.GetUltimatePayoutInfoPublicAsync()
               .Finally(BuildEnvelopeResult);
        }

        [HttpGet("jackpot/{pageSize}/{pageNumber}")]
        [ProducesEnvelope(typeof(List<JackpotWinnersAndNumbersResponseModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetJackpotPaging(int pageSize, int pageNumber)
        {
            return await WinningsService.GetUltimatePayoutInfoPublicAsync(pageSize: pageSize, pageNumber: pageNumber)
               .Finally(BuildEnvelopeResult);
        }

        [HttpGet("winnings")]
        [ProducesEnvelope(typeof(List<WinningHistoryReadServicModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWinningHistory()
        {
            //Guid guid = new Guid("39E6C869-2709-ED11-B47A-501AC5190011");
            return await WinningsService.GetWinningHistory(XUserExternalId)
               .Finally(BuildEnvelopeResult);
        }

        [HttpGet("winnings/currentWinnings")]
        [ProducesEnvelope(typeof(WinningHistoryReadServicModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTodayWiining()
        {
            return await WinningsService.GetTodayWinning(XUserExternalId)
               .Finally(BuildEnvelopeResult);
        }

        [HttpGet("winnings/pastWinnings")]
        [ProducesEnvelope(typeof(List<WinningHistoryReadServicModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPastWinnings()
        {
            return await WinningsService.GetPastWinnings(XUserExternalId)
               .Finally(BuildEnvelopeResult);
        }

    }
}