using Naylah.Core.DTOs.Mappings.Configuration;
using Naylah.Core.Events;
using Naylah.Core.Events.Contracts;
using Naylah.Core.Helpers.Contracts;
using System;

namespace Naylah.Core
{
    public class NaylahDomainManager : IDisposable
    {
        public NaylahDomainManager()
        {
            if (DomainEvent.Container != null)
            {
                DomainNotificationHandler = DomainEvent.Container.GetService<IHandler<DomainNotification>>();
            }
        }

        public IHandler<DomainNotification> DomainNotificationHandler { get; set; }

        public static DomainInitializationData Initialize()
        {
            var data = new DomainInitializationData();

            try
            {
                DTOMappings.RegisterMappings();

                data.Initialized = true;

                //Mapper.AssertConfigurationIsValid();
            }
            catch (Exception ex)
            {
                data.Initialized = false;
                data.Problems = ex;
            }

            return data;
        }

        public void Dispose()
        {
            if (DomainNotificationHandler != null)
            {
                DomainNotificationHandler.Dispose();
            }
        }
    }




    public class DomainInitializationData
    {
        public bool Initialized { get; set; }

        public Exception Problems { get; set; }
    }
}