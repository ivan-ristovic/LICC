using System.Collections.Generic;
using System.Linq;
using RICC.AST.Nodes;

namespace RICC.Core.Comparers.Common
{
    internal static class ASTNodeOperations
    {
        public static IEnumerable<DeclarationStatementNode> ExtractDeclarations(BlockStatementNode node)
        {
            return node.Children
                .Where(c => c is DeclarationStatementNode)
                .Cast<DeclarationStatementNode>()
                ;
        }
    }
}
