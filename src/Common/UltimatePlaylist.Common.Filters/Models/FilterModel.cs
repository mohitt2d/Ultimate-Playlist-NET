#region Usings

using System.Collections.Generic;

#endregion

namespace UltimatePlaylist.Common.Filters.Models
{
    public class FilterModel
    {
        #region Constructor(s)

        public FilterModel()
        {
            ValueFilters = new List<ValueFilter>();
            QuantityFilters = new List<QuantityFilter>();
            EnumFilters = new List<EnumFilter>();
        }

        #endregion

        #region Public Properties

        public IEnumerable<ValueFilter> ValueFilters { get; set; }

        public IEnumerable<QuantityFilter> QuantityFilters { get; set; }

        public IEnumerable<EnumFilter> EnumFilters { get; set; }

        #endregion
    }
}
