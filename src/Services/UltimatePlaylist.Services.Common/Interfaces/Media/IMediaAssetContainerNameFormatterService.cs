#region Usings

using System;
using UltimatePlaylist.Common.Enums;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Media
{
    public interface IMediaAssetContainerNameFormatterService
    {
        string Format(MediaAssetType mediaAssetType, DateTime timestamp, Guid guid);
    }
}
