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
        public static IServiceCollection AddEntityFrameworkRepository<TDbContext, TEntity, TIdentifier>(this IServiceCollection serviceCollection)
           where TEntity : class, IEntity<TIdentifier>, new()
           where TDbContext : DbContext
        {
            serviceCollection.AddScoped<EntityFrameworkRepository<TDbContext, TEntity, TIdentifier>>();
            serviceCollection.AddScoped<IRepository<TEntity, TIdentifier>>(x => x.GetRequiredService<EntityFrameworkRepository<TDbContext, TEntity, TIdentifier>>());
            return serviceCollection;
        }
    }
}
