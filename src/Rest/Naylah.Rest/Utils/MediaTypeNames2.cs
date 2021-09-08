using System;
using System.Collections.Generic;
using System.Text;

namespace Naylah.Rest
{
    public static class MediaTypeNames2
    {
        public static class Application
        {
            public const string Json = "application/json";
            public const string ProblemJson = "application/problem+json";
            
        }

        public static class Multipart
        {
            public const string FormData = "multipart/form-data";

        }
    }
}
