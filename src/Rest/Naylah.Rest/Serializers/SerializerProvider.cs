using Naylah.Rest.Serializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Naylah.Rest.Serializers
{
    public class SerializerProvider
    {
        protected internal Dictionary<string, ISerializer> Serializers = new Dictionary<string, ISerializer>();

        public virtual async Task<HttpContent> GetRequestContent<TIn>(RestRequestContent<TIn> restRequestContent)
        {
            switch (restRequestContent.ContentType)
            {
                case MediaTypeNames.Text.Plain:
                case MediaTypeNames.Text.RichText:
                case MediaTypeNames.Text.Xml:
                case MediaTypeNames2.Application.ProblemJson:
                case MediaTypeNames2.Application.Json:
                    {
                        if (!Serializers.TryGetValue(restRequestContent.ContentType, out var serializer))
                        {
                            throw new Exception($"No registered serializer for this {restRequestContent.ContentType} content-type");
                        }

                        var mediaTypeInfo = MediaTypeHeaderValue.Parse(restRequestContent.ContentType);
                        var encoding = !string.IsNullOrEmpty(mediaTypeInfo.CharSet) ?
                            Encoding.GetEncoding(mediaTypeInfo.CharSet) : Encoding.UTF8;

                        var svalue = await serializer.Serialize(restRequestContent.Content);
                        return new StringContent(svalue, encoding, restRequestContent.ContentType);
                    }
                case MediaTypeNames2.Multipart.FormData:
                    {
                        if (restRequestContent.Content is MultipartFormDataContent multipartFormDataContent) // Nullable types are not allowed in patterns
                        {
                            return multipartFormDataContent;
                        }
                        else
                        {
                            throw new Exception($"The {restRequestContent.ContentType} contentType must be MultipartFormDataContent as Content value");
                        }

                    }
                default:
                    throw new Exception($"The {restRequestContent.ContentType} contentType cannot be translated to request body");
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
    }
}
