#region Usings

using UltimatePlaylist.Common.Filters.Enums.Conditions;

#endregion

namespace UltimatePlaylist.Common.Filters.Models
{
    public class EnumFilter : FilterBase
    {
        #region Public Properties

        public EnumFilterCondition Condition { get; set; }

        public string Value { get; set; }

        #endregion
    }
}