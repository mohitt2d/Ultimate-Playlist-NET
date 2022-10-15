#region Usings

using System;
using UltimatePlaylist.Common.Enums;

#endregion
namespace UltimatePlaylist.MobileApi.Models.Ticket
{
    public class TicketStatsRequestModel
    {
        public Guid ExternalId { get; set; }

        public int IsErrorTriggered { get; set; }
    }

    public class TicketsStatsRequestModel
    {
        public long UserPlaylistSongId { get; set; }

        public int IsErrorTriggered { get; set; }
    }

    public class UserTicketStatsRequestModel
    {
        public int? IsErrorTriggered { get; set; }
        public long UserPlaylistSongId { get; set; }
    }

}//new2022-10-14-here
