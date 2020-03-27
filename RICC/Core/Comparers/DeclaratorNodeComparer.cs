using RICC.AST.Nodes;
using RICC.AST.Visitors;
using RICC.Core.Common;
using RICC.Core.Comparers.Common;

namespace RICC.Core.Comparers
{
    internal sealed class DeclaratorNodeComparer : ASTNodeComparerBase<DeclaratorNode>
    {
        public DeclaredSymbol? Symbol1 { get; set; }
        public DeclaredSymbol? Symbol2 { get; set; }


        public DeclaratorNodeComparer()
        {

        }

        public DeclaratorNodeComparer(DeclaredSymbol sym1, DeclaredSymbol sym2)
        {
            this.Symbol1 = sym1;
            this.Symbol2 = sym2;
        }


        public override MatchIssues Compare(DeclaratorNode n1, DeclaratorNode n2)
        {
            if (n1.Identifier != n2.Identifier)
                this.Issues.AddWarning(new DeclaratorMismatchWarning(n1, n2));

            if (n1 is VariableDeclaratorNode && n2 is VariableDeclaratorNode) {
                if (this.Symbol1 is DeclaredVariable v1 && this.Symbol2 is DeclaredVariable v2) {
                    string? v1init = v1.SymbolicInitializer?.ToString() ?? v1.Initializer?.GetText();
                    string? v2init = v2.SymbolicInitializer?.ToString() ?? v2.Initializer?.GetText();
                    if (!v1init?.Equals(v2init) ?? false)
                        this.Issues.AddError(new InitializerMismatchError(v1.Identifier, v1.Declarator.Line, v1init, v2init));
                }
            } else if (n1 is ArrayDeclaratorNode a1 && n2 is ArrayDeclaratorNode a2) {
                // TODO
            } else if (n1 is FunctionDeclaratorNode f1 && n2 is FunctionDeclaratorNode f2) {
                // TODO
            } else {
                // Avoid logging same issue twice
                if (n1.Identifier == n2.Identifier)
                    this.Issues.AddWarning(new DeclaratorMismatchWarning(n1, n2));
            }

            return this.Issues;
        }
    }
}
