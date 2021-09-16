using Naylah.Data.Access;
using System.Threading.Tasks;

namespace Naylah.Data
{
    public abstract class IntTableDataService<TEntity, TModel> : TableDataService<TEntity, TModel, int>
       where TEntity : class, IEntityUpdate<TModel>, IEntity<int>, IModifiable, ISoftDeletable, new()
       where TModel : class, IEntity<int>, new()
    {
        public IntTableDataService(IRepository<TEntity> repository, IUnitOfWork _unitOfWork) : base(repository, _unitOfWork)
        {
        }

        protected override async Task<TEntity> FindByIdAsync(int identifier)
        {
            return await FindByAsync(x => x.Id == identifier);
        }

    }
}