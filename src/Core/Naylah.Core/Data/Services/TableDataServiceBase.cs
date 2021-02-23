using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Naylah.Data;
using Naylah.Data.Access;
using Naylah.Domain.Abstractions;

namespace Naylah
{
    public abstract class TableDataServiceBase<TEntity, TIdentifier>
        : DataServiceBase
        where TEntity : class, IEntity<TIdentifier>, IModifiable, new()
    {
        internal readonly IRepository<TEntity, TIdentifier> Repository;
        protected bool UseSoftDelete { get; set; } = false;
        protected bool NotificationThrowException { get; set; } = false;

        protected internal virtual Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> Ordering { get; set; } =
            (q) => q.OrderByDescending(x => x.CreatedAt);

        public TableDataServiceBase(
            IRepository<TEntity, TIdentifier> repository,
            IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this.Repository = repository;
        }

        public TableDataServiceBase(
            IRepository<TEntity, TIdentifier> repository,
            IUnitOfWork unitOfWork,
            IHandler<Notification> notificationsHandler)
            : base(unitOfWork, notificationsHandler)
        {
            this.Repository = repository;
        }

        protected internal virtual IQueryable<TEntity> GetEntities()
        {
            return Repository.Entities;
        }

        protected virtual async Task<TEntity> FindByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return GetEntities().Where(predicate).FirstOrDefault();
        }

        protected virtual async Task<TEntity> FindByIdAsync(TIdentifier identifier)
        {
            return await FindByAsync(x => x.Id.Equals(identifier));
        }

        protected virtual async Task GenerateId(TEntity entity)
        {
            //application id generation...
        }

        protected virtual bool RaiseNotification(Notification notification)
        {
            if (NotificationThrowException)
            {
                throw new Exception(notification.Key + " - " + notification.Value);
            }
            else
            {
                NotificationsHandler.Handle(notification);
            }

            return true;
        }
    }
}