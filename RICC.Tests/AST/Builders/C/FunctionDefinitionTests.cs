using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using RICC.AST.Nodes;
using RICC.AST.Nodes.Common;

namespace RICC.Tests.AST.Builders.C
{
    internal sealed class FunctionDefinitionTests
    {
        [Test]
        public void NoParametersTest()
        {
            ASTNode ast = CASTProvider.BuildFromSource(@"void f() { }");
            FunctionDefinitionNode f = ast.Children.First().As<FunctionDefinitionNode>();
            this.AssertFunctionSignature(f, 1, DeclarationSpecifiersFlags.Private, "f", "void");
        }


        private void AssertFunctionSignature(FunctionDefinitionNode f,
                                             int line,
                                             DeclarationSpecifiersFlags declSpecs,
                                             string fname,
                                             string returnType = "void",
                                             params (string Type, string Identifier)[] @params)
        {
            Assert.That(f, Is.Not.Null);
            Assert.That(f.Line, Is.EqualTo(line));
            Assert.That(f.Parent, Is.Not.Null);
            Assert.That(f.Parent, Is.InstanceOf<TranslationUnitNode>());
            Assert.That(f.Children, Has.Count.EqualTo(@params?.Any() ?? false ? 4 : 3));
            Assert.That(f.DeclarationSpecifiers, Is.EqualTo(declSpecs));
            Assert.That(f.Identifier, Is.EqualTo(fname));
            Assert.That(f.ReturnType, Is.EqualTo(returnType));
            if (@params?.Any() ?? false) {
                Assert.That(f.Parameters, Is.Not.Null);
                Assert.That(f.Parameters!.Parameters.Select(p => (p.Identifier, p.DeclarationSpecifiers.Type)), Is.EqualTo(@params));
            }
        }
    }
}
