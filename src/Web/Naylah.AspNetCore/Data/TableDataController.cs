using Microsoft.AspNet.OData;
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

        public TableDataController(TableDataService<TEntity, TModel, TIdentifier> tableDataService)
        {
            this.tableDataService = tableDataService;
        }

        protected virtual IQueryable<TEntity> GetEntities()
        {
            return tableDataService.CreateWrapper().GetEntities();
        }

        [HttpGet("")]
        public virtual PageResult<TModel> GetAll()
        {
            return tableDataService.CreateODataWrapper().GetPaged(Request, GetEntities());
        }

        [HttpGet("{id}")]
        public virtual async Task<TModel> GetById(TIdentifier id)
        {
            return await tableDataService.GetById(id);
        }

        [HttpPost("")]
        public virtual async Task<TModel> Upsert([FromBody] TModel model)
        {
            return await tableDataService.UpsertAsync(model);
        }

        [HttpDelete("{id}")]
        public virtual async Task<TModel> Delete(TIdentifier id)
        {
            return await tableDataService.Delete(id);
        }

    }
}
