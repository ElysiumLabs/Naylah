﻿using Naylah.Error;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public class RestException : Exception
    {
        public RestException(HttpStatusCode httpStatusCode, Uri requestUri, string content, string message, Exception innerException)
          : base(message, innerException)
        {
            HttpStatusCode = httpStatusCode;
            RequestUri = requestUri;
            Content = content;
        }

        public HttpStatusCode HttpStatusCode { get; private set; }

        public Uri RequestUri { get; private set; }

        public string Content { get; private set; }

        public static RestException CreateException(Uri requestUri, IRestResponse response)
        {
            Exception innerException = null;

            var messageBuilder = new StringBuilder();

            messageBuilder.AppendLine(string.Format("Processing request [{0}] resulted with following errors:", requestUri));

            if (!response.IsSuccessful)
            {
                messageBuilder.AppendLine("- Server responded with unsuccessfult status code: " + response.StatusDescription);
            }

            if (response.ErrorException != null)
            {
                messageBuilder.AppendLine("- An exception occurred while processing request: " + response.ErrorMessage);

                innerException = response.ErrorException;
            }

            return new RestException(response.StatusCode, requestUri, response.Content, messageBuilder.ToString(), innerException);
        }

        public static RestException CreateException(Exception innerException, HttpRequestMessage requestMessage, HttpResponseMessage responseMessage, string responseContent)
        {
            if (responseMessage == null)
            {
                //http error throw without try read response
                throw innerException;
            }

            var requestUri = requestMessage.RequestUri;

            var messageBuilder = new StringBuilder();

            messageBuilder.AppendLine(string.Format("Processing request [{0}] resulted with following errors:", requestUri));

            messageBuilder.AppendLine(string.Format(innerException.Message));

            if (responseMessage != null)
            {
                messageBuilder.AppendLine("- Server responded with unsuccessfult status code: " + ((int)responseMessage.StatusCode).ToString() + " " + responseMessage.ReasonPhrase);
            }
            
            if ((responseMessage?.Content != null))
            {
                messageBuilder.AppendLine("- An exception occurred while processing request: " + responseContent);
            }

            return new RestException(responseMessage.StatusCode, requestUri, responseContent, messageBuilder.ToString(), innerException);
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
