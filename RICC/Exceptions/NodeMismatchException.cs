using System;

namespace RICC.Exceptions
{
    public sealed class NodeMismatchException : ArgumentException
    {
        public NodeMismatchException()
            : base()
        {

        }

        public NodeMismatchException(string? message) 
            : base(message)
        {

        }

        public NodeMismatchException(string? message, Exception? innerException) 
            : base(message, innerException)
        {

        }

        public NodeMismatchException(string? message, string? paramName)
            : base(message, paramName)
        {

        }
    }
}
