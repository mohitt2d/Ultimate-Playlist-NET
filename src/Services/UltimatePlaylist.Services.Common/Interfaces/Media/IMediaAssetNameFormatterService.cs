#region Usings

using System;
using UltimatePlaylist.Common.Enums;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Media
{
    public interface IMediaAssetNameFormatterService
    {
        string Format(string assetName, MediaAssetType mediaAssetType);

        string Format(string assetName, MediaAssetType mediaAssetType, DateTime timestamp);

        string Format(string assetName, MediaAssetType mediaAssetType, DateTime timestamp, Guid guid);
    }
}
