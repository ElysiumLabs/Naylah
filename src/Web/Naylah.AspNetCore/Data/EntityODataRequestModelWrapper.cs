using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

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

    }
}
