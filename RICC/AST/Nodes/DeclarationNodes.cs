using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
            this.TypeName = type.Trim();
            TypeCode? typeCode = Types.TypeCodeFor(this.TypeName);
            if (typeCode is null)
                Log.Warning("Unknown type: {Type}", this.TypeName);
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

        public override bool Equals(object? obj)
            => this.Equals(obj as DeclarationSpecifiersNode);

        public override bool Equals([AllowNull] ASTNode other)
        {
            if (!base.Equals(other))
                return false;

            var decl = other as DeclarationSpecifiersNode;
            if (!this.Keywords.Equals(decl?.Keywords))
                return false;
            return this.Type is { } ? this.Type.Equals(decl?.Type) : this.TypeName.Equals(decl?.TypeName);
        }
    }

    public abstract class DeclaratorNode : DeclarationNode
    {
        [JsonIgnore]
        public IdentifierNode IdentifierNode => this.Children.First().As<IdentifierNode>();

        [JsonIgnore]
        public string Identifier => this.IdentifierNode.Identifier;


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

        public override string GetText() => string.Join(", ", this.Children.Select(c => c.GetText()));
    }

    public sealed class VariableDeclaratorNode : DeclaratorNode
    {
        [JsonIgnore]
        public ExpressionNode? Initializer => this.Children.ElementAtOrDefault(1)?.As<ExpressionNode>();


        public VariableDeclaratorNode(int line, IdentifierNode identifier)
            : base(line, identifier) { }

        public VariableDeclaratorNode(int line, IdentifierNode identifier, ExpressionNode initializer)
            : base(line, identifier, initializer) { }


        public override string GetText()
            => this.Initializer is null ? this.Identifier : $"{this.Identifier} = {this.Initializer.GetText()}";
    }

    public sealed class ArrayDeclaratorNode : DeclaratorNode
    {
        [JsonIgnore]
        public ExpressionNode? SizeExpression => this.Children.ElementAtOrDefault(1) as ExpressionNode;

        [JsonIgnore]
        public ArrayInitializerListNode? Initializer
            => this.Children.Count > 2 ? this.Children[2].As<ArrayInitializerListNode>()
                                       : this.Children.ElementAtOrDefault(1) as ArrayInitializerListNode;


        public ArrayDeclaratorNode(int line, IdentifierNode identifier)
            : base(line, identifier) { }

        public ArrayDeclaratorNode(int line, IdentifierNode identifier, ExpressionNode sizeExpr)
            : base(line, identifier, sizeExpr) { }

        public ArrayDeclaratorNode(int line, IdentifierNode identifier, ArrayInitializerListNode init)
            : base(line, identifier, init)
        {

        }

        public ArrayDeclaratorNode(int line, IdentifierNode identifier, ExpressionNode sizeExpr, ArrayInitializerListNode init)
            : base(line, identifier, sizeExpr, init)
        {

        }


        public override string GetText()
        {
            var sb = new StringBuilder(this.Identifier);
            sb.Append('[').Append(this.SizeExpression?.ToString() ?? "").Append(']');
            if (this.Initializer is { }) 
                sb.Append(" = ").Append(this.Initializer.ToString());
            return sb.ToString();
        }
    }

    public sealed class ArrayInitializerListNode : ASTNode
    {
        [JsonIgnore]
        public IEnumerable<ExpressionNode> Initializers => this.Children.Cast<ExpressionNode>();


        public ArrayInitializerListNode(int line, IEnumerable<ExpressionNode> exprs)
            : base(line, exprs) { }

        public ArrayInitializerListNode(int line, params ExpressionNode[] exprs)
            : base(line, exprs) { }
    }

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
}
