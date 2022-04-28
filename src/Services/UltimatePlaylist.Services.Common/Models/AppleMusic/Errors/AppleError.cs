namespace UltimatePlaylist.Services.Common.Models.AppleMusic.Errors
{
    public class AppleError
    {
        public string Code { get; set; }

        public string Detail { get; set; }

        public string Id { get; set; }

        public AppleErrorSource Source { get; set; }

        public string Status { get; set; }

        public string Title { get; set; }
    }
}
