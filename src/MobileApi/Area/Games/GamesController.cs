#region usings

using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Common.Mvc.Attributes;
using UltimatePlaylist.Common.Mvc.Controllers;
using UltimatePlaylist.MobileApi.Models.Games;
using UltimatePlaylist.Services.Common.Interfaces.Games;
using UltimatePlaylist.Services.Games.Jobs;
using static UltimatePlaylist.Common.Mvc.Consts.Consts;


using UltimatePlaylist.Common.Mvc.Helpers;
using UltimatePlaylist.Database.Infrastructure.Entities.Games;
using UltimatePlaylist.Database.Infrastructure.Entities.Games.Specifications;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Games.Interfaces;
using UltimatePlaylist.Games.Models.Raffle;
using UltimatePlaylist.Services.Common.Interfaces.Ticket;
using Microsoft.Extensions.Options;

#endregion

namespace UltimatePlaylist.MobileApi.Area.Dsp
{
    [Area("Games")]
    [Route("[controller]")]
    //[AuthorizeRole(UserRole.User)]
    [ApiExplorerSettings(GroupName = MobileApiGroups.User)]
    public class GamesController : BaseControllerWithAuthentication
    {
        #region Private Members

        private readonly Lazy<IMapper> MapperProvider;
        private readonly Lazy<IWinningsInfoService> WinningsServiceProvider;
        private readonly Lazy<IGamesInfoService> GamesInfoServiceProvider;
        private readonly Lazy<IUltimatePayoutGameService> UltimatePayoutGameServiceProvider;
        private readonly Lazy<IDailyCashTicketsService> DailyCashTicketsServiceProvider;
        private readonly Lazy<IRaffleService> RaffleServiceProvider;
        private readonly Lazy<IRepository<DailyCashDrawingEntity>> DailyCashDrawingRepositoryProvider;
        private readonly Lazy<ILogger<DailyCashGameJob>> LoggerProvider;
        private readonly Lazy<IGamesWinningCollectionService> GamesWinningCollectionServiceProvider;
        private readonly Lazy<IWinningsService> WinningsProvider;
        private readonly IOptions<PlaylistConfig> PlaylistConfig;

        #endregion

        #region Constructor(s)

        public GamesController(
            Lazy<IMapper> mapperProvider,
            Lazy<IWinningsInfoService> winningsServiceProvider,
            Lazy<IGamesInfoService> gamesInfoServiceProvider,
            Lazy<IUltimatePayoutGameService> ultimatePayoutGameServiceProvider,


            Lazy<IDailyCashTicketsService> dailyCashTicketsServiceProvider,
            Lazy<IRaffleService> raffleServiceProvider,
            Lazy<IRepository<DailyCashDrawingEntity>> dailyCashDrawingRepositoryProvider,
            Lazy<ILogger<DailyCashGameJob>> loggerProvider,
            Lazy<IGamesWinningCollectionService> gamesWinningCollectionServiceProvider,

            Lazy<IWinningsService> winningsProvider,
            IOptions<PlaylistConfig> playlistConfig)
        {
            MapperProvider = mapperProvider;
            WinningsServiceProvider = winningsServiceProvider;
            GamesInfoServiceProvider = gamesInfoServiceProvider;
            UltimatePayoutGameServiceProvider = ultimatePayoutGameServiceProvider;
            DailyCashTicketsServiceProvider = dailyCashTicketsServiceProvider;
            RaffleServiceProvider = raffleServiceProvider;
            DailyCashDrawingRepositoryProvider = dailyCashDrawingRepositoryProvider;
            LoggerProvider = loggerProvider;
            GamesWinningCollectionServiceProvider = gamesWinningCollectionServiceProvider;
            this.WinningsProvider=winningsProvider;
            PlaylistConfig = playlistConfig;
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

        [HttpGet("run-daily-drawing-manually")]
        [AllowAnonymous]
        public async Task RunDailyCashGameJobManuallyAsync()
        {
            var dailyCashGameJob = new DailyCashGameJob(DailyCashTicketsServiceProvider, RaffleServiceProvider, DailyCashDrawingRepositoryProvider, WinningsProvider,
                LoggerProvider, GamesWinningCollectionServiceProvider, PlaylistConfig);
            await dailyCashGameJob.RunDailyCashGame();
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