namespace UltimatePlaylist.MobileApi.Models.Games
{
    public class WinnersResponseModel
    {
        public DateTime DateTimestamp { get; set; }

        public IList<WinnerProfileResponseModel> DailyCashDrawingsWinners { get; set; }

        public WinnerProfileResponseModel UltimatePayoutWinner { get; set; }

        public int[] UltimatePayoutWinningNumbers { get; set; }

        public IList<int[]> UltimatePayoutUserNumbers { get; set; }
    }
}
