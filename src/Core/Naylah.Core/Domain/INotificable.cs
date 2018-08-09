using Naylah.Domain.Abstractions;
using System.Collections.Generic;

namespace Naylah.Domain
{
    public interface INotificable
    {
        ICollection<Notification> Notifications { get; }
    }
}