using System;
using System.Diagnostics.CodeAnalysis;
using Serilog;

namespace LICC.Core.Issues
{
    public sealed class InitializerMismatchError : BaseError
    {
        public object? Expected { get; set; }
        public object? Actual { get; set; }
        public string Identifier { get; set; }
        public int Line { get; set; }
        public int? Order { get; set; }


        public InitializerMismatchError(string identifier, int line, object? expected, object? actual, int? order = null)
        {
            if (expected?.Equals(actual) ?? false)
                throw new ArgumentException("Expected different objects");
            this.Identifier = identifier;
            this.Line = line;
            this.Expected = expected;
            this.Actual = actual;
            this.Order = order;
        }

        public override string ToString() 
            => $"{base.ToString()}| {this.Identifier}{(this.Order is null ? "" : $"[{this.Order}]")} | exp: {this.Expected} | got: {this.Actual}";

        public override void LogIssue()
        {
            Log.Error("Initializer mismatch for {Identifier}{Order}, declared at line {Line}: expected {ExpectedValue}, got {ActualValue}",
                      this.Identifier, this.Order is null ? "" : $"[{this.Order}]", this.Line, this.Expected, this.Actual);
        }

        public override bool Equals(object? obj)
            => this.Equals(obj as InitializerMismatchError);

        public override bool Equals([AllowNull] BaseIssue other)
        {
            if (!base.Equals(other))
                return false;

            var o = other as InitializerMismatchError;
            return Equals(this.Identifier, o?.Identifier) && Equals(this.Expected, o?.Expected) && Equals(this.Actual, o?.Actual);
        }
    }
}
