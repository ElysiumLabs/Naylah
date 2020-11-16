using System;
using System.Threading.Tasks;

namespace Naylah.Data
{
    public interface IRepository<TEntity, TIdentifier> : 
        IQueryRepository<TEntity, TIdentifier>, 
        ICommandRepository<TEntity, TIdentifier>
        where TEntity : IEntity<TIdentifier>
    {

    }
}