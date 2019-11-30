using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RICC.AST.Nodes
{
    public abstract class DeclarationNode : ASTNode
    {
        protected DeclarationNode(int line, IEnumerable<ASTNode> children, ASTNode? parent = null)
            : base(line, children, parent)
        {

        }

        protected DeclarationNode(int line, ASTNode? parent = null)
            : base(line, parent)
        {

        }
    }

    public class DeclarationStatementNode : StatementNode
    {
        public DeclarationStatementNode(int line, DeclarationSpecifiersNode declSpecs, DeclarationNode decl, ASTNode? parent = null)
            : base(line, new ASTNode[] { declSpecs, decl }, parent)
        {

        }
    }

    public sealed class VariableDeclarationNode : DeclarationNode
    {
        public string Identifier => this.Children.Single().As<IdentifierNode>().Identifier;
        public object? Value { get; }


        public VariableDeclarationNode(int line, IdentifierNode identifier, object? value, ASTNode? parent = null)
            : base(line, new[] { identifier }, parent)
        {
            this.Value = value;
        }
    }

}
