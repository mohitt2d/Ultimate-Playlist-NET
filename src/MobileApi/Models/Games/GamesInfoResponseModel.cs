namespace UltimatePlaylist.MobileApi.Models.Games
{
    public class GamesInfoResponseModel
    {
        public DateTime NextUltimateDate { get; set; }

        public decimal NextUltimatePrize { get; set; }

        public DateTime NextDrawingDate { get; set; }

        public int TicketsCount { get; set; }

        public IList<UserWinningResponseModel> UnclaimedWinnings { get; set; }

        public bool IsUnclaimed { get; set; }
    }
}
