namespace UltimatePlaylist.MobileApi.Models.Games
{
    public class UltimatePayoutWinnerResponseModel
    {
        public bool IsCurrentUser { get; set; }

        public Guid ExternalId { get; set; }

        public string WinnerFullName { get; set; }

        public string WinnerUsername { get; set; }

        public string WinnerAvatarUrl { get; set; }

        public string Amount { get; set; }
    }
}
