#region Usings

using System;
using System.Linq;
using System.Linq.Expressions;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Specifications.Interfaces
{
    public interface IGroupedSpecification<TBase, TGrouping, TProjectionTarget> : ISpecification<TProjectionTarget>
        where TBase : class
        where TProjectionTarget : class
    {
        ISpecification<TBase> BaseSpecification { get; }

        Expression<Func<TBase, TGrouping>> Grouping { get; }

        Expression<Func<IGrouping<TGrouping, TBase>, TProjectionTarget>> GroupingProjection { get; }
    }
}
