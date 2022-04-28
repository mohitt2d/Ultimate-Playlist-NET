#region Usings

#endregion

namespace UltimatePlaylist.Common.Config
{
    public class AuthConfig
    {
        public JWTConfig JWT { get; set; }

        public string AppleWellKnown { get; set; }
    }
}
