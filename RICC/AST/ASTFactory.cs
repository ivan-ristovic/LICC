using System;
using System.IO;
using RICC.AST.Builders;
using RICC.AST.Builders.C;
using RICC.AST.Nodes;
using RICC.Exceptions;
using Serilog;

namespace RICC.AST
{
    public static class ASTFactory
    {
        public static bool TryBuildFromFile(string path, out ASTNode? ast)
        {
            ast = null;
            try {
                ast = BuildFromFile(path);
                return true;
            } catch (NotImplementedException e) {
                Log.Fatal(e, "{Path} contains syntax rules which aren't implemented yet: {Details}", path, e.Message ?? "unknown");
                return false;
            } catch (UnsupportedExtensionException e) {
                Log.Fatal(e, "{Path}", path);
                return false;
            } catch (Exception e) {
                Log.Fatal(e, "Exception occured while parsing file {Path}", path);
                return false;
            }
        }

        public static ASTNode BuildFromFile(string path)
        {
            Log.Information("Creating AST for file: {Path}", path);
            IASTBuilder builder = DeduceBuilderTypeForFile(path);
            string code = File.ReadAllText(path);
            return builder.BuildFromSource(code);
        }


        private static IASTBuilder DeduceBuilderTypeForFile(string path)
        {
            var fi = new FileInfo(path);
            return fi.Extension switch
            {
                ".c" => new CASTBuilder(),
                _ => throw new UnsupportedExtensionException(),
            };
        }
    }
}
