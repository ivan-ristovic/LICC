﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using RICC.AST.Nodes.Common;
using Serilog;

namespace RICC.AST.Nodes
{
    public abstract class DeclarationNode : ASTNode
    {
        protected DeclarationNode(int line, IEnumerable<ASTNode> children)
            : base(line, children) { }

        protected DeclarationNode(int line, params ASTNode[] children)
            : base(line, children) { }
    }

    public class DeclarationSpecifiersNode : ASTNode
    {
        public DeclarationKeywords Keywords { get; }
        public string TypeName { get; }

        [JsonIgnore]
        public Type? Type { get; }


        public DeclarationSpecifiersNode(int line, string specs, string type)
            : base(line)
        {
            this.Keywords = DeclarationKeywords.Parse(specs);
            this.TypeName = type;
            TypeCode? typeCode = Types.TypeCodeFor(type);
            if (typeCode is null)
                Log.Warning("Unknown type: {Type}", type);
            else
                this.Type = Types.ToType(typeCode.Value);
        }


        public override string GetText()
        {
            var sb = new StringBuilder();
            string declSpecs = this.Keywords.ToString();
            if (!string.IsNullOrWhiteSpace(declSpecs))
                sb.Append(declSpecs).Append(' ');
            sb.Append(this.TypeName);
            return sb.ToString();
        }
    }

    public abstract class DeclaratorNode : DeclarationNode
    {
        [JsonIgnore]
        public string Identifier => this.Children.First().As<IdentifierNode>().Identifier;


        public DeclaratorNode(int line, IdentifierNode identifier, params ASTNode[] children)
            : base(line, new[] { identifier }.Concat(children)) { }

        public DeclaratorNode(int line, IdentifierNode identifier, IEnumerable<ASTNode> children)
            : base(line, new[] { identifier }.Concat(children)) { }
    }

    public sealed class DeclaratorListNode : DeclarationNode
    {
        [JsonIgnore]
        public IEnumerable<DeclaratorNode> Declarations => this.Children.Cast<DeclaratorNode>();


        public DeclaratorListNode(int line, IEnumerable<DeclaratorNode> decls)
            : base(line, decls) { }

        public DeclaratorListNode(int line, params DeclaratorNode[] decls)
            : base(line, decls) { }
    }

    public sealed class VariableDeclaratorNode : DeclaratorNode
    {
        [JsonIgnore]
        public ExpressionNode? Initializer => this.Children.ElementAtOrDefault(1)?.As<ExpressionNode>();


        public VariableDeclaratorNode(int line, IdentifierNode identifier, ExpressionNode? initializer)
            : base(line, identifier, initializer is null ? Enumerable.Empty<ASTNode>() : new[] { initializer }) { }


        public override string GetText() 
            => this.Initializer is null ? this.Identifier : $"{this.Identifier} = {this.Initializer.GetText()}";
    }

    public sealed class FunctionDeclaratorNode : DeclaratorNode
    {
        [JsonIgnore]
        public bool IsVariadic => this.ParametersNode?.IsVariadic ?? false;
        
        [JsonIgnore]
        public FunctionParametersNode? ParametersNode => this.Children.ElementAtOrDefault(1) as FunctionParametersNode ?? null;
        
        [JsonIgnore]
        public IEnumerable<FunctionParameterNode>? Parameters => this.ParametersNode?.Parameters;


        public FunctionDeclaratorNode(int line, IdentifierNode identifier, FunctionParametersNode? @params)
            : base(line, identifier, @params is null ? Enumerable.Empty<ASTNode>() : new[] { @params }) { }


        public override string GetText()
            => $"{this.Identifier}({this.ParametersNode?.GetText() ?? ""})";
    }
}
