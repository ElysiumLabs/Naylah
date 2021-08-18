using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naylah.Data.Access
{
    public interface IAsyncCountRepository
    {
        Task<long> GetCountAsync<TEntity>(IQueryable<TEntity> queryable);
    }
}
