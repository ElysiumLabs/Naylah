using System;

namespace Naylah.Data.Access
{
    public interface IUnitOfWork : IDisposable
    {
        int Commit();
    }
}