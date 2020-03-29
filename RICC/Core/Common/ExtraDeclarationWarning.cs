using System.Diagnostics.CodeAnalysis;
using RICC.AST.Nodes;
using Serilog;

namespace RICC.Core.Common
{
    public sealed class ExtraDeclarationWarning : BaseWarning
    {
        public DeclarationSpecifiersNode DeclarationSpecifiers { get; set; }
        public DeclaratorNode Declarator { get; set; }


        public ExtraDeclarationWarning(DeclarationSpecifiersNode declarationSpecifiers, DeclaratorNode declarator)
        {
            this.DeclarationSpecifiers = declarationSpecifiers;
            this.Declarator = declarator;
        }

        public override string ToString() => $"{base.ToString()}| {this.Declarator.Identifier}";

        public override void LogIssue()
        {
            Log.Warning("Extra declaration found: {Specs} {Identifier}, declared at line {Line}",
                        this.DeclarationSpecifiers, this.Declarator.Identifier, this.DeclarationSpecifiers.Line);
        }

        public override bool Equals(object? obj)
            => this.Equals(obj as ExtraDeclarationWarning);

        public override bool Equals([AllowNull] BaseIssue other)
        {
            if (!base.Equals(other))
                return false;

            var o = other as ExtraDeclarationWarning;
            return this.DeclarationSpecifiers.Equals(o?.DeclarationSpecifiers) && this.Declarator.Equals(o?.Declarator);
        }
    }
}
