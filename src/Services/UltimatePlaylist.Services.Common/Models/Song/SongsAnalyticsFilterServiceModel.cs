namespace UltimatePlaylist.Services.Common.Models.Song
{
    public class SongsAnalyticsFilterServiceModel
    {
        public SongsAnalyticsFilterServiceModel()
        {
            TimeRange = null;
            Genders = new List<string>();
            ZipCode = "";
            Age = new List<AgeServiceModel>();
            MusicGenres = new List<string>();
            Licensor = "";
        }

        public DateTime? TimeRange { get; set; }

        public List<string> Genders { get; set; }

        public string ZipCode { get; set; }

        public List<AgeServiceModel> Age { get; set; }

        public List<string> MusicGenres { get; set; }

        public string Licensor { get; set; }
    }
}
