using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace Naylah.Rest.Table
{
    public class TableLoadCollection<TModel, TIdentifier> : ObservableCollection<TModel>
        where TModel : class, IEntity<TIdentifier>
    {
        private PageResult<TModel> pageResult;

        private readonly TableService<TModel, TIdentifier> tableService;

        public TableLoadCollection(PageResult<TModel> pageResult, TableService<TModel, TIdentifier> tableService)
        {
            this.pageResult = pageResult;
            this.tableService = tableService;
            AddPagedResultItems();
        }

        public long? TotalCount { get { return pageResult.Count; } }

        public bool HasMoreItems { get { return Count < TotalCount; } }

        public async Task LoadMoreItems()
        {
            if (HasMoreItems)
            {
                pageResult.Query.Skip = Count;
                pageResult = await tableService.Get(pageResult.Query);
                AddPagedResultItems();
            }
        }

        private void AddPagedResultItems()
        {
            foreach (var item in pageResult.Items)
            {
                Add(item);
            }
        }
    }
}
