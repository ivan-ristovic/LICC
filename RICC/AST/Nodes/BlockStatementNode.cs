using System.Collections.Generic;
using System.Linq;

namespace RICC.AST.Nodes
{
    public sealed class BlockStatementNode : ASTNode
    {
        public BlockStatementNode(int line, IEnumerable<ASTNode> children, ASTNode? parent = null)
            : base(line, children, parent)
        {

        }

        public BlockStatementNode(int line, params ASTNode[] children)
            : base(line, children)
        {

        }

        public BlockStatementNode(int line, ASTNode? parent = null)
            : base(line, parent)
        {

        }
    }
}
