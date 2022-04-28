#region Usings

using System.Collections.Generic;

#endregion

namespace UltimatePlaylist.AdminApi.Models.Schedule
{
    public class PlaylistsScheduleCalendarInfoResponseModel
    {
        public IList<PlaylistScheduleCalendarResponseModel> PlaylistsCalendarInfo { get; set; }
    }
}
