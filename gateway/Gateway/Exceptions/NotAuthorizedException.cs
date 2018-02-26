using System;

namespace Gateway.Exceptions
{
    public class NotAuthorizedException : Exception
    {
        public NotAuthorizedException() { }

        public NotAuthorizedException(string message) : base(message) { }

        public NotAuthorizedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
