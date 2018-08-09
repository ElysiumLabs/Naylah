using Naylah.DI.Abstractions;
using Naylah.Domain.Abstractions;

namespace Naylah.Domain
{
    public static class DomainExtensions
    {
        public static IHandler<Notification> GetNotificationsHandler(this IDependencyResolver dependencyResolver)
        {
            return dependencyResolver.GetService<IHandler<Notification>>();
        }
    }
}