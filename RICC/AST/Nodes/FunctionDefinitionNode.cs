using System;
using System.Collections.Generic;
using System.Linq;
using RICC.AST.Nodes.Common;

namespace RICC.AST.Nodes
{
    public sealed class FunctionDefinitionNode : ASTNode
    {
        public DeclarationKeywords Keywords => this.Children[0].As<DeclarationSpecifiersNode>().Keywords;
        public FunctionDeclaratorNode Declarator => this.Children[1].As<FunctionDeclaratorNode>();
        public string ReturnTypeName => this.Children[0].As<DeclarationSpecifiersNode>().TypeName;
        public Type? ReturnType => this.Children[0].As<DeclarationSpecifiersNode>().Type;
        public string Identifier => this.Declarator.Identifier;
        public FunctionParametersNode? ParametersNode => this.Declarator.Parameters;
        public IReadOnlyList<FunctionParameterNode>? Parameters => this.ParametersNode?.Parameters;
        public BlockStatementNode Definition => this.Children[2].As<BlockStatementNode>();


        public FunctionDefinitionNode(int line, DeclarationSpecifiersNode declSpecs, FunctionDeclaratorNode decl, BlockStatementNode body)
            : base(line, declSpecs, decl, body)
        {

        }


        public override string GetText()
            => $"{this.Keywords} {this.ReturnTypeName} {this.Declarator.GetText()} {this.Definition.GetText()}";
    }

    public sealed class FunctionParametersNode : ASTNode
    {
        public IReadOnlyList<FunctionParameterNode> Parameters => this.Children.Cast<FunctionParameterNode>().ToList().AsReadOnly();
     
        public FunctionParametersNode(int line, IEnumerable<FunctionParameterNode> @params)
            : base(line, @params)
        {

        }

        public FunctionParametersNode(int line, params FunctionParameterNode[] @params)
            : base(line, @params)
        {

        }
     
        
        public override string GetText() => string.Join(", ", this.Children.Select(c => c.GetText()));
    }

    public class FunctionParameterNode : ASTNode
    {
        public DeclarationSpecifiersNode DeclarationSpecifiers => this.Children[0].As<DeclarationSpecifiersNode>();
        public string Identifier => this.Children[1].As<IdentifierNode>().Identifier;


        public FunctionParameterNode(int line, DeclarationSpecifiersNode declSpecs, IdentifierNode identifier)
            : base(line, declSpecs, identifier)
        {

        }
    }
}
