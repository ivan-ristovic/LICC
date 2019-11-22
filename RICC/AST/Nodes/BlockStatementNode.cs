using System.Collections.Generic;
using System.Linq;

namespace RICC.AST.Nodes
{
    public sealed class BlockStatementNode : ASTNode
    {
        public IReadOnlyList<StatementNode> Statements => this.Children.Cast<StatementNode>().ToList().AsReadOnly();


        public BlockStatementNode(int line, IEnumerable<StatementNode> children, ASTNode? parent = null)
            : base(line, children, parent)
        {

        }

        public BlockStatementNode(int line, params StatementNode[] children)
            : base(line, children)
        {

        }

        public BlockStatementNode(int line, ASTNode? parent = null)
            : base(line, parent)
        {

        }
    }
}
