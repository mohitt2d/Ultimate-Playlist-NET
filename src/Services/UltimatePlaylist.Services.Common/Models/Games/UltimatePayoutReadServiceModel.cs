namespace UltimatePlaylist.Services.Common.Models.Games
{
    public class UltimatePayoutReadServiceModel
    {
        public decimal NextUltimatePrize { get; set; }

        public int NextUltimateDate { get; set; }

        public int TicketsCount { get; set; }

        public UltimatePayoutWinnerReadServiceModel UltimatePayoutWinner { get; set; }

        public int[] UltimatePayoutWinningNumbers { get; set; }

        public IList<int[]> UltimatePayoutUserNumbers { get; set; }

        public IList<int[]> UltimatePayoutYesterdayChosenUserNumbers { get; set; }
    }
}
