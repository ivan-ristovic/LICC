using System;

namespace RICC.Exceptions
{
    public sealed class NodeMismatchException : SyntaxErrorException
    {
        public NodeMismatchException(Type expected, Type actual) 
            : base($"expected: {expected.Name}, got: {actual.Name}")
        {

        }
    }
}
