using Naylah.Data.Abstractions;
using Naylah.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Naylah.Data.Utils
{
    public class TableDataServiceWrapper<TEntity, TModel, TIdentifier>
        where TEntity : class, IEntity<TIdentifier>, IModifiable, IEntityUpdate<TModel>, new()
        where TModel : class, IEntity<TIdentifier>, new()
    {
        private readonly TableDataService<TEntity, TModel, TIdentifier> tableDataService;

        public TableDataServiceWrapper(TableDataService<TEntity, TModel, TIdentifier> tableDataService)
        {
            this.tableDataService = tableDataService;
        }

        public Func<IQueryable<TEntity>, IQueryable<TModel>> Projection { get { return tableDataService.Projection; } }

        public virtual IQueryable<TEntity> GetEntities()
        {
            return tableDataService.GetEntities();
        }
    }

    public static class TableDataServiceWrapperExtensions
    {
        public static TableDataServiceWrapper<TEntity, TModel, TIdentifier> CreateWrapper<TEntity, TModel, TIdentifier>(this TableDataService<TEntity, TModel, TIdentifier> tableDataService)
             where TEntity : class, IEntity<TIdentifier>, IModifiable, IEntityUpdate<TModel>, new()
            where TModel : class, IEntity<TIdentifier>, new()
        {
            return new TableDataServiceWrapper<TEntity, TModel, TIdentifier>(tableDataService);
        }
    }
}
