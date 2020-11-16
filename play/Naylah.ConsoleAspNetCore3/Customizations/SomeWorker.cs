using Naylah.ConsoleAspNetCore.ORM;
using Naylah.Data.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Naylah.ConsoleAspNetCore.Customizations
{
    public class SomeWorker : IUnitOfWork
    {
        private readonly TestDbContext testDbContext;

        public SomeWorker(ORM.TestDbContext testDbContext)
        {
            this.testDbContext = testDbContext;
        }
        public Task<int> CommitAsync()
        {
            return testDbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            
        }
    }
}
