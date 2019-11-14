using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using RICC.AST.Nodes;
using RICC.Extensions;
using static RICC.AST.Builders.C.CParser;

namespace RICC.AST.Builders.C
{
    public sealed partial class CASTBuilder : CBaseVisitor<ASTNode>, IASTBuilder
    {
        public override ASTNode VisitFunctionDefinition([NotNull] FunctionDefinitionContext ctx)
        {
            LogObj.Context(ctx);

            ASTNode declSpecs = this.Visit(ctx.children.First());
            ASTNode identifier = this.Visit(ctx.GetChild<DeclaratorContext>(0));
            ParameterTypeListContext pctx = ctx.GetChild<DeclaratorContext>(0)
                                               .GetChild<DirectDeclaratorContext>(0)
                                               .GetChild<ParameterTypeListContext>(0);
            ASTNode? @params = pctx is null ? null : this.Visit(pctx);
            BlockItemListContext defn = ctx.GetChild<CompoundStatementContext>(0)
                                           .GetChild<BlockItemListContext>(0);
            var body = new BlockStatementNode(defn.Start.Line, defn.children.Select(c => this.Visit(c)));

            return new FunctionDefinitionNode(ctx.Start.Line, declSpecs, identifier, @params, body);
        }

        public override ASTNode VisitDeclarator([NotNull] DeclaratorContext ctx) 
            => this.Visit(ctx.GetChild<DirectDeclaratorContext>(0));

        public override ASTNode VisitDirectDeclarator([NotNull] DirectDeclaratorContext ctx)
        {
            string identifier = ctx.Identifier()?.ToString() ?? string.Empty;
            return ctx.Identifier() is null ? this.Visit(ctx.GetChild<DirectDeclaratorContext>(0)) : new IdentifierNode(ctx.Start.Line, identifier);
        }

        public override ASTNode VisitDeclarationSpecifiers([NotNull] DeclarationSpecifiersContext ctx) 
            => new DeclarationSpecifiersNode(ctx.Start.Line, ctx.children.Select(c => c.GetText()));

        #region Parameter overrides
        public override ASTNode VisitParameterTypeList([NotNull] ParameterTypeListContext ctx) 
            => this.Visit(ctx.children.First());

        public override ASTNode VisitParameterList([NotNull] ParameterListContext ctx)
        {
            // TODO
            return new FunctionParametersNode(ctx.Start.Line, ctx.children.Select(c => this.Visit(c)));
        }

        public override ASTNode VisitParameterDeclaration([NotNull] ParameterDeclarationContext ctx)
        {
            // TODO
            ASTNode declSpecs = this.Visit(ctx.children.First());
            return new FunctionParameterNode(ctx.Start.Line, declSpecs, this.Visit(ctx.GetChild<DeclaratorContext>(0)));
        }
        #endregion
    }
}
