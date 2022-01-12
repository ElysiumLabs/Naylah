using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Naylah.Rest.Serializers
{
    public class NewtonsoftSerializer : ISerializer
    {
        private readonly JsonSerializerSettings serializerSettings;
        private readonly JsonSerializer serializer;

        public string ContentType => "application/json";

        public NewtonsoftSerializer(JsonSerializerSettings serializerSettings)
        {
            this.serializerSettings = serializerSettings;
            serializer = JsonSerializer.Create(serializerSettings);
        }

       

        public Task<string> Serialize<T>(T value)
        {
            var stringBuilder = new StringBuilder();
            using (var stringWriter = new StringWriter(stringBuilder))
            {
                using (var jsonTextWriter = new JsonTextWriter(stringWriter))
                {
                    serializer.Serialize(jsonTextWriter, value, typeof(T));
                }
            }

            return Task.FromResult(stringBuilder.ToString());
        }

        public Task SerializeToStream<T>(T value, StreamWriter streamWriter)
        {
            using (var jsonTextWriter = new JsonTextWriter(streamWriter))
            {
                serializer.Serialize(jsonTextWriter, value, typeof(T));
            }

            return Task.FromResult(0);
        }

        public Task<T> Deserialize<T>(string svalue)
        {
            using (var stringReader = new StringReader(svalue))
            {
                using (var jsonTextReader = new JsonTextReader(stringReader))
                {
                    return Task.FromResult(serializer.Deserialize<T>(jsonTextReader));
                }
            }
        }

        public Task<T> DeserializeFromStream<T>(StreamReader streamReader)
        {
            using (var jsonTextReader = new JsonTextReader(streamReader))
            {
                return Task.FromResult(serializer.Deserialize<T>(jsonTextReader));
            }
        }
    }

    public static class NewtonsoftSerializerExtensions
    {
        public static void UseNewtonsoftSerializer(this RestClient restClient, string contentType, JsonSerializerSettings jsonSerializerSettings)
        {
            restClient.UseSerializer(contentType, new NewtonsoftSerializer(jsonSerializerSettings));
        }

        public static void UseDefaultNewtonsoftSerializer(this RestClient restClient)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
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

            restClient.UseNewtonsoftSerializer(MediaTypeNames2.Application.Json, jsonSerializerSettings);
            restClient.UseNewtonsoftSerializer(MediaTypeNames2.Application.ProblemJson, jsonSerializerSettings);
        }
    }
}
