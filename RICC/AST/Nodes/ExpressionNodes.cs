using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using RICC.AST.Nodes.Common;
using RICC.Exceptions;

namespace RICC.AST.Nodes
{
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

        public override bool Equals([AllowNull] ASTNode other) 
            => base.Equals(other) && this.Identifier.Equals(other.As<IdentifierNode>().Identifier);
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

    public sealed class IncrementExpressionNode : ExpressionNode
    {
        [JsonIgnore]
        public ExpressionNode Expr => this.Children[0].As<ExpressionNode>();


        public IncrementExpressionNode(int line, ExpressionNode expr)
            : base(line, expr) { }


        public override string GetText() => $"{this.Expr.GetText()}++";
    }

    public sealed class DecrementExpressionNode : ExpressionNode
    {
        [JsonIgnore]
        public ExpressionNode Expr => this.Children[0].As<ExpressionNode>();


        public DecrementExpressionNode(int line, ExpressionNode expr)
            : base(line, expr) { }


        public override string GetText() => $"{this.Expr.GetText()}--";
    }

    public sealed class LiteralNode : ExpressionNode
    {
        public static LiteralNode FromString(int line, string str)
        {
            if (!Constants.TryConvert(str, out object? value, out string? suffix) || value is null)
                throw new NotImplementedException($"Literal {str} is not supported");    
            return new LiteralNode(line, value, suffix);
        }


        public object Value { get; }
        public string? Suffix { get; }
        public TypeCode TypeCode { get; }


        public LiteralNode(int line, object value, string? suffix = null)
            : base(line)
        {
            this.Suffix = suffix?.ToUpper();
            this.Value = value;
            this.TypeCode = Type.GetTypeCode(value.GetType());
        }


        public override string GetText() => this.Value?.ToString() ?? "";

        public override bool Equals([AllowNull] ASTNode other)
        {
            var lit = other as LiteralNode;
            return base.Equals(other) && this.TypeCode.Equals(lit?.TypeCode) && this.Value.Equals(lit?.Value);
        }
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
