using Microsoft.AspNetCore.Mvc;
using UltimatePlaylist.Common.Mvc.Attributes;
using CSharpFunctionalExtensions;
using UltimatePlaylist.Common.Mvc.Controllers;
using UltimatePlaylist.AdminApi.Models.Ticket;
using UltimatePlaylist.Services.Common.Interfaces.Ticket;
using AutoMapper;
using UltimatePlaylist.Services.Common.Interfaces.Identity;

namespace UltimatePlaylist.AdminApi.Area.Admin
{
    public class TicketController : BaseController
    {

        private readonly Lazy<IMapper> MapperProvider;
        private readonly Lazy<ITicketStatsService> TicketServiceServiceProvider;
        private readonly Lazy<IAdministratorIdentityService> IdentityServiceProvider;
        private ITicketStatsService TicketService => TicketServiceServiceProvider.Value;
        private IAdministratorIdentityService IdentityService => IdentityServiceProvider.Value;

        public TicketController(
            Lazy<IMapper> mapperProvider,
            Lazy<IAdministratorIdentityService> identityServiceProvider,
            Lazy<ITicketStatsService> ticketServiceServiceProvider)
        {
            MapperProvider = mapperProvider;
            TicketServiceServiceProvider = ticketServiceServiceProvider;
            IdentityServiceProvider = identityServiceProvider;
        }

        //new2022-10-14-from

        [HttpPost("ticket/popupStatus")]
        [ProducesEnvelope(typeof(TicketStatsRequestModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeTicketsStatus([FromBody] TicketsStatsRequestModel model)
        {
            string x = IdentityService.GetUserId(model.UserPlaylistSongId);
            long id = long.Parse(x, System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.AllowLeadingSign);

            return await TicketService.ReverseTicketsStatus(id, model.IsErrorTriggered)
                .Map(isErrorTriggered => new TicketStatsRequestModel() { IsErrorTriggered = model.IsErrorTriggered })
                .Finally(BuildEnvelopeResult);
        }

        [HttpGet("ticket/userStatus/{token}")]
        [ProducesEnvelope(typeof(UserTicketStatsRequestModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> UserTicketStatus(string token)
        {
            string x = IdentityService.GetUserId(token);
            long id = long.Parse(x, System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.AllowLeadingSign);
            return await TicketService.UserTicketStatus(id)
                .Map(isErrorTriggered => new UserTicketStatsRequestModel() { IsErrorTriggered = isErrorTriggered })
                .Finally(BuildEnvelopeResult);
        }


        //new2022-10-14-to
    }
}
