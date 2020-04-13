using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Newtonsoft.Json;
using LICC.AST.Nodes.Common;

namespace LICC.AST.Nodes
{
    public sealed class FuncDeclNode : DeclNode
    {
        [JsonIgnore]
        public bool IsVariadic => this.ParametersNode?.IsVariadic ?? false;

        [JsonIgnore]
        public FuncParamsNode? ParametersNode => this.Children.ElementAtOrDefault(1) as FuncParamsNode ?? null;

        [JsonIgnore]
        public IEnumerable<FuncParamNode>? Parameters => this.ParametersNode?.Parameters;


        public FuncDeclNode(int line, IdNode identifier)
            : base(line, identifier) { }

        public FuncDeclNode(int line, IdNode identifier, FuncParamsNode @params)
            : base(line, identifier, @params) { }


        public override string GetText()
            => $"{this.Identifier}({this.ParametersNode?.GetText() ?? ""})";
    }

    public sealed class LambdaFuncExprNode : ExprNode
    {
        [JsonIgnore]
        public BlockStatNode Definition => this.Children.Last().As<BlockStatNode>();

        [JsonIgnore]
        public FuncParamsNode? ParametersNode => this.Children.ElementAtOrDefault(0) as FuncParamsNode ?? null;

        [JsonIgnore]
        public IEnumerable<FuncParamNode>? Parameters => this.ParametersNode?.Parameters;


        public LambdaFuncExprNode(int line, BlockStatNode def)
            : base(line, def)
        {

        }

        public LambdaFuncExprNode(int line, FuncParamsNode @params, BlockStatNode def)
            : base(line, @params, def)
        {

        }


        public override string GetText()
            => $"lambda ({this.ParametersNode?.GetText() ?? ""}): {this.Definition.GetText()}";
    }

    public sealed class FuncDefNode : StatNode
    {
        [JsonIgnore]
        public DeclSpecsNode Specifiers => this.Children[0].As<DeclSpecsNode>();

        [JsonIgnore]
        public FuncDeclNode Declarator => this.Children[1].As<FuncDeclNode>();

        [JsonIgnore]
        public BlockStatNode Definition => this.Children[2].As<BlockStatNode>();

        [JsonIgnore]
        public string ReturnTypeName => this.Children[0].As<DeclSpecsNode>().TypeName;
        
        [JsonIgnore]
        public Type? ReturnType => this.Children[0].As<DeclSpecsNode>().Type;
        
        [JsonIgnore]
        public string Identifier => this.Declarator.Identifier;
        
        [JsonIgnore]
        public bool IsVariadic => this.Declarator.IsVariadic;

        [JsonIgnore]
        public DeclKeywords Keywords => this.Specifiers.Keywords;

        [JsonIgnore]
        public FuncParamsNode? ParametersNode => this.Declarator.ParametersNode;
        
        [JsonIgnore]
        public IEnumerable<FuncParamNode>? Parameters => this.ParametersNode?.Parameters;


        public FuncDefNode(int line, DeclSpecsNode declSpecs, FuncDeclNode decl, BlockStatNode body)
            : base(line, declSpecs, decl, body)
        {

        }


        public override string GetText()
            => $"{this.Keywords} {this.ReturnTypeName} {this.Declarator.GetText()} {this.Definition.GetText()}";
    }

    public sealed class FuncParamsNode : ASTNode
    {
        public bool IsVariadic { get; set; }

        [JsonIgnore]
        public IEnumerable<FuncParamNode> Parameters => this.Children.Cast<FuncParamNode>();
     

        public FuncParamsNode(int line, IEnumerable<FuncParamNode> @params)
            : base(line, @params)
        {

        }

        public FuncParamsNode(int line, params FuncParamNode[] @params)
            : base(line, @params)
        {

        }


        public override string GetText() => string.Join(", ", this.Children.Select(c => c.GetText()));

        public override bool Equals([AllowNull] ASTNode other)
            => base.Equals(other) && this.IsVariadic.Equals((other as FuncParamsNode)?.IsVariadic);
    }

    public class FuncParamNode : ASTNode
    {
        [JsonIgnore]
        public DeclSpecsNode Specifiers => this.Children[0].As<DeclSpecsNode>();
        
        [JsonIgnore]
        public DeclNode Declarator => this.Children[1].As<DeclNode>();


        public FuncParamNode(int line, DeclSpecsNode declSpecs, DeclNode declarator)
            : base(line, declSpecs, declarator)
        {

        }
    }
}
