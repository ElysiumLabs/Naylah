using Flurl.Http;
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
        public NaylahRestClientSettings Settings { get; protected set; }

        /// <summary>
        /// Internal HttpClient instance, be carefull my friend.
        /// </summary>
        protected internal HttpClient InternalHttpClient { get; set; }

        /// <summary>
        /// ContentType key driven serializer
        /// </summary>
        protected internal Dictionary<string, ISerializer> Serializers = new Dictionary<string, ISerializer>();

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
            object body = null,
            string contentType = null
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

            if (body != null)
            {
                contentType = contentType ?? MediaTypeNames2.Application.Json;

                if (!Serializers.TryGetValue(contentType, out var serializer))
                {
                    throw new Exception($"No registered serializer for this {contentType} content-type");
                }

                var mediaTypeInfo = MediaTypeHeaderValue.Parse(contentType);
                var encoding = !string.IsNullOrEmpty(mediaTypeInfo.CharSet) ?
                    Encoding.GetEncoding(mediaTypeInfo.CharSet) : Encoding.UTF8;

                var svalue = await serializer.Serialize((TIn)body);

                request.Content = new StringContent(svalue, encoding, contentType);
            }

            return request;
        }

        //public virtual async Task<TOut> GetResponse<TOut>(
        //    HttpContent content, CancellationToken? cancelationToken)
        //{
        //    if (content == null)
        //    {
        //        return default;
        //    }

        //    //TODO: verify if application/json etc
        //    content.Headers.TryGetValues("Content-Type", out var responseHeaders);
        //    var contentTypeHeader = responseHeaders.FirstOrDefault() ?? MediaTypeNames2.Application.Json;

        //    var mediaTypeInfo = MediaTypeHeaderValue.Parse(contentTypeHeader);
        //    var encoding = Encoding.GetEncoding(mediaTypeInfo.CharSet);

        //    if (!Serializers.TryGetValue(mediaTypeInfo.MediaType, out var serializer))
        //    {
        //        throw new Exception($"No registered serializer for this {contentTypeHeader} content-type");
        //    }

        //    var contentResponse = await content.ReadAsStringAsync();
        //    //var stream = new MemoryStream();
        //    //await content.CopyToAsync(stream);

        //    //using (stream)
        //    //{
        //    //    using (var streamReader = new StreamReader(stream, encoding))
        //    //    {
        //    //        return await serializer.DeserializeFromStream<TOut>(streamReader);
        //    //    }
        //    //}
        //}

        public virtual async Task<TOut> SendAsync<TOut>(
            HttpRequestMessage requestMessage, CancellationToken cancelationToken)
        {
            HttpResponseMessage response = null;

            try
            {
                response = await InternalHttpClient.SendAsync(requestMessage, cancelationToken);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception exception)
            {
                var restEx = await RestException.CreateException(exception, requestMessage, response);
                throw restEx;
            }

            return (TOut)await ParseResponse<TOut>(response.Content);
        }

        protected virtual async Task<object> ParseResponse<TOut>(HttpContent content)
        {
            try
            {
                content.Headers.TryGetValues("Content-Type", out var responseHeaders);
                var contentTypeHeader = responseHeaders.FirstOrDefault() ?? MediaTypeNames2.Application.Json;

                var mediaTypeInfo = MediaTypeHeaderValue.Parse(contentTypeHeader);
                var encoding = Encoding.GetEncoding(mediaTypeInfo.CharSet);

                if (!Serializers.TryGetValue(mediaTypeInfo.MediaType, out var serializer))
                {
                    throw new Exception($"No registered serializer for this {contentTypeHeader} content-type");
                }

                var stringContent = await content.ReadAsStringAsync();

                if (typeof(TOut) == typeof(string))
                {
                    return stringContent;
                }
                else
                {
                    return await serializer.Deserialize<TOut>(stringContent);
                }

                //if (typeof(TOut) == typeof(string))
                //{
                //    return await content.ReadAsStringAsync();
                //}
                //else
                //{
                //    using (var memoryStream = new MemoryStream(await content.ReadAsByteArrayAsync()))
                //    {
                //        using (var streamReader = new StreamReader(memoryStream, encoding))
                //        {
                //            return await serializer.DeserializeFromStream<TOut>(streamReader);
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw new Exception("Parsing response error", ex);
            }
        }

        public async Task<TOut> ExecuteAsync<TIn, TOut>(
            string resource, HttpMethod method, TIn data, Dictionary<string, string> headers = null, CancellationToken? cancelationToken = null)
        {
            var icancelationToken = cancelationToken ?? CancellationToken.None;

            HttpRequestMessage request = await CreateRequest<TIn>(resource, method, headers, data);

            return await SendAsync<TOut>(request, icancelationToken);
        }

        public virtual async Task ExecuteAsync<TIn>(
            string resource, HttpMethod method, TIn data, Dictionary<string, string> headers = null, CancellationToken? cancelationToken = null)
        {
            var icancelationToken = cancelationToken ?? CancellationToken.None;

            HttpRequestMessage request = await CreateRequest<TIn>(resource, method, headers, data);

            var response = await SendAsync<string>(request, icancelationToken);
        }

        public virtual async Task<TOut> ExecuteAsync<TOut>(
            string resource, HttpMethod method, Dictionary<string, string> headers = null, CancellationToken? cancelationToken = null)
        {
            var icancelationToken = cancelationToken ?? CancellationToken.None;

            HttpRequestMessage request = await CreateRequest<string>(resource, method, headers);

            return await SendAsync<TOut>(request, icancelationToken);

            //return await GetResponse<TOut>(request, cancelationToken);
        }

        public virtual async Task ExecuteAsync(
            string resource, HttpMethod method, Dictionary<string, string> headers = null, CancellationToken? cancelationToken = null)
        {
            var icancelationToken = cancelationToken ?? CancellationToken.None;

            HttpRequestMessage request = await CreateRequest<string>(resource, method, headers);

            var response = await SendAsync<string>(request, icancelationToken);
        }
    }
}
