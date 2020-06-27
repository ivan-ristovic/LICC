using System;
using System.Diagnostics.CodeAnalysis;
using LICC.AST.Nodes;
using Serilog;

namespace LICC.Core.Issues
{
    public sealed class StatMismatchWarning : BaseWarning
    {
        public StatNode Expected { get; set; }
        public StatNode Actual { get; set; }
        public string Message { get; set; }
        public int Line { get; set; }


        public StatMismatchWarning(int line, StatNode expected, StatNode actual, string message = "Statements differ")
        {
            if (expected.Equals(actual))
                throw new ArgumentException("Expected different objects");
            this.Expected = expected;
            this.Actual = actual;
            this.Message = message;
            this.Line = line;
        }


        public override string ToString() 
            => $"{base.ToString()}| {this.Message} at line {this.Line} | exp: {this.Expected} | got: {this.Actual}";

        public override void LogIssue()
        {
            Log.Error("Statement mismatch found at line {Line}: expected {ExpectedValue}, got {ActualValue}",
                this.Line, this.Expected, this.Actual);
        }

        public override bool Equals(object? obj)
            => this.Equals(obj as StatMismatchWarning);

        public override bool Equals([AllowNull] BaseIssue other)
        {
            if (!base.Equals(other))
                return false;

            var o = other as StatMismatchWarning;
            return Equals(this.Expected, o?.Expected) && Equals(this.Actual, o?.Actual);
        }
    }
}
