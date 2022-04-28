#region Usings

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace UltimatePlaylist.Common.Filters.Eval
{
    public abstract class BaseFilterEval
    {
        #region Protected Methods

        protected ParameterExpression Parameter<T>()
            => Expression.Parameter(typeof(T));

        protected MemberExpression Property(ParameterExpression parameter, string name)
            => GetProperty(parameter, name);

        protected ConstantExpression Constant(object value)
            => Expression.Constant(value);

        protected UnaryExpression ConvertForType(Expression expression, Type type)
            => Expression.Convert(expression, type);

        protected BinaryExpression Binary(ExpressionType type, Expression left, Expression right)
            => Expression.MakeBinary(type, left, right);

        protected MethodCallExpression Call<T>(Expression expression, string methodName, params Expression[] arguments)
            => Expression.Call(expression, Method<T>(methodName), arguments);

        protected MethodInfo Method<T>(string name)
            => typeof(T).GetMethod(name, new[] { typeof(T) });

        protected MethodInfo MethodEmpty<T>(string name)
            => typeof(T).GetMethod(name, Type.EmptyTypes);

        protected MethodCallExpression ToLower(Expression expression)
            => Expression.Call(expression, MethodEmpty<string>("ToLower"));

        protected Expression<Func<T, bool>> Lambda<T>(Expression expression, params ParameterExpression[] parameters)
            => Expression.Lambda<Func<T, bool>>(expression, parameters);

        #endregion

        #region Private Methods

        private MemberExpression GetProperty(ParameterExpression parameterExpression, string name)
        {
            var path = name.Split('.');
            var result = Expression.Property(parameterExpression, path[0]);

            return path.Skip(1).Aggregate(result, Expression.Property);
        }

        #endregion
    }
}
