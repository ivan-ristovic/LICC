using System;

namespace LICC.Exceptions
{
    public class SyntaxException : Exception
    {
        private static string FormErrorMessage(string msg, int line, int col)
            => $"L{line}:C{col}: {msg}";


        public SyntaxException(string message)
            : base(message)
        {

        }

        public SyntaxException(string message, Exception? innerException)
            : base(message, innerException)
        {

        }

        public SyntaxException(string message, int line, int col)
            : base(FormErrorMessage(message, line, col))
        {

        }

        public SyntaxException(string message, int line, int col, Exception? innerException)
            : base(FormErrorMessage(message, line, col), innerException)
        {

        }
    }
}
