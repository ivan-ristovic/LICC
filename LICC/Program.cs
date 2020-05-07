using System;
using System.IO;
using CommandLine;
using LICC.AST;
using LICC.AST.Exceptions;
using LICC.AST.Extensions;
using LICC.AST.Nodes;
using LICC.Core;
using Serilog;

namespace LICC
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

            if (!TryBuildFromFile(o.Source, out ASTNode? ast))
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

            if (!TryBuildFromFile(o.Source, out ASTNode? src) || !TryBuildFromFile(o.Destination, out ASTNode? dst))
                return 1;
            if (src is null || dst is null)
                return 1;

            if (!new ASTStructureMatcher(src, dst).AttemptMatch()) {
                Log.Fatal("Structue match failed.");
                return 1;
            }

            var comparer = new ASTNodeComparer(src, dst);
            comparer.AttemptMatch();

            return 0;
        }

        public static bool TryBuildFromFile(string path, out ASTNode? ast)
        {
            Log.Information("Creating AST for file: {Path}", path);

            ast = null;
            try {
                ast = ASTFactory.BuildFromFile(path);
                return true;
            } catch (SyntaxErrorException e) {
                Log.Fatal(e, "[{Path}] Syntax error: {Details}", path, e.Message ?? "unknown");
            } catch (NotImplementedException e) {
                Log.Fatal(e, "[{Path}] Not supported: {Details}", path, e.Message ?? "unknown");
            } catch (UnsupportedLanguageException e) {
                Log.Fatal(e, "[{Path}] Not supported language", path);
            } catch (Exception e) {
                Log.Fatal(e, "[{Path}] Exception occured", path);
            }

            return false;
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
