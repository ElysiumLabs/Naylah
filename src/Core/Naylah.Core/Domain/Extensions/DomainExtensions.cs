using Naylah.DI.Abstractions;
using Naylah.Domain.Abstractions;
using System;

namespace Naylah.Domain
{
    public static class DomainExtensions
    {
        [Obsolete]
        public static IHandler<Notification> GetNotificationsHandler(this IDependencyResolver dependencyResolver)
        {
            return dependencyResolver.GetService<IHandler<Notification>>();
        }

        public static IHandler<Notification> GetNotificationsHandler(this IServiceProvider serviceProvider)
        {
            return (IHandler<Notification>)serviceProvider.GetService(typeof(IHandler<Notification>));
        }
    }
}