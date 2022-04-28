namespace UltimatePlaylist.Services.Common.Models.Spotify.Request
{
    public class CreatePlaylistRequestModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool Public { get; set; }
    }
}
