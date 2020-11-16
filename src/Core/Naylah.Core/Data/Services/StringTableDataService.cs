using Naylah.Data.Access;
using System.Threading.Tasks;

namespace Naylah.Data
{
    public abstract class StringTableDataService<TEntity, TModel> : TableDataService<TEntity, TModel, string>
       where TEntity : class, IEntityUpdate<TModel>, IEntity<string>, IModifiable, new()
       where TModel : class, IEntity<string>, new()
    {
        public StringTableDataService(IUnitOfWork _unitOfWork, IRepository<TEntity, string> repository) : base(_unitOfWork, repository)
        {
        }

        protected override async Task<TEntity> FindByIdAsync(string identifier)
        {
            return await FindByAsync(x => x.Id == identifier);
        }

        protected override async Task GenerateId(TEntity entity)
        {
            entity.GenerateId();
        }
    }
}