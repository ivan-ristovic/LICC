using System;
using System.Linq;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using RICC.AST.Nodes;
using RICC.AST.Nodes.Common;
using RICC.Exceptions;
using static RICC.AST.Builders.C.CParser;

namespace RICC.AST.Builders.C
{
    public sealed partial class CASTBuilder : CBaseVisitor<ASTNode>, IASTBuilder<CParser>
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
                    ExpressionNode condition = this.Visit(ctx.expression()).As<ExpressionNode>();
                    StatementContext[] statements = ctx.statement();
                    StatementNode thenStatement = this.Visit(statements.First()).As<StatementNode>();
                    StatementNode? elseStatement = statements.Length > 1 ? this.Visit(statements.Last()).As<StatementNode>() : null;
                    if (elseStatement is null) 
                        return new IfStatementNode(ctx.Start.Line, condition, thenStatement);
                    else
                        return new IfStatementNode(ctx.Start.Line, condition, thenStatement, elseStatement);
                case "switch":
                    throw new NotImplementedException("switch");
                default:
                    throw new SyntaxException("Unknown construct", ctx.Start.Line, ctx.Start.Column);
            }
        }

        public override ASTNode VisitIterationStatement([NotNull] IterationStatementContext ctx)
        {
            IterationStatementNode it;
            StatementNode statement = this.Visit(ctx.statement()).As<StatementNode>();

            if (ctx.For() is { }) {
                ForConditionContext forCondition = ctx.forCondition();
                if (forCondition.forDeclaration() is { }) {
                    DeclarationNode decl = this.Visit(forCondition.forDeclaration()).As<DeclarationNode>();
                    GetForExpressions(forCondition, out ExpressionNode? cond, out ExpressionNode? inc);
                    return new ForStatementNode(ctx.Start.Line, decl, cond, inc, statement);
                } else {
                    ExpressionNode? initExpr = forCondition.expression() is { } ? this.Visit(forCondition.expression()).As<ExpressionNode>() : null;
                    GetForExpressions(forCondition, out ExpressionNode? cond, out ExpressionNode? inc);
                    return new ForStatementNode(ctx.Start.Line, initExpr, cond, inc, statement);
                }
            }

            ExpressionNode condition = this.Visit(ctx.expression()).As<ExpressionNode>();
            it = new WhileStatementNode(ctx.Start.Line, condition, statement);

            if (ctx.Do() is { })
                throw new NotImplementedException("do-while");

            return it;


            void GetForExpressions(ForConditionContext fctx, out ExpressionNode? cond, out ExpressionNode? inc)
            {
                cond = null;
                inc = null;
                
                int colonCount = 0;
                foreach (IParseTree child in fctx.children) {
                    if (child is ITerminalNode)
                        colonCount++;
                    else if (colonCount > 1)
                        inc = this.Visit(child).As<ExpressionNode>();
                    else if (colonCount > 0)
                        cond = this.Visit(child).As<ExpressionNode>();
                }
            }
        }

        public override ASTNode VisitForExpression([NotNull] ForExpressionContext ctx)
        {
            ExpressionListNode exprs;
            ExpressionNode expr = this.Visit(ctx.assignmentExpression()).As<ExpressionNode>();

            if (ctx.forExpression() is null)
                return new ExpressionListNode(ctx.Start.Line, expr);

            exprs = this.Visit(ctx.forExpression()).As<ExpressionListNode>();
            expr.Parent = exprs;
            return new ExpressionListNode(ctx.Start.Line, exprs.Expressions.Concat(new[] { expr }));
        }

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
