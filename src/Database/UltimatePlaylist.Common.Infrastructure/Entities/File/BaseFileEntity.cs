#region Usings

using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Database.Infrastructure.Entities.Base;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.File
{
    public abstract class BaseFileEntity : BaseEntity
    {
        #region Service

        public string Container { get; set; }

        public string Extension { get; set; }

        public string FileName { get; set; }

        public string Url { get; set; }

        public FileType Type { get; protected set; }

        #endregion
    }
}
