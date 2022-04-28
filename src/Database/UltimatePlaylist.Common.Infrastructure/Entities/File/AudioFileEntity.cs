#region Usings

using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Database.Infrastructure.Entities.Song;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.File
{
    public class AudioFileEntity : BaseFileEntity
    {
        #region Service

        public string InputAssetName { get; set; }

        public string InputContainerName { get; set; }

        public MediaServicesJobErrorCode? JobErrorCode { get; set; }

        public string JobErrorMessage { get; set; }

        public string JobName { get; set; }

        public MediaServicesJobState JobState { get; set; }

        public string OutputAssetName { get; set; }

        public string OutputContainerName { get; set; }

        public string StreamingUrl { get; set; }

        public string TransformName { get; set; }

        public long? SongId { get; set; }

        #endregion

        #region Navigation Properties

        public virtual SongEntity Song { get; set; }

        #endregion
    }
}
