#region Usings

using System;

#endregion

namespace UltimatePlaylist.MobileApi.Models.Dsp.AppleMusic
{
    public class AddSongToAppleMusicRequestModel
    {
        public Guid? SongExternalId { get; set; }

        public Guid? PlaylistExternalId { get; set; }
    }
}
