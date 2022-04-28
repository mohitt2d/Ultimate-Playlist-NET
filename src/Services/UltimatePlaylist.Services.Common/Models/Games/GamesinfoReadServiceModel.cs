namespace UltimatePlaylist.Services.Common.Models.Games
{
    public class GamesinfoReadServiceModel
    {
        public DateTime NextUltimateDate { get; set; }

        public decimal NextUltimatePrize { get; set; }

        public DateTime NextDrawingDate { get; set; }

        public int TicketsCount { get; set; }

        public IList<UserWinningReadServicModel> UnclaimedWinnings { get; set; }

        public bool IsUnclaimed { get; set; }
    }
}
