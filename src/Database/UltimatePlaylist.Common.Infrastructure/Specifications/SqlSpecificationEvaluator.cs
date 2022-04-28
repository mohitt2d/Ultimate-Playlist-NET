#region Usings

using System.Linq;
using Microsoft.EntityFrameworkCore;
using UltimatePlaylist.Database.Infrastructure.Specifications.Interfaces;
using Z.EntityFramework.Plus;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Specifications
{
    internal class SqlSpecificationEvaluator<T>
        where T : class
    {
        #region GetQuery

        public IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
        {
            return ApplySpecification(inputQuery, specification);
        }

        public IQueryable<TProjectionTarget> GetQueryWithProjection<TProjectionTarget>(
            IQueryable<T> inputQuery,
            IProjectedSpecification<T, TProjectionTarget> specification)
            where TProjectionTarget : class
        {
            var baseQuery = GetQuery(inputQuery, specification.BaseSpecification);
            var projectedQuery = baseQuery.Select(specification.Projection);

            return ApplySpecification(projectedQuery, specification);
        }

        internal IQueryable<TProjectionTarget> GetQueryWithGrouping<TGrouping, TProjectionTarget>(
            IQueryable<T> inputQuery,
            IGroupedSpecification<T, TGrouping, TProjectionTarget> specification)
            where TProjectionTarget : class
        {
            var baseQuery = GetQuery(inputQuery, specification.BaseSpecification);
            var projectedQuery = baseQuery
                .GroupBy(specification.Grouping)
                .Select(specification.GroupingProjection);

            return ApplySpecification(projectedQuery, specification);
        }

        private static IQueryable<TQueryTarget> ApplySpecification<TQueryTarget>(
            IQueryable<TQueryTarget> query,
            ISpecification<TQueryTarget> specification)
            where TQueryTarget : class
        {
            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }

            if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            else if (specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }

            if (specification.IsPagingEnabled)
            {
                query = query.Skip(specification.Skip)
                    .Take(specification.Take);
            }

            query = specification.Includes.Aggregate(
                query,
                (current, include) =>
                {
                    if (include.ignoreFilter)
                    {
                        return current.IncludeOptimized(include.Item1).IgnoreQueryFilters();
                    }

                    return current.IncludeOptimized(include.Item1);
                });

            query = specification.IncludeStrings.Aggregate(
               query,
               (current, include) =>
               {
                   if (include.Item2)
                   {
                       return current.Include(include.Item1).IgnoreQueryFilters();
                   }

                   return current.Include(include.Item1);
               });

            if (specification.IgnoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            return query;
        }

        #endregion
    }
}
