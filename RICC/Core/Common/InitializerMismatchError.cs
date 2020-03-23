using System;
using System.Diagnostics.CodeAnalysis;
using RICC.AST.Nodes;
using Serilog;

namespace RICC.Core.Common
{
    public sealed class InitializerMismatchError : BaseError
    {
        public object? Expected { get; set; }
        public object? Actual { get; set; }
        public string Identifier { get; set; }
        public int Line { get; set; }


        public InitializerMismatchError(string identifier, int line, object? expected, object? actual)
        {
            if (expected?.Equals(actual) ?? false)
                throw new ArgumentException("Expected different objects");
            this.Identifier = identifier;
            this.Line = line;
            this.Expected = expected;
            this.Actual = actual;
        }


        public override void LogIssue()
        {
            Log.Error("Initializer mismatch for {Identifier}, declared at line {Line}: expected {ExpectedValue}, got {ActualValue}",
                      this.Identifier, this.Line, this.Expected, this.Actual);
        }

        public override bool Equals(object? obj)
            => this.Equals(obj as InitializerMismatchError);

        public override bool Equals([AllowNull] BaseIssue other)
        {
            if (!base.Equals(other))
                return false;

            var o = other as InitializerMismatchError;
            return this.Identifier.Equals(o?.Identifier) && this.Expected == o?.Expected && this.Actual == o?.Actual;
        }
    }
}
