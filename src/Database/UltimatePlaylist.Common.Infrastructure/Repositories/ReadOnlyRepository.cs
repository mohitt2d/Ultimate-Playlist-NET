#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UltimatePlaylist.Database.Infrastructure.Context;
using UltimatePlaylist.Database.Infrastructure.Entities.Base;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Database.Infrastructure.Specifications;
using UltimatePlaylist.Database.Infrastructure.Specifications.Interfaces;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Repositories
{
    public class ReadOnlyRepository<TBaseEntity> : IReadOnlyRepository<TBaseEntity>
        where TBaseEntity : class, IBaseEntity
    {
        private readonly Lazy<IMapper> MapperProvider;

        #region Constructor(s)

        public ReadOnlyRepository(EFContext context, Lazy<IMapper> mapperProvider)
        {
            Context = context;
            MapperProvider = mapperProvider;
            Entities = Context.Set<TBaseEntity>();
        }

        #endregion

        #region Protected Properties

        protected EFContext Context { get; set; }

        protected DbSet<TBaseEntity> Entities { get; set; }

        protected IMapper Mapper => MapperProvider.Value;

        #endregion

        #region Get

        public virtual async Task<IReadOnlyList<TBaseEntity>> ListAsync(ISpecification<TBaseEntity> spec)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions()
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                },
                TransactionScopeAsyncFlowOption.Enabled);

            var result = await ApplySpecification(spec).ToListAsync();

            scope.Complete();

            return result;
        }

        public virtual async Task<IReadOnlyList<TProjectionTarget>> ListAndProjectAsync<TProjectionTarget>(IProjectedSpecification<TBaseEntity, TProjectionTarget> spec)
            where TProjectionTarget : class
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions()
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                },
                TransactionScopeAsyncFlowOption.Enabled);

            var result = await ApplySpecification(spec).ToListAsync();

            scope.Complete();

            return result;
        }

        public virtual async Task<IReadOnlyList<TProjectionTarget>> ListAndGroupAsync<TGrouping, TProjectionTarget>(
            IGroupedSpecification<TBaseEntity, TGrouping, TProjectionTarget> spec)
            where TProjectionTarget : class
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions()
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                },
                TransactionScopeAsyncFlowOption.Enabled);

            var result = await ApplySpecification(spec).ToListAsync();

            scope.Complete();

            return result;
        }

        public virtual async Task<int> CountAsync(ISpecification<TBaseEntity> spec)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions()
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                },
                TransactionScopeAsyncFlowOption.Enabled);
            var result = await ApplySpecification(spec).CountAsync();

            scope.Complete();

            return result;
        }

        public virtual async Task<TResult> MaxAsync<TResult>(ISpecification<TBaseEntity> spec, Expression<Func<TBaseEntity, TResult>> selector)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions()
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                },
                TransactionScopeAsyncFlowOption.Enabled);
            var result = await ApplySpecification(spec).MaxAsync(selector);

            scope.Complete();

            return result;
        }

        public virtual async Task<bool> AnyAsync(ISpecification<TBaseEntity> spec)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions()
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                },
                TransactionScopeAsyncFlowOption.Enabled);
            var result = await ApplySpecification(spec).AnyAsync();

            scope.Complete();

            return result;
        }

        public virtual async Task<TBaseEntity> FirstOrDefaultAsync(ISpecification<TBaseEntity> spec)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions()
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                },
                TransactionScopeAsyncFlowOption.Enabled);
            var result = await ApplySpecification(spec).FirstOrDefaultAsync();

            scope.Complete();

            return result;
        }


        public virtual long GetPlaylistMaxId()
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions()
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                },
                TransactionScopeAsyncFlowOption.Enabled);
            var baseQuery = Context.Set<TBaseEntity>().AsQueryable();
            var result = baseQuery.Last().Id;

            scope.Complete();

            return result;
        }

        public virtual async Task<TBaseEntity> SingleOrDefaultAsync(ISpecification<TBaseEntity> spec)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions()
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                },
                TransactionScopeAsyncFlowOption.Enabled);
            var result = await ApplySpecification(spec).SingleOrDefaultAsync();

            scope.Complete();

            return result;
        }

        #endregion

        #region ApplySpecification

        protected IQueryable<TBaseEntity> ApplySpecification(ISpecification<TBaseEntity> spec)
        {
            var baseQuery = Context.Set<TBaseEntity>().AsQueryable();

            var result = new SqlSpecificationEvaluator<TBaseEntity>().GetQuery(baseQuery, spec);

            return result;
        }

        protected IQueryable<TProjectionTarget> ApplySpecification<TProjectionTarget>(IProjectedSpecification<TBaseEntity, TProjectionTarget> spec)
            where TProjectionTarget : class
        {
            var baseQuery = Context.Set<TBaseEntity>().AsQueryable();

            return new SqlSpecificationEvaluator<TBaseEntity>().GetQueryWithProjection(baseQuery, spec);
        }

        protected IQueryable<TProjectionTarget> ApplySpecification<TGrouping, TProjectionTarget>(IGroupedSpecification<TBaseEntity, TGrouping, TProjectionTarget> spec)
            where TProjectionTarget : class
        {
            var baseQuery = Context.Set<TBaseEntity>().AsQueryable();

            return new SqlSpecificationEvaluator<TBaseEntity>().GetQueryWithGrouping(baseQuery, spec);
        }

        #endregion
    }
}
