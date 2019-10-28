using System;
using Antlr4.Runtime;
using AST.AST;
using AST.Common;
using Parsers.CPP14;

namespace AST
{
    internal static class Program
    {
        public static void Main(string[] _)
        {
            string code = @"
                #include <iostream.h>

                main()
                {
                    cout << ""Hello World!"";
                    return 0;
                }
            ";

            ICharStream stream = CharStreams.fromstring(code);
            ITokenSource lexer = new CPP14Lexer(stream);
            ITokenStream tokens = new CommonTokenStream(lexer);
            var parser = new CPP14Parser(tokens);
            parser.BuildParseTree = true;

            ASTNode ast = new ASTBuilderVisitor().VisitTranslationunit(parser.translationunit());
            new TestVisitorImpl().Visit(ast);


            Logger.Log("Done.");
            Console.ReadKey();
        }

        class TestVisitorImpl : ASTVisitor
        {
            public override void Visit(PrimaryExpressionNode node)
            {
                Logger.Log("AST Visitor", "Visiting PrimaryExpressionNode");
            }

            public override void Visit(Literal node)
            {
                Logger.Log("AST Visitor", "Visiting Literal");
            }

            public override void Visit(DeclarationNode node)
            {
                Logger.Log("AST Visitor", "Visiting DeclarationNode");
            }

            public override void Visit(FunctionDefinitionNode node)
            {
                Logger.Log("AST Visitor", "Visiting FunctionDefinitionNode");
            }
        }
    }
}
