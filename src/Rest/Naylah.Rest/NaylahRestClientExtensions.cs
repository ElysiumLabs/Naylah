using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Naylah.Rest
{
    public static class NaylahRestClientExtensions
    {
        public static void HandleRequestError(this RestClient client, IRestRequest request, IRestResponse response)
        {
            if (response.IsSuccessful)
            {
                return;
            }

            throw RestException.CreateException(client.BuildUri(request), response);
        }

    }
}
