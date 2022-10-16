using Microsoft.AspNetCore.Mvc;
using UltimatePlaylist.Common.Mvc.Attributes;
using CSharpFunctionalExtensions;
using UltimatePlaylist.Common.Mvc.Controllers;
using UltimatePlaylist.AdminApi.Models.Ticket;
using UltimatePlaylist.Services.Common.Interfaces.Ticket;
using AutoMapper;

namespace UltimatePlaylist.AdminApi.Area.Admin
{
    public class TicketController : BaseController
    { 

        private readonly Lazy<IMapper> MapperProvider;
        private readonly Lazy<ITicketStatsService> TicketServiceServiceProvider;
        private ITicketStatsService TicketService => TicketServiceServiceProvider.Value;

        public TicketController(
            Lazy<IMapper> mapperProvider,
            Lazy<ITicketStatsService> ticketServiceServiceProvider)
        {
            MapperProvider = mapperProvider;
            TicketServiceServiceProvider = ticketServiceServiceProvider;
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
    }
}
