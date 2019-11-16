using System;
using System.Collections.Generic;
using System.Text;

namespace RICC.AST.Nodes
{
    public /* abstract */ class StatementNode : ASTNode
    {
        public StatementNode(int line, IEnumerable<ASTNode> children, ASTNode? parent = null) 
            : base(line, children, parent)
        {

        }
    }

    public /* abstract */ class ExpressionStatementNode : StatementNode
    {
        public ExpressionStatementNode(int line, IEnumerable<ASTNode> children, ASTNode? parent = null) 
            : base(line, children, parent)
        {

        }
    }
}
