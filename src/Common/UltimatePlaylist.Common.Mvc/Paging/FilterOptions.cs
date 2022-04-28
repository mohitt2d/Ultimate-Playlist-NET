#region Usings

using System.Collections.Generic;
using UltimatePlaylist.Common.Filters.Enums;

#endregion

namespace UltimatePlaylist.Common.Mvc.Paging
{
    public class FilterOptions
    {
        #region Constructor(s)

        public FilterOptions(string label, string propertyName)
        {
            Label = label;
            Name = propertyName;
        }

        #endregion

        #region Public Properties

        public FilterKind Kind { get; set; }

        public string Name { get; set; }

        public string Label { get; set; }

        public IEnumerable<object> PossibleValues { get; set; }

        #endregion
    }
}
