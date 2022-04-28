#region Usings

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Specifications.Interfaces
{
    public interface ISpecification<T> : IApplyCriteriaVisitor<ISpecification<T>>
        where T : class
    {
        Expression<Func<T, bool>> Criteria { get; }

        List<(Expression<Func<T, object>>, bool ignoreFilter)> Includes { get; }

        List<(string, bool)> IncludeStrings { get; }

        Expression<Func<T, object>> OrderBy { get; }

        Expression<Func<T, object>> OrderByDescending { get; }

        int Take { get; }

        int Skip { get; }

        bool IsPagingEnabled { get; }

        bool IgnoreQueryFilters { get; set; }
    }
}
