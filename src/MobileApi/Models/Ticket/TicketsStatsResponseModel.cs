#region Usings

using System;
using System.Collections.Generic;
using UltimatePlaylist.Common.Mvc.Models;
using UltimatePlaylist.MobileApi.Models.Reward;

#endregion

namespace UltimatePlaylist.MobileApi.Models.Ticket
{
    public class TicketsStatsResponseModel
    {
        public int TicketsAmountForTodayDrawing { get; set; }

        public int TicketsAmountForJackpotDrawing { get; set; }

        public DateTime PlaylistExpirationTimeStamp { get; set; }

        public DateTime NextDrawingTimeStamp { get; set; }

        public IList<ActiveDrawingRewardResponseModel> Rewards { get; set; }

        public IList<CollectedDrawingRewardResponseModel> CollectedRewards { get; set; }
    }
}
