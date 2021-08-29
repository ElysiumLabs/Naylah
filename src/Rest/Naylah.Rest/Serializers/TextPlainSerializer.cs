using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Naylah.Rest.Serializers
{
    public class TextPlainSerializer : ISerializer
    {
        public string ContentType => MediaTypeNames.Text.Plain;

        public Task<string> Serialize<T>(T value)
        {
            return Task.FromResult((string)Convert.ChangeType(value, typeof(string)));
        }

        public Task SerializeToStream<T>(T value, StreamWriter streamWriter)
        {
            var stringBuilder = new StringBuilder();
            using (var stringWriter = new StringWriter(stringBuilder))
            {
                stringWriter.Write(value.ToString());
            }

            return Task.FromResult(0);
        }

        public Task<T> Deserialize<T>(string svalue)
        {
            return Task.FromResult((T)Convert.ChangeType(svalue, typeof(T)));
        }

        public async Task<T> DeserializeFromStream<T>(StreamReader streamReader)
        {
            var s = await streamReader.ReadToEndAsync();
            return await Deserialize<T>(s);
        }
    }

    public static class TextPlainSerializerExtensions
    {
        public static void UseTextPlainSerializer(this NaylahRestClient2 restClient, string contentType)
        {
            restClient.UseSerializer(contentType, new TextPlainSerializer());
        }

        public static void UseDefaultTextPlainSerializer(this NaylahRestClient2 restClient)
        {
            restClient.UseTextPlainSerializer(MediaTypeNames.Text.Plain);
        }
    }
}
