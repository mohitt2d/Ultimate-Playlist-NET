#region Usings

using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Database.Infrastructure.Entities.Base;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Song
{
    public class SongDSPEntity : BaseEntity
    {
        #region Service

        public DspType DspType { get; set; }

        public string Url { get; set; }

        public long? SongId { get; set; }

        public string SongDspId { get; set; }

        #endregion

        #region Navigation properties

        public virtual SongEntity Song { get; set; }

        #endregion
    }
}
