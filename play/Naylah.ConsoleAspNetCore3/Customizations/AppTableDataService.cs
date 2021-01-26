using Naylah.Core.Data.Services;
using Naylah.Data;
using Naylah.Data.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Naylah.ConsoleAspNetCore.Customizations
{
    public class StringTableDataServiceV2<TEntity>
        : TableDataServiceV2<TEntity, string>
        where TEntity : class, IEntity<string>, IModifiable, new()
    {
        public StringTableDataServiceV2(
            IRepository<TEntity, string> repository,
            IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
        }

        public StringTableDataServiceV2(
            IRepository<TEntity, string> repository,
            IUnitOfWork unitOfWork,
            Domain.Abstractions.IHandler<Notification> notificationsHandler) : base(repository, unitOfWork, notificationsHandler)
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

    public class StringAppTableDataService<TEntity, TModel> : TableDataService<TEntity, TModel, string>
        where TEntity : class, IEntity<string>, IModifiable, IEntityUpdate<TModel>, new()
        where TModel : class, IEntity<string>, new()
    {
        public StringAppTableDataService(IUnitOfWork _unitOfWork, IRepository<TEntity, string> repository) : base(_unitOfWork, repository)
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
