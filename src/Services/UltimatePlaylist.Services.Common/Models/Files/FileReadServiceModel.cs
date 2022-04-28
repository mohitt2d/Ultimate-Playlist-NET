#region Usings

using UltimatePlaylist.Common.Enums;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Files
{
    public class FileReadServiceModel : BaseReadServiceModel
    {
        public string FileName { get; set; }

        public string Container { get; set; }

        public string Url { get; set; }

        public string Extension { get; set; }

        public FileType Type { get; set; }
    }
}
