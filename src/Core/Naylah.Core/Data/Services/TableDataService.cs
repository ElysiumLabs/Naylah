using Naylah.Data.Abstractions;
using Naylah.Data.Access;
using Naylah.Data.Extensions;
using Naylah.Domain.Abstractions;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Naylah.Data.Services
{
    public abstract class TableDataService<TEntity, TModel, TIdentifier> : DataServiceBase
        where TEntity : class, IEntity<TIdentifier>, IModifiable, IEntityUpdate<TModel>, new()
        where TModel : class, IEntity<TIdentifier>, new()
    {
        protected IRepository<TEntity, TIdentifier> Repository;

        protected internal virtual Func<IQueryable<TEntity>, IQueryable<TModel>> Projection { get; set; } =
            (q) => q.Project().To<TModel>();

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

        protected virtual TModel ToModel(TEntity entity)
        {
            return new TModel() { Id = entity.Id };
        }

        protected virtual TEntity FindBy(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            return Repository.GetAllAsQueryable(includes).Where(predicate).FirstOrDefault();
        }

        protected virtual TEntity FindById(TIdentifier identifier, params Expression<Func<TEntity, object>>[] includes)
        {
            return FindBy(x => x.Id.Equals(identifier), includes);
        }

        protected internal virtual IQueryable<TEntity> GetEntities()
        {
            return Repository.GetAllAsQueryable().Where(x => !x.Deleted);
        }

        protected virtual void GenerateId(TEntity entity)
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

        protected virtual TModel UpdateInternal(TEntity entity, TModel model)
        {
            if (entity == null)
            {
                RaiseNotification(Notification.FromType(GetType(), "Entity is null"));
            }

            entity.UpdateFrom(model, new EntityUpdateOptions(UpsertType.Update));
            entity.UpdateUpdateAt();

            Repository.Update(entity);

            if (!Commit())
            {
                RaiseNotification(Notification.FromType(GetType(), "Transaction was not commited"));
            }

            return ToModel(entity);
        }

        public virtual TModel Create(TModel model)
        {
            var entity = Entity.Create<TEntity>();
            GenerateId(entity);

            entity.UpdateFrom(model, new EntityUpdateOptions(UpsertType.Insert));
            entity.UpdateCreatedAt();

            Repository.Create(entity);

            if (!Commit())
            {
                RaiseNotification(Notification.FromType(GetType(), "Transaction was not commited"));
            }

            return ToModel(entity);
        }

        public virtual TModel Update(TModel model, Expression<Func<TEntity, bool>> customPredicate = null)
        {
            var entity = customPredicate != null ? FindBy(customPredicate) : FindById(model.Id);

            if (entity == null)
            {
                RaiseNotification(Notification.FromType(GetType(), "Entity not found"));
            }

            return UpdateInternal(entity, model);
        }

        public virtual TModel Upsert(TModel model, Expression<Func<TEntity, bool>> customPredicate = null)
        {
            var entity = customPredicate != null ? FindBy(customPredicate) : FindById(model.Id);

            if (entity == null)
            {
                return Create(model);
            }
            else
            {
                return UpdateInternal(entity, model);
            }
        }

        public virtual TModel GetById(TIdentifier id, params Expression<Func<TEntity, object>>[] includes)
        {
            var entity = FindById(id);

            if (entity == null)
            {
                RaiseNotification(Notification.FromType(GetType(), "Entity not found"));
            }

            return ToModel(entity);
        }

        public virtual TModel Delete(TIdentifier id)
        {
            var entity = Repository.GetById(id);

            if (entity == null)
            {
                RaiseNotification(Notification.FromType(GetType(), "Entity not found"));
            }

            if (!UseSoftDelete)
            {
                Repository.Delete(entity);
            }
            else
            {
                entity.Deleted = true;
                Repository.Update(entity);
            }

            if (!Commit())
            {
                RaiseNotification(Notification.FromType(GetType(), "Transaction was not commited"));
            }

            return ToModel(entity);
        }

        public virtual IQueryable<TModel> GetAll(params Expression<Func<TEntity, bool>>[] predicates)
        {
            var entityQuery = GetEntities();

            if (predicates != null)
            {
                foreach (var predicate in predicates)
                {
                    entityQuery = entityQuery.Where(predicate);
                }
            }

            var projectedQuery = Projection.Invoke(entityQuery);
            return projectedQuery;
        }

    }

    public abstract class StringTableDataService<TEntity, TModel> : TableDataService<TEntity, TModel, string>
       where TEntity : class, IEntityUpdate<TModel>, IEntity<string>, IModifiable, new()
       where TModel : class, IEntity<string>, new()
    {
        public StringTableDataService(IUnitOfWork _unitOfWork, IRepository<TEntity, string> repository) : base(_unitOfWork, repository)
        {
        }

        protected override TEntity FindById(string identifier, params Expression<Func<TEntity, object>>[] includes)
        {
            return FindBy(x => x.Id == identifier, includes);
        }

        protected override void GenerateId(TEntity entity)
        {
            entity.GenerateId();
        }
    }

    public abstract class IntTableDataService<TEntity, TModel> : TableDataService<TEntity, TModel, int>
       where TEntity : class, IEntityUpdate<TModel>, IEntity<int>, IModifiable, new()
       where TModel : class, IEntity<int>, new()
    {
        public IntTableDataService(IUnitOfWork _unitOfWork, IRepository<TEntity, int> repository) : base(_unitOfWork, repository)
        {
        }

        protected override TEntity FindById(int identifier, params Expression<Func<TEntity, object>>[] includes)
        {
            return FindBy(x => x.Id == identifier, includes);
        }

    }
}