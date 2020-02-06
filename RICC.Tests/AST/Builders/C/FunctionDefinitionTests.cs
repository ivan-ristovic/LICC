using System.Linq;
using NUnit.Framework;
using RICC.AST.Builders.C;
using RICC.AST.Nodes;
using RICC.AST.Nodes.Common;
using RICC.Tests.AST.Builders.Common;

namespace RICC.Tests.AST.Builders.C
{
    internal sealed class FunctionDefinitionTests : FunctionDefinitionTestsBase<CASTBuilder>
    {
        [Test]
        public void NoParametersDefinitonTest()
        {
            this.AssertFunctionDefinition(
                "\nint f() { }",
                2,
                "f",
                "int",
                isVariadic: false,
                AccessModifiers.Unspecified);
        }

        [Test]
        public void ModifierDefinitionTest()
        {
            this.AssertFunctionDefinition(@"extern static time_t f_1() { }",
                1,
                "f_1",
                "time_t",
                isVariadic: false,
                AccessModifiers.Public,
                QualifierFlags.Static);
        }

        [Test]
        public void SingleParameterDefinitionTest()
        {
            this.AssertFunctionDefinition("\n\n\nvoid f(int x) { }", 4, "f", @params: ("int", "x"));
        }

        [Test]
        public void MultipleParametersDefinitionTest()
        {
            this.AssertFunctionDefinition(
                @"void f(int x, double y, float z, Point t) { }", 1, "f",
                @params: new[] { ("int", "x"), ("double", "y"), ("float", "z"), ("Point", "t") }
            );
        }

        [Test]
        public void SimpleDefinitionTest()
        {
            FunctionDefinitionNode f = this.AssertFunctionDefinition(@"
                unsigned int f(int x) { 
                    return x;
                }", 
                2, "f", "unsigned int", @params: ("int", "x")
            );
            Assert.That(f.Definition, Is.Not.Null);
            Assert.That(f.Definition, Is.InstanceOf<BlockStatementNode>());
            Assert.That(f.Definition.Parent, Is.EqualTo(f));
            Assert.That(f.Definition.Children.Single(), Is.Not.Null);
            Assert.That(f.Definition.Children.Single(), Is.InstanceOf<JumpStatementNode>());
        }

        [Test]
        public void ComplexDefinitionTest()
        {
            FunctionDefinitionNode f = this.AssertFunctionDefinition(@"
                float f(const unsigned int x, ...) {
                    int z = 4;
                    return 3.0;
                }", 
                2, "f", "float", isVariadic: true, @params: ("unsigned int", "x")
            );
            Assert.That(f.IsVariadic, Is.True);
            Assert.That(f.Definition, Is.Not.Null);
            Assert.That(f.Definition, Is.InstanceOf<BlockStatementNode>());
            Assert.That(f.Definition.Parent, Is.EqualTo(f));
            Assert.That(f.Definition.Children, Has.Exactly(2).Items);
            Assert.That(f.ParametersNode, Is.Not.Null);
            Assert.That(f.IsVariadic);
            Assert.That(f.Parameters?.First().DeclarationSpecifiers.Keywords.QualifierFlags, Is.EqualTo(QualifierFlags.Const));
        }
    }
}
