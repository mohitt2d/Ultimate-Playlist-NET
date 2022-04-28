#region Usings

using System;
using UltimatePlaylist.Common.Enums;

#endregion

namespace UltimatePlaylist.AdminApi.Models.Schedule
{
    public class PlaylistScheduleCalendarResponseModel
    {
        public Guid? PlayListExternalId { get;  set; }

        public DateTime PlaylistDate { get; set; }

        public PlaylistCompletionState State { get; set; }
    }
}
