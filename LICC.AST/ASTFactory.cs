using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using LICC.AST.Builders;
using LICC.AST.Exceptions;
using LICC.AST.Nodes;

namespace LICC.AST
{
    public static class ASTFactory
    {
        public static ASTNode BuildFromFile(string path)
        {
            var fi = new FileInfo(path);
            string code = File.ReadAllText(path);

            IEnumerable<Type> builderTypes = Assembly
                .GetExecutingAssembly()
                .GetExportedTypes()
                .Where(t => t.GetCustomAttributes<ASTBuilderAttribute>().Any(a => a.FileExtension == fi.Extension))
                ;
            if (!builderTypes.Any())
                throw new UnsupportedLanguageException();

            Type? builderType = builderTypes.SingleOrDefault();
            if (builderType is null)
                throw new AmbiguousMatchException("Multiple builders are registered to handle that file type.");

            if (!(Activator.CreateInstance(builderType) is IAbstractASTBuilder builder))
                throw new NotImplementedException("The builder for required file extension is found but does not inherit IAbstractASTBuilder class.");

            return builder.BuildFromSource(code);
        }
    }
}
