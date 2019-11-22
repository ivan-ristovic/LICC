using System.Linq;
using Antlr4.Runtime.Misc;
using RICC.AST.Nodes;
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
            StatementNode item = this.Visit(ctx.blockItem()).As<StatementNode>();

            if (ctx.blockItemList() is null) {
                block = new BlockStatementNode(ctx.Start.Line, item);
                item.Parent = block;
                return block;
            }

            block = this.Visit(ctx.blockItemList()).As<BlockStatementNode>();
            item.Parent = block;
            return new BlockStatementNode(ctx.Start.Line, block.Statements.Concat(new[] { item }));
        }

        public override ASTNode VisitBlockItem([NotNull] BlockItemContext ctx)
            => this.Visit(ctx.children.First());

        public override ASTNode VisitStatement([NotNull] StatementContext ctx)
            => this.Visit(ctx.children.First());

        public override ASTNode VisitExpressionStatement([NotNull] ExpressionStatementContext ctx)
            => ctx.expression() is null ? new EmptyStatementNode(ctx.Start.Line) : this.Visit(ctx.expression());

        public override ASTNode VisitSelectionStatement([NotNull] SelectionStatementContext ctx)
        {
            // if

            // switch

            return new IfStatementNode(0, null);
        }

        public override ASTNode VisitIterationStatement([NotNull] IterationStatementContext ctx) => base.VisitIterationStatement(ctx);

        public override ASTNode VisitDeclaration([NotNull] DeclarationContext ctx)
        {
            return new DeclarationStatementNode(ctx.Start.Line, Enumerable.Empty<ASTNode>());
        }

        public override ASTNode VisitJumpStatement([NotNull] JumpStatementContext context)
        {
            // return
            return new EmptyStatementNode(0);
        }

    }
}
