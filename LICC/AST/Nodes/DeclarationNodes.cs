using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using LICC.AST.Nodes.Common;
using Serilog;

namespace LICC.AST.Nodes
{
    public abstract class DeclarationNode : ASTNode
    {
        protected DeclarationNode(int line, IEnumerable<ASTNode> children)
            : base(line, children) { }

        protected DeclarationNode(int line, params ASTNode[] children)
            : base(line, children) { }
    }

    public class DeclSpecsNode : ASTNode
    {
        public DeclKeywords Keywords { get; }
        public string TypeName { get; }

        [JsonIgnore]
        public Type? Type { get; }


        public DeclSpecsNode(int line)
            : this(line, "object")
        {

        }

        public DeclSpecsNode(int line, string type)
            : this(line, "", type)
        {

        }

        public DeclSpecsNode(int line, string specs, string type)
            : base(line)
        {
            this.Keywords = DeclKeywords.Parse(specs);
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
            => this.Equals(obj as DeclSpecsNode);

        public override bool Equals([AllowNull] ASTNode other)
        {
            if (!base.Equals(other))
                return false;

            var decl = other as DeclSpecsNode;
            if (!this.Keywords.Equals(decl?.Keywords))
                return false;
            return this.Type is { } ? this.Type.Equals(decl?.Type) : this.TypeName.Equals(decl?.TypeName);
        }
    }

    public abstract class DeclNode : DeclarationNode
    {
        [JsonIgnore]
        public IdNode IdentifierNode => this.Children.First().As<IdNode>();

        [JsonIgnore]
        public string Identifier => this.IdentifierNode.Identifier;


        public DeclNode(int line, IdNode identifier, params ASTNode[] children)
            : base(line, new[] { identifier }.Concat(children)) { }

        public DeclNode(int line, IdNode identifier, IEnumerable<ASTNode> children)
            : base(line, new[] { identifier }.Concat(children)) { }
    }

    public sealed class DeclListNode : DeclarationNode
    {
        [JsonIgnore]
        public IEnumerable<DeclNode> Declarations => this.Children.Cast<DeclNode>();


        public DeclListNode(int line, IEnumerable<DeclNode> decls)
            : base(line, decls) { }

        public DeclListNode(int line, params DeclNode[] decls)
            : base(line, decls) { }

        public override string GetText() => string.Join(", ", this.Children.Select(c => c.GetText()));
    }

    public sealed class VarDeclNode : DeclNode
    {
        [JsonIgnore]
        public ExprNode? Initializer => this.Children.ElementAtOrDefault(1)?.As<ExprNode>();


        public VarDeclNode(int line, IdNode identifier)
            : base(line, identifier) { }

        public VarDeclNode(int line, IdNode identifier, ExprNode initializer)
            : base(line, identifier, initializer) { }


        public override string GetText()
            => this.Initializer is null ? this.Identifier : $"{this.Identifier} = {this.Initializer.GetText()}";
    }
}
