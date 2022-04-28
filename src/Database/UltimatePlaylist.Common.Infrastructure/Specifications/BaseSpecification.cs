#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UltimatePlaylist.Common.Extensions;
using UltimatePlaylist.Common.Filters;
using UltimatePlaylist.Common.Filters.Enums.Conditions;
using UltimatePlaylist.Common.Filters.Eval;
using UltimatePlaylist.Common.Filters.Extensions;
using UltimatePlaylist.Common.Filters.Models;
using UltimatePlaylist.Common.Models;
using UltimatePlaylist.Database.Infrastructure.Specifications.Interfaces;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Specifications
{
    public abstract class BaseSpecification<T> : ISpecification<T>
        where T : class
    {
        #region Constructor(s)

        protected BaseSpecification()
        {
            IncludeStrings = new List<(string, bool)>();
        }

        #endregion

        #region Private Enum

        private enum Operator
        {
            And,
            Or,
        }

        #endregion

        #region Public Properties

        public Expression<Func<T, bool>> Criteria { get; private set; }

        public List<(Expression<Func<T, object>>, bool ignoreFilter)> Includes { get; } = new List<(Expression<Func<T, object>>, bool ignoreFilter)>();

        public List<(string, bool)> IncludeStrings { get; }

        public Expression<Func<T, object>> OrderBy { get; private set; }

        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPagingEnabled { get; private set; } = false;

        public bool IgnoreQueryFilters { get; set; }

        #endregion

        #region Private Properties

        private Expression<Func<T, bool>> OrCriteria { get; set; }

        private Expression<Func<T, bool>> AndCriteria { get; set; }

        #endregion

        #region Public Methods

        public virtual ISpecification<T> Apply(ICriteriaVisitor criteriaVisitor)
        {
            Criteria = (Expression<Func<T, bool>>)criteriaVisitor.VisitCriteria(Criteria);
            return this;
        }

        public BaseSpecification<T> Count()
        {
            BaseSpecification<T> shallowCopiedSpec = (BaseSpecification<T>)MemberwiseClone();

            shallowCopiedSpec.DetachPaging();

            return shallowCopiedSpec;
        }

        #endregion

        #region AddInclude

        protected virtual void AddInclude(Expression<Func<T, object>> includeExpression, bool ignoreFilter = false)
        {
            Includes.Add((includeExpression, ignoreFilter));
        }

        #endregion

        #region IncludeStrings

        protected virtual void AddInclude(string includeString, bool ignoreFilter = false)
        {
            IncludeStrings.Add((includeString, ignoreFilter));
        }

        #endregion

        #region ApplyPaging

        protected virtual void ApplyPaging(Pagination pagination)
        {
            if (pagination?.Skip != null && pagination.PageSize != null)
            {
                Skip = pagination.Skip.Value;
                Take = pagination.PageSize.Value;
                IsPagingEnabled = true;
            }
        }

        protected BaseSpecification<T> DetachPaging()
        {
            Skip = 0;
            Take = 0;
            IsPagingEnabled = false;

            return this;
        }

        #endregion

        #region ApplyOrderBy

        protected virtual void ApplyOrderBy(Expression<Func<T, object>> orderByExpression, bool descending = false)
        {
            if (descending)
            {
                OrderByDescending = orderByExpression;
            }
            else
            {
                OrderBy = orderByExpression;
            }
        }

        #endregion

        #region ApplyOrderByDescending

        protected virtual void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
        }

        #endregion

        #region AddCriteria

        protected virtual void AddCriteria(Expression<Func<T, bool>> newCriteria)
        {
            Criteria = Criteria == null ? newCriteria : AddOperator(Criteria, newCriteria, Operator.And);
        }

        protected virtual void AddToAnd(Expression<Func<T, bool>> newCriteria)
        {
            AndCriteria = AddOperator(AndCriteria, newCriteria, Operator.And);
        }

        protected virtual void AddToOrResetAnd()
        {
            OrCriteria = AddOperator(OrCriteria, AndCriteria, Operator.Or);
            AndCriteria = null;
        }

        protected virtual void AddToCriteriaResetOr()
        {
            Criteria = AddOperator(Criteria, OrCriteria, Operator.And);
            OrCriteria = null;
            AndCriteria = null;
        }

        #endregion

        #region ApplyFilters

        protected virtual void ApplyFilters(IEnumerable<FilterModel> filters, Action<FilterModel> customFilters = null, IDictionary<string, string> fieldNameMappings = null, List<string> skipFields = null)
        {
            foreach (var filter in filters)
            {
                AddFilterToAnd(filter.QuantityFilters, fieldNameMappings, skipFields);
                AddFilterToAnd(filter.ValueFilters, fieldNameMappings, skipFields);
                AddFilterToAnd(filter.EnumFilters, fieldNameMappings, skipFields);

                customFilters?.Invoke(filter);

                AddToOrResetAnd();
            }

            AddToCriteriaResetOr();
        }

        #endregion

        #region AddFilter

        protected void AddDateTimePropertyQuantityFilter(QuantityFilter quantityFilter, Expression<Func<T, DateTime>> propertySelector, DateTime? customComparedValue = null)
        {
            DateTime comparedValue = customComparedValue ?? ((long)quantityFilter.Value).FromUnixTimestamp();
            AddPropertyQuantityFilter(quantityFilter, propertySelector, comparedValue);
        }

        protected void AddLongPropertyQuantityFilter(QuantityFilter quantityFilter, Expression<Func<T, long>> propertySelector, long? customComparedValue = null)
        {
            long comparedValue;
            if (customComparedValue.HasValue)
            {
                comparedValue = customComparedValue.Value;
            }
            else
            {
                long.TryParse(quantityFilter.Value.ToString().Split()[0], out comparedValue);
            }

            AddPropertyQuantityFilter(quantityFilter, propertySelector, comparedValue);
        }

        protected void AddDecimalPropertyQuantityFilter(QuantityFilter quantityFilter, Expression<Func<T, decimal>> propertySelector, decimal? customComparedValue = null)
        {
            decimal comparedValue;
            if (customComparedValue.HasValue)
            {
                comparedValue = customComparedValue.Value;
            }
            else
            {
                decimal.TryParse(quantityFilter.Value.ToString().Split()[0], out comparedValue);
            }

            AddPropertyQuantityFilter(quantityFilter, propertySelector, comparedValue);
        }

        protected void AddPropertyQuantityFilter<TValue>(QuantityFilter quantityFilter, Expression<Func<T, TValue>> propertySelector, TValue comparedValue)
        {
            UnaryExpression compareValueExpression = WrapConstantExpression(comparedValue);

            var compareExpression = quantityFilter.Condition.ToExpression();

            if (compareExpression != null)
            {
                AddToAnd(Expression.Lambda<Func<T, bool>>(
                                compareExpression(propertySelector.Body, compareValueExpression),
                                propertySelector.Parameters[0]));
            }
        }

        protected void AddStringValueFilter(ValueFilter valueFilter, Expression<Func<T, string>> propertySelector)
        {
            string comparedValue = valueFilter.Value.ToLower();

            UnaryExpression compareValueExpression = WrapConstantExpression(comparedValue);

            Expression leftSideExpresion;

            switch (valueFilter.Condition)
            {
                case ValueFilterCondition.Contains:
                    leftSideExpresion = Expression.Call(
                        propertySelector.Body,
                        typeof(string).GetMethod(nameof(string.Contains), new Type[] { typeof(string) }),
                        compareValueExpression);
                    break;
                case ValueFilterCondition.DoesNotContain:
                    leftSideExpresion = Expression.Not(Expression.Call(
                        propertySelector.Body,
                        typeof(string).GetMethod(nameof(string.Contains), new Type[] { typeof(string) }),
                        compareValueExpression));
                    break;
                case ValueFilterCondition.StartsWith:
                    leftSideExpresion = Expression.Call(
                        propertySelector.Body,
                        typeof(string).GetMethod(nameof(string.StartsWith), new Type[] { typeof(string) }),
                        compareValueExpression);
                    break;
                case ValueFilterCondition.EndsWith:
                    leftSideExpresion = Expression.Call(
                        propertySelector.Body,
                        typeof(string).GetMethod(nameof(string.EndsWith), new Type[] { typeof(string) }),
                        compareValueExpression);
                    break;
                default:
                    leftSideExpresion = null;
                    break;
            }

            if (leftSideExpresion != null)
            {
                AddToAnd(Expression.Lambda<Func<T, bool>>(
                                leftSideExpresion,
                                propertySelector.Parameters[0]));
            }
        }

        protected void AddEnumFilter<TEnum>(EnumFilter enumFilter, Expression<Func<T, TEnum>> propertySelector)
            where TEnum : struct, Enum
        {
            TEnum enumValue = Enum.Parse<TEnum>(enumFilter.Value);

            UnaryExpression compareValueExpression = WrapConstantExpression(enumValue);

            var compareExpression = enumFilter.Condition.ToExpression();

            if (compareExpression != null)
            {
                AddToAnd(Expression.Lambda<Func<T, bool>>(
                                compareExpression(propertySelector.Body, compareValueExpression),
                                propertySelector.Parameters[0]));
            }
        }

        #endregion

        #region Private Methods

        private Expression<Func<T, bool>> AddOperator(Expression<Func<T, bool>> criteria, Expression<Func<T, bool>> newCriteria, Operator expressionOperator = Operator.And)
        {
            if (criteria == null)
            {
                criteria = newCriteria;
                return criteria;
            }

            if (newCriteria == null)
            {
                return criteria;
            }

            var invokedExpr = Expression.Invoke(newCriteria, criteria.Parameters.Cast<Expression>());

            BinaryExpression binary = null;

            switch (expressionOperator)
            {
                case Operator.And:
                    binary = Expression.AndAlso(criteria.Body, invokedExpr);
                    break;
                case Operator.Or:
                    binary = Expression.Or(criteria.Body, invokedExpr);
                    break;
            }

            return Expression.Lambda<Func<T, bool>>(binary, criteria.Parameters);
        }

        private UnaryExpression WrapConstantExpression<TValue>(TValue value)
        {
            var dateWrapper = new
            {
                value,
            };
            var wrappedValueExpression = Expression.Convert(Expression.Property(Expression.Constant(dateWrapper), nameof(dateWrapper.value)), typeof(TValue));
            return wrappedValueExpression;
        }

        private void AddFilterToAnd<TFilter>(IEnumerable<TFilter> filters, IDictionary<string, string> fieldNameMappings = null, List<string> skipFields = null)
           where TFilter : FilterBase
        {
            skipFields = skipFields ?? new List<string>();
            fieldNameMappings = fieldNameMappings ?? new Dictionary<string, string>();

            filters
                .Where(filter => !skipFields.Contains(filter.FieldName))
                .ForEach(filter =>
                {
                    FilterEvaluator<T>.ApplyMappings(filter, fieldNameMappings);
                    AddToAnd(FilterEvaluator<T>.Evaluate((dynamic)filter));
                });
        }

        #endregion

        #region Subclasses

        public class LastValue : BaseSpecification<T>
        {
            #region Constructor(s)

            public LastValue(Expression<Func<T, object>> lastItemProperty)
                : base()
            {
                ApplyOrderByDescending(lastItemProperty);
                ApplyPaging(new Pagination(1, 1));
            }

            #endregion
        }

        #endregion
    }
}
