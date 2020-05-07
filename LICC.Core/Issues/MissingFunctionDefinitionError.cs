using System.Diagnostics.CodeAnalysis;
using LICC.AST.Nodes;
using Serilog;

namespace LICC.Core.Issues
{
    public sealed class MissingFunctionDefinitionError : BaseError
    {
        public DeclSpecsNode DeclarationSpecifiers { get; set; }
        public FuncDeclNode Declarator { get; set; }


        public MissingFunctionDefinitionError(DeclSpecsNode declarationSpecifiers, FuncDeclNode fdecl)
        {
            this.DeclarationSpecifiers = declarationSpecifiers;
            this.Declarator = fdecl;
        }

        public override string ToString() => $"{base.ToString()} | missing definition for function: {this.Declarator}";

        public override void LogIssue()
        {
            Log.Warning("Missing definition for function {Specs} {Declarator}, found at line {Line}",
                        this.DeclarationSpecifiers, this.Declarator, this.DeclarationSpecifiers.Line);
        }

        public override bool Equals(object? obj)
            => this.Equals(obj as MissingFunctionDefinitionError);

        public override bool Equals([AllowNull] BaseIssue other)
        {
            if (!base.Equals(other))
                return false;

            var o = other as MissingFunctionDefinitionError;
            return this.DeclarationSpecifiers.Equals(o?.DeclarationSpecifiers) && this.Declarator.Equals(o?.Declarator);
        }
    }
}
