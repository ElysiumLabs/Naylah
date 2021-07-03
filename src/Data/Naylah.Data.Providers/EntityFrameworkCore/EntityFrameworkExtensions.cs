using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Naylah;
using Naylah.Data;
using Naylah.Data.Providers.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EntityFrameworkExtensions
    {
        public static IServiceCollection AddEntityFrameworkRepository<TDbContext, TEntity>(this IServiceCollection serviceCollection)
           where TEntity : class, new()
           where TDbContext : DbContext
        {
            serviceCollection.AddScoped<EntityFrameworkRepository<TDbContext, TEntity>>();
            serviceCollection.AddScoped<IRepository<TEntity>>(x => x.GetRequiredService<EntityFrameworkRepository<TDbContext, TEntity>>());
            return serviceCollection;
        }

        [Obsolete("Don't need TIdentifier in Repository anymore")]
        public static IServiceCollection AddEntityFrameworkRepository<TDbContext, TEntity, TIdentifier>(this IServiceCollection serviceCollection)
           where TEntity : class, new()
           where TDbContext : DbContext
        {
            serviceCollection.AddScoped<EntityFrameworkRepository<TDbContext, TEntity>>();
            serviceCollection.AddScoped<IRepository<TEntity>>(x => x.GetRequiredService<EntityFrameworkRepository<TDbContext, TEntity>>());
            return serviceCollection;
        }
    }
}
