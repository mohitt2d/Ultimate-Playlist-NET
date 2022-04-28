#region Usings

using System.Collections.Generic;
using System.Threading.Tasks;
using UltimatePlaylist.Database.Infrastructure.Entities.Base;
using UltimatePlaylist.Database.Infrastructure.Specifications.Interfaces;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces
{
    public interface IRepository<TBaseEntity> : IReadOnlyRepository<TBaseEntity>
        where TBaseEntity : class, IBaseEntity
    {
        #region Set

        Task<TBaseEntity> AddAsync(TBaseEntity entity);

        Task<IEnumerable<TBaseEntity>> AddRangeAsync(IEnumerable<TBaseEntity> entity);

        Task<TBaseEntity> UpdateAndSaveAsync(TBaseEntity entity, bool saveChanges = true);

        Task<IEnumerable<TBaseEntity>> UpdateAndSaveRangeAsync(IEnumerable<TBaseEntity> entities, bool saveChanges = true);

        Task DeleteAsync(ISpecification<TBaseEntity> spec);

        #endregion
    }
}
