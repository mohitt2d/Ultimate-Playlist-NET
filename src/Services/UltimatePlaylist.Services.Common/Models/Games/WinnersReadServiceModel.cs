namespace UltimatePlaylist.Services.Common.Models.Games
{
    public class WinnersReadServiceModel
    {
        public DateTime DateTimestamp { get; set; }

        public IList<WinnerProfileReadServiceModel> DailyCashDrawingsWinners { get; set; }

        public WinnerProfileReadServiceModel UltimatePayoutWinner { get; set; }

        public int[] UltimatePayoutWinningNumbers { get; set; }

        public IList<int[]> UltimatePayoutUserNumbers { get; set; }
    }
}
