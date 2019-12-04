using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RICC.AST.Nodes.Common;

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

    public sealed class DeclarationListNode : DeclarationNode
    {
        public IReadOnlyList<DeclarationNode> Declarations => this.Children.Cast<DeclarationNode>().ToList().AsReadOnly();


        public DeclarationListNode(int line, IEnumerable<DeclarationNode> declarations, ASTNode? parent = null)
            : base(line, declarations, parent)
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

    public sealed class FunctionDeclarationNode : DeclarationNode
    {
        public DeclarationSpecifiersFlags DeclarationSpecifiers => this.Children[0].As<DeclarationSpecifiersNode>().Specifiers;
        public string ReturnType => this.Children[0].As<DeclarationSpecifiersNode>().Type;
        public string Identifier => this.Children[1].As<IdentifierNode>().Identifier;
        public FunctionParametersNode? Parameters => this.Children[2] as FunctionParametersNode ?? null;

        public FunctionDeclarationNode(int line, DeclarationSpecifiersNode declSpecs, IdentifierNode identifier, FunctionParametersNode? @params)
            : base(line, @params is null ? new ASTNode[] { declSpecs, identifier} : new ASTNode[] { declSpecs, identifier, @params })
        {

        }
    }

}
