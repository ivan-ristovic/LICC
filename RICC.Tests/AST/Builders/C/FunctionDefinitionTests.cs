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
            ASTNode ast = CASTProvider.BuildFromSource("\nint f() { }");
            FunctionDefinitionNode f = ast.Children.Single().As<FunctionDefinitionNode>();
            Assert.That(f.Parent, Is.EqualTo(ast));
            this.AssertFunctionSignature(f, 2, "f", "int", DeclarationSpecifiersFlags.Private);
        }

        [Test]
        public void ModifierTest()
        {
            ASTNode ast = CASTProvider.BuildFromSource(@"extern static time_t f_1() { }");
            FunctionDefinitionNode f = ast.Children.Single().As<FunctionDefinitionNode>();
            this.AssertFunctionSignature(f, 1, "f_1", "time_t", DeclarationSpecifiersFlags.Public | DeclarationSpecifiersFlags.Static);
        }

        [Test]
        public void SingleParameterTest()
        {
            ASTNode ast = CASTProvider.BuildFromSource("\n\n\nvoid f(int x) { }");
            FunctionDefinitionNode f = ast.Children.Single().As<FunctionDefinitionNode>();
            this.AssertFunctionSignature(f, 4, "f", @params: ("int", "x"));
        }

        [Test]
        public void MultipleParametersTest()
        {
            ASTNode ast = CASTProvider.BuildFromSource(@"void f(int x, double y, float z, Point t) { }");
            FunctionDefinitionNode f = ast.Children.Single().As<FunctionDefinitionNode>();
            this.AssertFunctionSignature(f, 1, "f", @params: new[] { ("int", "x"), ("double", "y"), ("float", "z"), ("Point", "t") });
        }

        [Test]
        public void SimpleDefinitionTest()
        {
            ASTNode ast = CASTProvider.BuildFromSource(@"
                unsigned int f(int x) { 
                    return x;
                }
            ");
            FunctionDefinitionNode f = ast.Children.Single().As<FunctionDefinitionNode>();
            this.AssertFunctionSignature(f, 2, "f", "unsigned int", @params: ("int", "x"));

            Assert.That(f.Definition, Is.Not.Null);
            Assert.That(f.Definition, Is.InstanceOf<BlockStatementNode>());
            Assert.That(f.Definition.Parent, Is.EqualTo(f));
            Assert.That(f.Definition.Children.Single(), Is.Not.Null);
            Assert.That(f.Definition.Children.Single(), Is.InstanceOf<JumpStatementNode>());
        }

        [Test]
        public void ComplexDefinitionTest()
        {
            ASTNode ast = CASTProvider.BuildFromSource(@"
                float f(unsigned int x, ...) {
                    int z = 4;
                    return 3f;
                }
            ");
            FunctionDefinitionNode f = ast.Children.Single().As<FunctionDefinitionNode>();
            this.AssertFunctionSignature(f, 2, "f", "float", @params: ("unsigned int", "x"));

            Assert.That(f.Definition, Is.Not.Null);
            Assert.That(f.Definition, Is.Not.Null);
            Assert.That(f.Definition, Is.InstanceOf<BlockStatementNode>());
            Assert.That(f.Definition.Parent, Is.EqualTo(f));
            Assert.That(f.Definition.Children, Has.Exactly(2).Items);
        }


        private void AssertFunctionSignature(FunctionDefinitionNode f,
                                             int line,
                                             string fname,
                                             string returnType = "void",
                                             DeclarationSpecifiersFlags declSpecs = DeclarationSpecifiersFlags.Private,
                                             params (string Type, string Identifier)[] @params)
        {
            Assert.That(f, Is.Not.Null);
            Assert.That(f.Line, Is.EqualTo(line));
            Assert.That(f.Parent, Is.Not.Null);
            Assert.That(f.Parent, Is.InstanceOf<TranslationUnitNode>());
            Assert.That(f.Children, Has.Exactly(@params?.Any() ?? false ? 4 : 3).Items);
            Assert.That(f.DeclarationSpecifiers, Is.EqualTo(declSpecs));
            Assert.That(f.Identifier, Is.EqualTo(fname));
            Assert.That(f.ReturnType, Is.EqualTo(returnType));
            if (@params?.Any() ?? false) {
                Assert.That(f.Parameters, Is.Not.Null);
                Assert.That(f.Parameters!.Parameters.Select(p => (p.DeclarationSpecifiers.TypeName, p.Identifier)), Is.EqualTo(@params));
            }
        }
    }
}
