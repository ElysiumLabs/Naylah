using Naylah.Data;
using Naylah.Data.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Naylah.ConsoleAspNetCore.Customizations
{
    public class StringAppTableDataService<TEntity, TModel> : TableDataService<TEntity, TModel, string>
        where TEntity : class, IEntity<string>, IModifiable, IEntityUpdate<TModel>, new()
        where TModel : class, IEntity<string>, new()
    {
        public StringAppTableDataService(IUnitOfWork _unitOfWork, IRepository<TEntity> repository) : base(repository, _unitOfWork)
        {
        }

        protected override Task<TEntity> FindByIdAsync(string identifier)
        {
            return FindByAsync(x => x.Id == identifier);
        }

        protected override Task GenerateId(TEntity entity)
        {
            entity.GenerateId();
            return Task.FromResult(1);
        }
    }
}
