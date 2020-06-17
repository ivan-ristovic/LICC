using System;
using System.Diagnostics.CodeAnalysis;
using LICC.AST.Nodes;
using Serilog;

namespace LICC.Core.Issues
{
    public sealed class ExprMismatchError : BaseError
    {
        public ExprNode Expected { get; set; }
        public ExprNode Actual { get; set; }
        public string Message { get; set; }
        public int Line { get; set; }


        public ExprMismatchError(int line, ExprNode expected, ExprNode actual, string message = "Expressions differ")
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
            Log.Error("Expression mismatch found at line {Line}: expected {ExpectedValue}, got {ActualValue}",
                this.Line, this.Expected, this.Actual);
        }

        public override bool Equals(object? obj)
            => this.Equals(obj as ExprMismatchError);

        public override bool Equals([AllowNull] BaseIssue other)
        {
            if (!base.Equals(other))
                return false;

            var o = other as ExprMismatchError;
            return Equals(this.Expected, o?.Expected) && Equals(this.Actual, o?.Actual);
        }
    }
}
