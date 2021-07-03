using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Naylah.Data
{
    public interface ICommandRepository<TEntity>
        where TEntity : class
    {
        ValueTask<TEntity> AddAsync(TEntity entity);

        ValueTask<TEntity> EditAsync(TEntity entity);

        Task RemoveAsync(TEntity entity);
    }

    public interface ICommandRangeRepository<TEntity>
        where TEntity : class
    {
        ValueTask<IEnumerable<TEntity>> AddAsync(IEnumerable<TEntity> entities);

        ValueTask<IEnumerable<TEntity>> EditAsync(IEnumerable<TEntity> entities);

        Task RemoveAsync(IEnumerable<TEntity> entities);
    }
}
