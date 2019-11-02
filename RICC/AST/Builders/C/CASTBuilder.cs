using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using RICC.AST.Nodes;
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
            parser.BuildParseTree = true;
            return this.VisitTranslationUnit(parser.translationUnit()); 
        }


        public override ASTNode VisitTranslationUnit([NotNull] CParser.TranslationUnitContext context)
        {
            Log.Debug("Visiting translation unit: {Context}", context);
            return this.VisitChildren(context);
        }

        // TODO
    }
}
