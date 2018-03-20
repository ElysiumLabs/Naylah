using Naylah.Core.Events.Contracts;
using System;
using System.Collections.Generic;

namespace Naylah.Core.Helpers.Contracts
{
    public interface IHandler<T> : IDisposable where T : IDomainEvent
    {
        void Handle(T args);

        IEnumerable<T> Notify();

        bool HasNotifications();
    }
}