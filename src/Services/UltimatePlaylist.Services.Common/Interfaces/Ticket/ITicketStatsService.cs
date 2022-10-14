#region Usings

using CSharpFunctionalExtensions;
using UltimatePlaylist.Services.Common.Models.Ticket;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Ticket
{
    public interface ITicketStatsService
    {
        Task<Result<TicketsStatsReadServiceModel>> UserTicketStatsAsync(Guid userExternalId);

        Task<Result<int?>> ReverseTicketStatus(Guid userExternalId, int isErrorTriggered);
        Task<Result<int>> ReverseTicketsStatus(long userPlaylistSongId, int isErrorTriggered);
    }
}
