using System.Collections.Generic;
using System.Linq;

namespace RICC.AST.Nodes
{
    public class DeclarationSpecifiersNode : ASTNode
    {
        public IReadOnlyList<string> Specifiers { get; }


        public DeclarationSpecifiersNode(int line, IEnumerable<string> specs, ASTNode? parent = null) 
            : base(line, Enumerable.Empty<ASTNode>(), parent)
        {
            this.Specifiers = specs.ToList().AsReadOnly();
        }
    }
}
