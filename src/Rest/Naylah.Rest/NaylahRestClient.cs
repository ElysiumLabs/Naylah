using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Naylah.Rest
{
    public class NaylahRestClient : RestClient
    {
        public NaylahRestClient(Uri baseUrl) : base(baseUrl)
        {
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DefaultValueHandling = DefaultValueHandling.Include,
                TypeNameHandling = TypeNameHandling.None,
                NullValueHandling = NullValueHandling.Ignore,
#if DEBUG
                Formatting = Formatting.Indented,
#else
                Formatting = Formatting.None,
#endif
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            };

            this.UseNewtonsoftJson(serializerSettings);
        }

        public async Task<IRestResponse> ExecuteAsync(IRestRequest request, bool thowFailure = true)
        {
            var response = await base.ExecuteAsync(request);

            if (thowFailure)
            {
                this.HandleRequestError(request, response);
            }

            return response;
        }

        public async Task<IRestResponse<T>> ExecuteAsync<T>(IRestRequest request, bool thowFailure = true)
        {
            var response = await base.ExecuteAsync<T>(request);

            if (thowFailure)
            {
                this.HandleRequestError(request, response);
            }

            return response;
        }
    }

//    public class NaylahRestClient2
//    {
//        private HttpClient httpClient;
//        private JsonSerializerSettings serializerSettings;

//        public NaylahRestClient2(Uri baseUrl)
//        {
//            httpClient = new HttpClient() { BaseAddress = baseUrl };
//            serializerSettings = new JsonSerializerSettings
//            {
//                ContractResolver = new CamelCasePropertyNamesContractResolver(),
//                DefaultValueHandling = DefaultValueHandling.Include,
//                TypeNameHandling = TypeNameHandling.None,
//                NullValueHandling = NullValueHandling.Ignore,
//#if DEBUG
//                Formatting = Formatting.Indented,
//#else
//                Formatting = Formatting.None,
//#endif
//                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
//            };

//        }

//        public async Task<IRestResponse> ExecuteAsync(IRestRequest request, bool thowFailure = true)
//        {
//            var httpRequestMessage = Convert(request);
//            var response = await httpClient.SendAsync(httpRequestMessage);
            
//            if (!response.IsSuccessStatusCode)
//            {
//                //this.HandleRequestError(request, response);
//            }
//            var json = request.Body.
//            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

//            return response;
//        }

//        private HttpRequestMessage Convert(IRestRequest request)
//        {
//            var requestMessage = new HttpRequestMessage()
//            {
//                RequestUri = new Uri(request.Resource)
//            };

//            switch (request.Method)
//            {
//                case Method.GET:
//                    requestMessage.Method = HttpMethod.Get;
//                    break;
//                case Method.POST:
//                    requestMessage.Method = HttpMethod.Post;
//                    break;
//                case Method.PUT:
//                    requestMessage.Method = HttpMethod.Put;
//                    break;
//                case Method.DELETE:
//                    requestMessage.Method = HttpMethod.Delete;
//                    break;
//                case Method.HEAD:
//                    requestMessage.Method = HttpMethod.Head;
//                    break;
//                case Method.OPTIONS:
//                    requestMessage.Method = HttpMethod.Options;
//                    break;
//                case Method.PATCH:
//                    requestMessage.Method = new HttpMethod("PATCH");
//                    break;
//                case Method.MERGE:
//                    requestMessage.Method = new HttpMethod("MERGE");
//                    break;
//                case Method.COPY:
//                    requestMessage.Method = new HttpMethod("COPY");
//                    break;
//                default:
//                    break;
//            }


//            return requestMessage;
//        }

//        public async Task<IRestResponse<T>> ExecuteAsync<T>(IRestRequest request, bool thowFailure = true)
//        {
//            var response = await base.ExecuteAsync<T>(request);

//            if (thowFailure)
//            {
//                this.HandleRequestError(request, response);
//            }

//            return response;
//        }
//    }
}
