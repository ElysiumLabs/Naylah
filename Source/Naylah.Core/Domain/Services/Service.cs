using Naylah.Domain.Abstractions;

namespace Naylah.Domain.Services
{
    public abstract class Service
    {
        public Service()
        {
            NotificationHandler = DomainEvent.Resolver.GetNotificationsHandler();
        }

        public IHandler<Notification> NotificationHandler { get; set; }
    }
}