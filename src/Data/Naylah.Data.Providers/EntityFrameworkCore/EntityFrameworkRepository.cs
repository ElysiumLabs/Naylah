using Microsoft.EntityFrameworkCore;
using Naylah.Data.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Naylah.Data.Providers.EntityFrameworkCore
{
    public class EntityFrameworkRepository<TDbContext, TEntity> : IRepository<TEntity>, 
        ICommandRangeRepository<TEntity>,
        IAsyncCountRepository, IAsyncEnumerableRepository
        where TDbContext : DbContext
        where TEntity : class
    {
        protected readonly TDbContext dbContext;

        public EntityFrameworkRepository(TDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<TEntity> Entities => dbContext.Set<TEntity>();

        public ValueTask<TEntity> AddAsync(TEntity entity)
        {
            dbContext.Set<TEntity>().Add(entity);
            return new ValueTask<TEntity>(entity);
        }

        public ValueTask<IEnumerable<TEntity>> AddAsync(IEnumerable<TEntity> entities)
        {
            dbContext.Set<TEntity>().AddRange(entities);
            return new ValueTask<IEnumerable<TEntity>>(entities);
        }

        public ValueTask<TEntity> EditAsync(TEntity entity)
        {
            dbContext.Set<TEntity>().Update(entity);
            return new ValueTask<TEntity>(entity);
        }

        public ValueTask<IEnumerable<TEntity>> EditAsync(IEnumerable<TEntity> entities)
        {
            dbContext.Set<TEntity>().UpdateRange(entities);
            return new ValueTask<IEnumerable<TEntity>>(entities);
        }

        public Task RemoveAsync(TEntity entity)
        {
            dbContext.Set<TEntity>().Remove(entity);
            return Task.FromResult(1);
        }

        public Task RemoveAsync(IEnumerable<TEntity> entities)
        {
            dbContext.Set<TEntity>().RemoveRange(entities);
            return Task.FromResult(1);
        }

        public async Task<IEnumerable<TEntity1>> AsEnumerableAsync<TEntity1>(
           IQueryable<TEntity1> queryable,
           CancellationToken cancellationToken = default
           )
        {
            return await queryable.ToListAsync(cancellationToken);
        }

        public async Task<long> GetCountAsync<TEntity1>(IQueryable<TEntity1> queryable)
        {
            return await queryable.LongCountAsync();
        }
    }
}