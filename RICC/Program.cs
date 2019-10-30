using System;
using System.IO;
using Antlr4.Runtime;
using RICC.Adapters;
using RICC.Adapters.C;
using RICC.Context;
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
            var comparer = new ComparerAlgorithm("Tests/test.c", "Tests/test.c");
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
