#region Usings

using CSharpFunctionalExtensions;
using System.Collections.Generic;
using UltimatePlaylist.Games.Models.Raffle;

#endregion

namespace UltimatePlaylist.Games.Interfaces
{
    public interface IRaffleService
    {
        Result<IEnumerable<RaffleUserTicketReadServiceModel>> GetRaffleWinners(ICollection<RaffleUserTicketReadServiceModel> raffleUsers, int selections);
    }
}
