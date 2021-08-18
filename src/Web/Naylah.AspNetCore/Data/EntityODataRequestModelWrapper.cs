using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Http;
using Naylah.Data.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Naylah.Data
{
    public class EntityODataRequestModelWrapper<TEntity, TModel, TIdentifier> : EntityODataRequestWrapper<TEntity>
        where TEntity : class, IEntity<TIdentifier>, IModifiable, IEntityUpdate<TModel>, new()
        where TModel : class, IEntity<TIdentifier>, new()
    {
        private readonly TableDataService<TEntity, TModel, TIdentifier> tableDataService;
        //private readonly ODataQueryOptions<TModel> modelOpts;

        public EntityODataRequestModelWrapper(TableDataService<TEntity, TModel, TIdentifier> tableDataService, HttpRequest request, ODataQuerySettings oDataQuerySettings = null) : base(request, oDataQuerySettings)
        {
            this.tableDataService = tableDataService;
            //modelOpts = GetEntityModelOptions<TModel>();
        }

        public bool ModelValidation { get; set; } = false;

        protected override IQueryable<TEntity> ApplyToInternal(IQueryable<TEntity> entities)
        {
            //if (ModelValidation)
            //{
            //    modelOpts.Validate(validationSettings);
            //}

            return base.ApplyToInternal(entities);
        }

        public virtual IQueryable<TModel> ApplyTo()
        {
            var wrapper = tableDataService.CreateWrapper();
            Func<IQueryable<TEntity>, IQueryable<TModel>> projectionFunc = wrapper.Project;
            return (IQueryable<TModel>)ProjectionApplyTo(wrapper.GetEntities(), projectionFunc);
        }

        protected override Task<long> RepositoryCount<TResult>(IQueryable<TResult> query)
        {
            var wrapper = tableDataService.CreateWrapper();

            var asyncCountRepo = (IAsyncCountRepository)wrapper.Repository;
            if (asyncCountRepo != null)
            {
                return asyncCountRepo.GetCountAsync(query);
            }

            return base.RepositoryCount(query);

        }

        protected override Task<IEnumerable<TResult>> RepositoryEnumerable<TResult>(IQueryable<TResult> query)
        {
            var wrapper = tableDataService.CreateWrapper();

            var asyncCountRepo = (IAsyncEnumerableRepository)wrapper.Repository;
            if (asyncCountRepo != null)
            {
                return asyncCountRepo.AsEnumerableAsync(query);
            }

            return base.RepositoryEnumerable(query);
        }
    }
}
