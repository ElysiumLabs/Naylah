using Flurl;
using RestSharp;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Naylah.Rest.Table
{
    public class TableService<TModel, TIdentifier> : ServiceBase
        where TModel : class, IEntity<TIdentifier>
    {
        public string Route { get; set; }

        public TableServiceMethodsRoute MethodsRoutes { get; set; } = new TableServiceMethodsRoute();

        public QueryOptions DefaultQueryOptions { get; set; } = new QueryOptions();

        public TableService(NaylahRestClient2 client) : base(client)
        {
            var basePath = "".AppendPathSegment(typeof(TModel).Name);
            Route = basePath;
        }

        //internal async Task<PageResult<TModel>> GetInternal(string url)
        //{
        //    var request = new RestRequest(url, Method.GET);
        //    var r = await client.ExecuteAsync<PageResult<TModel>>(request);
        //    var p = r.Data;
        //    return p;
        //}

        //public IQueryable<TModel> AsQueryable()
        //{
        //    var ctx = new DataServiceContext(client.BaseUrl);
        //    var ctxQ = ctx.CreateQuery<TModel>(Route);
        //    return new TableQueryable<TModel>(new CustomODataTableQueryContext<TModel>(ctxQ));
        //}

        protected virtual async Task<PageResult<TModel>> GetPagedQueryOptions(HttpRequestMessage request, QueryOptions queryOptions)
        {
            queryOptions = queryOptions ?? DefaultQueryOptions;

            var url = new Url(request.RequestUri);

            if (queryOptions.CustomRoute != null)
            {
                url = url + queryOptions.CustomRoute;
            }

            if (queryOptions.Filter != null)
            {
                url = url.SetQueryParam("$filter", queryOptions.Filter);
            }

            if (queryOptions.Order != null)
            {
                url = url.SetQueryParam("$orderby", queryOptions.Order);
            }

            if (queryOptions.Top != null)
            {
                url = url.SetQueryParam("$top", queryOptions.Top);
            }

            if (queryOptions.Skip != null)
            {
                url = url.SetQueryParam("$skip", queryOptions.Skip);
            }

            request.RequestUri = url.ToUri();

            var ctoken = CancellationToken.None;

            var r = await client.SendAsync(request, ctoken);
            var p = await client.GetResponse<PageResult<TModel>>(r.Content, ctoken);

            p.Query = queryOptions;

            return p;
        }


        public virtual async Task<PageResult<TModel>> Get(QueryOptions queryOptions)
        {
            var request = await client.CreateRequest<PageResult<TModel>>(Route.AppendPathSegment(MethodsRoutes.GetPaged), HttpMethod.Get);
            return await GetPagedQueryOptions(request, queryOptions);
        }

        public virtual async Task<TModel> Get(TIdentifier identifier)
        {
            var response = await client.ExecuteAsync<TModel>(
                Route.AppendPathSegment(MethodsRoutes.GetById.AppendPathSegment(identifier.ToString())),
                HttpMethod.Get
                );
            return response;
        }

        public virtual async Task<TModel> Upsert(TModel entity)
        {
            var response = await client.ExecuteAsync<TModel, TModel>(
                Route.AppendPathSegment(MethodsRoutes.Upsert),
                HttpMethod.Post,
                entity
                );
            return response;
        }

        public virtual async Task<TModel> Delete(TIdentifier identifier)
        {
            var response = await client.ExecuteAsync<TModel>(
                Route.AppendPathSegment(MethodsRoutes.Delete.AppendPathSegment(identifier.ToString())),
                HttpMethod.Delete
                );
            return response;
        }

    }






}
