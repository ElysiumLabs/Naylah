using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Naylah.Rest
{
    public class HttpContentWrapper
    {
        public HttpContent HttpContent { get; protected set; }

    }

    public class RestRequestContent<T>
    {
        public string ContentType { get; set; }

        public T Content { get; set; }

        public RestRequestContent() : this(default)
        {

        }

        public RestRequestContent(T content) : this(content, null)
        {
        }

        public RestRequestContent(T content, string contentType)
        {
            Content = content;
            ContentType = contentType;
        }
    }
}
