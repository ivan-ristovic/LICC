using System;

namespace RICC.Exceptions
{
    public class SyntaxErrorException : Exception
    {
        private static string FormErrorMessage(string msg, int line, int col)
            => $"L{line}:C{col}: {msg}";

        
        public SyntaxErrorException(string message)
            : base(message)
        {

        }

        public SyntaxErrorException(string message, Exception? innerException)
            : base(message, innerException)
        {

        }

        public SyntaxErrorException(string message, int line, int col)
            : base($"{message}, at: L{line}C{col}")
        {

        }

        public SyntaxErrorException(string message, int line, int col, Exception? innerException)
            : base(FormErrorMessage(message, line, col), innerException)
        {

        }
    }
}
