#region Usings

using System;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Spotify
{
    public class SpotifyAuthorizationReadServiceModel
    {
        public string AccessToken { get; set; }

        public string TokenType { get; set; }

        public string Scope { get; set; }

        public string RefreshToken { get; set; }

        public DateTime AccessTokenExpirationDate { get; set; }
    }
}
