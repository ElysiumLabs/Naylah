using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using System;
using System.Collections.Generic;
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
}
