using System;
using System.Threading.Tasks;

namespace Naylah.Data
{
    public interface IRepository<TEntity> : 
        IQueryRepository<TEntity>, 
        ICommandRepository<TEntity>
        where TEntity : class
    {

    }
}