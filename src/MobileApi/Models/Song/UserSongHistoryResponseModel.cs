#region Usings

using System;

#endregion

namespace UltimatePlaylist.MobileApi.Models.Song
{
    public class UserSongHistoryResponseModel
    {
        public Guid ExternalId { get; set; }

        public string Artist { get; set; }

        public string Title { get; set; }

        public TimeSpan Duration { get; set; }

        public bool IsAddedToSpotify { get; set; }

        public bool IsAddedToAppleMusic { get; set; }

        public string CoverFileUrl { get; set; }
    }
}
