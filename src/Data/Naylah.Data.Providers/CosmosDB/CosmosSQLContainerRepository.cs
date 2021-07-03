using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Naylah.Data.Providers.CosmosDB
{
    public class CosmosSQLContainerRepository<TEntity> : IRepository<TEntity>, ICommandRangeRepository<TEntity>
       where TEntity : class, IEntity<string>
    {
        private readonly Container container;

        public CosmosSQLContainerRepository(Container container, Func<TEntity, PartitionKey> partitionKeyResolver)
        {
            this.container = container;
            PartitionKeyResolver = partitionKeyResolver;
        }

        public Func<TEntity, PartitionKey> PartitionKeyResolver { get; set; }

        public IQueryable<TEntity> Entities => container.GetItemLinqQueryable<TEntity>(true);

        public async ValueTask<TEntity> AddAsync(TEntity entity)
        {
            var r = await container.CreateItemAsync(entity, PartitionKeyResolver.Invoke(entity));
            return r.Resource;
        }

        public async ValueTask<TEntity> EditAsync(TEntity entity)
        {
            var r = await container.UpsertItemAsync(entity, PartitionKeyResolver.Invoke(entity));
            return r.Resource;
        }

        public async Task RemoveAsync(TEntity entity)
        {
            var r = await container.DeleteItemAsync<TEntity>(entity.Id, PartitionKeyResolver.Invoke(entity));
        }

        public async ValueTask<IEnumerable<TEntity>> AddAsync(IEnumerable<TEntity> entities)
        {
            var tasks = entities.Select(entity => container.CreateItemAsync(entity, PartitionKeyResolver.Invoke(entity)));
            var rs = await Task.WhenAll(tasks);
            return rs.Select(x => x.Resource);
        }

        public async ValueTask<IEnumerable<TEntity>> EditAsync(IEnumerable<TEntity> entities)
        {
            var tasks = entities.Select(entity => container.UpsertItemAsync(entity, PartitionKeyResolver.Invoke(entity)));
            var rs = await Task.WhenAll(tasks);
            return rs.Select(x => x.Resource);
        }

        public async Task RemoveAsync(IEnumerable<TEntity> entities)
        {
            var tasks = entities.Select(entity => container.DeleteItemAsync<TEntity>(entity.Id, PartitionKeyResolver.Invoke(entity)));
            var rs = await Task.WhenAll(tasks);
        }

        public int GetCount(Expression<Func<TEntity, bool>> filter = null)
        {
            if (filter != null)
            {
                throw new NotImplementedException();
            }

            var countSql = "SELECT VALUE COUNT(1) FROM c";

            var countIterator = container.GetItemQueryIterator<int>(countSql);
            while (countIterator.HasMoreResults)
            {
                var response = countIterator.ReadNextAsync().Result;
                return response.FirstOrDefault();
            }

            return 0;
        }

    }
}
