using Naylah.Services.Client.MessageHandlers;
using System.Collections.Generic;

namespace Naylah.Services.Client
{
    public static class NaylahServiceClientSettingsExtensions
    {
        public static void AddAzureAPIManagementSubscriptionKeyMessageHandler(this NaylahServiceClientSettings settings)
        {
            settings.AddOrReplaceMessageHandler(
                new AzureAPIManagementSubscriptionKeyMessageHandler()
                );
        }

        public static void AddOrReplaceMessageHandler(this NaylahServiceClientSettings settings, ServiceClientBaseDelegatingHandler messageHandler)
        {
            if (settings.MessageHandlers.Contains(messageHandler))
            {
                settings.MessageHandlers.Remove(messageHandler);
                AddOrReplaceMessageHandler(settings, messageHandler);
                return;
            }

            settings.MessageHandlers.Add(messageHandler);
        }
    }

    public class NaylahServiceClientSettings
    {
        public NaylahServiceClientSettings()
        {
            MessageHandlers = new List<ServiceClientBaseDelegatingHandler>();
            CustomProperties = new Dictionary<string, string>();
        }

        public Dictionary<string, string> CustomProperties { get; private set; }

        public IList<ServiceClientBaseDelegatingHandler> MessageHandlers { get; private set; }

        public static NaylahServiceClientSettings GetDefaultSettings()
        {
            return new NaylahServiceClientSettings();
        }

        //public NaylahServiceClient BuildDefaultNaylahServiceClient(Uri serviceUri)
        //{
        //    var nClient = new NaylahServiceClient(serviceUri, this);

        //    return nClient;
        //}
    }
}