﻿using System;

namespace LICC.AST.Exceptions
{
    public sealed class UnsupportedLanguageException : ArgumentException
    {
        public UnsupportedLanguageException()
            : base()
        {

        }

        public UnsupportedLanguageException(string? message)
            : base(message ?? "Unsupported file extension")
        {

        }

        public UnsupportedLanguageException(string? message, Exception? innerException)
            : base(message ?? "Unsupported file extension", innerException)
        {

        }

        public UnsupportedLanguageException(string? message, string? paramName)
            : base(message ?? "Unsupported file extension", paramName)
        {

        }
    }
}
