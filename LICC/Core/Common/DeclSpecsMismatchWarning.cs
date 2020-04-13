using System;
using System.Diagnostics.CodeAnalysis;
using LICC.AST.Nodes;
using Serilog;

namespace LICC.Core.Common
{
    public sealed class DeclSpecsMismatchWarning : BaseWarning
    {
        public DeclSpecsNode Expected { get; set; }
        public DeclSpecsNode Actual { get; set; }
        public DeclNode Declarator { get; set; }


        public DeclSpecsMismatchWarning(DeclNode declarator, DeclSpecsNode expected, DeclSpecsNode actual)
        {
            if (expected.Equals(actual))
                throw new ArgumentException("Expected different objects");
            this.Expected = expected;
            this.Actual = actual;
            this.Declarator = declarator;
        }

        public override string ToString() => $"{base.ToString()}| {this.Declarator} | exp: {this.Expected} | got: {this.Actual}";

        public override void LogIssue()
        {
            Log.Warning("Declaration specifier mismatch for {Identifier}, declared at line {Line}: expected {ExpectedSpecs}, got {ActualSpecs}",
                        this.Declarator.Identifier, this.Actual.Line, this.Expected, this.Actual);
        }

        public override bool Equals(object? obj)
            => this.Equals(obj as DeclSpecsMismatchWarning);

        public override bool Equals([AllowNull] BaseIssue other)
        {
            if (!base.Equals(other))
                return false;

            var o = other as DeclSpecsMismatchWarning;
            return this.Declarator.Equals(o?.Declarator) && this.Expected.Equals(o?.Expected) && this.Actual.Equals(o?.Actual);
        }
    }
}
