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

            if (ctx.blockItemList() is null) {
                block = new BlockStatementNode(ctx.Start.Line, children: item);
                item.Parent = block;
                return block;
            }

            block = this.Visit(ctx.blockItemList()).As<BlockStatementNode>();
            item.Parent = block;
            return new BlockStatementNode(ctx.Start.Line, block.Children.Concat(new[] { item }));
        }

        public override ASTNode VisitBlockItem([NotNull] BlockItemContext ctx)
            => this.Visit(ctx.children.Single());

        public override ASTNode VisitStatement([NotNull] StatementContext ctx)
            => this.Visit(ctx.children.Single());

        public override ASTNode VisitExpressionStatement([NotNull] ExpressionStatementContext ctx)
            => ctx.expression() is null ? new EmptyStatementNode(ctx.Start.Line) : this.Visit(ctx.expression());

        public override ASTNode VisitSelectionStatement([NotNull] SelectionStatementContext ctx)
        {
            switch (ctx.children.First().GetText()) {
                case "if":
                    ExpressionNode expr = this.Visit(ctx.expression()).As<ExpressionNode>();
                    StatementContext[] statements = ctx.statement();
                    BlockStatementNode thenBlock = this.Visit(statements.First()).As<BlockStatementNode>();
                    BlockStatementNode? elseBlock = statements.Length > 1 ? this.Visit(statements.Last()).As<BlockStatementNode>() : null;

                    if (expr is LogicExpressionNode) {
                        LogicExpressionNode condition = expr.As<LogicExpressionNode>();
                        return new IfStatementNode(ctx.Start.Line, condition, thenBlock, elseBlock);
                    } else if (expr is RelationalExpressionNode) {
                        RelationalExpressionNode condition = expr.As<RelationalExpressionNode>();
                        return new IfStatementNode(ctx.Start.Line, condition, thenBlock, elseBlock);
                    } else {
                        var op = new RelationalOperatorNode(expr.Line, "!=", BinaryOperations.NotEqualsPrimitive);
                        var right = new LiteralNode(expr.Line, 0);
                        var condition = new RelationalExpressionNode(expr.Line, expr, op, right);
                        return new IfStatementNode(ctx.Start.Line, condition, thenBlock, elseBlock);
                    }
                case "switch":
                    throw new NotImplementedException();  // TODO 
                default:
                    throw new Exception("???");           // TODO
            }
        }

        public override ASTNode VisitIterationStatement([NotNull] IterationStatementContext ctx) => base.VisitIterationStatement(ctx);

        public override ASTNode VisitDeclaration([NotNull] DeclarationContext ctx)
        {
            // TODO static assert

            DeclarationSpecifiersNode declSpecs = this.Visit(ctx.declarationSpecifiers()).As<DeclarationSpecifiersNode>();

            // TODO if not null, also implement other decl types
            DeclarationListNode var = this.Visit(ctx.initDeclaratorList()).As<DeclarationListNode>();

            var decl = new DeclarationStatementNode(ctx.Start.Line, declSpecs, var);
            declSpecs.Parent = decl;
            var.Parent = decl;
            return decl;
        }

        public override ASTNode VisitJumpStatement([NotNull] JumpStatementContext ctx)
        {
            JumpStatementType type = JumpStatementTypeConverter.FromString(ctx.children.First().GetText());
            ExpressionNode? expr = null;
            IdentifierNode? label = null;
            if (type == JumpStatementType.Return)
                expr = ctx.expression() is { } ? this.Visit(ctx.expression()).As<ExpressionNode>() : null;
            if (type == JumpStatementType.Goto)
                label = new IdentifierNode(ctx.Start.Line, ctx.Identifier().GetText());

            var js = new JumpStatementNode(ctx.Start.Line, type, expr, label);
            if (expr is { })
                expr.Parent = js;
            if (label is { })
                label.Parent = js;
            return js;
        }

        public override ASTNode VisitInitDeclaratorList([NotNull] InitDeclaratorListContext ctx)
        {
            DeclarationListNode list;
            DeclarationNode decl = this.Visit(ctx.initDeclarator()).As<DeclarationNode>();

            if (ctx.initDeclaratorList() is null) {
                list = new DeclarationListNode(ctx.Start.Line, decl );
                decl.Parent = list;
                return list;
            }

            list = this.Visit(ctx.initDeclaratorList()).As<DeclarationListNode>();
            decl.Parent = list;
            return new DeclarationListNode(ctx.Start.Line, list.Declarations.Concat(new[] { decl }));
        }

        public override ASTNode VisitInitDeclarator([NotNull] InitDeclaratorContext ctx)
        {
            IdentifierNode identifier = this.Visit(ctx.declarator()).As<IdentifierNode>();
            ExpressionNode? init = null;
            if (ctx.initializer() is { }) 
                init = this.Visit(ctx.initializer()).As<ExpressionNode>();

            var decl = new VariableDeclarationNode(ctx.Start.Line, identifier, init);
            if (init is { })
                init.Parent = decl;
            return decl;
        }

        public override ASTNode VisitInitializer([NotNull] InitializerContext ctx) => this.Visit(ctx.assignmentExpression()); // TODO
    }
}
