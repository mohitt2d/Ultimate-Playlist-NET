#region Usings

using System;
using System.Collections.Generic;
using UltimatePlaylist.Games.Models.Raffle;

#endregion

namespace UltmiatePlaylist.Games.Tests.Helpers
{
    public static class RaffleTicketsHelper
    {
        public static ICollection<RaffleUserTicketReadServiceModel> GenerateRaffleTickets(long quantity)
        {
            var raffleTickets = new List<RaffleUserTicketReadServiceModel>();
            for(var i = 0; i < quantity; i++)
            {
                raffleTickets.Add(GenerateRaffleTicket());
            }

            return raffleTickets;
        }

        private static RaffleUserTicketReadServiceModel GenerateRaffleTicket()
        {
            var userExternalId = Guid.NewGuid();
            var userTicketExternalId = Guid.NewGuid();

            return new RaffleUserTicketReadServiceModel
            {
                UserExternalId = userExternalId,
                UserFriendlyTicketId = userTicketExternalId.ToString(),
                UserTicketExternalId = userTicketExternalId,
            };

        }  

    }
}
