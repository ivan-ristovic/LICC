﻿using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using RICC.AST.Nodes;
using RICC.Extensions;
using static RICC.AST.Builders.C.CParser;

namespace RICC.AST.Builders.C
{
    public sealed partial class CASTBuilder : CBaseVisitor<ASTNode>, IASTBuilder
    {
        public ASTNode BuildFromSource(string code)
        {
            ICharStream stream = CharStreams.fromstring(code);
            ITokenSource lexer = new CLexer(stream);
            ITokenStream tokens = new CommonTokenStream(lexer);
            var parser = new CParser(tokens);
            parser.AddErrorListener(new ThrowExceptionErrorListener());
            parser.BuildParseTree = true;
            return this.Visit(parser.compilationUnit()); 
        }


        public override ASTNode VisitCompilationUnit([NotNull] CompilationUnitContext ctx)
        {
            LogObj.Context(ctx);
            return ctx.translationUnit() is null ? new TranslationUnitNode(Enumerable.Empty<ASTNode>()) : this.Visit(ctx.translationUnit());
        }

        public override ASTNode VisitTranslationUnit([NotNull] TranslationUnitContext ctx)
        {
            ASTNode decl = this.Visit(ctx.externalDeclaration());

            if (ctx.translationUnit() is null)
                return new TranslationUnitNode(decl);

            TranslationUnitNode tu = this.Visit(ctx.translationUnit()).As<TranslationUnitNode>();
            decl.Parent = tu;
            return new TranslationUnitNode(tu.Children.Concat(new[] { decl }));
        }
    }
}
