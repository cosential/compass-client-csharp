using System;
using System.Runtime.Serialization;

namespace Cosential.Integrations.Compass.Client.Exceptions
{
    public class EndpointDoesNotSupportActionException : Exception
    {
        public EndpointDoesNotSupportActionException()
        {}
        public EndpointDoesNotSupportActionException(string message):base(message)
        {}
        public EndpointDoesNotSupportActionException(string message, Exception innerException):base(message,innerException)
        {}
        public EndpointDoesNotSupportActionException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}