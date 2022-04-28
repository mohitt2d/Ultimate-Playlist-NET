#region Usings

using System;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Analytics
{
    public class AnalyticsLastEarnedTicketsReadServiceModel
    {
        public int LatestEarnedTickets { get; set; }

        public bool IsAntiBotSystemActive { get; set; }

        public int SkippedSongsCount { get; set; }

        public bool IsSongSkippedSuccessfully { get; set; }

        public int SkippedSongsCountInLastHour { get; set; }

        public bool IsSkipLimitReached { get; set; }

        public bool CannotSkipSongTwice { get; set; }

        public DateTime? ExpirationOfSkipLimitTimestamp { get; set; }
    }
}
