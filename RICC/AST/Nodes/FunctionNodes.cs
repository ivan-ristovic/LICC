using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Newtonsoft.Json;
using RICC.AST.Nodes.Common;

namespace RICC.AST.Nodes
{
    public sealed class FunctionDeclaratorNode : DeclaratorNode
    {
        [JsonIgnore]
        public bool IsVariadic => this.ParametersNode?.IsVariadic ?? false;

        [JsonIgnore]
        public FunctionParametersNode? ParametersNode => this.Children.ElementAtOrDefault(1) as FunctionParametersNode ?? null;

        [JsonIgnore]
        public IEnumerable<FunctionParameterNode>? Parameters => this.ParametersNode?.Parameters;


        public FunctionDeclaratorNode(int line, IdentifierNode identifier)
            : base(line, identifier) { }

        public FunctionDeclaratorNode(int line, IdentifierNode identifier, FunctionParametersNode @params)
            : base(line, identifier, @params) { }


        public override string GetText()
            => $"{this.Identifier}({this.ParametersNode?.GetText() ?? ""})";
    }

    public sealed class LambdaFunctionNode : ExpressionNode
    {
        [JsonIgnore]
        public BlockStatementNode Definition => this.Children[1].As<BlockStatementNode>();

        [JsonIgnore]
        public FunctionParametersNode? ParametersNode => this.Children.ElementAtOrDefault(0) as FunctionParametersNode ?? null;

        [JsonIgnore]
        public IEnumerable<FunctionParameterNode>? Parameters => this.ParametersNode?.Parameters;


        public LambdaFunctionNode(int line, BlockStatementNode def)
            : base(line, def)
        {

        }

        public LambdaFunctionNode(int line, FunctionParametersNode @params, BlockStatementNode def)
            : base(line, @params, def)
        {

        }


        public override string GetText()
            => $"lambda ({this.ParametersNode?.GetText() ?? ""}): {this.Definition.GetText()}";
    }

    public sealed class FunctionDefinitionNode : ASTNode
    {
        [JsonIgnore]
        public DeclarationKeywords Keywords => this.Children[0].As<DeclarationSpecifiersNode>().Keywords;
        
        [JsonIgnore]
        public FunctionDeclaratorNode Declarator => this.Children[1].As<FunctionDeclaratorNode>();

        [JsonIgnore]
        public BlockStatementNode Definition => this.Children[2].As<BlockStatementNode>();

        [JsonIgnore]
        public string ReturnTypeName => this.Children[0].As<DeclarationSpecifiersNode>().TypeName;
        
        [JsonIgnore]
        public Type? ReturnType => this.Children[0].As<DeclarationSpecifiersNode>().Type;
        
        [JsonIgnore]
        public string Identifier => this.Declarator.Identifier;
        
        [JsonIgnore]
        public bool IsVariadic => this.Declarator.IsVariadic;
        
        [JsonIgnore]
        public FunctionParametersNode? ParametersNode => this.Declarator.ParametersNode;
        
        [JsonIgnore]
        public IEnumerable<FunctionParameterNode>? Parameters => this.ParametersNode?.Parameters;


        public FunctionDefinitionNode(int line, DeclarationSpecifiersNode declSpecs, FunctionDeclaratorNode decl, BlockStatementNode body)
            : base(line, declSpecs, decl, body)
        {

        }


        public override string GetText()
            => $"{this.Keywords} {this.ReturnTypeName} {this.Declarator.GetText()} {this.Definition.GetText()}";
    }

    public sealed class FunctionParametersNode : ASTNode
    {
        public bool IsVariadic { get; set; }

        [JsonIgnore]
        public IEnumerable<FunctionParameterNode> Parameters => this.Children.Cast<FunctionParameterNode>();
     

        public FunctionParametersNode(int line, IEnumerable<FunctionParameterNode> @params)
            : base(line, @params)
        {

        }

        public FunctionParametersNode(int line, params FunctionParameterNode[] @params)
            : base(line, @params)
        {

        }


        public override string GetText() => string.Join(", ", this.Children.Select(c => c.GetText()));

        public override bool Equals([AllowNull] ASTNode other)
            => base.Equals(other) && this.IsVariadic.Equals((other as FunctionParametersNode)?.IsVariadic);
    }

    public class FunctionParameterNode : ASTNode
    {
        [JsonIgnore]
        public DeclarationSpecifiersNode DeclarationSpecifiers => this.Children[0].As<DeclarationSpecifiersNode>();
        
        [JsonIgnore]
        public DeclaratorNode Declarator => this.Children[1].As<DeclaratorNode>();


        public FunctionParameterNode(int line, DeclarationSpecifiersNode declSpecs, DeclaratorNode declarator)
            : base(line, declSpecs, declarator)
        {

        }
    }
}
