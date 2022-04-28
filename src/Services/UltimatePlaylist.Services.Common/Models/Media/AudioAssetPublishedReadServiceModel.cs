#region Usings

using System;
using System.Collections.Generic;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Media
{
    public class AudioAssetPublishedReadServiceModel
    {
        public AudioAssetPublishedReadServiceModel()
        {
            DownloadUris = new List<Uri>();
            StreamingUris = new List<Uri>();
        }

        public IList<Uri> DownloadUris { get; set; }

        public IList<Uri> StreamingUris { get; set; }
    }
}
