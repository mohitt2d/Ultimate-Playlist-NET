#region Usings

using System;
using UltimatePlaylist.Common.Enums;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Media
{
    public class AudioAssetCreatedReadServiceModel
    {
        public Guid FileExternalId { get; set; }

        public string FileUrl { get; set; }

        public string InputAssetName { get; set; }

        public string InputContainerName { get; set; }

        public string JobName { get; set; }

        public MediaServicesJobState JobState { get; set; }

        public string OutputAssetName { get; set; }

        public string OutputContainerName { get; set; }

        public string TransformName { get; set; }
    }
}
