using Naylah.Data.Access;
using Naylah.Data.Extensions;
using Naylah.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Naylah.Data
{
    public abstract class TableDataService<TEntity, TModel, TIdentifier> : DataServiceBase
        where TEntity : class, IEntity<TIdentifier>, IModifiable, IEntityUpdate<TModel>, new()
        where TModel : class, IEntity<TIdentifier>, new()
    {
        protected internal IRepository<TEntity, TIdentifier> Repository;

        protected internal virtual Func<IQueryable<TEntity>, IQueryable<TModel>> Projection { get; set; } =
            (q) => q.Project().To<TModel>();

        protected internal virtual Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> Ordering { get; set; } =
            (q) => q.OrderByDescending(x => x.CreatedAt);

        protected bool UseSoftDelete { get; set; } = false;

        protected bool NotificationThrowException { get; set; } = false;

        public TableDataService(
           IUnitOfWork _unitOfWork,
           IRepository<TEntity, TIdentifier> repository
           ) : this(_unitOfWork, repository, null)
        {
        }

        public TableDataService(
            IUnitOfWork _unitOfWork,
            IRepository<TEntity, TIdentifier> repository,
            IHandler<Notification> notificationHandler
            ) : base(_unitOfWork, notificationHandler)
        {
            Repository = repository;
            NotificationThrowException = notificationHandler == null;
        }

        protected internal virtual TModel ToModel(TEntity entity)
        {
            var es = new List<TEntity>() { entity };
            return Projection?.Invoke(es.AsQueryable()).FirstOrDefault() ?? new TModel() { Id = entity.Id };
        }

        protected internal virtual TEntity ToEntity(TModel model, UpsertType upsertType)
        {
            var entity = Entity.Create<TEntity>();
            entity.UpdateFrom(model, new EntityUpdateOptions(upsertType));
            return entity;
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

        protected virtual async Task<TModel> UpdateInternalAsync(TEntity entity, TModel model)
        {
            if (entity == null)
            {
                RaiseNotification(Notification.FromType(GetType(), "Entity is null"));
            }

            entity.UpdateFrom(model, new EntityUpdateOptions(UpsertType.Update));
            entity.UpdateUpdateAt();

            entity = await Repository.EditAsync(entity);

            if (!await CommitAsync())
            {
                RaiseNotification(Notification.FromType(GetType(), "Transaction was not commited"));
            }

            return ToModel(entity);
        }



        public virtual async Task<TModel> Create(TModel model)
        {
            var entity = ToEntity(model, UpsertType.Insert);
            await GenerateId(entity);

            entity.UpdateCreatedAt();
            await Repository.AddAsync(entity);

            if (!await CommitAsync())
            {
                RaiseNotification(Notification.FromType(GetType(), "Transaction was not commited"));
            }

            return ToModel(entity);
        }

        public virtual async Task<TModel> UpdateAsync(TModel model, Expression<Func<TEntity, bool>> customPredicate = null)
        {
            var entity = customPredicate != null ? await FindByAsync(customPredicate) : await FindByIdAsync(model.Id);

            if (entity == null)
            {
                RaiseNotification(Notification.FromType(GetType(), "Entity not found"));
            }

            return await UpdateInternalAsync(entity, model);
        }

        public virtual async Task<TModel> UpsertAsync(TModel model, Expression<Func<TEntity, bool>> customPredicate = null)
        {
            var entity = customPredicate != null ? await FindByAsync(customPredicate) : await FindByIdAsync(model.Id);

            if (entity == null)
            {
                return await Create(model);
            }
            else
            {
                return await UpdateInternalAsync(entity, model);
            }
        }

        public virtual async Task<TModel> GetById(TIdentifier id)
        {
            var entity = await FindByIdAsync(id);

            if (entity == null)
            {
                RaiseNotification(Notification.FromType(GetType(), "Entity not found"));
            }

            return ToModel(entity);
        }

        public virtual async Task<TModel> Delete(TIdentifier id)
        {
            var entity = await FindByIdAsync(id);

            if (entity == null)
            {
                RaiseNotification(Notification.FromType(GetType(), "Entity not found"));
            }

            if (!UseSoftDelete)
            {
                await Repository.RemoveAsync(entity);
            }
            else
            {
                entity.Deleted = true;
                entity = await Repository.EditAsync(entity);
            }

            if (!await CommitAsync())
            {
                RaiseNotification(Notification.FromType(GetType(), "Transaction was not commited"));
            }

            return ToModel(entity);
        }

        protected internal virtual IQueryable<TModel> Project(IQueryable<TEntity> entities)
        {
            var entityQuery = entities;
            var projectedQuery = Projection.Invoke(entityQuery);
            return projectedQuery;
        }

        public virtual IQueryable<TModel> GetAll(IQueryable<TEntity> adptedEntities = null)
        {
            var entityQuery = adptedEntities ?? GetEntities();

            if (UseSoftDelete)
            {
                entityQuery = entityQuery.Where(x => !x.Deleted);
            }

            if (Ordering != null)
            {
                entityQuery = Ordering.Invoke(entityQuery);
            }

            return Project(entityQuery);
        }

    }
}