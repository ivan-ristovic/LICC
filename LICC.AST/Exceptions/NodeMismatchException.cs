using System;

namespace LICC.AST.Exceptions
{
    public sealed class NodeMismatchException : SyntaxErrorException
    {
        public NodeMismatchException(Type expected, Type actual) 
            : base($"expected: {expected.Name}, got: {actual.Name}")
        {

        }
    }
}
