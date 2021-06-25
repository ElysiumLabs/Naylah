using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Naylah.Data
{
    //public class IncrementalCollection<TEntity> : ObservableCollection<TEntity>
    //{
    //    public IncrementalCollection()
    //    {

    //    }

    //    public IncrementalCollection(Func<CancellationToken, IEnumerable<TEntity>> incrementalFunc)
    //    {
    //        this.incrementalFunc = incrementalFunc;
    //    }



    //    public bool HasMoreResults { get; protected set; }

    //    public async Task LoadMoreAsync(CancellationToken cancellationToken = default)
    //    {
    //        if (incrementalFunc == null)
    //        {
    //            return;
    //        }

    //        var task = incrementalFunc.Invoke(cancellationToken);
    //        var r = await task;


    //    }

    //}

    //public static class IncrementalCollectionExtensions
    //{

    //}

}
