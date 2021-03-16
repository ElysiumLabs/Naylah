using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Naylah.Data
{
    public class TableDataServiceCustom<TEntity, TIdentifier>
        : TableDataServiceBase<TEntity, TIdentifier>
        where TEntity : class, IEntity<TIdentifier>, IModifiable, new()
    {
        public TableDataServiceCustom(
            IRepository<TEntity, TIdentifier> repository,
            Access.IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }

        public TableDataServiceCustom(
            IRepository<TEntity, TIdentifier> repository,
            Access.IUnitOfWork unitOfWork,
            Domain.Abstractions.IHandler<Notification> notificationsHandler)
            : base(repository, unitOfWork, notificationsHandler)
        {
        }

        private void ValidateModel<TCustomModel>()
        {
            if (!typeof(IEntityUpdate<TCustomModel>).IsAssignableFrom(typeof(TEntity)))
                throw new NotAssignableException($"Cannot assign {nameof(TEntity)} into {nameof(IEntityUpdate<TCustomModel>)}");
        }

        public virtual IQueryable<TCustomModel> GetAll<TCustomModel>()
            where TCustomModel : class, IEntity<TIdentifier>, new()
        {
            return base.GetAll<TCustomModel>(base.GetEntities());
        }

        public new virtual async Task<TCustomModel> GetByIdAsync<TCustomModel>(
            TIdentifier id)
            where TCustomModel : class, IEntity<TIdentifier>, new()
        {
            return await base.GetByIdAsync<TCustomModel>(id);
        }

        public virtual async Task<TCustomModel> FindByAsync<TCustomModel>(
            Expression<Func<TEntity, bool>> @where)
            where TCustomModel : class, IEntity<TIdentifier>, new()
        {
            var entity = await base.FindByAsync(where);
            return ToModel<TCustomModel>(entity);
        }

        public virtual async Task<TModel> CreateEntityAsync<TModel>(
            TModel model)
            where TModel : class, IEntity<TIdentifier>, new()
        {
            return await this.CreateAsync(model);
        }

        public virtual async Task<TModelOut> CreateEntityAsync<TModelIn, TModelOut>(
            TModelIn model)
            where TModelIn : class, IEntity<TIdentifier>, new()
            where TModelOut : class, IEntity<TIdentifier>, new()
        {
            var createdEntity = await CreateAsync(model);
            var result = await this.FindByIdAsync(createdEntity.Id);

            return ToModel<TModelOut>(result);
        }

        public virtual async Task<TModel> UpdateEntityAsync<TModel>(
            TModel model,
            Expression<Func<TEntity, bool>> @where)
            where TModel : class, IEntity<TIdentifier>, new()
        {
            return await this.UpdateAsync(model, where);
        }

        public virtual async Task<TModel> UpdateEntityAsync<TModel>(
            TModel model)
            where TModel : class, IEntity<TIdentifier>, new()
        {
            return await this.UpdateAsync(model, null);
        }

        public virtual async Task<TModelOut> UpdateEntityAsync<TModelIn, TModelOut>(
            TModelIn model,
            Expression<Func<TEntity, bool>> @where)
            where TModelIn : class, IEntity<TIdentifier>, new()
            where TModelOut : class, IEntity<TIdentifier>, new()
        {
            var updatedEntity = await UpdateAsync(model, where);
            var result = await this.FindByIdAsync(updatedEntity.Id);

            return ToModel<TModelOut
            >(result);
        }
        public virtual async Task<TModelOut> UpdateEntityAsync<TModelIn, TModelOut>(
            TModelIn model)
            where TModelIn : class, IEntity<TIdentifier>, new()
            where TModelOut : class, IEntity<TIdentifier>, new()
        {
            return await this.UpdateEntityAsync<TModelIn, TModelOut>(model, null);
        }

        public virtual async Task<TModel> UpsertEntityAsync<TModel>(
            TModel model,
            Expression<Func<TEntity, bool>> @where)
            where TModel : class, IEntity<TIdentifier>, new()
        {
            return await this.UpsertAsync(model, where);
        }

        public virtual async Task<TModel> UpsertEntityAsync<TModel>(
            TModel model)
            where TModel : class, IEntity<TIdentifier>, new()
        {
            return await this.UpsertAsync(model, null);
        }

        public virtual async Task<TModelOut> UpsertEntityAsync<TModelIn, TModelOut>(
            TModelIn model,
            Expression<Func<TEntity, bool>> @where)
            where TModelIn : class, IEntity<TIdentifier>, new()
            where TModelOut : class, IEntity<TIdentifier>, new()
        {
            var updatedEntity = await UpsertAsync(model, where);
            var result = await this.FindByIdAsync(updatedEntity.Id);

            return ToModel<TModelOut>(result);
        }

        public virtual async Task<TModelOut> UpsertEntityAsync<TModelIn, TModelOut>(
            TModelIn model)
            where TModelIn : class, IEntity<TIdentifier>, new()
            where TModelOut : class, IEntity<TIdentifier>, new()
        {
            return await this.UpsertEntityAsync<TModelIn, TModelOut>(model, null);
        }

        public virtual async Task<TModel> DeleteEntityAsync<TModel>(
            TModel model)
            where TModel : class, IEntity<TIdentifier>, new()
        {
            return await this.DeleteAsync(model);
        }

        public virtual async Task<TModel> DeleteEntityAsync<TModel>(
            TIdentifier id)
            where TModel : class, IEntity<TIdentifier>, new()
        {
            return await this.DeleteAsync<TModel>(id);
        }

        internal override TEntity UpdateEntity(
            TEntity entity, object model, UpsertType upsertType)
        {
            var entityType = entity.GetType();

            var updateFromMethods = entityType.GetMethods()
                .Where(w => w.Name.Equals("UpdateFrom", StringComparison.OrdinalIgnoreCase));

            var actualUpdateFromMethod = updateFromMethods
                .SingleOrDefault(m => m.GetParameters()
                                        .Any(p => p.ParameterType == model.GetType()));

            actualUpdateFromMethod?.Invoke(entity, new object[] { model, new EntityUpdateOptions(upsertType) });
            return entity;
        }

        internal override async Task<TCustomModel> DeleteAsync<TCustomModel>(
            TIdentifier identifier)
        {
            ValidateModel<TCustomModel>();

            var model = await base.GetByIdAsync<TCustomModel>(identifier);

            return await DeleteAsync(model);
        }

        internal override async Task<TCustomModel> DeleteAsync<TCustomModel>(
            TCustomModel model)
        {
            ValidateModel<TCustomModel>();

            var entity = UpdateEntity(
                Activator.CreateInstance<TEntity>(),
                model, UpsertType.Instance);
            entity.Id = model.Id;

            await base.DeleteInternal(entity);
            return model;
        }

        internal override async Task<TCustomModel> CreateAsync<TCustomModel>(
            TCustomModel model)
        {
            ValidateModel<TCustomModel>();

            var entity = UpdateEntity(
                Activator.CreateInstance<TEntity>(),
                model, UpsertType.Insert);

            await base.GenerateId(entity);
            entity = await base.CreateInternalAsync(entity);

            if (!await CommitAsync())
            {
                RaiseNotification(Notification.FromType(GetType(), "Transaction was not commited"));
            }

            return ToModel<TCustomModel>(entity);
        }

        internal override async Task<TCustomModel> UpdateAsync<TCustomModel>(
            TCustomModel model,
            Expression<Func<TEntity, bool>> customPredicate = null)
        {
            ValidateModel<TCustomModel>();

            var entity = customPredicate != null ?
                await FindByAsync(customPredicate)
                : await FindByIdAsync(model.Id);

            if (entity == null)
            {
                RaiseNotification(Notification.FromType(GetType(), "Entity not found"));
            }

            UpdateEntity(entity, model, UpsertType.Update);

            if (!await CommitAsync())
            {
                RaiseNotification(Notification.FromType(GetType(), "Transaction was not commited"));
            }

            entity = await base.UpdateInternalAsync(entity);
            return ToModel<TCustomModel>(entity);
        }

        internal override async Task<TCustomModel> UpsertAsync<TCustomModel>(
            TCustomModel model,
            Expression<Func<TEntity, bool>> customPredicate = null)
        {
            var entity = customPredicate != null ?
                await FindByAsync(customPredicate)
                : await FindByIdAsync(model.Id);

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
    }
}