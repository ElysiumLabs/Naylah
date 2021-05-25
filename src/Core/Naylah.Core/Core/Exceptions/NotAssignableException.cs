namespace Naylah
{
#if NETSTANDARD2_0
    [System.Serializable]
    public class NotAssignableException : System.Exception
    {
        public NotAssignableException() { }
        public NotAssignableException(string message) : base(message) { }
        public NotAssignableException(string message, System.Exception inner) : base(message, inner) { }
        protected NotAssignableException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
#endif
}