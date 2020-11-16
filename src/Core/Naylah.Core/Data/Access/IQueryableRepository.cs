using System;
using System.Linq;
using System.Linq.Expressions;

namespace Naylah.Data
{
    public interface IQueryRepository<TEntity, TIdentifier> 
        where TEntity : IEntity<TIdentifier>
    {
        IQueryable<TEntity> Entities { get; }
    }
}