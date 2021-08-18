using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Naylah.Data.Access
{
    public interface IAsyncEnumerableRepository
    {
        Task<IEnumerable<TEntity>> AsEnumerableAsync<TEntity>(
            IQueryable<TEntity> queryable,
            CancellationToken cancellationToken = default
            );
    }
}
