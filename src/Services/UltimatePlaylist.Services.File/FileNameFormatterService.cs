#region Usings

using System;
using System.IO;
using UltimatePlaylist.Common.Extensions;
using UltimatePlaylist.Services.Common.Interfaces.Files;

#endregion

namespace UltimatePlaylist.Services.File
{
    public class FileNameFormatterService : IFileNameFormatterService
    {
        #region Public methods

        public string Format(string fileName)
        {
            return Format(fileName, DateTime.UtcNow);
        }

        public string Format(string fileName, DateTime timestamp)
        {
            return Format(fileName, DateTime.UtcNow, Guid.NewGuid());
        }

        public string Format(string fileName, DateTime timestamp, Guid guid)
        {
            return $"{timestamp.ToUnixTimestamp()}_{guid}{Path.GetExtension(fileName)}";
        }

        #endregion
    }
}
