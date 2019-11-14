using System;
using System.Linq;

namespace RICC.AST.Nodes
{
    public class IdentifierNode : ASTNode
    {
        public string Identifier { get; }


        public IdentifierNode(int line, string identifier, ASTNode? parent = null) 
            : base(line, Enumerable.Empty<ASTNode>(), parent)
        {
            if (string.IsNullOrWhiteSpace(identifier))
                throw new ArgumentException("Identifier name must be set.");
            this.Identifier = identifier;
        }
    }
}
