#region Usings

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UltimatePlaylist.Database.Infrastructure.Entities.Base;
using UltimatePlaylist.Database.Infrastructure.Specifications.Interfaces;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces
{
    public interface IReadOnlyRepository<TBaseEntity>
        where TBaseEntity : class, IBaseEntity
    {
        #region Get

        Task<IReadOnlyList<TBaseEntity>> ListAsync(ISpecification<TBaseEntity> spec);

        Task<IReadOnlyList<TProjectionTarget>> ListAndProjectAsync<TProjectionTarget>(IProjectedSpecification<TBaseEntity, TProjectionTarget> spec)
            where TProjectionTarget : class;

        Task<IReadOnlyList<TProjectionTarget>> ListAndGroupAsync<TGrouping, TProjectionTarget>(IGroupedSpecification<TBaseEntity, TGrouping, TProjectionTarget> spec)
            where TProjectionTarget : class;

        Task<int> CountAsync(ISpecification<TBaseEntity> spec);

        Task<TResult> MaxAsync<TResult>(ISpecification<TBaseEntity> spec, Expression<Func<TBaseEntity, TResult>> selector);

        Task<bool> AnyAsync(ISpecification<TBaseEntity> spec);

        Task<TBaseEntity> FirstOrDefaultAsync(ISpecification<TBaseEntity> spec);

        Task<TBaseEntity> SingleOrDefaultAsync(ISpecification<TBaseEntity> spec);

        #endregion
    }
}
