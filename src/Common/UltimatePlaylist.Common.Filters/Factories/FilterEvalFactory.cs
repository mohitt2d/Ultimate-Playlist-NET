#region Usings

using System.Collections.Generic;
using UltimatePlaylist.Common.Filters.Enums;
using UltimatePlaylist.Common.Filters.Eval;
using UltimatePlaylist.Common.Filters.Eval.Interfaces;

#endregion

namespace UltimatePlaylist.Common.Filters.Factories
{
    public class FilterEvalFactory
    {
        #region Private Members

        private static IDictionary<FilterType, BaseFilterEval> FilterEvals =
            new Dictionary<FilterType, BaseFilterEval>()
            {
                { FilterType.Value, new ValueFilterEval() },
                { FilterType.Quantity, new QuantityFilterEval() },
                { FilterType.Enum, new EnumFilterEval() },
            };

        #endregion

        #region Public Properties

        public static IFilterEval<TFilter> GetFilterEval<TFilter>(FilterType filterType)
            where TFilter : FilterBase
        {
            return (IFilterEval<TFilter>)FilterEvals[filterType];
        }

        #endregion
    }
}
