#region Usings

using System.Collections.Generic;
using UltimatePlaylist.AdminApi.Models.Song;
using UltimatePlaylist.Common.Mvc.Models;

#endregion

namespace UltimatePlaylist.AdminApi.Models.Schedule
{
    public class PlaylistsResponseModel : BaseResponseModel
    {
        public List<SongResponseModel> Songs { get; set; }
    }
}
