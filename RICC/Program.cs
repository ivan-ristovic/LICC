using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Serilog;

namespace RICC
{
    internal static class Program
    {
        public static void Main(string[] _)
        {
            SetupLogger();

            // TODO parse args

            string sampleCode = @"
                #include <stdio.h>

                int main() 
                {
                    int x = 1;
                    printf(""Hello world! %d\n"", x);
                    return 0;
                }
            ";


            var parser = new CParser(new CommonTokenStream(new CLexer(CharStreams.fromstring(sampleCode))));
            parser.BuildParseTree = true;
            ParseTreeWalker.Default.Walk(new Listener(), parser.translationUnit());

            Log.Information("Done! Press any key to exit...");
            Console.ReadKey();
        }


        private static void SetupLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .CreateLogger()
                ;
        }


        private class Listener : CBaseListener
        {
            public override void EnterPrimaryExpression([NotNull] CParser.PrimaryExpressionContext ctx)
            {
                Log.Debug("Entered primary expression: {Context}", ctx);
            }

            public override void EnterTranslationUnit([NotNull] CParser.TranslationUnitContext ctx)
            {
                Log.Debug("Entered translation unit: {Context}", ctx);
            }
        }
    }
}
