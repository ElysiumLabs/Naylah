using Naylah.Rest.Serializers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Naylah.Rest
{
    public static class SerializerExtensions
    {
        public static void UseSerializer(this NaylahRestClient2 restClient, string contentType, ISerializer serializer)
        {
            restClient.Serializers[contentType] = serializer;
        }
    }
}
