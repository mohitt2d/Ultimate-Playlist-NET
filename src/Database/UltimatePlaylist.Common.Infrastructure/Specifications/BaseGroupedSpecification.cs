#region Usings

using System;
using System.Linq;
using System.Linq.Expressions;
using UltimatePlaylist.Database.Infrastructure.Specifications.Interfaces;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Specifications
{
    public abstract class BaseGroupedSpecification<TBase, TGrouping, TProjectionTarget>
        : BaseSpecification<TProjectionTarget>,
        IGroupedSpecification<TBase, TGrouping, TProjectionTarget>
        where TBase : class
        where TProjectionTarget : class
    {
        #region Constructor(s)

        protected BaseGroupedSpecification(ISpecification<TBase> baseSpecification)
        {
            BaseSpecification = baseSpecification;
        }

        public ISpecification<TBase> BaseSpecification { get; }

        public abstract Expression<Func<TBase, TGrouping>> Grouping { get; }

        public abstract Expression<Func<IGrouping<TGrouping, TBase>, TProjectionTarget>> GroupingProjection { get; }

        #endregion
    }
}
