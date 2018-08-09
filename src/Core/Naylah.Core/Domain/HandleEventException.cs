using System;

namespace Naylah.Domain
{
    public class HandleEventException : Exception
    {
        public HandleEventException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}