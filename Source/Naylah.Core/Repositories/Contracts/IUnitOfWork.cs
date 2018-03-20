using System;

namespace Naylah.Core.Repositories.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        int Commit();
    }
}