#region Usings

using System;
using UltimatePlaylist.Common.Enums;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Ticket
{
    public class AddTicketWriteServiceModel
    {
        public Guid ExternalId { get; set; }

        public Guid PlaylistExternalId { get; set; }

        public TicketEarnedType EarnedType { get; set; }

        public TicketType Type { get; set; }
    }
}
