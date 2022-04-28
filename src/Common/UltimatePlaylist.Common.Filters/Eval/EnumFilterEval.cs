#region Usings

using System;
using System.Linq.Expressions;
using System.Reflection;
using UltimatePlaylist.Common.Filters.Eval.Interfaces;
using UltimatePlaylist.Common.Filters.Extensions;
using UltimatePlaylist.Common.Filters.Models;

#endregion

namespace UltimatePlaylist.Common.Filters.Eval
{
    public class EnumFilterEval : BaseFilterEval, IFilterEval<EnumFilter>
    {
        #region Eval

        public Expression<Func<T, bool>> Eval<T>(EnumFilter filter)
        {
            var parameter = Parameter<T>();
            var property = Property(parameter, filter.FieldName);

            var propertyType = ((PropertyInfo)property.Member).PropertyType;
            var value = Constant(Enum.Parse(propertyType, filter.Value));

            var expressionType = filter.Condition.ToExpressionType();
            var binaryExpression = Binary(expressionType, property, value);

            return Lambda<T>(binaryExpression, parameter);
        }

        #endregion
    }
}
