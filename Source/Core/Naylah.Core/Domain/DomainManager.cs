using Naylah.Domain.Abstractions;

namespace Naylah.Domain
{
    public class DomainManager
    {
        public DomainManager()
        {
            if (DomainEvent.Resolver != null)
            {
                NotificationHandler = DomainEvent.Resolver.GetNotificationsHandler();
            }
        }

        public IHandler<Notification> NotificationHandler { get; set; }

        //public static DomainInitializationData Initialize()
        //{
        //    var data = new DomainInitializationData();

        // try { DTOMappings.RegisterMappings();

        // data.Initialized = true;

        // //Mapper.AssertConfigurationIsValid(); } catch (Exception ex) { data.Initialized = false;
        // data.Problems = ex; }

        //    return data;
        //}

        //public void Dispose()
        //{
        //    if (DomainNotificationHandler != null)
        //    {
        //        DomainNotificationHandler.Dispose();
        //    }
        //}
    }
}