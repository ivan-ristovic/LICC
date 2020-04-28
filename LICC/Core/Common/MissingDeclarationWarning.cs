using System.Diagnostics.CodeAnalysis;
using LICC.AST.Nodes;
using Serilog;

namespace LICC.Core.Common
{
    public sealed class MissingDeclarationWarning : BaseWarning
    {
        public DeclSpecsNode DeclarationSpecifiers { get; set; }
        public DeclNode Declarator { get; set; }


        public MissingDeclarationWarning(DeclSpecsNode declarationSpecifiers, DeclNode declarator)
        {
            this.DeclarationSpecifiers = declarationSpecifiers;
            this.Declarator = declarator;
        }


        public override string ToString() => $"{base.ToString()} | missing: {this.Declarator}";

        public override void LogIssue()
        {
            Log.Warning("Missing declaration for {Specs} {Identifier}, declared at line {Line}",
                        this.DeclarationSpecifiers, this.Declarator.Identifier, this.DeclarationSpecifiers.Line);
        }

        public override bool Equals(object? obj)
            => this.Equals(obj as MissingDeclarationWarning);

        public override bool Equals([AllowNull] BaseIssue other)
        {
            if (!base.Equals(other))
                return false;

            var o = other as MissingDeclarationWarning;
            return this.DeclarationSpecifiers.Equals(o?.DeclarationSpecifiers) && this.Declarator.Equals(o?.Declarator);
        }
    }
}
