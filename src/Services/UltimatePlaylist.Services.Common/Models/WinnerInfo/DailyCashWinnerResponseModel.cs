namespace UltimatePlaylist.Services.Common.Models.Games
{
    public class DailyCashWinnerResponseModel
    {
        public string WinnerFullName { get; set; }

        public string WinnerUsername { get; set; }

        public string WinnerAvatarUrl { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }
    }
}