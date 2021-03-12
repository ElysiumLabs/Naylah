using Naylah.Data.Access;
using Naylah.Domain.Abstractions;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Naylah.Data
{
    public abstract class TableDataServiceBase<TEntity, TIdentifier> : QueryDataService<TEntity, TIdentifier>
        where TEntity : class, IEntity<TIdentifier>, IModifiable, new()
    {
        public TableDataServiceBase(IRepository<TEntity, TIdentifier> repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
        }

        public TableDataServiceBase(IRepository<TEntity, TIdentifier> repository, IUnitOfWork unitOfWork, IHandler<Notification> notificationsHandler) : base(repository, unitOfWork, notificationsHandler)
        {
        }

        //change later
        public virtual Task GenerateId(TEntity entity)
        {
            //application id generation...
            return Task.FromResult(1);
        }

        protected abstract Task<TCustomModel> CreateAsync<TCustomModel>(TCustomModel model)
            where TCustomModel : class, IEntity<TIdentifier>, new();

        protected abstract Task<TCustomModel> UpdateAsync<TCustomModel>(TCustomModel model, Expression<Func<TEntity, bool>> customPredicate = null)
            where TCustomModel : class, IEntity<TIdentifier>, new();

        protected abstract Task<TCustomModel> UpsertAsync<TCustomModel>(TCustomModel model, Expression<Func<TEntity, bool>> customPredicate = null)
            where TCustomModel : class, IEntity<TIdentifier>, new();

        protected abstract Task<TCustomModel> DeleteAsync<TCustomModel>(TIdentifier identifier)
            where TCustomModel : class, IEntity<TIdentifier>, new();

        protected abstract Task<TCustomModel> DeleteAsync<TCustomModel>(TCustomModel model)
            where TCustomModel : class, IEntity<TIdentifier>, new();

        protected internal virtual TEntity CreateEntity(TIdentifier identifier, UpsertType upsertType)
        {
            var entity = Activator.CreateInstance<TEntity>();
            entity.Id = identifier;

            return entity;
        }

        protected internal virtual TEntity CreateEntity(object model, UpsertType upsertType)
        {
            var entity = Activator.CreateInstance<TEntity>();
            UpdateEntity(entity, model, upsertType);
            return entity;
        }

        protected internal abstract TEntity UpdateEntity(TEntity entity, object model, UpsertType upsertType);


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