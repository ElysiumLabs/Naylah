using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naylah.Core.Repositories
{
    public interface IMappedRepository
    {
        IQueryable<TDestination> ProjectTo<TDestination>(IQueryable query);

    }

    public static class IMappedRepositoryExtensions
    {

    }
}
