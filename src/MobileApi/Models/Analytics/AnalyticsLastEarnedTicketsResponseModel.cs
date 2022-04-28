#region Usings

using System;

#endregion

namespace UltimatePlaylist.MobileApi.Models.Analytics
{
    public class AnalyticsLastEarnedTicketsResponseModel
    {
        public int LatestEarnedTickets { get; set; }

        public bool IsAntiBotSystemActive { get; set; }

        public int SkippedSongsCount { get; set; }

        public bool IsSkipLimitReached { get; set; }

        public int SkippedSongsCountInLastHour { get; set; }

        public DateTime? ExpirationOfSkipLimitTimestamp { get; set; }
    }
}
