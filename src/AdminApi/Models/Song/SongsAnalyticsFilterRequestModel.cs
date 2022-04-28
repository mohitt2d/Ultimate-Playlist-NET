namespace UltimatePlaylist.AdminApi.Models.Song
{
    public class SongsAnalyticsFilterRequestModel
    {
        public DateTime? TimeRange { get; set; }

        public List<string> Genders { get; set; }

        public string ZipCode { get; set; }

        public List<AgeRequestModel> Age { get; set; }

        public List<string> MusicGenres { get; set; }

        public string Licensor { get; set; }
    }
}
