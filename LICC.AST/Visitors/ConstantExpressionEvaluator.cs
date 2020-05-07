﻿using System;
using LICC.AST.Exceptions;
using LICC.AST.Nodes;
using LICC.AST.Nodes.Common;

namespace LICC.AST.Visitors
{
    public sealed class ConstantExpressionEvaluator : BaseASTVisitor<object?>
    {
        public static object? Evaluate(ExprNode node)
            => new ConstantExpressionEvaluator().Visit(node);

        public static bool TryEvaluateAs<T>(ExprNode node, out T result)
        {
            object? res = new ConstantExpressionEvaluator().Visit(node);
            if (res is { } && res is T castRes) {
                result = castRes;
                return true;
            } else {
                result = default!;
                return false;
            }
        }


        public override object? Visit(ArithmExprNode node)
        {
            (object? l, object? r) = this.VisitBinaryOperands(node);
            if (l is null || r is null || l is NullLitExprNode || r is NullLitExprNode)
                throw new EvaluationException("Null reference in expression");
            return node.Operator.As<ArithmOpNode>().ApplyTo(l, r);
        }

        public override object? Visit(RelExprNode node)
        {
            (object? l, object? r) = this.VisitBinaryOperands(node);
            if (l is null || r is null || l is NullLitExprNode || r is NullLitExprNode)
                    throw new EvaluationException("Null reference in expression");
            if (l is bool || r is bool)
                return node.Operator.As<RelOpNode>().ApplyTo(Convert.ToBoolean(l), Convert.ToBoolean(r));
            return node.Operator.As<RelOpNode>().ApplyTo(l, r);
        }

        public override object? Visit(LogicExprNode node)
        {
            (object? l, object? r) = this.VisitBinaryOperands(node);
            return node.Operator.As<BinaryLogicOpNode>().ApplyTo(Convert.ToBoolean(l), Convert.ToBoolean(r));
        }

        public override object? Visit(UnaryExprNode node)
        {
            object op = this.Visit(node.Operand as ASTNode) ?? throw new EvaluationException("Null reference in expression");
            return node.Operator.ApplyTo(op);
        }

        public override object? Visit(IncExprNode node)
        {
            object op = this.Visit(node.Expr as ASTNode) ?? throw new EvaluationException("Null reference in expression");
            return BinaryOperations.AddPrimitive(op, 1);
        }

        public override object? Visit(DecExprNode node)
        {
            object op = this.Visit(node.Expr as ASTNode) ?? throw new EvaluationException("Null reference in expression");
            return BinaryOperations.SubtractPrimitive(op, 1);
        }

        public override object? Visit(LitExprNode node)
            => node?.Value;

        public override object? Visit(NullLitExprNode node)
            => null;

        public override object? Visit(IdNode node)
            => throw new EvaluationException("Expression is not constant.");
        

        private (object? left, object? right) VisitBinaryOperands(BinaryExprNode expr)
            => (this.Visit(expr.LeftOperand as ASTNode), this.Visit(expr.RightOperand as ASTNode));
    }
}
