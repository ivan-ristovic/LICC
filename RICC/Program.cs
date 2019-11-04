using System;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using RICC.AST;
using RICC.AST.Nodes;
using RICC.Core;
using Serilog;

namespace RICC
{
    internal static class Program
    {
        public static void Main(string[] _)
        {
            SetupLogger();

            // TODO parse args

            // begin test
            ASTNode srcTree = ASTFactory.BuildFromSource("Tests/block.c");
            ASTNode dstTree = ASTFactory.BuildFromSource("Tests/test.c");
            var comparer = new ComparerAlgorithm(srcTree, dstTree);
            comparer.Execute();
            // end test

            Log.Information("Done! Press any key to exit...");
            Console.ReadKey();
        }

        private static void SetupLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .CreateLogger()
                ;
        }

    }
}
