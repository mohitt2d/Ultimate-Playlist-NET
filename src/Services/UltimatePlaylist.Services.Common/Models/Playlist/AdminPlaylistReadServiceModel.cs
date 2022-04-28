#region Usings

using System.Collections.Generic;
using UltimatePlaylist.Services.Common.Models.Song;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Playlist
{
    public class AdminPlaylistReadServiceModel : BaseReadServiceModel
    {
        public List<SongReadServiceModel> Songs { get; set; }
    }
}
