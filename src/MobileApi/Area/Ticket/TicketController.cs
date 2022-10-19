#region Usings

using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Common.Mvc.Attributes;
using UltimatePlaylist.Common.Mvc.Controllers;
using UltimatePlaylist.MobileApi.Models.Ticket;
using UltimatePlaylist.Services.Common.Interfaces.Ticket;
using static UltimatePlaylist.Common.Mvc.Consts.Consts;

#endregion

namespace UltimatePlaylist.MobileApi.Area.Ticket
{
    [Area("Ticket")]
    [Route("[controller]")]
    [AuthorizeRole(UserRole.User)]
    [ApiExplorerSettings(GroupName = MobileApiGroups.User)]
    public class TicketController : BaseControllerWithAuthentication
    {
        #region Private Members

        private readonly Lazy<IMapper> MapperProvider;
        private readonly Lazy<ITicketStatsService> TicketServiceServiceProvider;
        private readonly AuthConfig Config;
        
        #endregion

        #region Constructor(s)

        public TicketController(
            Lazy<IMapper> mapperProvider,
            Lazy<ITicketStatsService> ticketServiceServiceProvider)
        {
            MapperProvider = mapperProvider;
            TicketServiceServiceProvider = ticketServiceServiceProvider;
        }

        #endregion

        #region Private Properites

        private IMapper Mapper => MapperProvider.Value;

        private ITicketStatsService TicketService => TicketServiceServiceProvider.Value;

        #endregion

        #region GET

        [HttpGet("stats")]
        [ProducesEnvelope(typeof(TicketsStatsResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTicketsStats()
        {
            return await TicketService.UserTicketStatsAsync(XUserExternalId)
               .Map(stats => Mapper.Map<TicketsStatsResponseModel>(stats))
               .Finally(BuildEnvelopeResult);
        }

        [HttpGet("last-earned-not-displayed")]
        [ProducesEnvelope(typeof(LastEarnedNotDisplayedTicketsResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLatEarnedTicketsNotDiasplayed()
        {
            int s = 0;
            return await TicketService.UserTicketStatsAsync(XUserExternalId)
               .Map(tickets => new LastEarnedNotDisplayedTicketsResponseModel()
               {
                   LatestEarnedTickets = tickets.TicketsAmountForTodayDrawing,
               })
               .Finally(BuildEnvelopeResult);
        }

        //new2022-10-14-from

        [HttpPost("popupStatus")]
        [ProducesEnvelope(typeof(TicketStatsRequestModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeTicketsStatus([FromBody] TicketsStatsRequestModel model)
        {
            return await TicketService.ReverseTicketsStatus(model.UserPlaylistSongId, model.IsErrorTriggered)
                .Map(isErrorTriggered => new TicketStatsRequestModel() { IsErrorTriggered = model.IsErrorTriggered })
                .Finally(BuildEnvelopeResult);
        }

        [HttpGet("userStatus/{userId}")]
        [ProducesEnvelope(typeof(UserTicketStatsRequestModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> UserTicketStatus(int userId)
        {
            return await TicketService.UserTicketStatus(userId)
                .Map(isErrorTriggered => new UserTicketStatsRequestModel() { IsErrorTriggered = isErrorTriggered })
                .Finally(BuildEnvelopeResult);
        }


        //new2022-10-14-to

        #endregion
    }
}
