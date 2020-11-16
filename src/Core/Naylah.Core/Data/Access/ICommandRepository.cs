using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Naylah.Data
{
    public interface ICommandRepository<TEntity, TIdentifier>
        where TEntity : IEntity
    {
        ValueTask<TEntity> AddAsync(TEntity entity);

        ValueTask<TEntity> EditAsync(TEntity entity);

        Task RemoveAsync(TEntity entity);
    }

    public interface ICommandRangeRepository<TEntity, TIdentifier>
        where TEntity : IEntity
    {
        ValueTask<IEnumerable<TEntity>> AddAsync(IEnumerable<TEntity> entities);

        ValueTask<IEnumerable<TEntity>> EditAsync(IEnumerable<TEntity> entities);

        Task RemoveAsync(IEnumerable<TEntity> entities);
    }
}
