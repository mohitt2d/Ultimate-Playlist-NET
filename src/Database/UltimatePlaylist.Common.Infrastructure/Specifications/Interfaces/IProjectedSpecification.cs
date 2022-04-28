#region Usings

using System;
using System.Linq.Expressions;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Specifications.Interfaces
{
    public interface IProjectedSpecification<TBase, TProjectionTarget> : ISpecification<TProjectionTarget>
        where TBase : class
        where TProjectionTarget : class
    {
        ISpecification<TBase> BaseSpecification { get; }

        Expression<Func<TBase, TProjectionTarget>> Projection { get; }
    }
}
