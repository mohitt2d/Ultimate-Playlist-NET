#region Usings

using UltimatePlaylist.Common.Models;
using UltimatePlaylist.Common.Mvc.Paging;

#endregion

namespace UltimatePlaylist.MobileApi.Models.Playlist
{
    public class PlaylistHistroryPaginatedResponseModel : PaginatedResponse<PlaylistHistoryResponseModel>
    {
        public PlaylistHistroryPaginatedResponseModel(IList<PlaylistHistoryResponseModel> items, int count, Pagination pagination, DateTime nextPlaylistAvailable , int? customCount = null)
            : base(items, count, pagination, customCount)
        {
            NextPlaylistAvailable = nextPlaylistAvailable;
        }

        public DateTime NextPlaylistAvailable { get; set; }
    }
}
