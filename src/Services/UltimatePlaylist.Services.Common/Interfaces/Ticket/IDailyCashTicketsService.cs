#region Usings

using CSharpFunctionalExtensions;
using UltimatePlaylist.Games.Models.Raffle;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Ticket
{
    public interface IDailyCashTicketsService
    {
        Task<Result<List<RaffleUserTicketReadServiceModel>>> GetTicketsForDailyCashAsync();

        Task UseTickets(IEnumerable<Guid> ticketsExternalIds);
    }
}
