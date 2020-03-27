using System;

namespace RICC.Exceptions
{
    public class CompilationException : Exception
    {
        private static string FormErrorMessage(string msg, int line)
            => $"L{line}: {msg}";

        
        public CompilationException(string message)
            : base(message)
        {

        }

        public CompilationException(string message, Exception? innerException)
            : base(message, innerException)
        {

        }

        public CompilationException(string message, int line)
            : base(FormErrorMessage(message, line))
        {

        }

        public CompilationException(string message, int line, Exception? innerException)
            : base(FormErrorMessage(message, line), innerException)
        {

        }
    }
}
