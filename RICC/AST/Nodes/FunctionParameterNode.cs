using System;
using System.Linq;
using RICC.Exceptions;

namespace RICC.AST.Nodes
{
    public class FunctionParameterNode : ASTNode
    {
        public DeclarationSpecifiersNode DeclarationSpecifiers => (DeclarationSpecifiersNode)this.Children.First();


        public FunctionParameterNode(int line, ASTNode declSpecs, ASTNode identifier, ASTNode? parent = null)
            : base(line, new[] { declSpecs, identifier }, parent)
        {
            if (!(declSpecs is DeclarationSpecifiersNode))
                throw new NodeMismatchException("DeclarationSpecifiersNode expected.", nameof(declSpecs));
            if (!(identifier is IdentifierNode))
                throw new NodeMismatchException("IdentifierNode expected.", nameof(identifier));
        }
    }
}
