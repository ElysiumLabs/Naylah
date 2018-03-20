using Naylah.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Naylah.Core.Repositories.Identity
{
    public interface IUserLoginRepository
    {
        IQueryable<UserLogin> FindBy(Expression<Func<UserLogin, bool>> predicate);
    }
}
