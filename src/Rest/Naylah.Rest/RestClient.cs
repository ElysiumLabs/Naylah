using Flurl.Http;
using Microsoft.Extensions.Logging;
using Naylah.Rest.Serializers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Naylah.Rest
{
    public class RestClient
    {
        /// <summary>
        /// Settings for this client
        /// </summary>
        public RestClientSettings Settings { get; protected set; } = new RestClientSettings();

        /// <summary>
        /// Internal HttpClient instance, be carefull my friend.
        /// </summary>
        protected internal HttpClient InternalHttpClient { get; set; }

        /// <summary>
        /// ContentType key driven serializer
        /// </summary>
        protected internal Dictionary<string, ISerializer> Serializers = new Dictionary<string, ISerializer>();

        /// <summary>
        /// ContentType key driven serializer provider
        /// </summary>
        protected internal SerializerProvider SerializerProvider = new SerializerProvider();

        /// <summary>
        /// Humm... Logger? 
        /// </summary>
        protected internal ILogger Logger => Settings.Logger;

        public RestClient(Uri baseUrl) : this(new RestClientSettings() { BaseUri = baseUrl })
        {
        }

        public RestClient(RestClientSettings settings)
        {
            Settings = settings;
            ConfigureFromSettings(true);
        }

        protected virtual void ConfigureFromSettings(bool useDefaults)
        {
            var httpClient = Settings.HttpClientFactory?.Invoke() ?? new HttpClient()
            {
                BaseAddress = Settings.BaseUri,
                Timeout = Settings.DefaultTimeOut
            };
            httpClient.DefaultRequestHeaders.Clear();

            if (Settings.DefaultHeaders.Any())
            {
                foreach (var header in Settings.DefaultHeaders)
                {
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            if (!string.IsNullOrEmpty(Settings.AppName))
            {
                var productVersion = new ProductInfoHeaderValue(Settings.AppName, Settings.AppVersion);
                httpClient.DefaultRequestHeaders.UserAgent.Add(productVersion);
            }

            if (useDefaults)
            {
                this.UseDefaultNewtonsoftSerializer();
                this.UseDefaultTextPlainSerializer();
            }

            InternalHttpClient = httpClient;
        }

        public virtual async Task<HttpRequestMessage> CreateRequest<TIn>(
            string resource,
            HttpMethod method,
            Dictionary<string, string> customHeaders = null,
            RestRequestContent<TIn> restRequestContent = null
            )
        {
            var request = new HttpRequestMessage(method, resource);

            if (customHeaders != null)
            {
                foreach (var headerItem in customHeaders)
                {
                    request.Headers.TryAddWithoutValidation(headerItem.Key, headerItem.Value);
                }
            }

            if (restRequestContent != null)
            {
                restRequestContent.ContentType = restRequestContent.ContentType ?? Settings.DefaultContentType;
                request.Content = await SerializerProvider.GetRequestContent(restRequestContent);
            }

            return request;
        }

        private CancellationToken GetCancellationTokenWithTimeout(CancellationToken original, out CancellationTokenSource timeoutTokenSource)
        {
            timeoutTokenSource = null;

            timeoutTokenSource = CancellationTokenSource.CreateLinkedTokenSource(original);
            timeoutTokenSource.CancelAfter(Settings.DefaultTimeOut);
            return timeoutTokenSource.Token;

            //if (Settings.DefaultTimeOut.HasValue)
            //{
            //    timeoutTokenSource = CancellationTokenSource.CreateLinkedTokenSource(original);
            //    timeoutTokenSource.CancelAfter(Settings.Timeout.Value);
            //    return timeoutTokenSource.Token;
            //}
            //else
            //{
            //    return original;
            //}
        }

        public async Task<TOut> SendInternalAsync<TOut>(
            HttpRequestMessage request,
            CancellationToken cancellationToken = default,
            HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead)
        {
            var ct = GetCancellationTokenWithTimeout(cancellationToken, out var cts);

            try
            {
                using (var response = await InternalHttpClient.SendAsync(request, completionOption, ct).ConfigureAwait(false))
                {
                    return await GetResponseContent<TOut>(response);
                }
            }
            catch (Exception exception)
            {
                Logger?.LogError(exception, "Error sending the request");
                throw exception;
            }
        }

        public async Task<TOut> GetResponseContent<TOut>(HttpResponseMessage response)
        {
            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                if (response.IsSuccessStatusCode)
                {
                    response.Content.Headers.TryGetValues("Content-Type", out var responseHeaders);
                    var contentTypeHeader = responseHeaders.FirstOrDefault() ?? MediaTypeNames2.Application.Json;

                    var mediaTypeInfo = MediaTypeHeaderValue.Parse(contentTypeHeader);
                    var encoding = Encoding.GetEncoding(mediaTypeInfo.CharSet);

                    if (!Serializers.TryGetValue(mediaTypeInfo.MediaType, out var serializer))
                    {
                        throw new Exception($"No registered serializer for this {contentTypeHeader} content-type");
                    }

                    using (var streamReader = new StreamReader(responseStream, encoding))
                    {
                        return await serializer.DeserializeFromStream<TOut>(streamReader);
                    }
                }
                else
                {
                    using (var streamReader = new StreamReader(responseStream))
                    {
                        var responseContent = await streamReader.ReadToEndAsync();
                        throw RestException.CreateException(response.RequestMessage, response, responseContent);
                    }
                }
            }
        }

        public async Task<TOut> ExecuteContentAsync<TIn, TOut>(
            string resource, HttpMethod method, RestRequestContent<TIn> data, Dictionary<string, string> headers = null, CancellationToken? cancelationToken = null)
        {
            var icancelationToken = cancelationToken ?? CancellationToken.None;

            using (var request = await CreateRequest<TIn>(resource, method, headers, data))
            {
                return await SendInternalAsync<TOut>(request, icancelationToken);
            }
        }

        public async Task<TOut> ExecuteAsync<TIn, TOut>(
            string resource, HttpMethod method, TIn data, Dictionary<string, string> headers = null, CancellationToken? cancelationToken = null)
        {
            var icancelationToken = cancelationToken ?? CancellationToken.None;

            using (var request = await CreateRequest<TIn>(resource, method, headers, new RestRequestContent<TIn>(data)))
            {
                return await SendInternalAsync<TOut>(request, icancelationToken);
            }
        }

        public virtual async Task ExecuteAsync<TIn>(
            string resource, HttpMethod method, TIn data, Dictionary<string, string> headers = null, CancellationToken? cancelationToken = null)
        {
            var icancelationToken = cancelationToken ?? CancellationToken.None;

            using (var request = await CreateRequest<TIn>(resource, method, headers, new RestRequestContent<TIn>(data)))
            {
                var response = await SendInternalAsync<string>(request, icancelationToken);
            }
        }

        public virtual async Task<TOut> ExecuteAsync<TOut>(
            string resource, HttpMethod method, Dictionary<string, string> headers = null, CancellationToken? cancelationToken = null)
        {
            var icancelationToken = cancelationToken ?? CancellationToken.None;

            using (var request = await CreateRequest<string>(resource, method, headers))
            {
                return await SendInternalAsync<TOut>(request, icancelationToken);
            }
        }

        public virtual async Task ExecuteAsync(
            string resource, HttpMethod method, Dictionary<string, string> headers = null, CancellationToken? cancelationToken = null)
        {
            var icancelationToken = cancelationToken ?? CancellationToken.None;

            using (var request = await CreateRequest<string>(resource, method, headers))
            {
                var response = await SendInternalAsync<string>(request, icancelationToken);
            }
        }
    }
}
