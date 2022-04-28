#region Usings

using UltimatePlaylist.Common.Models;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Playlist
{
    public class PlaylistHistroryPaginatedReadServiceModel : PaginatedReadServiceModel<PlaylistHistoryReadServiceModel>
    {
        public DateTime NextPlaylistAvailable { get; set; }

        public PlaylistHistroryPaginatedReadServiceModel(
         IReadOnlyList<PlaylistHistoryReadServiceModel> items,
         Pagination pagination,
         int totalCount)
         : base(items, pagination, totalCount)
        {
        }
    }
}
