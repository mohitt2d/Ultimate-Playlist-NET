#region Usings

using System.Collections.Generic;
using UltimatePlaylist.Services.Common.Models.AppleMusic.Errors;

#endregion

namespace UltimatePlaylist.Services.Common.Models.AppleMusic.Responses
{
    public class AppleResponseRoot
    {
        public List<AppleError> Errors { get; set; }

        public string Href { get; set; }

        public object Meta { get; set; }

        public string Next { get; set; }
    }
}
