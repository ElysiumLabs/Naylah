using Naylah.Data.Access;
using System.Threading.Tasks;

namespace Naylah.Data
{
    public abstract class StringTableDataService<TEntity, TModel> : TableDataService<TEntity, TModel, string>
       where TEntity : class, IEntityUpdate<TModel>, IEntity<string>, IModifiable, new()
       where TModel : class, IEntity<string>, new()
    {
        public StringTableDataService(IRepository<TEntity> repository, IUnitOfWork _unitOfWork) : base(repository, _unitOfWork)
        {
        }

        protected override async Task<TEntity> FindByIdAsync(string identifier)
        {
            return await FindByAsync(x => x.Id == identifier);
        }

        protected internal override async Task GenerateId(TEntity entity)
        {
            entity.GenerateId(true);
        }
    }
}