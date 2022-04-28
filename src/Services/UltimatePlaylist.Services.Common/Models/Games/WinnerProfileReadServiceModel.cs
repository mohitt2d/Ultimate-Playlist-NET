namespace UltimatePlaylist.Services.Common.Models.Games
{
    public class WinnerProfileReadServiceModel
    {
        public Guid ExternalId { get; set; }

        public string WinnerFullName { get; set; }

        public string WinnerUsername { get; set; }

        public string WinnerAvatarUrl { get; set; }

        public decimal Amount { get; set; }
    }
}
