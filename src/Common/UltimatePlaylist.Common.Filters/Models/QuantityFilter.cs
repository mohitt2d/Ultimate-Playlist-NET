#region Usings

using UltimatePlaylist.Common.Filters.Enums.Conditions;

#endregion

namespace UltimatePlaylist.Common.Filters.Models
{
    public class QuantityFilter : FilterBase
    {
        public QuantityFilterCondition Condition { get; set; }

        public double Value { get; set; }

        public bool? IsDate { get; set; }
    }
}