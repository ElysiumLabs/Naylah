using Naylah.Data.Access;
using Naylah.Domain.Abstractions;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Naylah.Data
{
    public abstract class TableDataServiceBase<TEntity, TIdentifier> : QueryDataService<TEntity, TIdentifier>
        where TEntity : class, IEntity<TIdentifier>, IModifiable, ISoftDeletable, new()
    {
        public TableDataServiceBase(IRepository<TEntity> repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
        }

        public TableDataServiceBase(IRepository<TEntity> repository, IUnitOfWork unitOfWork, IHandler<Notification> notificationsHandler) : base(repository, unitOfWork, notificationsHandler)
        {
        }

        protected internal virtual Task GenerateId(TEntity entity)
        {
            //application id generation...
            return Task.FromResult(1);
        }

        internal virtual TEntity CreateEntity(TIdentifier identifier)
        {
            var entity = Activator.CreateInstance<TEntity>();
            entity.Id = identifier;
            return entity;
        }

        internal virtual TEntity CreateEntity(object model)
        {
            var entity = Activator.CreateInstance<TEntity>();
            UpdateEntityInternal(entity, model, UpsertType.Insert);
            return entity;
        }

        internal virtual TEntity UpdateEntity(TEntity entity, object model)
        {
            UpdateEntityInternal(entity, model, UpsertType.Update);
            return entity;
        }

        internal abstract TEntity UpdateEntityInternal(TEntity entity, object model, UpsertType upsertType);

        internal abstract Task<TCustomModel> CreateAsync<TCustomModel>(TCustomModel model)
            where TCustomModel : class, IEntity<TIdentifier>, new();

        internal abstract Task<TCustomModel> UpdateAsync<TCustomModel>(TCustomModel model, Expression<Func<TEntity, bool>> customPredicate = null)
            where TCustomModel : class, IEntity<TIdentifier>, new();

        internal abstract Task<TCustomModel> UpsertAsync<TCustomModel>(TCustomModel model, Expression<Func<TEntity, bool>> customPredicate = null)
            where TCustomModel : class, IEntity<TIdentifier>, new();

        internal abstract Task<TCustomModel> DeleteAsync<TCustomModel>(TIdentifier identifier)
            where TCustomModel : class, IEntity<TIdentifier>, new();

        internal abstract Task<TCustomModel> DeleteAsync<TCustomModel>(TCustomModel model)
            where TCustomModel : class, IEntity<TIdentifier>, new();

        
        protected virtual async Task<TEntity> CreateInternalAsync(TEntity entity)
        {
            if (entity == null)
            {
                RaiseNotification(Notification.FromType(GetType(), "Entity is null"));
            }

            entity.UpdateCreatedAt();
            entity = await Repository.AddAsync(entity);
            return entity;
        }

        protected virtual async Task<TEntity> UpdateInternalAsync(TEntity entity)
        {
            if (entity == null)
            {
                RaiseNotification(Notification.FromType(GetType(), "Entity is null"));
            }

            entity.UpdateUpdateAt();
            entity = await Repository.EditAsync(entity);
            return entity;
        }

        protected virtual async Task<TEntity> DeleteInternal(TEntity entity)
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
    }
}