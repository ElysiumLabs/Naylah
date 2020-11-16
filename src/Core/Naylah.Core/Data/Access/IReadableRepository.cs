using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Naylah.Data
{
    [Obsolete]
    public interface IReadableRepository<TEntity, TIdentifier> 
        where TEntity : IEntity<TIdentifier>
    {
        IEnumerable<TEntity> GetAll(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Expression<Func<TEntity, object>>[] includes = null,
            int? skip = null,
            int? take = null);

        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Expression<Func<TEntity, object>>[] includes = null,
            int? skip = null,
            int? take = null);

        TEntity GetById(
            TIdentifier id,
            Expression<Func<TEntity, object>>[] includes = null);

        int GetCount(Expression<Func<TEntity, bool>> filter = null);

        bool GetExists(Expression<Func<TEntity, bool>> filter = null);
    }
}