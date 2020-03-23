using System;
using System.IO;
using CommandLine;
using RICC.AST;
using RICC.AST.Nodes;
using RICC.Core;
using RICC.Extensions;
using Serilog;

namespace RICC
{
    internal static class Program
    {
        internal static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<CompareOptions, ASTOptions>(args)
                .MapResult(
                    (CompareOptions o) => CompareSources(o),
                    (ASTOptions o) => GenerateAST(o),
                    errs => 1
                );
        }


        private static int GenerateAST(ASTOptions o)
        {
            SetupLogger(o.Verbose);

            if (string.IsNullOrWhiteSpace(o.Source)) {
                Log.Fatal("Missing source path");
                return 1;
            }

            if (!ASTFactory.TryBuildFromFile(o.Source, out ASTNode? ast))
                return 1;

            Log.Debug("AST created");
            Log.Debug("Generating JSON...");
            string? json = ast?.ToJson(o.Compact);
            if (json is null)
                return 1;

            if (string.IsNullOrWhiteSpace(o.OutputPath)) {
                Log.Debug("Dumping AST JSON...");
                Console.WriteLine(json);
            } else {
                try {
                    File.WriteAllText(o.OutputPath, json);
                } catch (IOException e) {
                    Log.Fatal(e, "Failed to save JSON to file {Path}", o.OutputPath);
                    return 1;
                }
            }

            return 0;
        }

        private static int CompareSources(CompareOptions o)
        {
            SetupLogger(o.Verbose);

            if (string.IsNullOrWhiteSpace(o.Source) || string.IsNullOrWhiteSpace(o.Destination)) {
                Log.Fatal("Missing source/destination path");
                return 1;
            }

            if (!ASTFactory.TryBuildFromFile(o.Source, out ASTNode? src) || !ASTFactory.TryBuildFromFile(o.Destination, out ASTNode? dst))
                return 1;
            if (src is null || dst is null)
                return 1;

            var comparer = new ASTComparer(src, dst);
            comparer.AttemptMatch();

            return 0;
        }

        private static void SetupLogger(bool verbose)
        {
            LoggerConfiguration lcfg = new LoggerConfiguration()
                .WriteTo.Console(outputTemplate: "\r[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .Enrich.FromLogContext()
                ;

            if (verbose)
                lcfg.MinimumLevel.Verbose();
            else
                lcfg.MinimumLevel.Information();

            Log.Logger = lcfg.CreateLogger();
        }
    }
}
