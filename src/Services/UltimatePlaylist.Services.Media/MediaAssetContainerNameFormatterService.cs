#region Usings

using System;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Common.Extensions;
using UltimatePlaylist.Services.Common.Interfaces.Media;

#endregion

namespace UltimatePlaylist.Services.Media
{
    public class MediaAssetContainerNameFormatterService : IMediaAssetContainerNameFormatterService
    {
        #region Public methods

        public string Format(MediaAssetType mediaAssetType, DateTime timestamp, Guid guid)
        {
            return $"asset-{timestamp.ToUnixTimestamp()}-{guid}-{mediaAssetType}"
                .ToLower();
        }

        #endregion
    }
}
