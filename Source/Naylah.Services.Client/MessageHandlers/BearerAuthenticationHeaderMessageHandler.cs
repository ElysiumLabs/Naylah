using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Naylah.Services.Client.MessageHandlers
{
    public class BearerAuthenticationHeaderMessageHandler : ServiceClientBaseDelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(ServiceClient?.CurrentUser?.AccessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ServiceClient.CurrentUser.AccessToken);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}