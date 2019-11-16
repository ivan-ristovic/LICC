using System;
using System.Collections.Generic;
using System.Linq;

namespace RICC.AST.Nodes
{
    public abstract class ASTNode
    {
        public ASTNode? Parent { get; set; }
        public int Line { get; }
        public IReadOnlyList<ASTNode> Children { get; }


        protected ASTNode(int line, ASTNode? parent = null)
            : this(line, Enumerable.Empty<ASTNode>(), parent)
        {

        }

        protected ASTNode(int line, IEnumerable<ASTNode> children, ASTNode? parent = null)
        {
            this.Line = line;
            this.Children = children.ToList().AsReadOnly();
            this.Parent = parent;
        }

        protected ASTNode(int line, params ASTNode[] children)
            : this(line, children ?? Array.Empty<ASTNode>(), null)
        {

        }
    }
}
