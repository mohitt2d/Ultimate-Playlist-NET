#region Usings

using System;
using System.Linq.Expressions;
using UltimatePlaylist.Database.Infrastructure.Specifications.Interfaces;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Specifications
{
    public abstract class BaseProjectedSpecification<TBase, TProjectionTarget> : BaseSpecification<TProjectionTarget>, IProjectedSpecification<TBase, TProjectionTarget>
        where TBase : class
        where TProjectionTarget : class
    {
        #region Constructor(s)

        protected BaseProjectedSpecification(ISpecification<TBase> baseSpecification)
        {
            BaseSpecification = baseSpecification;
        }

        public ISpecification<TBase> BaseSpecification { get; }

        public abstract Expression<Func<TBase, TProjectionTarget>> Projection { get; }

        #endregion
    }
}
