using System;

namespace RICC.Exceptions
{
    public sealed class UnsupportedExtensionException : ArgumentException
    {
        public UnsupportedExtensionException()
            : base()
        {

        }

        public UnsupportedExtensionException(string? message)
            : base(message ?? "Unsupported file extension")
        {

        }

        public UnsupportedExtensionException(string? message, Exception? innerException)
            : base(message ?? "Unsupported file extension", innerException)
        {

        }

        public UnsupportedExtensionException(string? message, string? paramName)
            : base(message ?? "Unsupported file extension", paramName)
        {

        }
    }
}
