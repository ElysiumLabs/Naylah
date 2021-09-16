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
    class TableDataServiceBkp<TEntity, TModel, TIdentifier> : QueryDataService<TEntity, TIdentifier>
        where TEntity : class, IEntity<TIdentifier>, IEntityUpdate<TModel>, IModifiable, ISoftDeletable, new()
        where TModel : class, IEntity<TIdentifier>, new()
    {
        public TableDataServiceBkp(IRepository<TEntity> repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
        }

        public TableDataServiceBkp(IRepository<TEntity> repository, IUnitOfWork unitOfWork, IHandler<Notification> notificationsHandler) : base(repository, unitOfWork, notificationsHandler)
        {
        }

        //change later
        public virtual Task GenerateId(TEntity entity)
        {
            //application id generation...
            return Task.FromResult(1);
        }

        protected internal virtual TModel ToModel(TEntity entity)
        {
            var es = new List<TEntity>() { entity };
            return Project<TModel>(es.AsQueryable()).FirstOrDefault() ?? new TModel() { Id = entity.Id };
        }

        protected internal virtual TEntity ToEntity(TModel model, UpsertType upsertType)
        {
            var entity = Entity.Create<TEntity>();
            entity.UpdateFrom(model, new EntityUpdateOptions(upsertType));
            return entity;
        }

        protected internal virtual TEntity ToEntity(TIdentifier identifier)
        {
            var entity = Entity.Create<TEntity>();
            entity.Id = identifier;
            return entity;
        }

        protected virtual async Task<TEntity> CreateInternalAsync(TEntity entity, TModel model)
        {
            if (entity == null)
            {
                RaiseNotification(Notification.FromType(GetType(), "Entity is null"));
            }

            entity.UpdateFrom(model, new EntityUpdateOptions(UpsertType.Insert));
            entity.UpdateUpdateAt();

            entity = await Repository.AddAsync(entity);

            return entity;
        }

        protected virtual async Task<TEntity> UpdateInternalAsync(TEntity entity, TModel model)
        {
            if (entity == null)
            {
                RaiseNotification(Notification.FromType(GetType(), "Entity is null"));
            }

            entity.UpdateFrom(model, new EntityUpdateOptions(UpsertType.Update));
            entity.UpdateUpdateAt();

            entity = await Repository.EditAsync(entity);

            return entity;
        }

        protected async Task<TEntity> DeleteInternal(TEntity entity)
        {
            if (!UseSoftDelete)
            {
                await Repository.RemoveAsync(entity);
            }
            else
            {
                entity.Deleted = true;
                entity = await Repository.EditAsync(entity);
            }

            return entity;
        }

        public virtual async Task<TModel> Create(TModel model)
        {
            var entity = ToEntity(model, UpsertType.Insert);
            await GenerateId(entity);

            entity = await CreateInternalAsync(entity, model);

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

            entity = await UpdateInternalAsync(entity, model);

            if (!await CommitAsync())
            {
                RaiseNotification(Notification.FromType(GetType(), "Transaction was not commited"));
            }

            return ToModel(entity);
        }

        public virtual async Task<TModel> UpsertAsync(TModel model, Expression<Func<TEntity, bool>> customPredicate = null)
        {
            var entity = customPredicate != null ? await FindByAsync(customPredicate) : await FindByIdAsync(model.Id);

            if (entity == null)
            {
                entity = ToEntity(model, UpsertType.Insert);
                await GenerateId(entity);

                entity = await CreateInternalAsync(entity, model);
            }
            else
            {
                entity = await UpdateInternalAsync(entity, model);
            }

            if (!await CommitAsync())
            {
                RaiseNotification(Notification.FromType(GetType(), "Transaction was not commited"));
            }

            return ToModel(entity);
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

            entity = await DeleteInternal(entity);

            if (!await CommitAsync())
            {
                RaiseNotification(Notification.FromType(GetType(), "Transaction was not commited"));
            }

            return ToModel(entity);
        }



        //protected internal virtual IQueryable<TModel> Project(IQueryable<TEntity> entities)
        //{
        //    var entityQuery = entities;
        //    var projectedQuery = Projection.Invoke(entityQuery);
        //    return projectedQuery.Cast<TModel>();
        //}

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

            return Project<TModel>(entityQuery);
        }

    }
}