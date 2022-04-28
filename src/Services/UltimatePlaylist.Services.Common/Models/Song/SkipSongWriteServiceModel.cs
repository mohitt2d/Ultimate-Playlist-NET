#region Usings

using System;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Song
{
    public class SkipSongWriteServiceModel
    {
        public Guid UserExternalId { get; set; }

        public Guid PlaylistExternalId { get; set; }

        public Guid SongExternalId { get; set; }

        public int? ActualListeningSecond { get; set; }
    }
}
