#region Usings

using System;
using System.IO;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Common.Extensions;
using UltimatePlaylist.Services.Common.Interfaces.Media;

#endregion

namespace UltimatePlaylist.Services.Media
{
    public class MediaAssetNameFormatterService : IMediaAssetNameFormatterService
    {
        #region Public methods

        public string Format(string assetName, MediaAssetType mediaAssetType)
        {
            return Format(assetName, mediaAssetType, DateTime.UtcNow);
        }

        public string Format(string assetName, MediaAssetType mediaAssetType, DateTime timestamp)
        {
            return Format(assetName, mediaAssetType, DateTime.UtcNow, Guid.NewGuid());
        }

        public string Format(string assetName, MediaAssetType mediaAssetType, DateTime timestamp, Guid guid)
        {
            return $"{timestamp.ToUnixTimestamp()}_{guid}_{mediaAssetType}{Path.GetExtension(assetName)}";
        }

        #endregion
    }
}
