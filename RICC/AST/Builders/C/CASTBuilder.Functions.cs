using System;
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

            DeclarationSpecifiersNode declSpecs = this.Visit(ctx.declarationSpecifiers()).As<DeclarationSpecifiersNode>();
            IdentifierNode identifier = this.Visit(ctx.declarator()).As<IdentifierNode>();
            ParameterTypeListContext pctx = ctx.declarator().directDeclarator().parameterTypeList();
            FunctionParametersNode? @params = pctx is null ? null : this.Visit(pctx).As<FunctionParametersNode>();
            BlockStatementNode body = this.Visit(ctx.compoundStatement()).As<BlockStatementNode>();

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
        {
            string[] specs = ctx.children.Select(c => c.GetText()).ToArray();
            int unsignedIndex = Array.IndexOf(specs, "unsigned");
            string type = unsignedIndex != -1 ? string.Join(" ", specs[unsignedIndex..]) : specs.Last();
            return new DeclarationSpecifiersNode(ctx.Start.Line, type, specs);
        }

        #region Parameter overrides
        public override ASTNode VisitParameterTypeList([NotNull] ParameterTypeListContext ctx)
            => this.Visit(ctx.parameterList());

        public override ASTNode VisitParameterList([NotNull] ParameterListContext ctx)
        {
            FunctionParametersNode @params;
            FunctionParameterNode param = this.Visit(ctx.parameterDeclaration()).As<FunctionParameterNode>();

            if (ctx.parameterList() is null) {
                @params = new FunctionParametersNode(ctx.Start.Line, param);
                param.Parent = @params;
                return @params;
            }

            @params = this.Visit(ctx.parameterList()).As<FunctionParametersNode>();
            param.Parent = @params;
            return new FunctionParametersNode(ctx.Start.Line, @params.Parameters.Concat(new[] { param }));
        }

        public override ASTNode VisitParameterDeclaration([NotNull] ParameterDeclarationContext ctx)
        {
            DeclarationSpecifiersNode declSpecs = this.Visit(ctx.declarationSpecifiers()).As<DeclarationSpecifiersNode>();
            IdentifierNode identifier = this.Visit(ctx.declarator()).As<IdentifierNode>();
            var param = new FunctionParameterNode(ctx.Start.Line, declSpecs, identifier);
            declSpecs.Parent = identifier.Parent = param;
            return param;
        }
        #endregion
    }
}
