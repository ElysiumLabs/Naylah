using Naylah.Rest.Exceptions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Naylah.Rest
{
    public static class NaylahRestClientExtensions
    {
        public static void HandleRequestError(this NaylahRestClient client, IRestRequest request, IRestResponse response)
        {
            client.HandleRequestError(request, response);
        }

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
