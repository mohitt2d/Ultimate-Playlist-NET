#region Usings

using System;

#endregion

namespace UltimatePlaylist.MobileApi.Models.Dsp.Spotify
{
    public class SpotifyAuthorizationWithTokensRequestModel
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public DateTime AccessTokenExpirationDate { get; set; }
    }
}
