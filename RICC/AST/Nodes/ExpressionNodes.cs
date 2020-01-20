using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace RICC.AST.Nodes
{
    public static class ASTNodeFactory
    {
        public static ExpressionNode CreateLiteralNode(int line, string value)
        {
            // TODO order
            if (int.TryParse(value, out int res_int))
                return new LiteralNode(line, res_int);
            else if (bool.TryParse(value, out bool res_bool))
                return new LiteralNode(line, res_bool);
            else if (byte.TryParse(value, out byte res_byte))
                return new LiteralNode(line, res_byte);
            else if (char.TryParse(value, out char res_char))
                return new LiteralNode(line, res_char);
            else if (short.TryParse(value, out short res_short))
                return new LiteralNode(line, res_short);
            else if (ushort.TryParse(value, out ushort res_ushort))
                return new LiteralNode(line, res_ushort);
            else if (uint.TryParse(value, out uint res_uint))
                return new LiteralNode(line, res_uint);
            else if (long.TryParse(value, out long res_long))
                return new LiteralNode(line, res_long);
            else if (ulong.TryParse(value, out ulong res_ulong))
                return new LiteralNode(line, res_ulong);
            else if (double.TryParse(value, out double res_double))
                return new LiteralNode(line, res_double);
            else if (float.TryParse(value, out float res_float))
                return new LiteralNode(line, res_float);
            else if (decimal.TryParse(value, out decimal res_decimal))
                return new LiteralNode(line, res_decimal);
            else
                return new LiteralNode(line, value);
        }
    }


    public abstract class ExpressionNode : ASTNode
    {
        protected ExpressionNode(int line, IEnumerable<ASTNode> children)
            : base(line, children) { }

        protected ExpressionNode(int line, params ASTNode[] children)
            : base(line, children) { }
    }

    public sealed class ExpressionListNode : ExpressionNode
    {
        [JsonIgnore]
        public IEnumerable<ExpressionNode> Expressions => this.Children.Cast<ExpressionNode>();


        public ExpressionListNode(int line, params ExpressionNode[] expressions)
            : base(line, expressions) { }

        public ExpressionListNode(int line, IEnumerable<ExpressionNode> expressions)
            : base(line, expressions) { }


        public override string GetText() => string.Join(", ", this.Children.Select(c => c.GetText()));
    }

    public class UnaryExpressionNode : ExpressionNode
    {
        [JsonIgnore]
        public UnaryOperatorNode Operator => this.Children[0].As<UnaryOperatorNode>();

        [JsonIgnore]
        public ExpressionNode Operand => this.Children[1].As<ExpressionNode>();


        public UnaryExpressionNode(int line, UnaryOperatorNode @operator, ExpressionNode operand)
            : base(line, @operator, operand) { }
    }

    public abstract class BinaryExpressionNode : ExpressionNode
    {
        [JsonIgnore]
        public BinaryOperatorNode Operator => this.Children[1].As<BinaryOperatorNode>();

        [JsonIgnore]
        public ExpressionNode LeftOperand => this.Children[0].As<ExpressionNode>();

        [JsonIgnore]
        public ExpressionNode RightOperand => this.Children[2].As<ExpressionNode>();


        protected BinaryExpressionNode(int line, ExpressionNode left, BinaryOperatorNode @operator, ExpressionNode right)
            : base(line, left, @operator, right) { }
    }

    public sealed class ArithmeticExpressionNode : BinaryExpressionNode
    {
        public ArithmeticExpressionNode(int line, ExpressionNode left, ArithmeticOperatorNode @operator, ExpressionNode right)
            : base(line, left, @operator, right) { }
    }

    public sealed class LogicExpressionNode : BinaryExpressionNode
    {
        public LogicExpressionNode(int line, ExpressionNode left, BinaryLogicOperatorNode @operator, ExpressionNode right)
            : base(line, left, @operator, right) { }
    }

    public sealed class RelationalExpressionNode : BinaryExpressionNode
    {
        public RelationalExpressionNode(int line, ExpressionNode left, RelationalOperatorNode @operator, ExpressionNode right)
            : base(line, left, @operator, right) { }
    }

    public sealed class AssignmentExpressionNode : BinaryExpressionNode
    {
        public AssignmentExpressionNode(int line, ExpressionNode left, AssignmentOperatorNode @operator, ExpressionNode right)
            : base(line, left, @operator, right) { }
    }

    public sealed class IdentifierNode : ExpressionNode
    {
        public string Identifier { get; }


        public IdentifierNode(int line, string identifier)
            : base(line)
        {
            if (string.IsNullOrWhiteSpace(identifier))
                throw new ArgumentException("Identifier name must be set.");
            this.Identifier = identifier;
        }


        public override string GetText() => this.Identifier;
    }

    public sealed class FunctionCallExpressionNode : ExpressionNode
    {
        [JsonIgnore]
        public string Identifier => this.Children[0].As<IdentifierNode>().Identifier;

        [JsonIgnore]
        public ExpressionListNode? Arguments => this.Children.ElementAtOrDefault(1)?.As<ExpressionListNode>();


        public FunctionCallExpressionNode(int line, IdentifierNode identifier)
            : base(line, identifier) { }

        public FunctionCallExpressionNode(int line, IdentifierNode identifier, ExpressionListNode parameters)
            : base(line, identifier, parameters) { }
     
        
        public override string GetText() => $"{this.Identifier}({this.Arguments?.GetText() ?? ""})";
    }

    public sealed class ArrayAccessExpressionNode : ExpressionNode
    {
        [JsonIgnore]
        public ExpressionNode Array => this.Children[0].As<ExpressionNode>();

        [JsonIgnore]
        public ExpressionNode IndexExpression => this.Children[1].As<ExpressionNode>();


        public ArrayAccessExpressionNode(int line, ExpressionNode array, ExpressionNode indexExpr)
            : base(line, array, indexExpr) { }


        public override string GetText() => $"{this.Array.GetText()}[{this.IndexExpression.GetText()}]";
    }

    public sealed class IncrementExpression : ExpressionNode
    {
        [JsonIgnore]
        public ExpressionNode Expr => this.Children[0].As<ExpressionNode>();


        public IncrementExpression(int line, ExpressionNode expr)
            : base(line, expr) { }
     

        public override string GetText() => $"{this.Expr.GetText()}++";
    }

    public sealed class DecrementExpression : ExpressionNode
    {
        [JsonIgnore]
        public ExpressionNode Expr => this.Children[0].As<ExpressionNode>();


        public DecrementExpression(int line, ExpressionNode expr)
            : base(line, expr) { }
     
        
        public override string GetText() => $"{this.Expr.GetText()}--";
    }

    public sealed class LiteralNode : ExpressionNode
    {
        public object Value { get; }
        public TypeCode TypeCode { get; }


        public LiteralNode(int line, object value)
            : base(line)
        {
            this.Value = value;
            this.TypeCode = Type.GetTypeCode(value.GetType());
        }
     
        
        public override string GetText() => this.Value?.ToString() ?? "";
    }

    public sealed class ConditionalExpressionNode : ExpressionNode
    {
        [JsonIgnore]
        public ExpressionNode Condition => this.Children[0].As<ExpressionNode>();

        [JsonIgnore]
        public ExpressionNode ThenExpression => this.Children[1].As<ExpressionNode>();

        [JsonIgnore]
        public ExpressionNode ElseExpression => this.Children[1].As<ExpressionNode>();


        public ConditionalExpressionNode(int line, ExpressionNode cond, ExpressionNode @then, ExpressionNode @else)
            : base(line, cond, @then, @else) { }


        public override string GetText() 
            => $"{this.Condition.GetText()} ? {this.ThenExpression.GetText()} : {this.ElseExpression.GetText()}";
    }
}
