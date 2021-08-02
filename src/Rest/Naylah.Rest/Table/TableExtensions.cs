using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Naylah.Rest.Table
{
    public static class TableExtensions
    {
        public static async Task<TableLoadCollection<TModel, TIdentifier>> ToLoadCollection<TModel, TIdentifier>(this TableService<TModel, TIdentifier> tableService, QueryOptions queryOptions = null)
            where TModel : class, IEntity<TIdentifier>
        {
            var pageResult = await tableService.Get(queryOptions);
            var increCol = new TableLoadCollection<TModel, TIdentifier>(pageResult, tableService);
            return increCol;
        }

    }
}
