#region Usings

using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Database.Infrastructure.Entities.Base;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Song
{
    public class SongSocialMediaEntity : BaseEntity
    {
        #region Service

        public string Url { get; set; }

        public SocialMediaType Type { get; set; }

        public long? SongId { get; set; }

        #endregion

        #region Navigation properties

        public virtual SongEntity Song { get; set; }

        #endregion
    }
}
