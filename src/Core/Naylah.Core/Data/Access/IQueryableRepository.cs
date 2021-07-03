using System;
using System.Linq;
using System.Linq.Expressions;

namespace Naylah.Data
{
    public interface IQueryRepository<TEntity>
        where TEntity : class
    {
        IQueryable<TEntity> Entities { get; }
    }
}