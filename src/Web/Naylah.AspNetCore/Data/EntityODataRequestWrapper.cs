using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Http;
using Naylah.Data.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Naylah.Data
{
    public class EntityODataRequestWrapper<TEntity>
        where TEntity : class, new()
    {
        public bool UseLongCount { get; set; } = false;

        protected readonly HttpRequest request;
        protected readonly ODataQuerySettings querySettings;
        protected readonly ODataQueryOptions<TEntity> entityOpts;
        protected readonly ODataValidationSettings validationSettings;

        protected virtual ODataQueryOptions<TType> GetEntityModelOptions<TType>() where TType : class, new()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<TType>(nameof(TType)).EntityType.Count().Filter().Select().Expand().OrderBy().Page();
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

        protected virtual IQueryable<TEntity> ApplyToInternal(IQueryable<TEntity> entities)
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

            return query;
        }

        protected virtual IQueryable ProjectionApplyTo(IQueryable<TEntity> entities, Func<IQueryable<TEntity>, IQueryable> projection)
        {
            var query = ApplyToInternal(entities);

            if (projection != null)
            {
                return projection.Invoke(query);
            }
            else if (entityOpts.SelectExpand != null)
            {
                return entityOpts.SelectExpand.ApplyTo(query, querySettings);
            }

            return query;
        }

        public virtual IQueryable ApplyTo(IQueryable<TEntity> entities)
        {
            return ProjectionApplyTo(entities, null);
        }

        public virtual IQueryable<TModel> ApplyTo<TModel>(IQueryable<TEntity> entities, Func<IQueryable<TEntity>, IQueryable<TModel>> projection)
        {
            return ProjectionApplyTo(entities, projection).Cast<TModel>();
        }

        public PageResult<object> Paged(IQueryable query)
        {
            return Paged(query.Cast<object>());
        }

        public PageResult<TResult> Paged<TResult>(IQueryable<TResult> query)
        {
            long totalCount = UseLongCount ? query.LongCount() : query.Count();

            var skip = entityOpts.Skip?.Value;
            var top = entityOpts.Top?.Value ?? querySettings.PageSize;

            if (skip > 0)
            {
                query = query.Skip(skip.Value);
            }

            if (top > 0)
            {
                query = query.Take(top.Value);
            }

            var result = query.AsEnumerable();

            return GetPageResult(result, totalCount);
        }
    }
}
