using RICC.AST.Nodes;
using RICC.AST.Visitors;
using RICC.Core.Common;

namespace RICC.Core.Comparers
{
    internal sealed class DeclaratorNodeComparer : ASTNodeComparerBase<DeclaratorNode>
    {
        public override MatchIssues Compare(DeclaratorNode n1, DeclaratorNode n2)
        {
            if (n1 is VariableDeclaratorNode v1 && n2 is VariableDeclaratorNode v2) {
                var evaluator = new ExpressionEvaluator();
                
                // TODO fix if initializer contains something other than constants - identifiers or function calls
                object? v1init = v1.Initializer is { } ? evaluator.Visit(v1.Initializer) : null;
                object? v2init = v2.Initializer is { } ? evaluator.Visit(v2.Initializer) : null;
                if (!v1init?.Equals(v2init) ?? false)
                    this.Issues.AddError(new InitializerMismatchError(v1.Identifier, v1.Line, v1.Initializer, v2.Initializer));
            } else if (n1 is ArrayDeclaratorNode a1 && n2 is ArrayDeclaratorNode a2) {
                // TODO
            } else if (n1 is FunctionDeclaratorNode f1 && n2 is FunctionDeclaratorNode f2) {
                // TODO
            }

            return this.Issues;
        }
    }
}
