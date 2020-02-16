using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using RICC.AST.Nodes;

namespace RICC.Core.Comparers
{
    internal sealed class DeclarationStatementNodeComparer : IEqualityComparer<DeclarationStatementNode>
    {
        public bool Equals([AllowNull] DeclarationStatementNode x, [AllowNull] DeclarationStatementNode y) => x.Equals(y);

        public int GetHashCode([DisallowNull] DeclarationStatementNode obj) => obj.GetHashCode();
    }
}