using System.Collections.Generic;

namespace RICC.AST.Nodes
{
    public sealed class BlockStatementNode : ASTNode
    {
        public BlockStatementNode(int line, IEnumerable<ASTNode> children) 
            : base(line, children)
        {

        }
    }
}
