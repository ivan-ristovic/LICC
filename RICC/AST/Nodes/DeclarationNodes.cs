using System;
using System.Collections.Generic;
using System.Text;

namespace RICC.AST.Nodes
{
    public /* abstract*/ class DeclarationNode : ASTNode
    {
        public DeclarationNode(int line, IEnumerable<ASTNode> children, ASTNode? parent = null) 
            : base(line, children, parent)
        {

        }
    }
}
