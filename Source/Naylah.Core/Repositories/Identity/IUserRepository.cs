using Naylah.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Naylah.Core.Repositories.Identity
{
    public interface IUserRepository : IMappedRepository
    {
        IQueryable<User> GetAllAsQueryable(params Expression<Func<User, object>>[] includes);
        void Add(User u);
        void Delete(User u);
        void Update(User u);
    }
}
