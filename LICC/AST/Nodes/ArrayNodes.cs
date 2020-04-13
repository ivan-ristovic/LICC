using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace LICC.AST.Nodes
{
    public sealed class ArrDeclNode : DeclNode
    {
        [JsonIgnore]
        public ExprNode? SizeExpression => this.Children.ElementAtOrDefault(1) as ExprNode;

        [JsonIgnore]
        public ArrInitListNode? Initializer
            => this.Children.Count > 2 ? this.Children[2].As<ArrInitListNode>()
                                       : this.Children.ElementAtOrDefault(1) as ArrInitListNode;


        public ArrDeclNode(int line, IdNode identifier)
            : base(line, identifier) { }

        public ArrDeclNode(int line, IdNode identifier, ExprNode sizeExpr)
            : base(line, identifier, sizeExpr) { }

        public ArrDeclNode(int line, IdNode identifier, ArrInitListNode init)
            : base(line, identifier, init)
        {

        }

        public ArrDeclNode(int line, IdNode identifier, ExprNode sizeExpr, ArrInitListNode init)
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

    public sealed class ArrInitListNode : ASTNode
    {
        [JsonIgnore]
        public IEnumerable<ExprNode> Initializers => this.Children.Cast<ExprNode>();


        public ArrInitListNode(int line, IEnumerable<ExprNode> exprs)
            : base(line, exprs) { }

        public ArrInitListNode(int line, params ExprNode[] exprs)
            : base(line, exprs) { }


        public override string GetText() => $"{{ {string.Join(", ", this.Initializers.Select(i => i.GetText()))} }}";
    }
}
