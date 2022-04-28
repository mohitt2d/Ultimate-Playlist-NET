#region Usings

using System;
using UltimatePlaylist.Common.Enums;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Schedule
{
    public class PlaylistScheduleCalendarReadServiceModel
    {
        public Guid? PlayListExternalId { get; set; }

        public DateTime PlaylistDate { get; set; }

        public PlaylistCompletionState State { get; set; }
    }
}
