using Naylah.Services.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Naylah.Services.Client.MessageHandlers
{
    public class AzureAPIManagementSubscriptionKeyMessageHandler : ServiceClientBaseDelegatingHandler
    {

        private string APIManagementSettingsKey;

        public AzureAPIManagementSubscriptionKeyMessageHandler(
            string _APIManagementSettingsKey = "Ocp-Apim-Subscription-Key"
            )
        {
            APIManagementSettingsKey = _APIManagementSettingsKey;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(ServiceClient?.ServiceSettings?.CustomProperties[APIManagementSettingsKey]))
            {
                request.Headers.TryAddWithoutValidation("Ocp-Apim-Subscription-Key", ServiceClient.ServiceSettings.CustomProperties[APIManagementSettingsKey]);
            }


            return await base.SendAsync(request, cancellationToken);
        }
    }
}
