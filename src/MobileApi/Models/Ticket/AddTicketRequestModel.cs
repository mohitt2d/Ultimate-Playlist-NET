#region Usings

using System;
using UltimatePlaylist.Common.Enums;

#endregion

namespace UltimatePlaylist.MobileApi.Models.Ticket
{
    public class AddTicketRequestModel
    {
        public Guid ExternalId { get; set; }

        public string EarnedType { get; set; }

        public string Type { get; set; }
    }
}
