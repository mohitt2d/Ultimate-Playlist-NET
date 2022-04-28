#region Usings

using System;

#endregion

namespace UltimatePlaylist.Services.Common.Models.AppleMusic
{
    public class AddSongToAppleMusicWriteServiceModel
    {
        public Guid SongExternalId { get; set; }

        public Guid PlaylistExternalId { get; set; }
    }
}
