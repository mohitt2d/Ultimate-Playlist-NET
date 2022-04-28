#region Usings

using UltimatePlaylist.Common.Enums;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Files
{
    public class AudioFileReadServiceModel : FileReadServiceModel
    {
        public string InputAssetName { get; set; }

        public string InputContainerName { get; set; }

        public MediaServicesJobErrorCode JobErrorCode { get; set; }

        public string JobErrorMessage { get; set; }

        public string JobName { get; set; }

        public MediaServicesJobState JobState { get; set; }

        public string OutputAssetName { get; set; }

        public string OutputContainerName { get; set; }

        public string StreamingUrl { get; set; }

        public string TransformName { get; set; }
    }
}
