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
                decl = new FunctionDeclaratorNode(fname.Line, fname, @params: null);
            FunctionDeclaratorNode fdecl = decl.As<FunctionDeclaratorNode>();
            BlockStatementNode body = this.Visit(ctx.compoundStatement()).As<BlockStatementNode>();

            return new FunctionDefinitionNode(ctx.Start.Line, declSpecs, fdecl, body);
        }

        public override ASTNode VisitDeclarator([NotNull] DeclaratorContext ctx)
            => this.Visit(ctx.directDeclarator());

        public override ASTNode VisitDirectDeclarator([NotNull] DirectDeclaratorContext ctx)
        {
            if (ctx.declarator() is { })
                return this.Visit(ctx.declarator());

            if (ctx.Identifier() is { } && ctx.ChildCount == 1)
                return new IdentifierNode(ctx.Start.Line, ctx.Identifier()?.ToString() ?? "<unknown_name>");

            if (ctx.parameterTypeList() is { } || (ctx.ChildCount >= 3 && AreParenTokensPresent(ctx))) {
                IdentifierNode fname = this.Visit(ctx.directDeclarator()).As<IdentifierNode>();
                if (ctx.parameterTypeList() is { }) {
                    FunctionParametersNode @params = this.Visit(ctx.parameterTypeList()).As<FunctionParametersNode>();
                    return new FunctionDeclaratorNode(ctx.Start.Line, fname, @params);
                } else {
                    return new FunctionDeclaratorNode(ctx.Start.Line, fname);
                }
            } else if (ctx.assignmentExpression() is { } || (ctx.ChildCount >= 3 && AreBracketsTokensPresent(ctx))) {
                IdentifierNode arrName = this.Visit(ctx.directDeclarator()).As<IdentifierNode>();
                if (ctx.assignmentExpression() is { }) {
                    ExpressionNode sizeExpr = this.Visit(ctx.assignmentExpression()).As<ExpressionNode>();
                    return new ArrayDeclaratorNode(ctx.Start.Line, arrName, sizeExpr);
                } else {
                    return new ArrayDeclaratorNode(ctx.Start.Line, arrName);
                }
            } else if (ctx.identifierList() is { }) {
                // TODO
            }
            // TODO array declaration

            return this.Visit(ctx.directDeclarator());


            static bool AreParenTokensPresent(DirectDeclaratorContext ctx)
                => ctx.GetToken(LeftParen, 0) is { } && ctx.GetToken(RightParen, 0) is { };

            static bool AreBracketsTokensPresent(DirectDeclaratorContext ctx)
                => ctx.GetToken(LeftBracket, 0) is { } && ctx.GetToken(RightBracket, 0) is { };
        }

        public override ASTNode VisitDeclarationSpecifiers([NotNull] DeclarationSpecifiersContext ctx)
        {
            string[] specs = ctx.children.Select(c => c.GetText()).ToArray();
            int unsignedIndex = Array.IndexOf(specs, "unsigned");
            string type = unsignedIndex != -1 ? string.Join(" ", specs[unsignedIndex..]) : specs.Last();
            return new DeclarationSpecifiersNode(ctx.Start.Line, string.Join(' ', specs), type);
        }

        #region Parameter overrides
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
            param.Parent = @params;
            return new FunctionParametersNode(ctx.Start.Line, @params.Parameters.Concat(new[] { param }));
        }

        public override ASTNode VisitParameterDeclaration([NotNull] ParameterDeclarationContext ctx)
        {
            DeclarationSpecifiersNode declSpecs = this.Visit(ctx.declarationSpecifiers()).As<DeclarationSpecifiersNode>();
            IdentifierNode identifier = this.Visit(ctx.declarator()).As<IdentifierNode>();
            return new FunctionParameterNode(ctx.Start.Line, declSpecs, identifier);
        }
        #endregion
    }
}
