#region Usings

using System;
using System.Linq.Expressions;
using UltimatePlaylist.Common.Filters.Enums.Conditions;
using UltimatePlaylist.Common.Filters.Eval.Interfaces;
using UltimatePlaylist.Common.Filters.Models;

#endregion

namespace UltimatePlaylist.Common.Filters.Eval
{
    public class ValueFilterEval : BaseFilterEval, IFilterEval<ValueFilter>
    {
        #region Eval

        public Expression<Func<T, bool>> Eval<T>(ValueFilter filter)
        {
            var condition = filter.Condition;
            var conditionName = condition == ValueFilterCondition.DoesNotContain
                ? ValueFilterCondition.Contains.ToString()
                : condition.ToString();

            var parameter = Parameter<T>();
            var property = Property(parameter, filter.FieldName);
            var value = Constant(filter.Value.ToLower());

            var methodExpression = Call<string>(ToLower(property), conditionName, value);
            var finalExpression = condition == ValueFilterCondition.DoesNotContain
                ? Expression.Not(methodExpression)
                : (Expression)methodExpression;

            return Lambda<T>(finalExpression, parameter);
        }

        #endregion
    }
}
