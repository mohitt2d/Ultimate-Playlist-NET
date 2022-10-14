#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimatePlaylist.Common.Enums;



namespace UltimatePlaylist.Services.Common.Models.Ticket
{
    public class UpdateTicketWriteServiceModel
    {
        public Guid ExternalId { get; set; }

        public int IsErrorTriggered { get; set; }

        public Guid PlaylistExternalId { get; set; }

        public TicketEarnedType EarnedType { get; set; }

        public TicketType Type { get; set; }
    }
}
#endregion