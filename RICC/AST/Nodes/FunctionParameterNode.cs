using System;
using System.Linq;

namespace RICC.AST.Nodes
{
    public class FunctionParameterNode : ASTNode
    {
        public DeclarationSpecifiersNode DeclarationSpecifiers => (DeclarationSpecifiersNode)this.Children.First();


        public FunctionParameterNode(int line, ASTNode declSpecs, ASTNode identifier, ASTNode? parent = null)
            : base(line, new[] { declSpecs, identifier }, parent)
        {

        }
    }
}
