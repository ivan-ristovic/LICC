using System;
using System.IO;
using System.Linq;
using System.Reflection;
using LICC.AST.Builders;
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

            Type? builderType = Assembly
                .GetExecutingAssembly()
                .GetExportedTypes()
                .SingleOrDefault(t => t.GetCustomAttributes<ASTBuilderAttribute>().Any(a => a.FileExtension == fi.Extension))
                ;
            if (builderType is null)
                throw new UnsupportedLanguageException();

            if (!(Activator.CreateInstance(builderType) is IAbstractASTBuilder builder))
                throw new NotImplementedException("The builder for required file extension is found but does not inherit IAbstractASTBuilder class.");

            return builder.BuildFromSource(code);
        }
    }
}
