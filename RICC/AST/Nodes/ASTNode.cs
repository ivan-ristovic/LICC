using System.Collections.Generic;
using System.Linq;

namespace RICC.AST.Nodes
{
    public abstract class ASTNode
    {
        public ASTNode? Parent { get; set; }
        public int Line { get; }
        public IReadOnlyList<ASTNode> Children { get; }


        protected ASTNode(int line, IEnumerable<ASTNode> children, ASTNode? parent = null)
        {
            this.Line = line;
            this.Children = children.ToList().AsReadOnly();
            this.Parent = parent;
        }
    }
}
