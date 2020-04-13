using System;
using System.Diagnostics.CodeAnalysis;
using LICC.AST.Nodes;
using Serilog;

namespace LICC.Core.Common
{
    public sealed class DeclaratorMismatchWarning : BaseWarning
    {
        public DeclNode Expected { get; set; }
        public DeclNode Actual { get; set; }


        public DeclaratorMismatchWarning(DeclNode expected, DeclNode actual)
        {
            if (expected.Equals(actual))
                throw new ArgumentException("Expected different objects");
            this.Expected = expected;
            this.Actual = actual;
        }


        public override string ToString() => $"{base.ToString()}| exp: {this.Expected} | got: {this.Actual}";

        public override void LogIssue()
        {
            Log.Warning("Declarator mismatch for {Identifier}, declared at line {Line}: expected {ExpectedDecl}, got {ActualDecl}",
                        this.Expected.Identifier, this.Actual.Line, this.Expected, this.Actual);
        }

        public override bool Equals(object? obj)
            => this.Equals(obj as DeclaratorMismatchWarning);

        public override bool Equals([AllowNull] BaseIssue other)
        {
            if (!base.Equals(other))
                return false;

            var o = other as DeclaratorMismatchWarning;
            return this.Expected.Equals(o?.Expected) && this.Actual.Equals(o?.Actual);
        }
    }
}
