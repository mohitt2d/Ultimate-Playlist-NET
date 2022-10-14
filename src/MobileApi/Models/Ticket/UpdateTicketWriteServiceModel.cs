#region Usings

using System;
using UltimatePlaylist.Common.Enums;

#endregion
namespace UltimatePlaylist.MobileApi.Models.Ticket
{
    public class UpdateTicketWriteServiceModel
    {
        public Guid ExternalId { get; set; }

        public int IsErrorTriggered { get; set; }

        public Guid PlaylistExternalId { get; set; }

        public TicketEarnedType EarnedType { get; set; }

        public TicketType Type { get; set; }
    }
}//new2022-10-14-here
