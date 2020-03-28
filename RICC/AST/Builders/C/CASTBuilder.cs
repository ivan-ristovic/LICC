using System;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using RICC.AST.Nodes;
using RICC.Exceptions;
using RICC.Extensions;
using static RICC.AST.Builders.C.CParser;

namespace RICC.AST.Builders.C
{
    public sealed partial class CASTBuilder : CBaseVisitor<ASTNode>, IASTBuilder<CParser>
    {
        public CParser CreateParser(string code)
        {
            ICharStream stream = CharStreams.fromstring(code);
            var lexer = new CLexer(stream);
            lexer.AddErrorListener(new ThrowExceptionErrorListener());
            ITokenStream tokens = new CommonTokenStream(lexer);
            var parser = new CParser(tokens);
            parser.BuildParseTree = true;
            parser.RemoveErrorListeners();
            parser.AddErrorListener(new ThrowExceptionErrorListener());
            return parser;
        }

        public ASTNode BuildFromSource(string code)
            => this.Visit(this.CreateParser(code).compilationUnit());

        public ASTNode BuildFromSource(string code, Func<CParser, ParserRuleContext> entryProvider)
            => this.Visit(entryProvider(this.CreateParser(code)));


        public override ASTNode Visit(IParseTree tree)
        {
            LogObj.Visit(tree as ParserRuleContext);
            try {
                return base.Visit(tree);
            } catch (NullReferenceException e) {
                throw new SyntaxException("Source file contained unexpected content", e);
            }
        }

        public override ASTNode VisitCompilationUnit([NotNull] CompilationUnitContext ctx)
            => ctx.translationUnit() is null ? new SourceComponentNode(Enumerable.Empty<ASTNode>()) : this.Visit(ctx.translationUnit());

        public override ASTNode VisitTranslationUnit([NotNull] TranslationUnitContext ctx)
        {
            ASTNode decl = this.Visit(ctx.externalDeclaration());

            if (ctx.translationUnit() is null)
                return new SourceComponentNode(decl);

            SourceComponentNode tu = this.Visit(ctx.translationUnit()).As<SourceComponentNode>();
            decl.Parent = tu;
            return new SourceComponentNode(tu.Children.Concat(new[] { decl }));
        }
    }
}
