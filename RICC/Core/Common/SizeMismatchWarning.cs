using System;
using System.Diagnostics.CodeAnalysis;
using Serilog;

namespace RICC.Core.Common
{
    public sealed class SizeMismatchWarning : BaseWarning
    {
        public object? Expected { get; set; }
        public object? Actual { get; set; }
        public string Identifier { get; set; }
        public int Line { get; set; }


        public SizeMismatchWarning(string identifier, int line, object? expected, object? actual)
        {
            if (expected?.Equals(actual) ?? false)
                throw new ArgumentException("Expected different objects");
            this.Identifier = identifier;
            this.Line = line;
            this.Expected = expected;
            this.Actual = actual;
        }

        public override string ToString() => $"{base.ToString()}| {this.Identifier} | exp: {this.Expected} | got: {this.Actual}";

        public override void LogIssue()
        {
            Log.Warning("Size mismatch for {Identifier}, declared at line {Line}: expected {ExpectedValue}, got {ActualValue}",
                       this.Identifier, this.Line, this.Expected, this.Actual);
        }

        public override bool Equals(object? obj)
            => this.Equals(obj as SizeMismatchWarning);

        public override bool Equals([AllowNull] BaseIssue other)
        {
            if (!base.Equals(other))
                return false;

            var o = other as SizeMismatchWarning;
            return Equals(this.Identifier, o?.Identifier) && Equals(this.Expected, o?.Expected) && Equals(this.Actual, o?.Actual);
        }
    }
}
