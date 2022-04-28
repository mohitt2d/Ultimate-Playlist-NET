#region Usings

using System;
using System.Collections.Generic;
using UltimatePlaylist.Common.Mvc.Models;
using UltimatePlaylist.MobileApi.Models.Song;

#endregion

namespace UltimatePlaylist.MobileApi.Models.Playlist
{
    public class PlaylistHistoryResponseModel : BaseResponseModel
    {
        public string Date { get; set; }

        public DateTime DateTimeStamp { get; set; }

        public int SongCount { get; set; }

        public IList<UserSongHistoryResponseModel> Songs { get; set; }
    }
}
