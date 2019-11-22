using System.Collections.Generic;
using System.Linq;
using RICC.AST.Nodes.Common;

namespace RICC.AST.Nodes
{
    public class DeclarationSpecifiersNode : ASTNode
    {
        public DeclarationSpecifiersFlags Specifiers { get; }
        public string ReturnType { get; }


        public DeclarationSpecifiersNode(int line, IEnumerable<string> specs, ASTNode? parent = null) 
            : base(line, parent)
        {
            this.Specifiers = DeclarationSpecifiers.Parse(specs);
            this.ReturnType = specs.Last();
        }
    }
}
