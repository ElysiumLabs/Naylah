using Naylah.Data.Abstractions;
using Naylah.Data.Access;
using Naylah.Data.Extensions;
using Naylah.Domain.Abstractions;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Naylah.Services
{
    public class TableEntityDataService<TEntity, TModel, TIdentifier> : DataService
        where TEntity : class, IEntity<TIdentifier>, IEntityDataActionCRUDModel<TModel>, new()
        where TModel : class, new()
    {
        public bool UseSoftDelete { get; set; } = false;

        protected IRepository<TEntity, TIdentifier> repository;

        public TableEntityDataService(
           IUnitOfWork _unitOfWork,
           IRepository<TEntity, TIdentifier> repository
           ) : this(_unitOfWork, repository, null)
        {
        }

        public TableEntityDataService(
            IUnitOfWork _unitOfWork,
            IRepository<TEntity, TIdentifier> repository,
            IHandler<Notification> notificationHandler
            ) : base(_unitOfWork, notificationHandler)
        {
            this.repository = repository;
        }

        public virtual TModel Add(TModel model)
        {
            var entity = Entity.Create<TEntity>();

            entity.CreateFrom(model);

            repository.Create(entity);

            return
                Commit()
                ? entity.ReadTo()
                : null;
        }

        public virtual TModel Update(TIdentifier id, TModel model)
        {
            var entity = repository.GetById(id);

            if (entity == null)
            {
                throw new Exception("Entity not found");
            }

            entity.UpdateFrom(model);

            repository.Update(entity);

            return Commit()
                ? entity.ReadTo()
                : null;
        }

        public virtual TModel GetById(TIdentifier id, params Expression<Func<TEntity, object>>[] includes)
        {
            var entity = repository.GetById(id, includes);

            if (entity == null)
            {
                throw new Exception("Entity not found");
            }

            return entity.ReadTo();
        }

        public virtual TModel Delete(TIdentifier id)
        {
            var entity = repository.GetById(id);

            if (entity == null)
            {
                throw new Exception("Entity not found");
            }

            var m = entity.ReadTo();

            entity.DeleteFrom(m, UseSoftDelete);

            if (!UseSoftDelete)
            {
                repository.Delete(entity);
            }
            else
            {
                return Update(entity.Id, m);
            }

            Commit();

            return null;
        }

        public virtual IQueryable<TModel> GetAll()
        {
            var q =
                repository
                .GetAllAsQueryable()
                .Where(x => !x.Deleted)
                .Select(x => x.ReadTo())
                .Project().To<TModel>();

            return q;
        }
    }
}