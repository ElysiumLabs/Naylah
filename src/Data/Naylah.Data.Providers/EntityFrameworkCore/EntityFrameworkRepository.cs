using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naylah.Data.Providers.EntityFrameworkCore
{
    public class EntityFrameworkRepository<TDbContext, TEntity, TIdentifier> : IRepository<TEntity, TIdentifier>, ICommandRangeRepository<TEntity, TIdentifier>
       where TDbContext : DbContext
       where TEntity : class, IEntity<TIdentifier>
    {
        protected readonly TDbContext dbContext;

        public EntityFrameworkRepository(TDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<TEntity> Entities => dbContext.Set<TEntity>();

        public ValueTask<TEntity> AddAsync(TEntity entity)
        {
            dbContext.Entry(entity).State = EntityState.Added;
            return new ValueTask<TEntity>(entity);
        }

        public async ValueTask<IEnumerable<TEntity>> AddAsync(IEnumerable<TEntity> entities)
        {
            foreach (var item in entities)
            {
                await AddAsync(item);
            }

            return entities;
        }

        public ValueTask<TEntity> EditAsync(TEntity entity)
        {
            dbContext.Entry(entity).State = EntityState.Modified;
            return new ValueTask<TEntity>(entity);
        }

        public async ValueTask<IEnumerable<TEntity>> EditAsync(IEnumerable<TEntity> entities)
        {
            foreach (var item in entities)
            {
                await EditAsync(item);
            }
            return entities;
        }

        public Task RemoveAsync(TEntity entity)
        {
            dbContext.Entry(entity).State = EntityState.Deleted;
            return Task.FromResult(1);
        }

        public async Task RemoveAsync(IEnumerable<TEntity> entities)
        {
            foreach (var item in entities)
            {
                await RemoveAsync(item);
            }
        }
    }
}