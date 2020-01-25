using System;
using RICC.AST.Nodes;
using RICC.AST.Nodes.Common;
using RICC.Exceptions;

namespace RICC.AST.Visitors
{
    public sealed class ExpressionEvaluator : BaseASTVisitor<object>
    {
        public static object? Evaluate(ExpressionNode node)
            => new ExpressionEvaluator().Visit(node);

        public static bool TryEvaluateAs<T>(ExpressionNode node, out T result)
        {
            object? res = new ExpressionEvaluator().Visit(node);
            if (res is { } && res is T castRes) {
                result = castRes;
                return true;
            } else {
                result = default!;
                return false;
            }
        }


        public override object Visit(ArithmeticExpressionNode node)
        {
            (object? l, object? r) = this.VisitBinaryOperands(node);
            if (l is null || r is null)
                throw new EvaluationException();    // TODO
            return node.Operator.As<ArithmeticOperatorNode>().ApplyTo(l, r);
        }

        public override object Visit(RelationalExpressionNode node)
        {
            (object? l, object? r) = this.VisitBinaryOperands(node);
            if (l is null || r is null)
                throw new EvaluationException();    // TODO
            if (l is bool || r is bool)
                return node.Operator.As<RelationalOperatorNode>().ApplyTo(Convert.ToBoolean(l), Convert.ToBoolean(r));
            return node.Operator.As<RelationalOperatorNode>().ApplyTo(l, r);
        }

        public override object Visit(LogicExpressionNode node)
        {
            (object? l, object? r) = this.VisitBinaryOperands(node);
            return node.Operator.As<BinaryLogicOperatorNode>().ApplyTo(Convert.ToBoolean(l), Convert.ToBoolean(r));
        }

        public override object Visit(UnaryExpressionNode node)
        {
            object op = this.Visit(node.Operand as ASTNode);
            return node.Operator.ApplyTo(op);
        }

        public override object Visit(IncrementExpressionNode node)
        {
            object op = this.Visit(node.Expr as ASTNode);
            return BinaryOperations.AddPrimitive(op, 1);
        }

        public override object Visit(DecrementExpressionNode node)
        {
            object op = this.Visit(node.Expr as ASTNode);
            return BinaryOperations.SubtractPrimitive(op, 1);
        }

        public override object Visit(LiteralNode node) 
            => node.Value;


        private (object? left, object? right) VisitBinaryOperands(BinaryExpressionNode expr)
            => (this.Visit(expr.LeftOperand as ASTNode), this.Visit(expr.RightOperand as ASTNode));
    }
}
