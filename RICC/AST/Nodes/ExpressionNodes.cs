using System;
using System.Collections.Generic;
using System.Text;
using RICC.AST.Nodes.Common;
using RICC.Exceptions;

namespace RICC.AST.Nodes
{
    public static class ASTNodeFactory
    {
        public static ExpressionNode CreateLiteralNode(int line, string value, ASTNode? parent = null)
        {
            // TODO order
            if (int.TryParse(value, out int res_int))
                return new LiteralNode<int>(line, res_int, parent);
            else if (bool.TryParse(value, out bool res_bool))
                return new LiteralNode<bool>(line, res_bool, parent);
            else if (byte.TryParse(value, out byte res_byte))
                return new LiteralNode<byte>(line, res_byte, parent);
            else if (char.TryParse(value, out char res_char))
                return new LiteralNode<char>(line, res_char, parent);
            else if (short.TryParse(value, out short res_short))
                return new LiteralNode<short>(line, res_short, parent);
            else if (ushort.TryParse(value, out ushort res_ushort))
                return new LiteralNode<ushort>(line, res_ushort, parent);
            else if (uint.TryParse(value, out uint res_uint))
                return new LiteralNode<uint>(line, res_uint, parent);
            else if (long.TryParse(value, out long res_long))
                return new LiteralNode<long>(line, res_long, parent);
            else if (ulong.TryParse(value, out ulong res_ulong))
                return new LiteralNode<ulong>(line, res_ulong, parent);
            else if (double.TryParse(value, out double res_double))
                return new LiteralNode<double>(line, res_double, parent);
            else if (float.TryParse(value, out float res_float))
                return new LiteralNode<float>(line, res_float, parent);
            else if (decimal.TryParse(value, out decimal res_decimal))
                return new LiteralNode<decimal>(line, res_decimal, parent);
            else
                return new LiteralNode<string>(line, value, parent);
        }
    }


    public abstract class ExpressionNode : ASTNode
    {
        protected ExpressionNode(int line, IEnumerable<ASTNode> children, ASTNode? parent = null)
            : base(line, children, parent)
        {

        }

        protected ExpressionNode(int line, ASTNode? parent = null)
            : base(line, parent)
        {

        }


        public abstract object Evaluate();

        public bool EvaluateAs<T>(out T? result) where T : struct
        {
            object res = this.Evaluate();
            if (res is T castRes) {
                result = castRes;
                return true;
            } else {
                result = default;
                return false;
            }
        }
    }

    public class UnaryExpressionNode : ExpressionNode
    {
        public UnaryOperatorNode Operator => this.Children[0].As<UnaryOperatorNode>();
        public ExpressionNode Operand => this.Children[1].As<ExpressionNode>();


        public UnaryExpressionNode(int line, UnaryOperatorNode @operator, ExpressionNode operand, ASTNode? parent = null)
            : base(line, new ASTNode[] { @operator, operand }, parent)
        {

        }


        public override object Evaluate() => this.Operator.ApplyTo(this.Operand);
    }

    public abstract class BinaryExpressionNode : ExpressionNode
    {
        public BinaryOperatorNode Operator => this.Children[1].As<BinaryOperatorNode>();
        public ExpressionNode LeftOperand => this.Children[0].As<ExpressionNode>();
        public ExpressionNode RightOperand => this.Children[2].As<ExpressionNode>();


        protected BinaryExpressionNode(int line, ExpressionNode left, BinaryOperatorNode @operator, ExpressionNode right, ASTNode? parent = null)
            : base(line, new ASTNode[] { left, @operator, right }, parent)
        {

        }
    }

    public sealed class ArithmeticExpressionNode : BinaryExpressionNode
    {
        public ArithmeticExpressionNode(int line, ExpressionNode left, ArithmeticOperatorNode @operator, ExpressionNode right, ASTNode? parent = null)
            : base(line, left, @operator, right, parent)
        {

        }


        public override object Evaluate() => this.Operator.As<ArithmeticOperatorNode>().ApplyTo(this.LeftOperand.Evaluate(), this.RightOperand.Evaluate());
    }

    public sealed class LogicExpressionNode : BinaryExpressionNode
    {
        public LogicExpressionNode(int line, ExpressionNode left, LogicOperatorNode @operator, ExpressionNode right, ASTNode? parent = null)
            : base(line, left, @operator, right, parent)
        {

        }


        public override object Evaluate()
        {
            if (!this.LeftOperand.EvaluateAs(out bool? left) || !this.RightOperand.EvaluateAs(out bool? right))
                throw new EvaluationException("Logic expression not returning bool!");

            return this.Operator.As<LogicOperatorNode>().ApplyTo(left ?? false, right ?? false);
        }
    }

    public sealed class RelationalExpressionNode : BinaryExpressionNode
    {
        public RelationalExpressionNode(int line, ExpressionNode left, RelationalOperatorNode @operator, ExpressionNode right, ASTNode? parent = null)
            : base(line, left, @operator, right, parent)
        {

        }


        public override object Evaluate() => this.Operator.As<RelationalOperatorNode>().ApplyTo(this.LeftOperand.Evaluate(), this.RightOperand.Evaluate());
    }

    public sealed class IdentifierNode : ExpressionNode
    {
        public string Identifier { get; }


        public IdentifierNode(int line, string identifier, ASTNode? parent = null)
            : base(line, parent)
        {
            if (string.IsNullOrWhiteSpace(identifier))
                throw new ArgumentException("Identifier name must be set.");
            this.Identifier = identifier;
        }


        public override object Evaluate() => throw new NotImplementedException();   // TODO
    }

    public sealed class AssignmentExpressionNode : ExpressionNode
    {
        public AssignmentOperatorNode Operator => this.Children[1].As<AssignmentOperatorNode>();
        public ExpressionNode LeftValue => this.Children[0].As<ExpressionNode>();
        public ExpressionNode RightValue => this.Children[2].As<ExpressionNode>();


        public AssignmentExpressionNode(int line, ExpressionNode left, AssignmentOperatorNode @operator, ExpressionNode right, ASTNode? parent = null)
            : base(line, new ASTNode[] { left, @operator, right }, parent)
        {

        }


        public override object Evaluate() => this.RightValue.Evaluate();   // TODO assign to left value
    }

    public sealed class LiteralNode<TValue> : ExpressionNode
    {
        public TValue Value { get; }


        public LiteralNode(int line, TValue value, ASTNode? parent = null)
            : base(line, parent)
        {
            this.Value = value;
        }


        public override object Evaluate() => this.Value ?? throw new EvaluationException("Literal value not set.");
    }
}
