#region Usings

using System.Transactions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UltimatePlaylist.Common.Extensions;
using UltimatePlaylist.Database.Infrastructure.Context;
using UltimatePlaylist.Database.Infrastructure.Entities.Base;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Database.Infrastructure.Specifications.Interfaces;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Repositories
{
    public class Repository<TBaseEntity> : ReadOnlyRepository<TBaseEntity>, IRepository<TBaseEntity>
        where TBaseEntity : class, IBaseEntity
    {
        #region Constructor(s)

        public Repository(EFContext context, Lazy<IMapper> mapperProvider)
            : base(context, mapperProvider)
        {
        }

        #endregion

        #region Set

        public async Task<TBaseEntity> AddAsync(TBaseEntity entity)
        {
            await Entities.AddAsync(entity);
            await Context.SaveChangesAsync();

            var result = entity;
            

            return result;
        }
        public async Task<IEnumerable<TBaseEntity>> AddRangeAsync(IEnumerable<TBaseEntity> entities)
        {
            await Entities.AddRangeAsync(entities);
            await Context.SaveChangesAsync();

            var result = entities;

            return result;
        }

        public async Task<TBaseEntity> UpdateAndSaveAsync(TBaseEntity entity, bool saveChanges = true)
        {
            Context.Entry(entity).State = EntityState.Modified;
            entity.Updated = DateTime.UtcNow;
            Context.Update(entity);

            if (saveChanges)
            {
                 await Context.SaveChangesAsync();
            }

            var result = entity;

            return result;
        }

        public async Task<IEnumerable<TBaseEntity>> UpdateAndSaveRangeAsync(IEnumerable<TBaseEntity> entities, bool saveChanges = true)
        {
            entities.ForEach(s =>
            {
                Context.Entry(s).State = EntityState.Modified;
                s.Updated = DateTime.UtcNow;
            });

            Entities.UpdateRange(entities);

            if (saveChanges)
            {
                await Context.SaveChangesAsync();
            }

            var result = entities;

            return result;
        }

        public async Task DeleteAsync(ISpecification<TBaseEntity> spec)
        {
            var entities = await ListAsync(spec);
            entities.ToList().ForEach(entity =>
            {
                entity.Updated = DateTime.UtcNow;
                entity.IsDeleted = true;
                Context.Update(entity);
            });
            await Context.SaveChangesAsync();
        }

        #endregion
    }
}
