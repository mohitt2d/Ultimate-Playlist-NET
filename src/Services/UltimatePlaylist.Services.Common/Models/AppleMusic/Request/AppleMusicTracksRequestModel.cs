#region Usings

using System.Collections.Generic;

#endregion

namespace UltimatePlaylist.Services.Common.Models.AppleMusic.Request
{
    public class AppleMusicTracksRequestModel
    {
        public List<AppleMusicSongRequestModel> Data { get; set; }
    }
}
