#region Usings

#endregion

namespace UltimatePlaylist.Services.Common.Models.Song
{
    public class SongReadServiceModel : BaseReadServiceModel
    {
        public string Artist { get; set; }

        public string Title { get; set; }

        public string Album { get; set; }

        public string PrimaryGenres { get; set; }

        public int TotalSongPlays { get; set; }

        public int TotalAddedToDSP { get; set; }

        public string CoverUrl { get; set; }
    }
}
