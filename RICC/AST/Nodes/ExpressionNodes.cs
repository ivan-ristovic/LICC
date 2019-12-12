using System;
using System.Collections.Generic;
using System.Text;
using RICC.AST.Nodes.Common;
using RICC.Exceptions;

namespace RICC.AST.Nodes
{
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
                throw new Exception("TODO"); // TODO new exception type for this

            return this.Operator.As<LogicOperatorNode>().ApplyTo(left ?? false, right ?? false);
        }
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

    public sealed class LiteralNode<TValue> : ExpressionNode
    {
        public TValue Value { get; }


        public LiteralNode(int line, TValue value, ASTNode? parent = null)
            : base(line, parent)
        {
            this.Value = value;
        }


        public override object Evaluate() => this.Value ?? new object();    // FIXME
    }
}
