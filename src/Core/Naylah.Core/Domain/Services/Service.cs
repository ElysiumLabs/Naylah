using Naylah.Domain.Abstractions;

namespace Naylah.Domain.Services
{

    public abstract class Service
    {
        public Service()
        {
#if NETSTANDARD2_0
            NotificationHandler = DomainEvent.Resolver.GetNotificationsHandler();
#endif
        }

        public IHandler<Notification> NotificationHandler { get; set; }
    }
}