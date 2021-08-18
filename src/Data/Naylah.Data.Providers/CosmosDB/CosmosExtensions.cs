using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Naylah.Data.Providers.CosmosDB
{
    public static class CosmosExtensions
    {
        public static async Task<IList<TResult>> ToCosmosListAsync<TResult>(this FeedIterator<TResult> feedIterator, CancellationToken cancellationToken = default)
        {
            var results = new List<TResult>();

            while (feedIterator.HasMoreResults)
            {
                var result = await feedIterator.ReadNextAsync(cancellationToken);
                results.AddRange(result);
            }

            return results;
        }
    }
}
