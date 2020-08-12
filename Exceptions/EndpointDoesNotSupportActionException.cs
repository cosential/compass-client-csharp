using System;
using System.Runtime.Serialization;

namespace Cosential.Integrations.Compass.Client.Exceptions
{
    [Serializable]
    public class EndpointDoesNotSupportActionException : Exception
    {
        public EndpointDoesNotSupportActionException()
        {}
        public EndpointDoesNotSupportActionException(string message):base(message)
        {}
        public EndpointDoesNotSupportActionException(string message, Exception innerException):base(message,innerException)
        {}
        protected EndpointDoesNotSupportActionException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}