using System;
using System.IO;
using RICC.AST.Builders;
using RICC.AST.Builders.C;
using RICC.AST.Nodes;
using Serilog;

namespace RICC.AST
{
    public static class ASTFactory
    {
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
                _ => throw new ArgumentException("Unsupported file extension"),
            };
        }
    }
}
