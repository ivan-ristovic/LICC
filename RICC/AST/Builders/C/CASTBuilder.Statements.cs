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
        public override ASTNode VisitCompoundStatement([NotNull] CompoundStatementContext ctx)
        {
            return ctx.blockItemList() is null
                ? new BlockStatementNode(ctx.Start.Line)
                : this.Visit(ctx.blockItemList());
        }

        public override ASTNode VisitBlockItemList([NotNull] BlockItemListContext ctx)
        {
            BlockStatementNode block;
            ASTNode item = this.Visit(ctx.blockItem());

            if (ctx.blockItemList() is null)
                return new BlockStatementNode(ctx.Start.Line, item);

            block = this.Visit(ctx.blockItemList()).As<BlockStatementNode>();
            return new BlockStatementNode(ctx.Start.Line, block.Children.Concat(new[] { item }));
        }

        public override ASTNode VisitBlockItem([NotNull] BlockItemContext ctx)
            => this.Visit(ctx.children.Single());

        public override ASTNode VisitStatement([NotNull] StatementContext ctx)
            => this.Visit(ctx.children.Single());

        public override ASTNode VisitLabeledStatement([NotNull] LabeledStatementContext ctx)
        {
            if (ctx.Identifier() is null)
                throw new NotImplementedException();

            string label = ctx.Identifier().GetText();
            StatementNode statement = this.Visit(ctx.statement()).As<StatementNode>();
            return new LabeledStatementNode(ctx.Start.Line, label, statement);
        }

        public override ASTNode VisitExpressionStatement([NotNull] ExpressionStatementContext ctx)
        {
            if (ctx.expression() is null)
                return new EmptyStatementNode(ctx.Start.Line);

            ExpressionNode expr = this.Visit(ctx.expression()).As<ExpressionNode>();
            return new ExpressionStatementNode(ctx.Start.Line, expr);
        }

        public override ASTNode VisitSelectionStatement([NotNull] SelectionStatementContext ctx)
        {
            switch (ctx.children.First().GetText()) {
                case "if":
                    ExpressionNode expr = this.Visit(ctx.expression()).As<ExpressionNode>();
                    StatementContext[] statements = ctx.statement();
                    StatementNode thenStatement = this.Visit(statements.First()).As<StatementNode>();
                    StatementNode? elseStatement = statements.Length > 1 ? this.Visit(statements.Last()).As<StatementNode>() : null;

                    if (expr is LogicExpressionNode) {
                        LogicExpressionNode condition = expr.As<LogicExpressionNode>();
                        return new IfStatementNode(ctx.Start.Line, condition, thenStatement, elseStatement);
                    } else if (expr is RelationalExpressionNode) {
                        RelationalExpressionNode condition = expr.As<RelationalExpressionNode>();
                        return new IfStatementNode(ctx.Start.Line, condition, thenStatement, elseStatement);
                    } else {
                        var op = new RelationalOperatorNode(expr.Line, "!=", BinaryOperations.NotEqualsPrimitive);
                        var right = new LiteralNode(expr.Line, 0);
                        var condition = new RelationalExpressionNode(expr.Line, expr, op, right);
                        return new IfStatementNode(ctx.Start.Line, condition, thenStatement, elseStatement);
                    }
                case "switch":
                    throw new NotImplementedException();  // TODO 
                default:
                    throw new Exception("???");           // TODO
            }
        }

        public override ASTNode VisitIterationStatement([NotNull] IterationStatementContext ctx) => base.VisitIterationStatement(ctx);

        public override ASTNode VisitJumpStatement([NotNull] JumpStatementContext ctx)
        {
            JumpStatementType type = JumpStatementTypeConverter.FromString(ctx.children.First().GetText());
            switch (type) {
                case JumpStatementType.Return:
                    ExpressionNode? expr = ctx.expression() is { } ? this.Visit(ctx.expression()).As<ExpressionNode>() : null;
                    return new JumpStatementNode(ctx.Start.Line, expr);
                case JumpStatementType.Goto:
                    var label = new IdentifierNode(ctx.Start.Line, ctx.Identifier().GetText());
                    return new JumpStatementNode(ctx.Start.Line, label);
                default:
                    return new JumpStatementNode(ctx.Start.Line, type);
            }
        }
    }
}
