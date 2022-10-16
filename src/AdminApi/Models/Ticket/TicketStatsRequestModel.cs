namespace UltimatePlaylist.AdminApi.Models.Ticket
{
    public class TicketStatsRequestModel
    {
        public Guid ExternalId { get; set; }

        public int IsErrorTriggered { get; set; }
    }

    public class TicketsStatsRequestModel
    {
        public string UserPlaylistSongId { get; set; }

        public int IsErrorTriggered { get; set; }
    }

    public class UserTicketStatsRequestModel
    {
        public int? IsErrorTriggered { get; set; }
        public long UserPlaylistSongId { get; set; }
    }
}
