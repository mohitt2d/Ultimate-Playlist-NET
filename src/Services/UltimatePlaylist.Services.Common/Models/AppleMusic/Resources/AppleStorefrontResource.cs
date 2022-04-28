#region Usings

using UltimatePlaylist.Services.Common.Models.AppleMusic.Attributes;

#endregion

namespace UltimatePlaylist.Services.Common.Models.AppleMusic.Resources
{
    public class AppleStorefrontResource : AppleResourceRoot
    {
        public override string Type => "storefronts";

        public AppleStroefrontAttributes Attributes { get; set; }
    }
}
