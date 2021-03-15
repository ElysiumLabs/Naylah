using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naylah.Data
{
    [Route("[controller]")]
    [Produces("application/json")]
    public abstract class TableDataController<TEntity, TModel, TIdentifier> : ControllerBase
          where TEntity : class, IEntity<TIdentifier>, IModifiable, IEntityUpdate<TModel>, new()
          where TModel : class, IEntity<TIdentifier>, new()
    {
        private readonly TableDataService<TEntity, TModel, TIdentifier> tableDataService;

        protected ODataQuerySettings oDataQuerySettings = new ODataQuerySettings();

        protected virtual IQueryable<TEntity> GetEntities()
        {
            return tableDataService.CreateWrapper().GetEntities();
        }

        public TableDataController(TableDataService<TEntity, TModel, TIdentifier> tableDataService)
        {
            this.tableDataService = tableDataService;
        }

        [HttpGet("")]
        public virtual async Task<PageResult<TModel>> GetAll()
        {
            //implementation of logic

            //var odataWrapper = Request.CreateODataWrapper(tableDataService);
            //var e = odataWrapper.ApplyTo();
            //return odataWrapper.Paged(e);

            //or

            return Request.CreateODataWrapper(tableDataService, oDataQuerySettings).GetPaged();
        }

        [HttpGet("{id}")]
        public virtual async Task<TModel> GetById(TIdentifier id)
        {
            return await tableDataService.GetByIdAsync(id);
        }

        [HttpPost("")]
        public virtual async Task<TModel> Upsert([FromBody] TModel model)
        {
            return await tableDataService.UpsertAsync(model);
        }

        [HttpDelete("{id}")]
        public virtual async Task<TModel> Delete(TIdentifier id)
        {
            return await tableDataService.DeleteAsync(id);
        }

    }
}
