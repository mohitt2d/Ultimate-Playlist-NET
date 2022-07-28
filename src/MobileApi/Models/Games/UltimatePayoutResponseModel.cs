namespace UltimatePlaylist.MobileApi.Models.Games
{
    public class UltimatePayoutResponseModel
    {
        public string NextUltimatePrize { get; set; }

        public int NextUltimateDate { get; set; }

        public int TicketsCount { get; set; }

        public UltimatePayoutWinnerResponseModel UltimatePayoutWinner { get; set; }

        public int[] UltimatePayoutWinningNumbers { get; set; }

        public IList<int[]> UltimatePayoutUserNumbers { get; set; }

        public IList<int[]> UltimatePayoutYesterdayChosenUserNumbers { get; set; }
    }
}
