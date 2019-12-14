using System;
using System.Collections.Generic;
using System.Linq;
using RICC.AST.Nodes.Common;
using Serilog;

namespace RICC.AST.Nodes
{
    public class DeclarationSpecifiersNode : ASTNode
    {
        public DeclarationSpecifiersFlags Specifiers { get; }
        public string TypeName { get; }
        public Type? Type { get; }


        public DeclarationSpecifiersNode(int line, string type, IEnumerable<string> specs, ASTNode? parent = null)
            : base(line, parent)
        {
            this.Specifiers = DeclarationSpecifiers.Parse(specs);
            this.TypeName = type;
            TypeCode? typeCode = Types.TypeCodeFor(type);
            if (typeCode is null)
                Log.Warning("Unknown type: {Type}", type);
            else
                this.Type = Types.ToType(typeCode.Value);
        }
    }

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
        public string Identifier => this.Children.First().As<IdentifierNode>().Identifier;
        public ExpressionNode? Initializer => this.Children.ElementAtOrDefault(1)?.As<ExpressionNode>();


        public VariableDeclarationNode(int line, IdentifierNode identifier, ExpressionNode? initializer, ASTNode? parent = null)
            : base(line, initializer is null ? new ASTNode[] { identifier } : new ASTNode[] { identifier, initializer }, parent)
        {

        }
    }

    public sealed class FunctionDeclarationNode : DeclarationNode
    {
        public DeclarationSpecifiersFlags DeclarationSpecifiers => this.Children[0].As<DeclarationSpecifiersNode>().Specifiers;
        public string ReturnTypeName => this.Children[0].As<DeclarationSpecifiersNode>().TypeName;
        public Type? ReturnType => this.Children[0].As<DeclarationSpecifiersNode>().Type;
        public string Identifier => this.Children[1].As<IdentifierNode>().Identifier;
        public FunctionParametersNode? Parameters => this.Children[2] as FunctionParametersNode ?? null;

        public FunctionDeclarationNode(int line, DeclarationSpecifiersNode declSpecs, IdentifierNode identifier, FunctionParametersNode? @params)
            : base(line, @params is null ? new ASTNode[] { declSpecs, identifier} : new ASTNode[] { declSpecs, identifier, @params })
        {

        }
    }

}
