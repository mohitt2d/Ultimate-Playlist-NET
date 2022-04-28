#region Usings

using UltimatePlaylist.Services.Common.Interfaces.AppleMusic;

#endregion

namespace UltimatePlaylist.Services.Common.Models.AppleMusic.Responses
{
    public class AppleMusicPlaylistDataResponseModel : IAppleResource
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public string Href { get; set; }

        public AppleMusicPlaylistAttributesResponseModel Attributes { get; set; }
    }
}
