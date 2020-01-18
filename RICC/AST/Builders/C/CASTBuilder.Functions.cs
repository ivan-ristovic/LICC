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
            ASTNode decl = this.Visit(ctx.declarator());
            if (decl is IdentifierNode fname)
                decl = new FunctionDeclaratorNode(fname.Line, fname);
            FunctionDeclaratorNode fdecl = decl.As<FunctionDeclaratorNode>();
            BlockStatementNode body = this.Visit(ctx.compoundStatement()).As<BlockStatementNode>();

            return new FunctionDefinitionNode(ctx.Start.Line, declSpecs, fdecl, body);
        }

        public override ASTNode VisitParameterTypeList([NotNull] ParameterTypeListContext ctx)
        {
            FunctionParametersNode @params = this.Visit(ctx.parameterList()).As<FunctionParametersNode>();
            if (ctx.ChildCount > 1)
                @params.IsVariadic = true;
            return @params;
        }

        public override ASTNode VisitParameterList([NotNull] ParameterListContext ctx)
        {
            FunctionParametersNode @params;
            FunctionParameterNode param = this.Visit(ctx.parameterDeclaration()).As<FunctionParameterNode>();

            if (ctx.parameterList() is null)
                return new FunctionParametersNode(ctx.Start.Line, param);

            @params = this.Visit(ctx.parameterList()).As<FunctionParametersNode>();
            return new FunctionParametersNode(ctx.Start.Line, @params.Parameters.Concat(new[] { param }));
        }

        public override ASTNode VisitParameterDeclaration([NotNull] ParameterDeclarationContext ctx)
        {
            DeclarationSpecifiersNode declSpecs = this.Visit(ctx.declarationSpecifiers()).As<DeclarationSpecifiersNode>();
            DeclaratorNode decl = this.Visit(ctx.declarator()).As<DeclaratorNode>();
            return new FunctionParameterNode(ctx.Start.Line, declSpecs, decl);
        }
    }
}
