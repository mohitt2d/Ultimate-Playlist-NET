#region Usings

using System;

#endregion

namespace UltimatePlaylist.Games.Models.Raffle
{
    public class RaffleUserTicketReadServiceModel
    {
        public Guid UserExternalId { get; set; }

        public Guid UserTicketExternalId { get; set; }

        public string UserFriendlyTicketId { get; set; }
    }
}
