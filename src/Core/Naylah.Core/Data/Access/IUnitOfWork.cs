using System;
using System.Threading.Tasks;

namespace Naylah.Data.Access
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CommitAsync();
    }
}