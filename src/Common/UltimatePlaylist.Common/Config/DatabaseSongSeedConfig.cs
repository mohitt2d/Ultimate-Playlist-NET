#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace UltimatePlaylist.Common.Config
{
    public class DatabaseSongSeedConfig
    {
        public Guid ExternalId { get; set; }

        public string StreamingUrl { get; set; }

        public string Title { get; set; }

        public string Artist { get; set; }

        public string AppleMusicUrl { get; set; }

        public string SpotifyUrl { get; set; }

        public string CoverUrl { get; set; }

        public string SpotifyId { get; set; }

        public string AppleMusicId { get; set; }

        public DateTime FirstRelaseDate { get; set; }

        public TimeSpan Duration { get; set; }
    }
}
