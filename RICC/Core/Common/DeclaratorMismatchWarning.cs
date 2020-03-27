using System;
using System.Diagnostics.CodeAnalysis;
using RICC.AST.Nodes;
using Serilog;

namespace RICC.Core.Common
{
    public sealed class DeclaratorMismatchWarning : BaseWarning
    {
        public DeclaratorNode Expected { get; set; }
        public DeclaratorNode Actual { get; set; }


        public DeclaratorMismatchWarning(DeclaratorNode expected, DeclaratorNode actual)
        {
            if (expected.Equals(actual))
                throw new ArgumentException("Expected different objects");
            this.Expected = expected;
            this.Actual = actual;
        }


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
