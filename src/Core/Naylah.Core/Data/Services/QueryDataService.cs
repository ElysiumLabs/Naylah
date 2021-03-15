using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Naylah.Data;
using Naylah.Data.Access;
using Naylah.Data.Extensions;
using Naylah.Domain.Abstractions;

namespace Naylah
{
    public abstract class QueryDataService<TEntity, TIdentifier> : DataServiceBase
        where TEntity : class, IEntity<TIdentifier>, IModifiable, new()
    {
        protected internal readonly IRepository<TEntity, TIdentifier> Repository;

        protected bool UseSoftDelete { get; set; } = false;

        protected bool NotificationThrowException { get; set; } = false;

        protected internal virtual Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> Ordering { get; set; } =
            (q) => q.OrderByDescending(x => x.CreatedAt);

        public QueryDataService(
            IRepository<TEntity, TIdentifier> repository,
            IUnitOfWork unitOfWork)
            : this(repository, unitOfWork, null)
        {
        }

        public QueryDataService(
            IRepository<TEntity, TIdentifier> repository,
            IUnitOfWork unitOfWork,
            IHandler<Notification> notificationsHandler)
            : base(unitOfWork, notificationsHandler)
        {
            Repository = repository;
            NotificationThrowException = notificationsHandler == null;
        }

        public virtual IQueryable<TCustomModel> GetAll<TCustomModel>(IQueryable<TEntity> adptedEntities = null)
            where TCustomModel : class, IEntity<TIdentifier>, new()
        {
            var entityQuery = adptedEntities ?? GetEntities();

            if (Ordering != null)
            {
                entityQuery = Ordering.Invoke(entityQuery);
            }

            return Project<TCustomModel>(entityQuery);
        }

        protected internal virtual IQueryable<TEntity> GetEntities()
        {
            var q = Repository.Entities;

            if (UseSoftDelete)
            {
                q = q.Where(x => !x.Deleted);
            }

            return q;
        }

        protected virtual Task<TEntity> FindByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(GetEntities().Where(predicate).FirstOrDefault());
        }

        protected virtual async Task<TEntity> FindByIdAsync(TIdentifier identifier)
        {
            return await FindByAsync(x => x.Id.Equals(identifier));
        }

        protected virtual bool RaiseNotification(Notification notification)
        {
            if (NotificationThrowException)
            {
                throw new Exception(notification.Key + " - " + notification.Value);
            }
            else
            {
                NotificationsHandler?.Handle(notification);
            }

            return true;
        }

        internal virtual IQueryable<TCustomModel> Project<TCustomModel>(IQueryable<TEntity> entities)
        {
            return entities.Project().To<TCustomModel>();
        }

        protected internal virtual TCustomModel ToModel<TCustomModel>(TEntity entity)
            where TCustomModel : IEntity<TIdentifier>, new()
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var es = new List<TEntity>() { entity };

            return Project<TCustomModel>(es.AsQueryable()).FirstOrDefault();
        }

        protected virtual async Task<TCustomModel> GetByIdAsync<TCustomModel>(TIdentifier id)
            where TCustomModel : class, IEntity<TIdentifier>, new()
        {
            var entity = await FindByIdAsync(id);

            if (entity == null)
            {
                RaiseNotification(Notification.FromType(GetType(), "Entity not found"));
            }

            return ToModel<TCustomModel>(entity);
        }
    }
}