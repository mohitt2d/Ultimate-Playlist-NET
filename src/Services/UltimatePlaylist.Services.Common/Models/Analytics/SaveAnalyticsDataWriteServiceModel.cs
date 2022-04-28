#region Usings

using System;
using UltimatePlaylist.Common.Enums;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Analytics
{
    public class SaveAnalyticsDataWriteServiceModel
    {
        public AnalitycsEventType EventType { get; set; }

        public Guid PlaylistExternalId { get; set; }

        public Guid SongExternalId { get; set; }

        public int? ActualListeningSecond { get; set; }
    }
}
