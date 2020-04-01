using System;
using System.Linq;
using Antlr4.Runtime.Misc;
using LICC.AST.Nodes;
using Serilog;
using static LICC.AST.Builders.C.CParser;

namespace LICC.AST.Builders.C
{
    public sealed partial class CASTBuilder : CBaseVisitor<ASTNode>, IASTBuilder<CParser>
    {
        public override ASTNode VisitDeclaration([NotNull] DeclarationContext ctx)
        {
            if (ctx.staticAssertDeclaration() is { } || ctx.initDeclaratorList() is null)
                throw new NotImplementedException();

            DeclarationSpecifiersNode declSpecs = this.Visit(ctx.declarationSpecifiers()).As<DeclarationSpecifiersNode>();
            DeclaratorListNode var = this.Visit(ctx.initDeclaratorList()).As<DeclaratorListNode>();
            return new DeclarationStatementNode(ctx.Start.Line, declSpecs, var);
        }

        public override ASTNode VisitDeclarator([NotNull] DeclaratorContext ctx)
            => this.Visit(ctx.directDeclarator());

        public override ASTNode VisitDirectDeclarator([NotNull] DirectDeclaratorContext ctx)
        {
            if (ctx.declarator() is { })
                return this.Visit(ctx.declarator());

            if (ctx.Identifier() is { }) {
                if (ctx.ChildCount == 1)
                    return new VariableDeclaratorNode(ctx.Start.Line, new IdentifierNode(ctx.Start.Line, ctx.Identifier().ToString() ?? "<unknown_name>"));
                else
                    throw new NotImplementedException("bit field");
            }

            if (ctx.typeQualifierList() is { } || ctx.typeSpecifier() is { } || ctx.pointer() is { })
                throw new NotImplementedException("qualified arrays and function pointers");

            DeclaratorNode decl = this.Visit(ctx.directDeclarator()).As<DeclaratorNode>();
            if (decl is VariableDeclaratorNode var) {
                if (AreBracketsTokensPresent(ctx)) {
                    if (ctx.assignmentExpression() is { }) {
                        ExpressionNode sizeExpr = this.Visit(ctx.assignmentExpression()).As<ExpressionNode>();
                        return new ArrayDeclaratorNode(ctx.Start.Line, var.IdentifierNode, sizeExpr);
                    } else {
                        return new ArrayDeclaratorNode(ctx.Start.Line, var.IdentifierNode);
                    }
                } else if (AreParenTokensPresent(ctx)) {
                    if (ctx.parameterTypeList() is { }) {
                        FunctionParametersNode @params = this.Visit(ctx.parameterTypeList()).As<FunctionParametersNode>();
                        return new FunctionDeclaratorNode(ctx.Start.Line, var.IdentifierNode, @params);
                    } else {
                        return new FunctionDeclaratorNode(ctx.Start.Line, var.IdentifierNode);
                    }
                } else {
                    return var;
                }
            } else if (decl is ArrayDeclaratorNode arr) {
                if (AreBracketsTokensPresent(ctx)) {
                    throw new NotImplementedException("multidimensional arrays");
                } else if (AreParenTokensPresent(ctx)) {
                    Log.Warning("Potential syntax error in line: {Line}. Parsing will continue but results are not guaranteed...", ctx.Start.Line);
                } else {
                    return arr;
                }
            } else {
                Log.Warning("Potential syntax error in line: {Line}. Parsing will continue but results are not guaranteed...", ctx.Start.Line);
            }

            return decl;


            static bool AreParenTokensPresent(DirectDeclaratorContext ctx)
                => ctx.GetToken(LeftParen, 0) is { } && ctx.GetToken(RightParen, 0) is { };

            static bool AreBracketsTokensPresent(DirectDeclaratorContext ctx)
                => ctx.GetToken(LeftBracket, 0) is { } && ctx.GetToken(RightBracket, 0) is { };
        }

        public override ASTNode VisitDeclarationSpecifiers([NotNull] DeclarationSpecifiersContext ctx)
        {
            string[] specs = ctx.children.Select(c => c.GetText()).ToArray();
            int unsignedIndex = Array.IndexOf(specs, "unsigned");
            string type = unsignedIndex != -1 ? string.Join(' ', specs[unsignedIndex..]) : specs.Last();
            return new DeclarationSpecifiersNode(ctx.Start.Line, string.Join(' ', specs), type);
        }

        public override ASTNode VisitInitDeclaratorList([NotNull] InitDeclaratorListContext ctx)
        {
            DeclaratorNode decl = this.Visit(ctx.initDeclarator()).As<DeclaratorNode>();

            if (ctx.initDeclaratorList() is null)
                return new DeclaratorListNode(ctx.Start.Line, decl);

            DeclaratorListNode list = this.Visit(ctx.initDeclaratorList()).As<DeclaratorListNode>();
            return new DeclaratorListNode(ctx.Start.Line, list.Declarations.Concat(new[] { decl }));
        }

        public override ASTNode VisitInitDeclarator([NotNull] InitDeclaratorContext ctx)
        {
            DeclaratorNode declarator = this.Visit(ctx.declarator()).As<DeclaratorNode>();
            ASTNode? init = null;
            if (ctx.initializer() is { })
                init = this.Visit(ctx.initializer());

            if (declarator is VariableDeclaratorNode var)
                return init is null ? var : new VariableDeclaratorNode(ctx.Start.Line, var.IdentifierNode, init.As<ExpressionNode>());

            if (declarator is ArrayDeclaratorNode arr) {
                if (arr.SizeExpression is null) {
                    if (init is null)
                        return new ArrayDeclaratorNode(ctx.Start.Line, arr.IdentifierNode);
                    else
                        return new ArrayDeclaratorNode(ctx.Start.Line, arr.IdentifierNode, init.As<ArrayInitializerListNode>());
                } else {
                    if (init is null)
                        return new ArrayDeclaratorNode(ctx.Start.Line, arr.IdentifierNode, arr.SizeExpression);
                    else
                        return new ArrayDeclaratorNode(ctx.Start.Line, arr.IdentifierNode, arr.SizeExpression, init.As<ArrayInitializerListNode>());
                }
            }

            return declarator;
        }

        public override ASTNode VisitInitializerList([NotNull] InitializerListContext ctx)
        {
            if (ctx.designation() is { })
                throw new NotImplementedException();

            ExpressionNode init = this.Visit(ctx.initializer()).As<ExpressionNode>();

            if (ctx.initializerList() is null)
                return new ArrayInitializerListNode(ctx.Start.Line, init);

            ArrayInitializerListNode list = this.Visit(ctx.initializerList()).As<ArrayInitializerListNode>();
            return new ArrayInitializerListNode(ctx.Start.Line, list.Initializers.Concat(new[] { init }));
        }

        public override ASTNode VisitInitializer([NotNull] InitializerContext ctx)
        {
            if (ctx.assignmentExpression() is { })
                return this.Visit(ctx.assignmentExpression());
            return this.Visit(ctx.initializerList());
        }
    }
}
