#region Usings

using System.Collections.Generic;
using UltimatePlaylist.Services.Common.Interfaces.AppleMusic;

#endregion

namespace UltimatePlaylist.Services.Common.Models.AppleMusic.Responses
{
    public class AppleDataResponseRoot<TResource> : AppleResponseRoot
        where TResource : IAppleResource
    {
        public List<TResource> Data { get; set; }
    }
}
