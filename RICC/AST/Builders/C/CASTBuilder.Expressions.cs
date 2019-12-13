using System.Linq;
using Antlr4.Runtime.Misc;
using RICC.AST.Nodes;
using RICC.AST.Nodes.Common;
using Serilog;
using static RICC.AST.Builders.C.CParser;

namespace RICC.AST.Builders.C
{
    public sealed partial class CASTBuilder : CBaseVisitor<ASTNode>, IASTBuilder
    {
        public override ASTNode VisitExpression([NotNull] ExpressionContext ctx) => this.Visit(ctx.assignmentExpression()); // TODO list

        public override ASTNode VisitAssignmentExpression([NotNull] AssignmentExpressionContext ctx) => this.Visit(ctx.conditionalExpression());

        public override ASTNode VisitConditionalExpression([NotNull] ConditionalExpressionContext ctx) => this.Visit(ctx.logicalOrExpression());

        public override ASTNode VisitLogicalOrExpression([NotNull] LogicalOrExpressionContext ctx) => this.Visit(ctx.logicalAndExpression());

        public override ASTNode VisitLogicalAndExpression([NotNull] LogicalAndExpressionContext ctx) => this.Visit(ctx.inclusiveOrExpression());

        public override ASTNode VisitInclusiveOrExpression([NotNull] InclusiveOrExpressionContext ctx) => this.Visit(ctx.exclusiveOrExpression());

        public override ASTNode VisitExclusiveOrExpression([NotNull] ExclusiveOrExpressionContext ctx) => this.Visit(ctx.andExpression());

        public override ASTNode VisitAndExpression([NotNull] AndExpressionContext ctx) => this.Visit(ctx.equalityExpression());

        public override ASTNode VisitEqualityExpression([NotNull] EqualityExpressionContext ctx) => this.Visit(ctx.relationalExpression());

        public override ASTNode VisitRelationalExpression([NotNull] RelationalExpressionContext ctx) => this.Visit(ctx.shiftExpression());

        public override ASTNode VisitShiftExpression([NotNull] ShiftExpressionContext ctx)
        {
            if (ctx.ChildCount > 1) {
                ExpressionNode left = this.Visit(ctx.shiftExpression()).As<ExpressionNode>();
                ExpressionNode right = this.Visit(ctx.additiveExpression()).As<ExpressionNode>();
                string sign = ctx.children[1].GetText();
                var op = new ArithmeticOperatorNode(ctx.Start.Line, sign, BinaryOperations.DetermineFromSign(sign));
                var expr = new ArithmeticExpressionNode(ctx.Start.Line, left, op, right);
                left.Parent = right.Parent = op.Parent = expr;
                return expr;
            } else {
                return this.Visit(ctx.additiveExpression());
            }
        }

        public override ASTNode VisitAdditiveExpression([NotNull] AdditiveExpressionContext ctx)
        {
            if (ctx.ChildCount > 1) {
                ExpressionNode left = this.Visit(ctx.additiveExpression()).As<ExpressionNode>();
                ExpressionNode right = this.Visit(ctx.multiplicativeExpression()).As<ExpressionNode>();
                string sign = ctx.children[1].GetText();
                var op = new ArithmeticOperatorNode(ctx.Start.Line, sign, BinaryOperations.DetermineFromSign(sign));
                var expr = new ArithmeticExpressionNode(ctx.Start.Line, left, op, right);
                left.Parent = right.Parent = op.Parent = expr;
                return expr;
            } else {
                return this.Visit(ctx.multiplicativeExpression());
            }
        }

        public override ASTNode VisitMultiplicativeExpression([NotNull] MultiplicativeExpressionContext ctx)
        {
            if (ctx.ChildCount > 1) {
                ExpressionNode left = this.Visit(ctx.multiplicativeExpression()).As<ExpressionNode>();
                ExpressionNode right = this.Visit(ctx.castExpression()).As<ExpressionNode>();
                string sign = ctx.children[1].GetText();
                var op = new ArithmeticOperatorNode(ctx.Start.Line, sign, BinaryOperations.DetermineFromSign(sign));
                var expr = new ArithmeticExpressionNode(ctx.Start.Line, left, op, right);
                left.Parent = right.Parent = op.Parent = expr;
                return expr;
            } else {
                return this.Visit(ctx.castExpression());
            }
        }

        public override ASTNode VisitCastExpression([NotNull] CastExpressionContext ctx) => this.Visit(ctx.unaryExpression());

        public override ASTNode VisitUnaryExpression([NotNull] UnaryExpressionContext ctx) => this.Visit(ctx.postfixExpression());

        public override ASTNode VisitPostfixExpression([NotNull] PostfixExpressionContext ctx)
        {
            return this.Visit(ctx.primaryExpression());
        }

        public override ASTNode VisitPrimaryExpression([NotNull] PrimaryExpressionContext ctx)
        {
            // TODO
            if (ctx.Identifier() is { })
                return new IdentifierNode(ctx.Start.Line, ctx.Identifier().GetText());
            else if (ctx.Constant() is { })
                return ASTNodeFactory.CreateLiteralNode(ctx.Start.Line, ctx.Constant().GetText());
            else if (ctx.StringLiteral() is { }) 
                return new LiteralNode<string>(ctx.Start.Line, string.Join("", ctx.StringLiteral().Select(t => t.GetText()[1..^1])));
            else if (ctx.expression() is { })
                return this.Visit(ctx.expression());
            else // TODO
                return null; 
        }
    }
}
