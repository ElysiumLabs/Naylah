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
    public abstract class TableDataService<TEntity, TModel, TIdentifier> : TableDataServiceBase<TEntity, TIdentifier>
        where TEntity : class, IEntity<TIdentifier>, IEntityUpdate<TModel>, IModifiable, new()
        where TModel : class, IEntity<TIdentifier>, new()
    {
        public TableDataService(IRepository<TEntity, TIdentifier> repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
        }

        public TableDataService(IRepository<TEntity, TIdentifier> repository, IUnitOfWork unitOfWork, IHandler<Notification> notificationsHandler) : base(repository, unitOfWork, notificationsHandler)
        {
        }

        public IQueryable<TModel> GetAll()
        {
            return GetAll<TModel>();
        }

        public async Task<TModel> GetByIdAsync(TIdentifier id)
        {
            return await GetByIdAsync<TModel>(id);
        }

        public virtual async Task<TModel> CreateAsync(TModel model)
        {
            return await CreateAsync<TModel>(model);
        }

        public virtual async Task<TModel> UpdateAsync(TModel model)
        {
            return await UpdateAsync<TModel>(model);
        }

        public virtual async Task<TModel> UpsertAsync(TModel model)
        {
            return await UpsertAsync<TModel>(model);
        }

        public async Task<TModel> DeleteAsync(TIdentifier identifier)
        {
            return await DeleteAsync<TModel>(identifier);
        }


        protected override internal TEntity UpdateEntity(TEntity entity, object model, UpsertType upsertType)
        {
            if (!(model is TModel tmodel))
            {
                throw new Exception("not good");
            }

            entity.Id = tmodel.Id;

            entity.UpdateFrom(tmodel);
            return entity;
        }

        protected override async Task<TCustomModel> CreateAsync<TCustomModel>(TCustomModel model)
        {
            var entity = CreateEntity(model, UpsertType.Insert);
            await GenerateId(entity);

            entity = await CreateInternalAsync(entity);

            if (!await CommitAsync())
            {
                RaiseNotification(Notification.FromType(GetType(), "Transaction was not commited"));
            }

            return ToModel<TCustomModel>(entity);
        }

        protected override async Task<TCustomModel> UpdateAsync<TCustomModel>(TCustomModel model, Expression<Func<TEntity, bool>> customPredicate = null)
        {
            var entity = customPredicate != null ? await FindByAsync(customPredicate) : await FindByIdAsync(model.Id);

            if (entity == null)
            {
                RaiseNotification(Notification.FromType(GetType(), "Entity not found"));
            }

            UpdateEntity(entity, model, UpsertType.Update);

            entity = await UpdateInternalAsync(entity);

            if (!await CommitAsync())
            {
                RaiseNotification(Notification.FromType(GetType(), "Transaction was not commited"));
            }

            return ToModel<TCustomModel>(entity);
        }

        protected override async Task<TCustomModel> UpsertAsync<TCustomModel>(TCustomModel model, Expression<Func<TEntity, bool>> customPredicate = null)
        {
            var entity = customPredicate != null ? await FindByAsync(customPredicate) : await FindByIdAsync(model.Id);

            if (entity == null)
            {
                entity = CreateEntity(model, UpsertType.Insert);
                await GenerateId(entity);

                entity = await CreateInternalAsync(entity);
            }
            else
            {
                UpdateEntity(entity, model, UpsertType.Update);

                entity = await UpdateInternalAsync(entity);
            }

            if (!await CommitAsync())
            {
                RaiseNotification(Notification.FromType(GetType(), "Transaction was not commited"));
            }

            return ToModel<TCustomModel>(entity);
        }

        protected override async Task<TCustomModel> DeleteAsync<TCustomModel>(TIdentifier identifier)
        {
            var entity = await FindByIdAsync(identifier);

            if (entity == null)
            {
                RaiseNotification(Notification.FromType(GetType(), "Entity not found"));
            }

            entity = await DeleteInternal(entity);

            if (!await CommitAsync())
            {
                RaiseNotification(Notification.FromType(GetType(), "Transaction was not commited"));
            }

            return ToModel<TCustomModel>(entity);
        }

        protected override async Task<TCustomModel> DeleteAsync<TCustomModel>(TCustomModel model)
        {
            return await DeleteAsync<TCustomModel>(model.Id);
        }

        protected async Task<TEntity> CreateInternalAsync(TEntity entity, TModel model)
        {
            return await CreateInternalAsync(entity, model);
        }

        


    }
}