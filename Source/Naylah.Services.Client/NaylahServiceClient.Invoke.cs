using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Naylah.Services.Client
{
    public partial class NaylahServiceClient
    {
        public async Task<JToken> InvokeApiAsync(string apiName, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await MobileServiceClient.InvokeApiAsync(apiName, cancellationToken);
        }

        public async Task<JToken> InvokeApiAsync(string apiName, JToken body, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await MobileServiceClient.InvokeApiAsync(apiName, body, cancellationToken);
        }

        public async Task<JToken> InvokeApiAsync(string apiName, HttpMethod method, IDictionary<string, string> parameters, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await MobileServiceClient.InvokeApiAsync(apiName, method, parameters, cancellationToken);
        }

        public async Task<HttpResponseMessage> InvokeApiAsync(string apiName, HttpContent content, HttpMethod method, IDictionary<string, string> requestHeaders, IDictionary<string, string> parameters)
        {
            return await MobileServiceClient.InvokeApiAsync(apiName, content, method, requestHeaders, parameters);
        }

        public async Task<JToken> InvokeApiAsync(string apiName, JToken body, HttpMethod method, IDictionary<string, string> parameters, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await MobileServiceClient.InvokeApiAsync(apiName, body, method, parameters, cancellationToken);
        }

        public async Task<HttpResponseMessage> InvokeApiAsync(string apiName, HttpContent content, HttpMethod method, IDictionary<string, string> requestHeaders, IDictionary<string, string> parameters, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await MobileServiceClient.InvokeApiAsync(apiName, content, method, requestHeaders, parameters, cancellationToken);
        }

        public async Task<T> InvokeApiAsync<T>(string apiName, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await MobileServiceClient.InvokeApiAsync<T>(apiName, cancellationToken);
        }

        public async Task<T> InvokeApiAsync<T>(string apiName, HttpMethod method, IDictionary<string, string> parameters, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await MobileServiceClient.InvokeApiAsync<T>(apiName, method, parameters, cancellationToken);
        }

        public async Task<U> InvokeApiAsync<T, U>(string apiName, T body, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await MobileServiceClient.InvokeApiAsync<T, U>(apiName, body, cancellationToken);
        }

        public async Task<U> InvokeApiAsync<T, U>(string apiName, T body, HttpMethod method, IDictionary<string, string> parameters, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await MobileServiceClient.InvokeApiAsync<T, U>(apiName, body, method, parameters, cancellationToken);
        }
    }
}