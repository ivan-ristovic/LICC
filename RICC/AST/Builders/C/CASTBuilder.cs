using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using RICC.AST.Nodes;
using RICC.Extensions;
using Serilog;
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

            IParseTree translationUnit = ctx.translationUnit();
            if (translationUnit is null)
                return new TranslationUnitNode(Enumerable.Empty<ASTNode>());

            ASTNode child = this.Visit(translationUnit);
            var compilationUnit = new TranslationUnitNode(new[] { child });
            child.Parent = compilationUnit;
            return compilationUnit;
        }
    }
}
