using System;
using System.Collections.Generic;
using System.Text;

namespace Naylah.Rest
{
    public class NaylahRestRequestContent<T>
    {
        public string ContentType { get; set; }

        public T Content { get; set; }

        public NaylahRestRequestContent() : this(default)
        {

        }

        public NaylahRestRequestContent(T content) : this(content, null)
        {
        }

        public NaylahRestRequestContent(T content, string contentType)
        {
            Content = content;
            ContentType = contentType;
        }
    }
}
