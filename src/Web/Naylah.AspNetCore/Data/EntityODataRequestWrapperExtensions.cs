using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Naylah.Data
{
    public static class EntityODataRequestWrapperExtensions
    {
        public static EntityODataRequestWrapper<TEntity> CreateODataWrapper<TEntity>(
            this HttpRequest httpRequest,
            ODataQuerySettings oDataQuerySettings = null
            )
            where TEntity : class, new()
        {
            return new EntityODataRequestWrapper<TEntity>(httpRequest, oDataQuerySettings);
        }

        public static EntityODataRequestModelWrapper<TEntity, TModel, TIdentifier> CreateODataWrapper<TEntity, TModel, TIdentifier>(
            this HttpRequest httpRequest,
            TableDataService<TEntity, TModel, TIdentifier> tableDataService,
            ODataQuerySettings oDataQuerySettings = null
            )
            where TEntity : class, IEntity<TIdentifier>, IModifiable, IEntityUpdate<TModel>, new()
            where TModel : class, IEntity<TIdentifier>, new()
        {
            return new EntityODataRequestModelWrapper<TEntity, TModel, TIdentifier>(tableDataService, httpRequest, oDataQuerySettings);
        }

        public static async Task<PageResult<TModel>> GetPaged<TEntity, TModel, TIdentifier>(
           this EntityODataRequestModelWrapper<TEntity, TModel, TIdentifier> oDataRequestWrapper
           )
           where TEntity : class, IEntity<TIdentifier>, IModifiable, IEntityUpdate<TModel>, new()
           where TModel : class, IEntity<TIdentifier>, new()
        {
            var p = oDataRequestWrapper.ApplyTo();
            return await oDataRequestWrapper.Paged(p);
        }

    }
}
