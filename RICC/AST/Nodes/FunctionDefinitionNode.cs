using System;
using System.Collections.Generic;
using System.Linq;
using RICC.AST.Nodes.Common;

namespace RICC.AST.Nodes
{
    public sealed class FunctionDefinitionNode : ASTNode
    {
        public DeclarationSpecifiersFlags DeclarationSpecifiers => this.Children[0].As<DeclarationSpecifiersNode>().Specifiers;
        public string ReturnType => this.Children[0].As<DeclarationSpecifiersNode>().Type;
        public string Identifier => this.Children[1].As<IdentifierNode>().Identifier;
        public FunctionParametersNode? Parameters => this.Children[2] as FunctionParametersNode ?? null;
        public BlockStatementNode Definition => this.Children[this.Children.Count - 1].As<BlockStatementNode>();


        public FunctionDefinitionNode(int line, DeclarationSpecifiersNode declSpecs, IdentifierNode identifier, FunctionParametersNode? @params, BlockStatementNode body)
            : base(line, @params is null ? new ASTNode[] { declSpecs, identifier, body } : new ASTNode[] { declSpecs, identifier, @params, body })
        {

        }
    }

    public sealed class FunctionParametersNode : ASTNode
    {
        public IReadOnlyList<FunctionParameterNode> Parameters => this.Children.Cast<FunctionParameterNode>().ToList().AsReadOnly();
     
        public FunctionParametersNode(int line, IEnumerable<FunctionParameterNode> @params, ASTNode? parent = null)
            : base(line, @params, parent)
        {

        }

        public FunctionParametersNode(int line, params FunctionParameterNode[] @params)
            : base(line, @params)
        {

        }
    }

    public class FunctionParameterNode : ASTNode
    {
        public DeclarationSpecifiersNode DeclarationSpecifiers => this.Children[0].As<DeclarationSpecifiersNode>();
        public string Identifier => this.Children[1].As<IdentifierNode>().Identifier;


        public FunctionParameterNode(int line, DeclarationSpecifiersNode declSpecs, IdentifierNode identifier, ASTNode? parent = null)
            : base(line, new ASTNode[] { declSpecs, identifier }, parent)
        {

        }
    }
}
