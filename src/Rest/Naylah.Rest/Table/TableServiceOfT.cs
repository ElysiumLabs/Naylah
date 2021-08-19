using Flurl;
using RestSharp;
using System.Threading.Tasks;

namespace Naylah.Rest.Table
{
    public class TableService<TModel, TIdentifier> : ServiceBase
        where TModel : class, IEntity<TIdentifier>
    {
        public string Route { get; set; }

        public TableServiceMethodsRoute MethodsRoutes { get; set; } = new TableServiceMethodsRoute();

        public QueryOptions DefaultQueryOptions { get; set; } = new QueryOptions();

        public TableService(NaylahRestClient client) : base(client)
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

        protected virtual async Task<PageResult<TModel>> GetPagedQueryOptions(RestRequest request, QueryOptions queryOptions)
        {
            queryOptions = queryOptions ?? DefaultQueryOptions;

            if (queryOptions.Filter != null)
            {
                request.AddParameter("$filter", queryOptions.Filter, ParameterType.QueryString);
            }

            if (queryOptions.Order != null)
            {
                request.AddParameter("$orderby", queryOptions.Order, ParameterType.QueryString);
            }

            if (queryOptions.Top != null)
            {
                request.AddParameter("$top", queryOptions.Top, ParameterType.QueryString);
            }

            if (queryOptions.Skip != null)
            {
                request.AddParameter("$skip", queryOptions.Skip, ParameterType.QueryString);
            }

            var r = await client.ExecuteAsync<PageResult<TModel>>(request);
            var p = r.Data;

            p.Query = queryOptions;

            return p;
        }


        public virtual async Task<PageResult<TModel>> Get(QueryOptions queryOptions)
        {
            var request = new RestRequest(Route.AppendPathSegment(MethodsRoutes.GetPaged), Method.GET);
            return await GetPagedQueryOptions(request, queryOptions);
        }

        public virtual async Task<TModel> Get(TIdentifier identifier)
        {
            var request = new RestRequest(Route.AppendPathSegment(MethodsRoutes.GetById.AppendPathSegment(identifier.ToString())), Method.GET);
            var response = await client.ExecuteAsync<TModel>(request);
            return response.Data;
        }

        public virtual async Task<TModel> Upsert(TModel entity)
        {
            var request = new RestRequest(Route.AppendPathSegment(MethodsRoutes.Upsert), Method.POST);
            var response = await client.ExecuteAsync<TModel>(request);
            return response.Data;
        }

        public virtual async Task<TModel> Delete(TIdentifier identifier)
        {
            var request = new RestRequest(Route.AppendPathSegment(MethodsRoutes.Delete.AppendPathSegment(identifier.ToString())), Method.DELETE);
            var response = await client.ExecuteAsync<TModel>(request);
            return response.Data;
        }

    }






}
