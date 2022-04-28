#region Usings

using UltimatePlaylist.Common.Filters.Enums.Conditions;

#endregion

namespace UltimatePlaylist.Common.Filters.Models
{
    public class ValueFilter : FilterBase
    {
        public ValueFilterCondition Condition { get; set; }

        public string Value { get; set; }
    }
}