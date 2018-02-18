using System;

namespace Gateway
{
    public class RouteNotFoundException : Exception
    {
        public string RouteKey { get; }

        public RouteNotFoundException() { }

        public RouteNotFoundException(string route)
            : base(string.Format("Route '{0}' is not found.", route))
        {
            RouteKey = route;
        }

        public RouteNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
