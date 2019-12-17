using System;
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
            ASTNode srcTree = ASTFactory.BuildFromFile("Samples/func.c");
            ASTNode dstTree = ASTFactory.BuildFromFile("Samples/hello.c");
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
