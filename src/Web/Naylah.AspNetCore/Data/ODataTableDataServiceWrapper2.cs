using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Http;
using Naylah.Data.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Naylah.Data
{
    public static class EntityODataModelWrapperExtensions
    {
        public static EntityODataRequestWrapper<TEntity> CreateODataWrapper<TEntity>(
            this HttpRequest httpRequest, ODataQuerySettings oDataQuerySettings = null
            )
            where TEntity : class,new()
        {
            return new EntityODataRequestWrapper<TEntity>(httpRequest, oDataQuerySettings);
        }

        public static EntityODataRequestModelWrapper<TEntity, TModel, TIdentifier> CreateODataWrapper<TEntity, TModel, TIdentifier>(
            this TableDataService<TEntity, TModel, TIdentifier> tableDataService, HttpRequest httpRequest, ODataQuerySettings oDataQuerySettings = null
            )
            where TEntity : class, IEntity<TIdentifier>, IModifiable, IEntityUpdate<TModel>, new()
            where TModel : class, IEntity<TIdentifier>, new()
        {
            return new EntityODataRequestModelWrapper<TEntity, TModel, TIdentifier>(tableDataService, httpRequest, oDataQuerySettings);
        }
    }

    public class EntityODataRequestWrapper<TEntity>
        where TEntity : class, new()
    {
        protected readonly HttpRequest request;
        protected readonly ODataQuerySettings querySettings;
        protected readonly ODataQueryOptions<TEntity> entityOpts;
        protected readonly ODataValidationSettings validationSettings;

        protected ODataQueryOptions<TType> GetEntityModelOptions<TType>() where TType : class, new()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<TType>(nameof(TType)).EntityType.Filter().OrderBy().Page().Expand();
            var m = builder.GetEdmModel();

            return new ODataQueryOptions<TType>(new ODataQueryContext(m, typeof(TType), null), request);
        }

        protected PageResult<TResult> GetPageResult<TResult>(IEnumerable<TResult> queryResult, long totalCount)
        {
            var nextPageLink = ODataUtils.TryBuildNextLink(queryResult, totalCount, request); //refrac
            return new PageResult<TResult>(queryResult, nextPageLink, totalCount);
        }

        public EntityODataRequestWrapper(HttpRequest request, ODataQuerySettings oDataQuerySettings = null)
        {
            this.request = request;
            this.querySettings = oDataQuerySettings ?? new ODataQuerySettings();
            
            validationSettings = new ODataValidationSettings() { MaxTop = querySettings.PageSize };
            validationSettings.AllowedQueryOptions = AllowedQueryOptions.Filter | AllowedQueryOptions.Top | AllowedQueryOptions.Skip | AllowedQueryOptions.OrderBy;

            entityOpts = GetEntityModelOptions<TEntity>();
        }

        public virtual IQueryable<TModel> ApplyTo<TModel>(IQueryable<TEntity> entities, Func<IQueryable<TEntity>, IQueryable<TModel>> projection = null)
        {
            return ApplyTo(entities, projection);
        }

        public virtual IQueryable ApplyTo(IQueryable<TEntity> entities, Func<IQueryable<TEntity>, IQueryable> projection = null) 
        {
            IQueryable<TEntity> query = entities.AsQueryable();

            try
            {
                query = (entityOpts.Filter?.ApplyTo(query, querySettings) ?? query) as IQueryable<TEntity>;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in filter syntax", ex);
            }

            try
            {
                query = entityOpts.OrderBy?.ApplyTo(query, querySettings).AsQueryable() ?? query;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in order syntax", ex);
            }

            if (projection != null)
            {
                return projection.Invoke(query);
            }

            return entityOpts.SelectExpand?.ApplyTo(query, querySettings) ?? query;
        }

        public PageResult<TResult> Paged<TResult>(IQueryable query)
        {
            var castedQuery = query.Cast<TResult>();

            var totalCount = castedQuery.LongCount();
            var skip = entityOpts.Skip?.Value;
            var top = entityOpts.Top?.Value ?? querySettings.PageSize;

            if (skip > 0)
            {
                castedQuery = castedQuery.Skip(skip.Value);
            }

            if (top > 0)
            {
                castedQuery = castedQuery.Take(top.Value);
            }

            var result = castedQuery.AsEnumerable();

            return GetPageResult(result, totalCount);
        }
    }

    public class EntityODataRequestModelWrapper<TEntity, TModel, TIdentifier> : EntityODataRequestWrapper<TEntity>
        where TEntity : class, IEntity<TIdentifier>, IModifiable, IEntityUpdate<TModel>, new()
        where TModel : class, IEntity<TIdentifier>, new()
    {
        private readonly TableDataService<TEntity, TModel, TIdentifier> tableDataService;
        private ODataQueryOptions<TModel> modelOpts;

        public EntityODataRequestModelWrapper(TableDataService<TEntity, TModel, TIdentifier> tableDataService ,HttpRequest request, ODataQuerySettings oDataQuerySettings = null) : base(request, oDataQuerySettings)
        {
            this.tableDataService = tableDataService;
            modelOpts = GetEntityModelOptions<TModel>();
        }

        public bool ModelValidation { get; set; } = true;

        public override IQueryable ApplyTo(IQueryable<TEntity> entities, Func<IQueryable<TEntity>, IQueryable> projection = null)
        {
            if (ModelValidation)
            {
                modelOpts.Validate(validationSettings);
            }

            return base.ApplyTo(entities, projection ?? tableDataService.CreateWrapper().Projection);
        }

        public PageResult<TModel> Paged(IQueryable<TModel> query)
        {
            return Paged<TModel>(query);
        }
        
    }
}
