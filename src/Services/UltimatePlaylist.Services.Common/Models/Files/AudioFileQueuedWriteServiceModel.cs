#region Usings

using System;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Files
{
    public class AudioFileQueuedWriteServiceModel
    {
        public Guid ExternalId { get; set; }

        public string JobName { get; set; }

        public string OutputAssetName { get; set; }

        public string OutputContainerName { get; set; }

        public string TransformName { get; set; }
    }
}
