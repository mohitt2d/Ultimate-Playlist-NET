#region Usings

using System;
using System.Collections.Generic;
using UltimatePlaylist.Services.Common.Models.Reward;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Ticket
{
    public class TicketsStatsReadServiceModel
    {
        public int TicketsAmountForTodayDrawing { get; set; }

        public int TicketsAmountForJackpotDrawing { get; set; }

        public DateTime PlaylistExpirationTimeStamp { get; set; }

        public DateTime NextDrawingTimeStamp { get; set; }

        public IList<ActiveDrawingRewardReadServiceModel> Rewards { get; set; }

        public IList<CollectedDrawingRewardReadServiceModel> CollectedRewards { get; set; }
    }
}
