using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Naylah;
using Naylah.Data;
using Naylah.Data.Providers.CosmosDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CosmosDBExtensions
    {
        public static IServiceCollection AddCosmosDBSQLRepository<TEntity>(this IServiceCollection serviceCollection, 
            Func<IServiceProvider, Container> containerResolver,
            Func<TEntity, PartitionKey> partitionKeyResolver
            )
         where TEntity : class, IEntity<string>, new()
        {
            serviceCollection.AddScoped(x => {
                var container = containerResolver.Invoke(x);
                return new CosmosSQLContainerRepository<TEntity>(container, partitionKeyResolver);
            });

            serviceCollection.AddScoped<IRepository<TEntity>>(x => x.GetRequiredService<CosmosSQLContainerRepository<TEntity>>());
            return serviceCollection;
        }
    }
}
