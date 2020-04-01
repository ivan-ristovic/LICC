using System;

namespace LICC.Exceptions
{
    public sealed class NodeMismatchException : SyntaxException
    {
        public NodeMismatchException(Type expected, Type actual) 
            : base($"expected: {expected.Name}, got: {actual.Name}")
        {

        }
    }
}
