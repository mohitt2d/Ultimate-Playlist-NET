#region Usings

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using UltimatePlaylist.Common.Filters.Enums.Conditions;

#endregion

namespace UltimatePlaylist.Common.Filters.Extensions
{
    public static class ConditionToFuncExtensions
    {
        #region Private Members

        private static IDictionary<QuantityFilterCondition, Func<Expression, Expression, BinaryExpression>> QuatityFiltersExpressions =>
            new Dictionary<QuantityFilterCondition, Func<Expression, Expression, BinaryExpression>>()
            {
                { QuantityFilterCondition.DoesNotEqual, Expression.NotEqual },
                { QuantityFilterCondition.Equals, Expression.Equal },
                { QuantityFilterCondition.GreaterThan, Expression.GreaterThan },
                { QuantityFilterCondition.GreaterThanOrEqual, Expression.GreaterThanOrEqual },
                { QuantityFilterCondition.LessThan, Expression.LessThan },
                { QuantityFilterCondition.LessThanOrEqual, Expression.LessThanOrEqual },
            };

        private static IDictionary<EnumFilterCondition, Func<Expression, Expression, BinaryExpression>> EnumFiltersExpressions =>
            new Dictionary<EnumFilterCondition, Func<Expression, Expression, BinaryExpression>>()
            {
                        { EnumFilterCondition.DoesNotEqual, Expression.NotEqual },
                        { EnumFilterCondition.Equals, Expression.Equal },
            };

        #endregion

        #region Public Methods

        public static Func<Expression, Expression, BinaryExpression> ToExpression(this QuantityFilterCondition condition)
        {
            return QuatityFiltersExpressions[condition];
        }

        public static Func<Expression, Expression, BinaryExpression> ToExpression(this EnumFilterCondition condition)
        {
            return EnumFiltersExpressions[condition];
        }

        #endregion
    }
}
