using System.Linq;
using NUnit.Framework;
using RICC.AST.Builders;
using RICC.AST.Nodes;
using RICC.AST.Nodes.Common;

namespace RICC.Tests.AST.Builders.Common
{
    internal abstract class FunctionDefinitionTestsBase<TBuilder> where TBuilder : IASTBuilder, new()
    {
        protected FunctionDefinitionNode AssertFunctionDefinition(string src,
                                                                  int line,
                                                                  string fname,
                                                                  string returnType = "void",
                                                                  bool isVariadic = false,
                                                                  AccessModifiers access = AccessModifiers.Unspecified,
                                                                  QualifierFlags qualifiers = QualifierFlags.None,
                                                                  params (string Type, string Identifier)[] @params)
        {
            ASTNode ast = new TBuilder().BuildFromSource(src);
            FunctionDefinitionNode f = ast.Children.Single().As<FunctionDefinitionNode>();
            Assert.That(f, Is.Not.Null);
            Assert.That(f.Parent, Is.EqualTo(ast));
            Assert.That(f.Line, Is.EqualTo(line));
            Assert.That(f.Declarator, Is.Not.Null);
            Assert.That(f.Keywords.AccessModifiers, Is.EqualTo(access));
            Assert.That(f.Keywords.QualifierFlags, Is.EqualTo(qualifiers));
            Assert.That(f.Identifier, Is.EqualTo(fname));
            Assert.That(f.ReturnTypeName, Is.EqualTo(returnType));
            Assert.That(f.IsVariadic, Is.EqualTo(isVariadic));
            if (@params?.Any() ?? false) {
                Assert.That(f.Parameters, Is.Not.Null);
                Assert.That(f.Parameters, Has.Exactly(@params.Length).Items);
                Assert.That(f.ParametersNode, Is.Not.Null);
                Assert.That(f.Parameters.Select(p => (p.DeclarationSpecifiers.TypeName, p.Declarator.Identifier)), Is.EqualTo(@params));
            }
            return f;
        }
    }
}
