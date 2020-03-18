using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace RICC.AST.Nodes
{
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


        public override string GetText() => $"{{ {string.Join(", ", this.Initializers.Select(i => i.GetText()))} }}";
    }
}
