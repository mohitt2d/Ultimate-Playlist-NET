namespace UltimatePlaylist.Services.Common.Models.Spotify.Request
{
    public class SpotifyAuthorizeCodeRequestModel
    {
        public string GrantType { get; set; }

        public string Code { get; set; }

        public string RedirectUri { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }
    }
}
