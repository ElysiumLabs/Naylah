using Microsoft.AspNetCore.Mvc;
using Naylah.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Naylah.ConsoleAspNetCore.Customizations
{


    public abstract class CustomTableDataController<TEntity, TModel, TIdentifier> : TableDataController<TEntity, TModel, TIdentifier>
          where TEntity : class, IEntity<TIdentifier>, IModifiable, IEntityUpdate<TModel>, new()
          where TModel : class, IEntity<TIdentifier>, new()
    {
        private readonly TableDataService<TEntity, TModel, TIdentifier> tableDataService;

        protected CustomTableDataController(TableDataService<TEntity, TModel, TIdentifier> tableDataService) : base(tableDataService)
        {
            this.tableDataService = tableDataService;
        }

        [HttpPost("bulk/add")]
        public async Task<IEnumerable<TModel>> AddRange([FromBody]IEnumerable<TModel> models)
        {
            return await tableDataService.AddRangeAsync(models);
        }

        [HttpPost("bulk/edit")]
        public async Task<IEnumerable<TModel>> EditRange([FromBody] IEnumerable<TModel> models)
        {
            return await tableDataService.EditRangeAsync(models);
        }

        [HttpPost("bulk/delete")]
        public async Task DeleteRange([FromBody] IEnumerable<TIdentifier> models)
        {
            await tableDataService.RemoveRangeAsync(models);
        }
    }
}

