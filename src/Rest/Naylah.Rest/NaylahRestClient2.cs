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
    public class NaylahRestClient2
    {
        /// <summary>
        /// Settings for this client
        /// </summary>
        public NaylahRestClientSettings Settings { get; protected set; } = new NaylahRestClientSettings();

        /// <summary>
        /// Internal HttpClient instance, be carefull my friend.
        /// </summary>
        protected internal HttpClient InternalHttpClient { get; set; }

        /// <summary>
        /// ContentType key driven serializer
        /// </summary>
        protected internal Dictionary<string, ISerializer> Serializers = new Dictionary<string, ISerializer>();

        /// <summary>
        /// Humm... Logger? 
        /// </summary>
        protected internal ILogger Logger  => Settings.Logger;

        public NaylahRestClient2(Uri baseUrl) : this(new NaylahRestClientSettings() { BaseUri = baseUrl })
        {
        }

        public NaylahRestClient2(NaylahRestClientSettings settings)
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
            NaylahRestRequestContent<TIn> requestContent = null
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

            if (requestContent != null)
            {
                var contentType = requestContent.ContentType ?? Settings.DefaultContentType;

                if (!Serializers.TryGetValue(contentType, out var serializer))
                {
                    throw new Exception($"No registered serializer for this {contentType} content-type");
                }

                var mediaTypeInfo = MediaTypeHeaderValue.Parse(contentType);
                var encoding = !string.IsNullOrEmpty(mediaTypeInfo.CharSet) ?
                    Encoding.GetEncoding(mediaTypeInfo.CharSet) : Encoding.UTF8;

                var svalue = await serializer.Serialize(requestContent.Content);

                request.Content = new StringContent(svalue, encoding, contentType);
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

        public async Task<TOut> SendAsync<TOut>(
            HttpRequestMessage request,
            CancellationToken cancellationToken = default,
            HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead)
        {
            var ct = GetCancellationTokenWithTimeout(cancellationToken, out var cts);

            try
            {
                using (var response = await InternalHttpClient.SendAsync(request, completionOption, ct).ConfigureAwait(false))
                {
                    return await GetResponse<TOut>(response);
                }
            }
            catch (Exception exception)
            {
                Logger?.LogError(exception, "Error sending the request");
                throw exception;
            }
        }

        public async Task<TOut> GetResponse<TOut>(HttpResponseMessage response)
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
                        try
                        {
                            response.EnsureSuccessStatusCode();
                        }
                        catch (Exception ex)
                        {
                            throw RestException.CreateException(ex, response.RequestMessage, response, responseContent);
                        }
                    }

                    return default;
                }
            }
        }

        
        public async Task<TOut> ExecuteAsync<TIn, TOut>(
            string resource, HttpMethod method, TIn data, Dictionary<string, string> headers = null, CancellationToken? cancelationToken = null)
        {
            var icancelationToken = cancelationToken ?? CancellationToken.None;

            using(var request = await CreateRequest<TIn>(resource, method, headers, new NaylahRestRequestContent<TIn>(data)))
            {
                return await SendAsync<TOut>(request, icancelationToken);
            }
        }

        public virtual async Task ExecuteAsync<TIn>(
            string resource, HttpMethod method, TIn data, Dictionary<string, string> headers = null, CancellationToken? cancelationToken = null)
        {
            var icancelationToken = cancelationToken ?? CancellationToken.None;

            using (var request = await CreateRequest<TIn>(resource, method, headers, new NaylahRestRequestContent<TIn>(data)))
            {
                var response = await SendAsync<string>(request, icancelationToken);
            }
        }

        public virtual async Task<TOut> ExecuteAsync<TOut>(
            string resource, HttpMethod method, Dictionary<string, string> headers = null, CancellationToken? cancelationToken = null)
        {
            var icancelationToken = cancelationToken ?? CancellationToken.None;

            using (var request = await CreateRequest<string>(resource, method, headers))
            {
                return await SendAsync<TOut>(request, icancelationToken);
            }
        }

        public virtual async Task ExecuteAsync(
            string resource, HttpMethod method, Dictionary<string, string> headers = null, CancellationToken? cancelationToken = null)
        {
            var icancelationToken = cancelationToken ?? CancellationToken.None;

            using (var request = await CreateRequest<string>(resource, method, headers))
            {
                var response = await SendAsync<string>(request, icancelationToken);
            }
        }
    }
}
