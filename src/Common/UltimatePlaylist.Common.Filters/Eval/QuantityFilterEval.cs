#region Usings

using System;
using System.Linq.Expressions;
using System.Reflection;
using UltimatePlaylist.Common.Extensions;
using UltimatePlaylist.Common.Filters.Eval.Interfaces;
using UltimatePlaylist.Common.Filters.Extensions;
using UltimatePlaylist.Common.Filters.Models;

#endregion

namespace UltimatePlaylist.Common.Filters.Eval
{
    public class QuantityFilterEval : BaseFilterEval, IFilterEval<QuantityFilter>
    {
        #region Eval

        public Expression<Func<T, bool>> Eval<T>(QuantityFilter filter)
        {
            var parameter = Parameter<T>();
            var property = Property(parameter, filter.FieldName);

            var propertyType = ((PropertyInfo)property.Member).PropertyType;
            var filterValue = ValueForMember(propertyType, filter.Value);
            var value = ConvertForType(filterValue, propertyType);

            var expressionType = filter.Condition.ToExpressionType();
            var binary = Binary(expressionType, property, value);

            return Lambda<T>(binary, parameter);
        }

        #endregion

        #region Private Methods

        private ConstantExpression ValueForMember(Type type, double value)
        {
            object convertedValue = value;

            if (type == typeof(int) || type == typeof(int?))
            {
                convertedValue = Convert.ToInt32(value);
            }
            else if (type == typeof(long) || type == typeof(long?))
            {
                convertedValue = Convert.ToInt64(value);
            }
            else if (type == typeof(decimal) || type == typeof(decimal?))
            {
                convertedValue = Convert.ToDecimal(value);
            }
            else if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                convertedValue = Convert.ToInt64(value).FromUnixTimestamp();
            }

            return Constant(convertedValue);
        }

        #endregion
    }
}
