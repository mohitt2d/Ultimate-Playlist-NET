﻿namespace UltimatePlaylist.Services.Common.Models.Games
{
    public class UltimatePayoutWinnerReadServiceModel
    {
        public bool IsCurrentUser { get; set; }

        public Guid ExternalId { get; set; }

        public string WinnerFullName { get; set; }

        public string WinnerUsername { get; set; }

        public string WinnerAvatarUrl { get; set; }

        public decimal Amount { get; set; }
    }
}
