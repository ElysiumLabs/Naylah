using System.Collections.Generic;

namespace Naylah.Domain.Abstractions
{
    public interface IHandler<T> where T : IEvent
    {
        void Handle(T @event);

        IEnumerable<T> Notify();

        bool HasEvents();
    }
}