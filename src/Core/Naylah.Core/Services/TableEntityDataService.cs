using Naylah.Data.Abstractions;
using Naylah.Data.Access;
using Naylah.Domain;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Naylah.Services
{
    public class TableEntityDataService<TEntity, TModel> : DataService
        where TEntity : Entity, IEntityDataActionCRUDModel<TModel>, new()
        where TModel : class, new()
    {
        public bool UseSoftDelete { get; set; } = false;

        protected IRepository<TEntity> repository;

        public TableEntityDataService(
            IUnitOfWork _unitOfWork,
            IRepository<TEntity> repository
            ) : base(_unitOfWork)
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

        public virtual TModel Update(string id, TModel model)
        {
            var entity = repository.GetById(id?.Trim());

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

        public virtual TModel GetById(string id, params Expression<Func<TEntity, object>>[] includes)
        {
            var entity = repository.GetById(id, includes);

            if (entity == null)
            {
                throw new Exception("Entity not found");
            }

            return entity.ReadTo();
        }

        public virtual TModel Delete(string id)
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

        public virtual IQueryable<TModel> GetAll(Expression<Func<TEntity, object>>[] includes, int? skip = null, int? take = null)
        {
            return repository
                .GetAll(null, includes, skip, take)
                .Where(x => !x.Deleted)
                .ToList()
                .Select(x => x.ReadTo())
                .AsQueryable();
        }
    }
}