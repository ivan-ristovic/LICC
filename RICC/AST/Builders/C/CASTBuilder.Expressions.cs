using System;
using System.Linq;
using Antlr4.Runtime.Misc;
using RICC.AST.Nodes;
using RICC.AST.Nodes.Common;
using static RICC.AST.Builders.C.CParser;

namespace RICC.AST.Builders.C
{
    public sealed partial class CASTBuilder : CBaseVisitor<ASTNode>, IASTBuilder
    {
        public override ASTNode VisitExpression([NotNull] ExpressionContext ctx)
        {
            if (ctx.expression() is { })
                throw new NotImplementedException("expression list");

            return this.Visit(ctx.assignmentExpression());
        }

        public override ASTNode VisitAssignmentExpression([NotNull] AssignmentExpressionContext ctx)
        {
            if (ctx.DigitSequence() is { })
                throw new NotImplementedException("digit sequence");

            if (ctx.conditionalExpression() is { })
                return this.Visit(ctx.conditionalExpression());

            ExpressionNode unary = this.Visit(ctx.unaryExpression()).As<ExpressionNode>();
            string symbol = ctx.children[1].GetText();
            var op = new AssignmentOperatorNode(ctx.Start.Line, symbol, BinaryOperations.AssignmentFromSymbol(symbol));
            ExpressionNode expr = this.Visit(ctx.assignmentExpression()).As<ExpressionNode>();

            return new AssignmentExpressionNode(ctx.Start.Line, unary, op, expr);
        }

        public override ASTNode VisitConditionalExpression([NotNull] ConditionalExpressionContext ctx)
        {
            ExpressionNode expr = this.Visit(ctx.logicalOrExpression()).As<ExpressionNode>();
            if (ctx.expression() is null)
                return expr;

            ExpressionNode thenExpr = this.Visit(ctx.expression()).As<ExpressionNode>();
            ExpressionNode elseExpr = this.Visit(ctx.conditionalExpression()).As<ExpressionNode>();
            return new ConditionalExpressionNode(ctx.Start.Line, expr, thenExpr, elseExpr);
        }

        public override ASTNode VisitLogicalOrExpression([NotNull] LogicalOrExpressionContext ctx)
        {
            if (ctx.ChildCount > 1) {
                ExpressionNode left = this.Visit(ctx.logicalOrExpression()).As<ExpressionNode>();
                ExpressionNode right = this.Visit(ctx.logicalAndExpression()).As<ExpressionNode>();
                string symbol = ctx.children[1].GetText();
                var op = new BinaryLogicOperatorNode(ctx.Start.Line, symbol, (x, y) => x || y);
                return new LogicExpressionNode(ctx.Start.Line, left, op, right);
            } else {
                return this.Visit(ctx.logicalAndExpression());
            }
        }

        public override ASTNode VisitLogicalAndExpression([NotNull] LogicalAndExpressionContext ctx)
        {
            if (ctx.ChildCount > 1) {
                ExpressionNode left = this.Visit(ctx.logicalAndExpression()).As<ExpressionNode>();
                ExpressionNode right = this.Visit(ctx.inclusiveOrExpression()).As<ExpressionNode>();
                string symbol = ctx.children[1].GetText();
                var op = new BinaryLogicOperatorNode(ctx.Start.Line, symbol, (x, y) => x && y);
                return new LogicExpressionNode(ctx.Start.Line, left, op, right);
            } else {
                return this.Visit(ctx.inclusiveOrExpression());
            }
        }

        public override ASTNode VisitInclusiveOrExpression([NotNull] InclusiveOrExpressionContext ctx)
        {
            if (ctx.ChildCount > 1) {
                ExpressionNode left = this.Visit(ctx.inclusiveOrExpression()).As<ExpressionNode>();
                ExpressionNode right = this.Visit(ctx.exclusiveOrExpression()).As<ExpressionNode>();
                string symbol = ctx.children[1].GetText();
                var op = new ArithmeticOperatorNode(ctx.Start.Line, symbol, BinaryOperations.BitwiseOrPrimitive);
                return new ArithmeticExpressionNode(ctx.Start.Line, left, op, right);
            } else {
                return this.Visit(ctx.exclusiveOrExpression());
            }
        }

        public override ASTNode VisitExclusiveOrExpression([NotNull] ExclusiveOrExpressionContext ctx)
        {
            if (ctx.ChildCount > 1) {
                ExpressionNode left = this.Visit(ctx.exclusiveOrExpression()).As<ExpressionNode>();
                ExpressionNode right = this.Visit(ctx.andExpression()).As<ExpressionNode>();
                string symbol = ctx.children[1].GetText();
                var op = new ArithmeticOperatorNode(ctx.Start.Line, symbol, BinaryOperations.BitwiseXorPrimitive);
                return new ArithmeticExpressionNode(ctx.Start.Line, left, op, right);
            } else {
                return this.Visit(ctx.andExpression());
            }
        }

        public override ASTNode VisitAndExpression([NotNull] AndExpressionContext ctx)
        {
            if (ctx.ChildCount > 1) {
                ExpressionNode left = this.Visit(ctx.andExpression()).As<ExpressionNode>();
                ExpressionNode right = this.Visit(ctx.equalityExpression()).As<ExpressionNode>();
                string symbol = ctx.children[1].GetText();
                var op = new ArithmeticOperatorNode(ctx.Start.Line, symbol, BinaryOperations.BitwiseAndPrimitive);
                return new ArithmeticExpressionNode(ctx.Start.Line, left, op, right);
            } else {
                return this.Visit(ctx.equalityExpression());
            }
        }

        public override ASTNode VisitEqualityExpression([NotNull] EqualityExpressionContext ctx)
        {
            if (ctx.equalityExpression() is null)
                return this.Visit(ctx.relationalExpression());

            ExpressionNode left = this.Visit(ctx.equalityExpression()).As<ExpressionNode>();
            ExpressionNode right = this.Visit(ctx.relationalExpression()).As<ExpressionNode>();
            string symbol = ctx.children[1].GetText();
            var op = new RelationalOperatorNode(ctx.Start.Line, symbol, BinaryOperations.RelationalFromSymbol(symbol));
            return new RelationalExpressionNode(ctx.Start.Line, left, op, right);
        }

        public override ASTNode VisitRelationalExpression([NotNull] RelationalExpressionContext ctx)
        {
            if (ctx.relationalExpression() is null)
                return this.Visit(ctx.shiftExpression());

            ExpressionNode left = this.Visit(ctx.relationalExpression()).As<ExpressionNode>();
            ExpressionNode right = this.Visit(ctx.shiftExpression()).As<ExpressionNode>();
            string symbol = ctx.children[1].GetText();
            var op = new RelationalOperatorNode(ctx.Start.Line, symbol, BinaryOperations.RelationalFromSymbol(symbol));
            return new RelationalExpressionNode(ctx.Start.Line, left, op, right);
        }

        public override ASTNode VisitShiftExpression([NotNull] ShiftExpressionContext ctx)
        {
            if (ctx.ChildCount > 1) {
                ExpressionNode left = this.Visit(ctx.shiftExpression()).As<ExpressionNode>();
                ExpressionNode right = this.Visit(ctx.additiveExpression()).As<ExpressionNode>();
                string symbol = ctx.children[1].GetText();
                var op = new ArithmeticOperatorNode(ctx.Start.Line, symbol, BinaryOperations.ArithmeticFromSymbol(symbol));
                return new ArithmeticExpressionNode(ctx.Start.Line, left, op, right);
            } else {
                return this.Visit(ctx.additiveExpression());
            }
        }

        public override ASTNode VisitAdditiveExpression([NotNull] AdditiveExpressionContext ctx)
        {
            if (ctx.ChildCount > 1) {
                ExpressionNode left = this.Visit(ctx.additiveExpression()).As<ExpressionNode>();
                ExpressionNode right = this.Visit(ctx.multiplicativeExpression()).As<ExpressionNode>();
                string symbol = ctx.children[1].GetText();
                var op = new ArithmeticOperatorNode(ctx.Start.Line, symbol, BinaryOperations.ArithmeticFromSymbol(symbol));
                return new ArithmeticExpressionNode(ctx.Start.Line, left, op, right);
            } else {
                return this.Visit(ctx.multiplicativeExpression());
            }
        }

        public override ASTNode VisitMultiplicativeExpression([NotNull] MultiplicativeExpressionContext ctx)
        {
            if (ctx.ChildCount > 1) {
                ExpressionNode left = this.Visit(ctx.multiplicativeExpression()).As<ExpressionNode>();
                ExpressionNode right = this.Visit(ctx.castExpression()).As<ExpressionNode>();
                string symbol = ctx.children[1].GetText();
                var op = new ArithmeticOperatorNode(ctx.Start.Line, symbol, BinaryOperations.ArithmeticFromSymbol(symbol));
                return new ArithmeticExpressionNode(ctx.Start.Line, left, op, right);
            } else {
                return this.Visit(ctx.castExpression());
            }
        }

        public override ASTNode VisitCastExpression([NotNull] CastExpressionContext ctx)
        {
            if (ctx.unaryExpression() is null)
                throw new NotImplementedException("cast operator");
            return this.Visit(ctx.unaryExpression());
        }

        public override ASTNode VisitUnaryExpression([NotNull] UnaryExpressionContext ctx)
        {
            if (ctx.postfixExpression() is { })
                return this.Visit(ctx.postfixExpression());

            ExpressionNode expr;
            if (ctx.unaryExpression() is null) {
                if (ctx.castExpression() is null)
                    throw new NotImplementedException("extended unary expressions (sizeof, alignof, gcc extensions");
                expr = this.Visit(ctx.castExpression()).As<ExpressionNode>();
            } else {
                expr = this.Visit(ctx.unaryExpression()).As<ExpressionNode>();
            }
            string symbol = ctx.children[0].GetText();
            var op = new UnaryOperatorNode(ctx.Start.Line, symbol, UnaryOperations.UnaryFromSymbol(symbol));
            return new UnaryExpressionNode(ctx.Start.Line, op, expr);
        }

        public override ASTNode VisitPostfixExpression([NotNull] PostfixExpressionContext ctx)
        {
            if (ctx.primaryExpression() is { })
                return this.Visit(ctx.primaryExpression());

            if (ctx.typeName() is { } || ctx.initializerList() is { })
                throw new NotImplementedException("initializers");

            ExpressionNode expr = this.Visit(ctx.postfixExpression()).As<ExpressionNode>();
            switch (ctx.children[1].GetText()) {
                case "(":
                    if (expr is IdentifierNode fname) {
                        if (ctx.argumentExpressionList() is { }) {
                            ExpressionListNode? args = this.Visit(ctx.argumentExpressionList()).As<ExpressionListNode>();
                            return new FunctionCallExpressionNode(ctx.Start.Line, fname, args);
                        } else {
                            return new FunctionCallExpressionNode(ctx.Start.Line, fname);
                        }
                    } else {
                        throw new NotImplementedException("complex function calls");
                    }
                case "[":
                    ExpressionNode indexExpr = this.Visit(ctx.expression()).As<ExpressionNode>();
                    return new ArrayAccessExpressionNode(ctx.Start.Line, expr, indexExpr);
                case "++":
                    return new IncrementExpressionNode(ctx.Start.Line, expr);
                case "--":
                    return new DecrementExpressionNode(ctx.Start.Line, expr);
                case "->":
                    throw new NotImplementedException("->");
                case ".":
                    throw new NotImplementedException("struct field access");
                default:
                    throw new NotImplementedException("unknown postfix expression");
            }
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
                return new LiteralNode(ctx.Start.Line, string.Join("", ctx.StringLiteral().Select(t => t.GetText()[1..^1])));
            else // TODO
                return null;
        }

        public override ASTNode VisitArgumentExpressionList([NotNull] ArgumentExpressionListContext ctx)
        {
            ExpressionListNode args;
            ExpressionNode arg = this.Visit(ctx.assignmentExpression()).As<ExpressionNode>();

            if (ctx.argumentExpressionList() is null)
                return new ExpressionListNode(ctx.Start.Line, arg);

            args = this.Visit(ctx.argumentExpressionList()).As<ExpressionListNode>();
            arg.Parent = args;
            return new ExpressionListNode(ctx.Start.Line, args.Expressions.Concat(new[] { arg }));
        }
    }
}
