using Naylah.Core.Extensions;
using Naylah.Services.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Naylah.Service.Client.Services
{
    public static class NaylahServicesClientServiceExtensions
    {
        public static void AddService(this NaylahServiceClient serviceClient, INaylahServiceBase customService)
        {
            var existingService = GetService(serviceClient, customService);

            if (existingService != null)
            {
                throw new Exception("Service already registered");
            }

            var actualServices = serviceClient.Services.ToList();
            actualServices.Add(customService);

            customService.Attach(serviceClient);

            serviceClient.Services = actualServices;

            serviceClient.ConfigureServiceClient();
        }

        public static void RemoveService(this NaylahServiceClient serviceClient, INaylahServiceBase customService)
        {
            var actualServices = serviceClient.Services.ToList();
            actualServices.Remove(customService);

            customService.Dettach();

            serviceClient.Services = actualServices;

            serviceClient.ConfigureServiceClient();
        }

        public static void ResetServices(this NaylahServiceClient serviceClient)
        {
            serviceClient.Services.ForEach((x) => x.Dettach());

            serviceClient.Services = new List<INaylahServiceBase>();

            serviceClient.ConfigureServiceClient();
        }

        public static INaylahServiceBase GetService<T>(this NaylahServiceClient serviceClient) where T : INaylahServiceBase
        {
            return serviceClient.Services.Where(x => x.GetType() == typeof(T)).FirstOrDefault();
        }

        public static INaylahServiceBase GetService(this NaylahServiceClient serviceClient, INaylahServiceBase serviceType)
        {
            return serviceClient.Services.Where(x => x.GetType() == serviceType.GetType()).FirstOrDefault();
        }
    }
}