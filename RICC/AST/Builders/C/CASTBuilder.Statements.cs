using System.Linq;
using Antlr4.Runtime.Misc;
using RICC.AST.Nodes;
using RICC.Extensions;
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
                block = new BlockStatementNode(ctx.Start.Line, item);
                item.Parent = block;
                return block;
            }

            block = (BlockStatementNode)this.Visit(ctx.blockItemList());
            item.Parent = block;
            return new BlockStatementNode(ctx.Start.Line, block.Children.Concat(new[] { item }));
        }

        public override ASTNode VisitBlockItem([NotNull] BlockItemContext ctx)
            => this.Visit(ctx.children.First());

        public override ASTNode VisitStatement([NotNull] StatementContext ctx)
        {
            return new StatementNode(ctx.Start.Line, Enumerable.Empty<ASTNode>());
        }

        public override ASTNode VisitDeclaration([NotNull] DeclarationContext ctx)
        {
            return new DeclarationNode(ctx.Start.Line, Enumerable.Empty<ASTNode>());
        }
    }
}
