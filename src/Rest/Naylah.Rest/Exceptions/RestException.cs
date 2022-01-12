using Naylah.Error;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public class RestException : Exception
    {
        public RestException(HttpStatusCode httpStatusCode, Uri requestUri, string content, string message)
          : base(message)
        {
            HttpStatusCode = httpStatusCode;
            RequestUri = requestUri;
            Content = content;
        }

        public HttpStatusCode HttpStatusCode { get; private set; }

        public Uri RequestUri { get; private set; }

        public string Content { get; private set; }

        public static RestException CreateException(HttpRequestMessage requestMessage, HttpResponseMessage responseMessage, string responseContent)
        {
            var requestUri = requestMessage.RequestUri;

            var messageBuilder = new StringBuilder();

            messageBuilder.AppendLine(string.Format("Processing request [{0}] resulted with following errors:", requestUri));

            if (responseMessage != null)
            {
                messageBuilder.AppendLine("- Server responded with unsuccessfult status code: " + ((int)responseMessage.StatusCode).ToString() + " " + responseMessage.ReasonPhrase);
            }
            
            if ((responseMessage?.Content != null))
            {
                messageBuilder.AppendLine("- An exception occurred while processing request: " + responseContent);
            }
             
            return new RestException(responseMessage.StatusCode, requestUri, responseContent, messageBuilder.ToString());
        }

     }

    public static class RestExceptionExtensions
    {
        public static ErrorPresentation AsErrorPresentation(this RestException restException)
        {
            try
            {
                if (string.IsNullOrEmpty(restException.Content))
                {
                    throw new ArgumentNullException("Content is empty");
                }

                return JsonConvert.DeserializeObject<ErrorPresentation>(restException.Content);
            }
            catch (Exception)
            {
                return default(ErrorPresentation);
            }
        }
    }
}
