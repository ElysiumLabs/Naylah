using System;
using System.Collections.Generic;
using System.Text;

namespace Naylah.Error
{
    public class ErrorPresentation
    {
        public static ErrorPresentation Create(int code, string title)
        {
            var error = Create();

            error.Code = code;
            error.Title = title;

            return error;
        }

        public static ErrorPresentation Create()
        {
            var error = new ErrorPresentation
            {
            };

            return error;
        }

        public string Instance { get; set; }

        public int? Code { get; set; }

        public string Title { get; set; }

        public string Detail { get; set; }

        public string InternalDetail { get; set; }

        public IList<ErrorPresentation> Inners { get; set; }
    }
}
