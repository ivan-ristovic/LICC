using System;
using System.IO;
using RICC.AST.Builders;
using RICC.AST.Builders.C;
using RICC.AST.Builders.Lua;
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
            } catch (SyntaxException e) {
                Log.Fatal(e, "[{Path}] Syntax error - {Details}", path, e.Message ?? "unknown");
                return false;
            } catch (NotImplementedException e) {
                Log.Fatal(e, "[{Path}] Not supported - {Details}", path, e.Message ?? "unknown");
                return false;
            } catch (UnsupportedLanguageException e) {
                Log.Fatal(e, "[{Path}] Not supported language", path);
                return false;
            } catch (Exception e) {
                Log.Fatal(e, "[{Path}] Unknown error", path);
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
                ".lua" => new LuaASTBuilder(),
                _ => throw new UnsupportedLanguageException(),
            };
        }
    }
}
