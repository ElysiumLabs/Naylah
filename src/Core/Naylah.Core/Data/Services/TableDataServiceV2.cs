using System.Threading.Tasks;
using System.Linq;
using Naylah.Data;
using Naylah.Data.Access;
using System;
using Naylah.Data.Extensions;
using System.Linq.Expressions;

namespace Naylah.Core.Data.Services
{
    ///// <summary>
    ///// An abstract service that only encapsulates the <typeparamref name="TEntity"/> and the <typeparamref name="TIdentifier"/>
    ///// </summary>
    ///// <remarks>All models must have an <see cref="IEntityUpdate{TModel}"/> implemented on the <typeparamref name="TEntity"/>.</remarks>
    ///// <typeparam name="TEntity">Entity type.</typeparam>
    ///// <typeparam name="TIdentifier">Identifier type.</typeparam>
    //public class TableDataServiceV2<TEntity, TIdentifier>
    //    : TableDataServiceBase<TEntity, TIdentifier>
    //    where TEntity : class, IEntity<TIdentifier>, IModifiable, new()
    //{
    //    /// <summary>
    //    /// Method to check if the inferred model is assignable from the <typeparamref name="TEntity"/>.
    //    /// </summary>
    //    /// <typeparam name="TModel">An implemented <see cref="IEntity{TIdentifier}"/>.</typeparam>
    //    /// <returns>true if assignable, otherwise throws <see cref="NotAssignableException"/>.</returns>
    //    /// <exception cref="NotAssignableException"><typeparamref name="TModel"/> is not implemented in <typeparamref name="TEntity"/>.</exception>
    //    private bool IsModelAssignable<TModel>()
    //    {
    //        if (!typeof(IEntityUpdate<TModel>).IsAssignableFrom(typeof(TEntity)))
    //        {
    //            throw new NotAssignableException($"Cannot assign {nameof(TEntity)} into {nameof(IEntityUpdate<TModel>)}.");
    //        }

    //        return true;
    //    }

    //    /// <summary>
    //    /// Dinamically calls the <see cref="IEntityUpdate{TSource}.UpdateFrom(TSource, EntityUpdateOptions)"/> method.
    //    /// </summary>
    //    /// <param name="baseEntity">Entity to be updated.</param>
    //    /// <param name="model">Model to update the entity from.</param>
    //    /// <param name="upsertType">Upsert type.</param>
    //    /// <typeparam name="TModel">An implemented <see cref="IEntity{TIdentifier}"/>.</typeparam>
    //    /// <returns>The <typeparamref name="TEntity"/> updated.</returns>
    //    private TEntity DynamicUpdateFrom<TModel>(
    //        TEntity baseEntity,
    //        TModel model,
    //        UpsertType upsertType)
    //    {
    //        IsModelAssignable<TModel>();

    //        var genericTypeWithUpdateFrom = typeof(IEntityUpdate<>).MakeGenericType(typeof(TModel));
    //        var updateFromMethod = genericTypeWithUpdateFrom.GetMethod(nameof(IEntityUpdate<TModel>.UpdateFrom));

    //        updateFromMethod.Invoke(baseEntity, new object[] { model, new EntityUpdateOptions(upsertType) });

    //        return baseEntity;
    //    }

    //    /// <summary>
    //    /// Base constructor responsible for obtaining required services through DI.
    //    /// </summary>
    //    /// <param name="repository"></param>
    //    /// <param name="unitOfWork"></param>
    //    public TableDataServiceV2(
    //        IRepository<TEntity, TIdentifier> repository,
    //        IUnitOfWork unitOfWork)
    //        : base(repository, unitOfWork)
    //    {
    //    }

    //    /// <summary>
    //    /// Base constructor responsible for obtaining required services through DI.
    //    /// </summary>
    //    public TableDataServiceV2(
    //        IRepository<TEntity, TIdentifier> repository,
    //        IUnitOfWork unitOfWork,
    //        Domain.Abstractions.IHandler<Notification> notificationsHandler)
    //        : base(repository, unitOfWork, notificationsHandler)
    //    {
    //    }

    //    protected internal virtual IQueryable<TModel> Projection<TModel>(
    //        IQueryable<TEntity> entities)
    //        where TModel : class, IEntity<TIdentifier>, new()
    //    {
    //        IsModelAssignable<TModel>();
    //        return entities.Project().To<TModel>();
    //    }

    //    /// <summary>
    //    /// Cast the <typeparamref name="TEntity"/> into a <typeparamref name="TModel"/>.
    //    /// </summary>
    //    /// <param name="model">Model to be casted.</param>
    //    /// <param name="upsertType">Upsert type to be called inside the <see cref="IEntityUpdate{TSource}.UpdateFrom(TSource, EntityUpdateOptions)"/> method.</param>
    //    /// <typeparam name="TModel">Model type.</typeparam>
    //    /// <returns>The <typeparamref name="TEntity"/>.</returns>
    //    /// <exception cref="NotAssignableException"><typeparamref name="TModel"/> is not assignable.</exception>
    //    protected internal virtual TEntity ToEntity<TModel>(
    //        TModel model,
    //        UpsertType upsertType)
    //        where TModel : class, IEntity<TIdentifier>, new()
    //    {
    //        IsModelAssignable<TModel>();
    //        return DynamicUpdateFrom(Entity.Create<TEntity>(), model, upsertType);
    //    }

    //    private async Task<TEntity> EntityUpdateInternalAsync<TModel>(
    //        TEntity entity, TModel model)
    //        where TModel : class, IEntity<TIdentifier>, new()
    //    {
    //        IsModelAssignable<TModel>();
    //        if (entity == null)
    //        {
    //            RaiseNotification(Notification.FromType(GetType(), "Entity is null"));
    //        }

    //        DynamicUpdateFrom(entity, model, UpsertType.Update);
    //        entity.UpdateUpdateAt();

    //        entity = await Repository.EditAsync(entity);

    //        if (!await CommitAsync())
    //        {
    //            RaiseNotification(Notification.FromType(GetType(), "Transaction was not commited"));
    //        }
    //        return entity;
    //    }

    //    protected virtual async Task<TModel> UpdateInternalAsync<TModel>(
    //        TEntity entity,
    //        TModel model)
    //        where TModel : class, IEntity<TIdentifier>, new()
    //    {
    //        var entityResult = await EntityUpdateInternalAsync(entity, model);
    //        return ToModel<TModel>(entityResult);
    //    }

    //    protected virtual async Task<TModelOut> UpdateInternalAsync<TModelIn, TModelOut>(
    //        TEntity entity, TModelIn model)
    //        where TModelIn : class, IEntity<TIdentifier>, new()
    //        where TModelOut : class, IEntity<TIdentifier>, new()
    //    {
    //        IsModelAssignable<TModelIn>();
    //        IsModelAssignable<TModelOut>();
    //        var entityResult = await EntityUpdateInternalAsync(entity, model);
    //        return ToModel<TModelOut>(entityResult);
    //    }

    //    private async Task<TEntity> EntityCreate<TModel>(
    //        TModel model)
    //        where TModel : class, IEntity<TIdentifier>, new()
    //    {
    //        IsModelAssignable<TModel>();
    //        var entity = ToEntity(model, UpsertType.Insert);
    //        await GenerateId(entity);

    //        entity.UpdateCreatedAt();
    //        await Repository.AddAsync(entity);

    //        if (!await CommitAsync())
    //        {
    //            RaiseNotification(Notification.FromType(GetType(), "Transaction was not commited"));
    //        }

    //        return (entity);
    //    }

    //    public virtual async Task<TModel> Create<TModel>(
    //        TModel model)
    //        where TModel : class, IEntity<TIdentifier>, new()
    //    {
    //        var result = await EntityCreate(model);
    //        return ToModel<TModel>(result);
    //    }

    //    public virtual async Task<TModelOut> Create<TModelIn, TModelOut>(
    //        TModelIn model)
    //        where TModelIn : class, IEntity<TIdentifier>, new()
    //        where TModelOut : class, IEntity<TIdentifier>, new()
    //    {
    //        IsModelAssignable<TModelOut>();
    //        var result = await EntityCreate(model);
    //        return ToModel<TModelOut>(result);
    //    }

    //    public virtual async Task<TModelOut> UpdateAsync<TModelIn, TModelOut>(
    //        TModelIn model, Expression<Func<TEntity, bool>> customPredicate = null)
    //        where TModelIn : class, IEntity<TIdentifier>, new()
    //        where TModelOut : class, IEntity<TIdentifier>, new()
    //    {
    //        IsModelAssignable<TModelIn>();
    //        IsModelAssignable<TModelOut>();
    //        var entity = customPredicate != null ? await FindByAsync(customPredicate) : await FindByIdAsync(model.Id);

    //        if (entity == null)
    //        {
    //            RaiseNotification(Notification.FromType(GetType(), "Entity not found"));
    //        }

    //        return await UpdateInternalAsync<TModelIn, TModelOut>(entity, model);
    //    }

    //    public virtual async Task<TModel> UpsertAsync<TModel>(
    //        TModel model,
    //        Expression<Func<TEntity, bool>> customPredicate = null)
    //        where TModel : class, IEntity<TIdentifier>, new()
    //    {
    //        IsModelAssignable<TModel>();
    //        var entity = customPredicate != null ? await FindByAsync(customPredicate) : await FindByIdAsync(model.Id);

    //        if (entity == null)
    //        {
    //            return await Create(model);
    //        }
    //        else
    //        {
    //            return await UpdateInternalAsync(entity, model);
    //        }
    //    }

    //    public virtual async Task<TModelOut> UpsertAsync<TModelIn, TModelOut>(
    //        TModelIn model,
    //        Expression<Func<TEntity, bool>> customPredicate = null)
    //        where TModelIn : class, IEntity<TIdentifier>, new()
    //        where TModelOut : class, IEntity<TIdentifier>, new()
    //    {
    //        IsModelAssignable<TModelIn>();
    //        IsModelAssignable<TModelOut>();
    //        var entity = customPredicate != null ? await FindByAsync(customPredicate) : await FindByIdAsync(model.Id);

    //        if (entity == null)
    //        {
    //            return await Create<TModelIn, TModelOut>(model);
    //        }
    //        else
    //        {
    //            return await UpdateInternalAsync<TModelIn, TModelOut>(entity, model);
    //        }
    //    }

    //    public virtual async Task<TModel> GetById<TModel>(
    //        TIdentifier id)
    //        where TModel : class, IEntity<TIdentifier>, new()
    //    {
    //        IsModelAssignable<TModel>();
    //        var entity = await FindByIdAsync(id);

    //        if (entity == null)
    //        {
    //            RaiseNotification(Notification.FromType(GetType(), "Entity not found"));
    //        }

    //        return ToModel<TModel>(entity);
    //    }

    //    public virtual async Task<TModel> Delete<TModel>(
    //        TIdentifier id)
    //        where TModel : class, IEntity<TIdentifier>, new()
    //    {
    //        IsModelAssignable<TModel>();
    //        var entity = await FindByIdAsync(id);

    //        if (entity == null)
    //        {
    //            RaiseNotification(Notification.FromType(GetType(), "Entity not found"));
    //        }

    //        if (!UseSoftDelete)
    //        {
    //            await Repository.RemoveAsync(entity);
    //        }
    //        else
    //        {
    //            entity.Deleted = true;
    //            entity = await Repository.EditAsync(entity);
    //        }

    //        if (!await CommitAsync())
    //        {
    //            RaiseNotification(Notification.FromType(GetType(), "Transaction was not commited"));
    //        }

    //        return ToModel<TModel>(entity);
    //    }

    //    protected internal virtual IQueryable<TModel> Project<TModel>(
    //        IQueryable<TEntity> entities)
    //        where TModel : class, IEntity<TIdentifier>, new()
    //    {
    //        IsModelAssignable<TModel>();
    //        var entityQuery = entities;
    //        var projectedQuery = Projection<TModel>(entityQuery);
    //        return projectedQuery;
    //    }

    //    public virtual IQueryable<TModel> GetAll<TModel>(
    //        IQueryable<TEntity> adptedEntities = null)
    //        where TModel : class, IEntity<TIdentifier>, new()
    //    {
    //        IsModelAssignable<TModel>();
    //        var entityQuery = adptedEntities ?? GetEntities();

    //        if (UseSoftDelete)
    //        {
    //            entityQuery = entityQuery.Where(x => !x.Deleted);
    //        }

    //        if (Ordering != null)
    //        {
    //            entityQuery = Ordering.Invoke(entityQuery);
    //        }

    //        return Project<TModel>(entityQuery);
    //    }
    //}
}