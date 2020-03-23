using System;
using System.Diagnostics.CodeAnalysis;
using RICC.AST.Nodes;
using Serilog;

namespace RICC.Core.Common
{
    public sealed class DeclSpecsMismatchWarning : BaseWarning
    {
        public DeclarationSpecifiersNode Expected { get; set; }
        public DeclarationSpecifiersNode Actual { get; set; }
        public DeclaratorNode Declarator { get; set; }


        public DeclSpecsMismatchWarning(DeclaratorNode declarator, DeclarationSpecifiersNode expected, DeclarationSpecifiersNode actual)
        {
            if (expected.Equals(actual))
                throw new ArgumentException("Expected different objects");
            this.Expected = expected;
            this.Actual = actual;
            this.Declarator = declarator;
        }


        public override void LogIssue()
        {
            Log.Warning("Declaration specifier mismatch for {Identifier}, declared at line {Line}: expected {ExpectedSpecs}, got {ActualSpecs}",
                        this.Declarator.Identifier, this.Expected.Line, this.Expected, this.Actual);
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
