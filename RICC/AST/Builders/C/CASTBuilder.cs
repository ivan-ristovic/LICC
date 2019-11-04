using System;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using RICC.AST.Nodes;
using RICC.Extensions;
using Serilog;

namespace RICC.AST.Builders.C
{
    public sealed class CASTBuilder : CBaseVisitor<ASTNode>, IASTBuilder
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


        public override ASTNode VisitCompilationUnit([NotNull] CParser.CompilationUnitContext ctx)
        {
            LogObj.Context(ctx);

            IParseTree translationUnit = ctx.children.First();
            if (translationUnit is null)
                return new TranslationUnitNode(Enumerable.Empty<ASTNode>());

            ASTNode child = this.Visit(ctx.children.First());
            return new TranslationUnitNode(new[] { child });
        }

        public override ASTNode VisitTranslationUnit([NotNull] CParser.TranslationUnitContext ctx)
        {
            return new TranslationUnitNode(ctx.children.Select(c => this.Visit(c)));
        }

        public override ASTNode VisitFunctionDefinition([NotNull] CParser.FunctionDefinitionContext ctx)
        {
            LogObj.Context(ctx);

            // visit children and get info
            string type = ctx.GetChild(1).GetText();
            var body = new BlockStatementNode(ctx.Start.Line, Enumerable.Empty<ASTNode>());

            return new FunctionDefinitionNode(ctx.Start.Line, "f", Enumerable.Empty<(string, Type)>(), null, body);
        }

        // TODO
    }
}
