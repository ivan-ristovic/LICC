using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using RICC.AST.Nodes;
using RICC.AST.Visitors;

namespace RICC.Core.Comparers
{
    internal sealed class DeclaratorNodeComparer : IEqualityComparer<DeclaratorNode>
    {
        public bool Equals([AllowNull] DeclaratorNode x, [AllowNull] DeclaratorNode y)
        {
            if (x.Identifier != y.Identifier)
                return false;

            if (x is VariableDeclaratorNode v1 && y is VariableDeclaratorNode v2) {
                var evaluator = new ExpressionEvaluator();
                
                // TODO fix if initializer contains something other than constants - identifiers or function calls
                object? v1init = v1.Initializer is { } ? evaluator.Visit(v1.Initializer) : null;
                object? v2init = v2.Initializer is { } ? evaluator.Visit(v2.Initializer) : null;
                if (!v1init?.Equals(v2init) ?? false) {
                    CoreLog.VariableInitializerMismatch(v1.Identifier, v1.Line, v1init, v2init);
                    return false;
                }
            } else if (x is ArrayDeclaratorNode a1 && y is ArrayDeclaratorNode a2) {
                // TODO
            } else if (x is FunctionDeclaratorNode f1 && y is FunctionDeclaratorNode f2) {
                // TODO
            }

            return true;
        }

        public int GetHashCode([DisallowNull] DeclaratorNode obj) => obj.GetHashCode();
    }
}
