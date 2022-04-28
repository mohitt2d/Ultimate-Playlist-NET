namespace UltimatePlaylist.Services.Common.Models.Song
{
    public class SkipSongReadServiceModel
    {
        public int SkippedSongsCount { get; set; }

        public bool IsSkipLimitReached { get; set; }

        public bool IsSongSkippedSuccessfully { get; set; }

        public int SkippedSongsCountInLastHour { get; set; }

        public bool CannotSkipSongTwice { get; set; }

        public DateTime? ExpirationOfSkipLimitTimestamp { get; set; }
    }
}
