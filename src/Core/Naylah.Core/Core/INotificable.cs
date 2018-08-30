using System.Collections.Generic;

namespace Naylah
{
    public interface INotificable
    {
        ICollection<Notification> Notifications { get; }
    }
}