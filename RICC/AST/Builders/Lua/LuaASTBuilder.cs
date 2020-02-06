using System;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using RICC.AST.Nodes;
using RICC.Exceptions;
using RICC.Extensions;
using static RICC.AST.Builders.Lua.LuaParser;

namespace RICC.AST.Builders.Lua
{
    public sealed partial class LuaASTBuilder : LuaBaseVisitor<ASTNode>, IASTBuilder
    {
        public ASTNode BuildFromSource(string code)
        {
            ICharStream stream = CharStreams.fromstring(code);
            var lexer = new LuaLexer(stream);
            lexer.AddErrorListener(new ThrowExceptionErrorListener());
            ITokenStream tokens = new CommonTokenStream(lexer);
            var parser = new LuaParser(tokens);
            parser.BuildParseTree = true;
            parser.RemoveErrorListeners();
            parser.AddErrorListener(new ThrowExceptionErrorListener());
            return this.Visit(parser.chunk());
        }


        public override ASTNode Visit(IParseTree tree)
        {
            LogObj.Visit(tree as ParserRuleContext);
            try {
                return base.Visit(tree);
            } catch (NullReferenceException e) {
                throw new SyntaxException("Source file contained unexpected content", e);
            }
        }

        public override ASTNode VisitChunk([NotNull] ChunkContext ctx)
        {
            // TODO
            return new TranslationUnitNode(Enumerable.Empty<ASTNode>());
        }
    }
}
