using System.Threading.Tasks;

namespace Naylah.Data
{
    public class StringTableDataServiceCustom<TEntity>
        : TableDataServiceCustom<TEntity, string>
        where TEntity : class, IEntity<string>, IModifiable, new()
    {
        public StringTableDataServiceCustom(
            IRepository<TEntity, string> repository,
            Access.IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }

        public StringTableDataServiceCustom(
            IRepository<TEntity, string> repository,
            Access.IUnitOfWork unitOfWork,
            Domain.Abstractions.IHandler<Notification> notificationsHandler)
            : base(repository, unitOfWork, notificationsHandler)
        {
        }

        protected override async Task<TEntity> FindByIdAsync(string identifier)
        {
            return await FindByAsync(x => x.Id.Equals(identifier));
        }

        protected internal override async Task GenerateId(TEntity entity)
        {
            entity.GenerateId(true);
        }
    }
}