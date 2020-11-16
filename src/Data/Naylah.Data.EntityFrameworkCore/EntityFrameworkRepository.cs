using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naylah.Data.EntityFrameworkCore
{
    public class EntityFrameworkRepository<TDbContext, TEntity, TIdentifier> : IRepository<TEntity, TIdentifier>
       where TDbContext : DbContext
       where TEntity : class, IEntity<TIdentifier>
    {
        private readonly TDbContext dbContext;

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

        public ValueTask<TEntity> EditAsync(TEntity entity)
        {
            dbContext.Entry(entity).State = EntityState.Modified;
            return new ValueTask<TEntity>(entity);
        }

        public Task RemoveAsync(TEntity entity)
        {
            dbContext.Entry(entity).State = EntityState.Deleted;
            return Task.FromResult(1);
        }
    }
}