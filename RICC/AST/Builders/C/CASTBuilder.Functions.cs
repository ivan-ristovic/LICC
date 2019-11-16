using System.Linq;
using Antlr4.Runtime.Misc;
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

            ASTNode declSpecs = this.Visit(ctx.declarationSpecifiers());
            ASTNode identifier = this.Visit(ctx.declarator());
            ParameterTypeListContext pctx = ctx.declarator().directDeclarator().parameterTypeList();
            ASTNode? @params = pctx is null ? null : this.Visit(pctx);
            BlockItemListContext defn = ctx.compoundStatement().blockItemList();
            var body = new BlockStatementNode(defn.Start.Line, defn.children.Select(c => this.Visit(c)));

            var fn = new FunctionDefinitionNode(ctx.Start.Line, declSpecs, identifier, @params, body);
            declSpecs.Parent = identifier.Parent = body.Parent = fn;
            if (@params is { })
                @params.Parent = fn;
            return fn;
        }

        public override ASTNode VisitDeclarator([NotNull] DeclaratorContext ctx)
            => this.Visit(ctx.directDeclarator());

        public override ASTNode VisitDirectDeclarator([NotNull] DirectDeclaratorContext ctx)
        {
            // TODO
            string identifier = ctx.Identifier()?.ToString() ?? string.Empty;
            return ctx.Identifier() is null ? this.Visit(ctx.directDeclarator()) : new IdentifierNode(ctx.Start.Line, identifier);
        }

        public override ASTNode VisitDeclarationSpecifiers([NotNull] DeclarationSpecifiersContext ctx)
            => new DeclarationSpecifiersNode(ctx.Start.Line, ctx.children.Select(c => c.GetText()));

        #region Parameter overrides
        public override ASTNode VisitParameterTypeList([NotNull] ParameterTypeListContext ctx)
            => this.Visit(ctx.parameterList());

        public override ASTNode VisitParameterList([NotNull] ParameterListContext ctx)
        {
            FunctionParametersNode @params;
            ASTNode param = this.Visit(ctx.parameterDeclaration());

            if (ctx.parameterList() is null) {
                @params = new FunctionParametersNode(ctx.Start.Line, new[] { param });
                param.Parent = @params;
                return @params;
            }

            @params = (FunctionParametersNode)this.Visit(ctx.parameterList());
            param.Parent = @params;
            return new FunctionParametersNode(ctx.Start.Line, @params.Children.Concat(new[] { param }));
        }

        public override ASTNode VisitParameterDeclaration([NotNull] ParameterDeclarationContext ctx)
        {
            ASTNode declSpecs = this.Visit(ctx.declarationSpecifiers());
            ASTNode identifier = this.Visit(ctx.declarator());
            var param = new FunctionParameterNode(ctx.Start.Line, declSpecs, identifier);
            declSpecs.Parent = identifier.Parent = param;
            return param;
        }
        #endregion
    }
}
