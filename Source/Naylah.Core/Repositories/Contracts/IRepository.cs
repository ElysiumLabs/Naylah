using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Naylah.Core.Repositories.Contracts
{
    public interface IRepository<T> : IDisposable where T : class, new()
    {
        T Add(T entity);

        T Create();

        T Delete(T entity);

        T SoftDelete(T entity);

        void Edit(T entity);

        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);

        IEnumerable<T> GetAllAsEnumerable(params Expression<Func<T, object>>[] includes);

        IQueryable<T> GetAllAsQueryable(params Expression<Func<T, object>>[] includes);

        T Find(params object[] keyvalues);

        void Save();
    }
}