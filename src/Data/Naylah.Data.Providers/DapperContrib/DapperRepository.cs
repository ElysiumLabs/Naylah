using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naylah.Data.Providers.DapperContrib
{
    public class DapperRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private readonly IDbConnection dbConnection;

        public IQueryable<TEntity> Entities => throw new NotImplementedException("Dapper dont implement IQueryable");

        public DapperRepository(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public async ValueTask<IEnumerable<TEntity>> QueryAsync(string sql)
        {
            return await dbConnection.QueryAsync<TEntity>(sql);
        }

        public async ValueTask<TEntity> GetAsync(dynamic id)
        {
            return await dbConnection.GetAsync<TEntity>(id);
        }

        public async ValueTask<TEntity> AddAsync(TEntity entity)
        {
            await dbConnection.InsertAsync(entity);
            return entity;
        }

        public async ValueTask<TEntity> EditAsync(TEntity entity)
        {
            await dbConnection.UpdateAsync(entity);
            return entity;
        }

        public async Task RemoveAsync(TEntity entity)
        {
            await dbConnection.DeleteAsync(entity);
        }
    }
}
