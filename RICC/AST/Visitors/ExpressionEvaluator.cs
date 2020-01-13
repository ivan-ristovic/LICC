using RICC.AST.Nodes;
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
#pragma warning disable CS8653 // A default expression introduces a null value for a type parameter.
                result = default;
#pragma warning restore CS8653 // A default expression introduces a null value for a type parameter.
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
            return node.Operator.As<RelationalOperatorNode>().ApplyTo(l, r);
        }

        public override object Visit(LogicExpressionNode node)
        {
            (object? l, object? r) = this.VisitBinaryOperands(node);
            if (!(l is bool) || !(r is bool))
                throw new EvaluationException();    // TODO
            return node.Operator.As<LogicOperatorNode>().ApplyTo((bool)l, (bool)r);
        }

        public override object Visit(LiteralNode node) 
            => node.Value;


        private (object? left, object? right) VisitBinaryOperands(BinaryExpressionNode expr)
            => (this.Visit(expr.LeftOperand as ASTNode), this.Visit(expr.RightOperand as ASTNode));
    }
}
