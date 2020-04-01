using System;
using System.IO;
using LICC.AST.Builders.C;
using LICC.AST.Builders.Lua;
using LICC.AST.Builders.Pseudo;
using LICC.AST.Nodes;
using LICC.Exceptions;
using Serilog;

namespace LICC.AST
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

            var fi = new FileInfo(path);
            string code = File.ReadAllText(path);
            return fi.Extension switch
            {
                ".c" => new CASTBuilder().BuildFromSource(code),
                ".lua" => new LuaASTBuilder().BuildFromSource(code),
                ".psc" => new PseudoASTBuilder().BuildFromSource(code),
                _ => throw new UnsupportedLanguageException(),
            };
        }
    }
}
