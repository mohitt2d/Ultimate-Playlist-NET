#region Usings

using System.Collections.Generic;

#endregion

namespace UltimatePlaylist.Services.Common.Models.AppleMusic.Attributes
{
    public class AppleStroefrontAttributes
    {
        public string DefaultLanguageTag { get; set; }

        public string Name { get; set; }

        public List<string> SupportedLanguageTags { get; set; }
    }
}
