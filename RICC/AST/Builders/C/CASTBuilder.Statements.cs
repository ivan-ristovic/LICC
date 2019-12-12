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
            => this.Visit(ctx.children.Single());

        public override ASTNode VisitStatement([NotNull] StatementContext ctx)
            => this.Visit(ctx.children.Single());

        public override ASTNode VisitExpressionStatement([NotNull] ExpressionStatementContext ctx)
            => ctx.expression() is null ? new EmptyStatementNode(ctx.Start.Line) : this.Visit(ctx.expression());

        public override ASTNode VisitSelectionStatement([NotNull] SelectionStatementContext ctx)
        {
            // if
            // switch
            // TODO
            return new IfStatementNode(0, null);
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
            // return in current example - this will be used for break, continue as well
            return new EmptyStatementNode(ctx.Start.Line);
        }

        public override ASTNode VisitInitDeclaratorList([NotNull] InitDeclaratorListContext ctx)
        {
            DeclarationListNode list;
            DeclarationNode decl = this.Visit(ctx.initDeclarator()).As<DeclarationNode>();

            if (ctx.initDeclaratorList() is null) {
                list = new DeclarationListNode(ctx.Start.Line, new[] { decl });
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
            if (ctx.initializer() is { }) {
                // TODO get value
                init = this.Visit(ctx.initializer()).As<ExpressionNode>();
                // TODO set parent
            }

            return new VariableDeclarationNode(ctx.Start.Line, identifier, init);
        }

        public override ASTNode VisitInitializer([NotNull] InitializerContext ctx) => this.Visit(ctx.assignmentExpression()); // TODO
    }
}
