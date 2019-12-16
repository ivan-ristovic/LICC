using System;
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

        public override ASTNode VisitLogicalOrExpression([NotNull] LogicalOrExpressionContext ctx)
        {

            if (ctx.ChildCount > 1) {
                ExpressionNode left = this.Visit(ctx.logicalOrExpression()).As<ExpressionNode>();
                ExpressionNode right = this.Visit(ctx.logicalAndExpression()).As<ExpressionNode>();
                string sign = ctx.children[1].GetText();
                var op = new LogicOperatorNode(ctx.Start.Line, sign, (x, y) => x || y);
                var expr = new LogicExpressionNode(ctx.Start.Line, left, op, right);
                left.Parent = right.Parent = op.Parent = expr;
                return expr;
            } else {
                return this.Visit(ctx.logicalAndExpression());
            }
        }

        public override ASTNode VisitLogicalAndExpression([NotNull] LogicalAndExpressionContext ctx)
        {
            if (ctx.ChildCount > 1) {
                ExpressionNode left = this.Visit(ctx.logicalAndExpression()).As<ExpressionNode>();
                ExpressionNode right = this.Visit(ctx.inclusiveOrExpression()).As<ExpressionNode>();
                string sign = ctx.children[1].GetText();
                var op = new LogicOperatorNode(ctx.Start.Line, sign, (x, y) => x && y);
                var expr = new LogicExpressionNode(ctx.Start.Line, left, op, right);
                left.Parent = right.Parent = op.Parent = expr;
                return expr;
            } else {
                return this.Visit(ctx.inclusiveOrExpression());
            }
        }

        public override ASTNode VisitInclusiveOrExpression([NotNull] InclusiveOrExpressionContext ctx) => this.Visit(ctx.exclusiveOrExpression());

        public override ASTNode VisitExclusiveOrExpression([NotNull] ExclusiveOrExpressionContext ctx) => this.Visit(ctx.andExpression());

        public override ASTNode VisitAndExpression([NotNull] AndExpressionContext ctx) => this.Visit(ctx.equalityExpression());

        public override ASTNode VisitEqualityExpression([NotNull] EqualityExpressionContext ctx)
        {
            if (ctx.equalityExpression() is null)
                return this.Visit(ctx.relationalExpression());

            ExpressionNode left = this.Visit(ctx.equalityExpression()).As<ExpressionNode>();
            ExpressionNode right = this.Visit(ctx.relationalExpression()).As<ExpressionNode>();
            string sign = ctx.children[1].GetText();
            var op = new RelationalOperatorNode(ctx.Start.Line, sign, BinaryOperations.RelationalFromSymbol(sign));
            var expr = new RelationalExpressionNode(ctx.Start.Line, left, op, right);
            left.Parent = right.Parent = op.Parent = expr;
            return expr;
        }

        public override ASTNode VisitRelationalExpression([NotNull] RelationalExpressionContext ctx)
        {
            if (ctx.relationalExpression() is null)
                return this.Visit(ctx.shiftExpression());

            ExpressionNode left = this.Visit(ctx.relationalExpression()).As<ExpressionNode>();
            ExpressionNode right = this.Visit(ctx.shiftExpression()).As<ExpressionNode>();
            string sign = ctx.children[1].GetText();
            var op = new RelationalOperatorNode(ctx.Start.Line, sign, BinaryOperations.RelationalFromSymbol(sign));
            var expr = new RelationalExpressionNode(ctx.Start.Line, left, op, right);
            left.Parent = right.Parent = op.Parent = expr;
            return expr;
        }

        public override ASTNode VisitShiftExpression([NotNull] ShiftExpressionContext ctx)
        {
            if (ctx.ChildCount > 1) {
                ExpressionNode left = this.Visit(ctx.shiftExpression()).As<ExpressionNode>();
                ExpressionNode right = this.Visit(ctx.additiveExpression()).As<ExpressionNode>();
                string sign = ctx.children[1].GetText();
                var op = new ArithmeticOperatorNode(ctx.Start.Line, sign, BinaryOperations.ArithmeticFromSymbol(sign));
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
                var op = new ArithmeticOperatorNode(ctx.Start.Line, sign, BinaryOperations.ArithmeticFromSymbol(sign));
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
                var op = new ArithmeticOperatorNode(ctx.Start.Line, sign, BinaryOperations.ArithmeticFromSymbol(sign));
                var expr = new ArithmeticExpressionNode(ctx.Start.Line, left, op, right);
                left.Parent = right.Parent = op.Parent = expr;
                return expr;
            } else {
                return this.Visit(ctx.castExpression());
            }
        }

        public override ASTNode VisitCastExpression([NotNull] CastExpressionContext ctx) => this.Visit(ctx.unaryExpression());          // TODO

        public override ASTNode VisitUnaryExpression([NotNull] UnaryExpressionContext ctx) => this.Visit(ctx.postfixExpression());      // TODO

        public override ASTNode VisitPostfixExpression([NotNull] PostfixExpressionContext ctx)
        {
            if (ctx.primaryExpression() is { })
                return this.Visit(ctx.primaryExpression());

            if (ctx.postfixExpression() is { }) {
                string token = ctx.children[1].GetText();
                return token switch
                {
                    "[" => throw new NotImplementedException(),     // TODO array access
                    "(" => throw new NotImplementedException(),     // TODO function call
                    "." => throw new NotImplementedException(),     // TODO struct field access
                    "->" => throw new NotImplementedException(),    // TODO pointer
                    "++" => throw new NotImplementedException(),    // TODO ++
                    "--" => throw new NotImplementedException(),    // TODO --
                    _ => throw new NotImplementedException()
                };
            }

            // Should not reach here
            throw new NotImplementedException();
        }

        public override ASTNode VisitPrimaryExpression([NotNull] PrimaryExpressionContext ctx)
        {
            // TODO
            if (ctx.Identifier() is { })
                return new IdentifierNode(ctx.Start.Line, ctx.Identifier().GetText());
            else if (ctx.Constant() is { })
                return ASTNodeFactory.CreateLiteralNode(ctx.Start.Line, ctx.Constant().GetText());
            else if (ctx.expression() is { })
                return this.Visit(ctx.expression());
            else if (ctx.StringLiteral() is { }) 
                return new LiteralNode<string>(ctx.Start.Line, string.Join("", ctx.StringLiteral().Select(t => t.GetText()[1..^1])));
            else // TODO
                return null; 
        }
    }
}
