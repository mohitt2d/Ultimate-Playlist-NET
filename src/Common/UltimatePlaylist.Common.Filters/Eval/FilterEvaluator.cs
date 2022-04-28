#region Usings

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using UltimatePlaylist.Common.Filters.Enums;
using UltimatePlaylist.Common.Filters.Factories;
using UltimatePlaylist.Common.Filters.Models;

#endregion

namespace UltimatePlaylist.Common.Filters.Eval
{
    public class FilterEvaluator<T>
    {
        #region Public Methods

        public static Expression<Func<T, bool>> Evaluate(QuantityFilter filter)
            => Evaluate(filter, FilterType.Quantity);

        public static Expression<Func<T, bool>> Evaluate(ValueFilter filter)
            => Evaluate(filter, FilterType.Value);

        public static Expression<Func<T, bool>> Evaluate(EnumFilter filter)
            => Evaluate(filter, FilterType.Enum);

        public static void ApplyMappings<TFilter>(TFilter filter, IDictionary<string, string> fieldNameMappings)
            where TFilter : FilterBase
        {
            if (fieldNameMappings.ContainsKey(filter.FieldName))
            {
                filter.FieldName = fieldNameMappings[filter.FieldName];
            }
        }

        #endregion

        #region Private Methods

        private static Expression<Func<T, bool>> Evaluate<TFilter>(TFilter filter, FilterType filterType)
            where TFilter : FilterBase
        {
            return FilterEvalFactory
                .GetFilterEval<TFilter>(filterType)
                .Eval<T>(filter);
        }

        #endregion
    }
}