using System;
using System.Linq;
using System.Linq.Expressions;

namespace Naylah.Data.Access
{
    public interface IQueryableRepository<TEntity, TIdentifier> where TEntity : IEntity
    {
        IQueryable<TEntity> GetAllAsQueryable(Expression<Func<TEntity, object>>[] includes = null);
    }
}