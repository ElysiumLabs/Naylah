using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Naylah.Data.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Naylah.Data.Providers.CosmosDB
{
    public class CosmosSQLContainerRepository<TEntity> : IRepository<TEntity>, ICommandRangeRepository<TEntity>, 
        IAsyncCountRepository, IAsyncEnumerableRepository
        where TEntity : class, IEntity<string>
    {
        protected readonly Container container;
        private readonly bool allowSynchronousQueryExecution;

        public CosmosSQLContainerRepository(Container container, Func<TEntity, PartitionKey> partitionKeyResolver, bool allowSynchronousQueryExecution = false)
        {
            this.container = container;
            this.allowSynchronousQueryExecution = allowSynchronousQueryExecution;
            PartitionKeyResolver = partitionKeyResolver;
        }

        public Func<TEntity, PartitionKey> PartitionKeyResolver { get; set; }

        public IQueryable<TEntity> Entities => container.
            GetItemLinqQueryable<TEntity>(allowSynchronousQueryExecution);
            //ToCosmosAsyncQuery<TEntity>(); TODO


        public virtual async ValueTask<TEntity> AddAsync(TEntity entity)
        {
            var r = await container.CreateItemAsync(entity, PartitionKeyResolver.Invoke(entity));
            return r.Resource;
        }

        public virtual async ValueTask<TEntity> EditAsync(TEntity entity)
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

        public async Task<IEnumerable<TEntity1>> AsEnumerableAsync<TEntity1>(
            IQueryable<TEntity1> queryable,
            CancellationToken cancellationToken = default
            )
        {
            return await queryable.ToFeedIterator().ToCosmosListAsync(cancellationToken);
        }
       
        public async Task<long> GetCountAsync<TEntity1>(IQueryable<TEntity1> queryable)
        {
            //var filterQueryDefinition = Entities.Where(filter).ToQueryDefinition();
            var filterQueryDefinition = queryable.ToQueryDefinition();
            var wposStart = filterQueryDefinition.QueryText.IndexOf("WHERE");

            string where = "";

            if (wposStart >= 0)
            {
                where = filterQueryDefinition.QueryText.Substring(wposStart);
            }

            var countSql = "SELECT VALUE COUNT(1) FROM root " + where;

            var countIterator = container.GetItemQueryIterator<int>(countSql);
            while (countIterator.HasMoreResults)
            {
                var response = await countIterator.ReadNextAsync();
                return response.FirstOrDefault();
            }

            return 0;
        }

    }
}
