#region Usings

using System.Collections.Generic;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Schedule
{
    public class PlaylistsScheduleCalendarInfoReadServiceModel
    {
        public IList<PlaylistScheduleCalendarReadServiceModel> PlaylistsCalendarInfo { get; set; }
    }
}
