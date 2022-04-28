#region Usings

using System;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Reward
{
    public class CollectedDrawingRewardReadServiceModel
    {
        public Guid ExternalId { get; set; }

        public string Name { get; set; }

        public decimal Value { get; set; }

        public DateTime CollectedDate { get; set; }
    }
}
