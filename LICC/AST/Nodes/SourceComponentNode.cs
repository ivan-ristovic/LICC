using System.Collections.Generic;

namespace LICC.AST.Nodes
{
    public sealed class SourceComponentNode : ASTNode
    {
        public string? Name { get; set; }


        public SourceComponentNode(IEnumerable<ASTNode> children)
            : base(1, children) { }

        public SourceComponentNode(params ASTNode[] children)
            : base(1, children) { }

        public SourceComponentNode(string name, IEnumerable<ASTNode> children)
            : base(1, children) 
        {
            this.Name = name;
        }

        public SourceComponentNode(string name, params ASTNode[] children)
            : base(1, children)
        {
            this.Name = name;
        }
    }
}
