using System;
using System.Collections.Generic;
using System.Text;
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
    }

    public class UnaryExpressionNode : ExpressionNode
    {
        public UnaryOperatorNode Operator => this.Children[0].As<UnaryOperatorNode>();
        public ExpressionNode Operand => this.Children[1].As<ExpressionNode>();


        public UnaryExpressionNode(int line, UnaryOperatorNode @operator, ExpressionNode operand, ASTNode? parent = null)
            : base(line, new ASTNode[] { @operator, operand }, parent)
        {

        }
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
    }

    public sealed class LogicExpressionNode : BinaryExpressionNode
    {
        public LogicExpressionNode(int line, ExpressionNode left, LogicOperatorNode @operator, ExpressionNode right, ASTNode? parent = null)
            : base(line, left, @operator, right, parent)
        {

        }
    }
}
