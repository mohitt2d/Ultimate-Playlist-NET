#region Usings

using CSharpFunctionalExtensions;
using UltimatePlaylist.Services.Common.Models.Ticket;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Ticket
{
    public interface ITicketStatsService
    {
        Task<Result<TicketsStatsReadServiceModel>> UserTicketStatsAsync(Guid userExternalId);
    }
}
