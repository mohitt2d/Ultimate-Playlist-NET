#region Usings

using System.Collections.Generic;
using System.Linq.Expressions;
using UltimatePlaylist.Common.Filters.Enums.Conditions;

#endregion

namespace UltimatePlaylist.Common.Filters.Extensions
{
    public static class ConditionToExpressionTypeExtensions
    {
        #region Private Members

        private static IDictionary<QuantityFilterCondition, ExpressionType> QuatityFiltersExpressions =>
            new Dictionary<QuantityFilterCondition, ExpressionType>()
            {
                { QuantityFilterCondition.DoesNotEqual, ExpressionType.NotEqual },
                { QuantityFilterCondition.Equals, ExpressionType.Equal },
                { QuantityFilterCondition.GreaterThan, ExpressionType.GreaterThan },
                { QuantityFilterCondition.GreaterThanOrEqual, ExpressionType.GreaterThanOrEqual },
                { QuantityFilterCondition.LessThan, ExpressionType.LessThan },
                { QuantityFilterCondition.LessThanOrEqual, ExpressionType.LessThanOrEqual },
            };

        private static IDictionary<EnumFilterCondition, ExpressionType> EnumFiltersExpressions =>
            new Dictionary<EnumFilterCondition, ExpressionType>()
            {
                        { EnumFilterCondition.DoesNotEqual, ExpressionType.NotEqual },
                        { EnumFilterCondition.Equals, ExpressionType.Equal },
            };

        #endregion

        #region Public Methods

        public static ExpressionType ToExpressionType(this QuantityFilterCondition condition)
        {
            return QuatityFiltersExpressions[condition];
        }

        public static ExpressionType ToExpressionType(this EnumFilterCondition condition)
        {
            return EnumFiltersExpressions[condition];
        }

        #endregion
    }
}
