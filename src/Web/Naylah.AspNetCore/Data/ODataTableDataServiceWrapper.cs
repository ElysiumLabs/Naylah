using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Http;
using Naylah.Data.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Naylah.Data
{

    public class ODataTableDataServiceWrapper<TEntity, TModel, TIdentifier>
        where TEntity : class, IEntity<TIdentifier>, IModifiable, IEntityUpdate<TModel>, new()
        where TModel : class, IEntity<TIdentifier>, new()
    {
        protected TableDataServiceWrapper<TEntity, TModel, TIdentifier> tableServiceWrapper;

        public ODataTableDataServiceWrapper(TableDataService<TEntity, TModel, TIdentifier> tableDataService, ODataQuerySettings oDataQuerySettings = null)
        {
            tableServiceWrapper = tableDataService.CreateWrapper();
            ODataQuerySettings = oDataQuerySettings ?? new ODataQuerySettings();

            ODataValidationSettings = new ODataValidationSettings() { MaxTop = ODataQuerySettings.PageSize };
            ODataValidationSettings.AllowedQueryOptions = AllowedQueryOptions.Filter | AllowedQueryOptions.Top | AllowedQueryOptions.Skip | AllowedQueryOptions.OrderBy;
        }

        public ODataQuerySettings ODataQuerySettings { get; }
        public ODataValidationSettings ODataValidationSettings { get; }

        public bool ModelValidation { get; set; } = true;

        protected virtual ODataQueryOptions<TType> GetEntityModelOptions<TType>(HttpRequest request) where TType : class, new()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<TType>(nameof(TType)).EntityType.Filter().OrderBy().Page().Expand();
            var m = builder.GetEdmModel();

            return new ODataQueryOptions<TType>(new ODataQueryContext(m, typeof(TType), null), request);
        }

        public virtual PageResult<TModel> GetPaged(HttpRequest httpRequest, IQueryable<TEntity> adaptedEntities = null)
        {
            var modelOpts = GetEntityModelOptions<TModel>(httpRequest);
            var entityOpts = GetEntityModelOptions<TEntity>(httpRequest);

            if (ModelValidation)
            {
                modelOpts.Validate(ODataValidationSettings);
            }
            else
            {
                entityOpts.Validate(ODataValidationSettings);
            }

            var entities = adaptedEntities ?? tableServiceWrapper.GetEntities();

            IQueryable<TEntity> query = entities.AsQueryable();

            try
            {
                query = (entityOpts.Filter?.ApplyTo(query, ODataQuerySettings) ?? query) as IQueryable<TEntity>;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in filter syntax", ex);
            }

            try
            {
                query = entityOpts.OrderBy?.ApplyTo(query, ODataQuerySettings).AsQueryable() ?? tableServiceWrapper.Ordering?.Invoke(query) ?? query;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in order syntax", ex);
            }

            var queryResult = tableServiceWrapper.Project(query);

            var totalCount = queryResult.Count();
            var skip = entityOpts.Skip?.Value;
            var top = entityOpts.Top?.Value ?? ODataQuerySettings.PageSize;

            if (skip > 0)
            {
                queryResult = queryResult.Skip(skip.Value);
            }

            if (top > 0)
            {
                queryResult = queryResult.Take(top.Value);
            }

            var result = queryResult.AsEnumerable();

            return GetPageResult(result, totalCount, httpRequest);
        }

        protected virtual PageResult<TModel> GetPageResult(IEnumerable<TModel> queryResult, int totalCount, HttpRequest httpRequest)
        {
            var nextPageLink = ODataUtils.TryBuildNextLink(queryResult, totalCount, httpRequest); //refrac
            return new PageResult<TModel>(queryResult, nextPageLink, totalCount);
        }
    }

    public static class ODataTableServiceWrapperExtensions
    {
        public static ODataTableDataServiceWrapper<TEntity, TModel, TIdentifier> CreateODataWrapper<TEntity, TModel, TIdentifier>(
            this TableDataService<TEntity, TModel, TIdentifier> tableDataService, ODataQuerySettings oDataQuerySettings = null
            )
            where TEntity : class, IEntity<TIdentifier>, IModifiable, IEntityUpdate<TModel>, new()
            where TModel : class, IEntity<TIdentifier>, new()
        {
            return new ODataTableDataServiceWrapper<TEntity, TModel, TIdentifier>(tableDataService, oDataQuerySettings);
        }
    }
}
