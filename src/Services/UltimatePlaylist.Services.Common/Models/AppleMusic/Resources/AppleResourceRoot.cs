#region Usings

using UltimatePlaylist.Services.Common.Interfaces.AppleMusic;

#endregion

namespace UltimatePlaylist.Services.Common.Models.AppleMusic.Resources
{
    public abstract class AppleResourceRoot : IAppleResource
    {
        public string Href { get; set; }

        public string Id { get; set; }

        public abstract string Type { get; }

        public object Meta { get; set; }
    }
}
