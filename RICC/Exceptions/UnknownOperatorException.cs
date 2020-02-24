using System;

namespace RICC.Exceptions
{
    public sealed class UnknownOperatorException : ArgumentException
    {
        public string? Symbol { get; }


        public UnknownOperatorException()
        {

        }

        public UnknownOperatorException(string symbol)
            : base("Unknown operator")
        {
            this.Symbol = symbol;
        }

        public UnknownOperatorException(string symbol, Exception? innerException)
            : base("Unknown operator", innerException)
        {
            this.Symbol = symbol;
        }

        public UnknownOperatorException(string symbol, string? paramName)
            : base("Unknown operator", paramName)
        {
            this.Symbol = symbol;
        }
    }
}
