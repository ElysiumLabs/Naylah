using System.Collections.Generic;

namespace Naylah
{
    public interface INotifiable
    {
        ICollection<Notification> Notifications { get; }
    }
}