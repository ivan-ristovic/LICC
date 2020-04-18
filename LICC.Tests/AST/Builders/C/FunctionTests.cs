using System.Linq;
using NUnit.Framework;
using LICC.AST.Builders.C;
using LICC.AST.Nodes;
using LICC.AST.Nodes.Common;
using LICC.Tests.AST.Builders.Common;

namespace LICC.Tests.AST.Builders.C
{
    internal sealed class FunctionTests : FunctionTestsBase
    {
        [Test]
        public void NoParametersDefinitonTest()
        {
            this.AssertFunctionSignature(
                "\nint f() { }",
                2,
                "f",
                "int",
                isVariadic: false,
                AccessModifiers.Unspecified
            );
        }

        [Test]
        public void ModifierDefinitionTest()
        {
            this.AssertFunctionSignature(@"extern static time_t f_1() { }",
                1,
                "f_1",
                "time_t",
                isVariadic: false,
                AccessModifiers.Public,
                QualifierFlags.Static
            );
        }

        [Test]
        public void SingleParameterDefinitionTest()
        {
            this.AssertFunctionSignature("\n\n\nvoid f(int x) { }", 4, "f", @params: ("int", "x"));
        }

        [Test]
        public void MultipleParametersDefinitionTest()
        {
            this.AssertFunctionSignature(
                @"void f(int x, double y, float z, Point t) { }", 1, "f",
                @params: new[] { ("int", "x"), ("double", "y"), ("float", "z"), ("Point", "t") }
            );
        }

        [Test]
        public void SimpleDefinitionTest()
        {
            FuncDefNode f = this.AssertFunctionSignature(@"
                unsigned int f(int x) { 
                    return x;
                }", 
                2, "f", "unsigned int", @params: ("int", "x")
            );
            Assert.That(f.Definition.Children.Single(), Is.InstanceOf<JumpStatNode>());
        }

        [Test]
        public void ComplexDefinitionTest()
        {
            FuncDefNode f = this.AssertFunctionSignature(@"
                float f(const unsigned int x, ...) {
                    int z = 4;
                    return 3.0;
                }", 
                2, "f", "float", isVariadic: true, @params: ("unsigned int", "x")
            );
            Assert.That(f.IsVariadic, Is.True);
            Assert.That(f.Definition.Children, Has.Exactly(2).Items);
            Assert.That(f.ParametersNode, Is.Not.Null);
            Assert.That(f.IsVariadic);
            Assert.That(f.Parameters?.First().Specifiers.Modifiers.QualifierFlags, Is.EqualTo(QualifierFlags.Const));
        }

        [Test]
        public void FunctionReturnExpressionTest()
        {
            this.AssertReturnValue("int g() { return 3; }", 3);
            this.AssertReturnValue("int g() { return 3.3; }", 3.3);
            this.AssertReturnValue("int g() { return 3 + 1 - 2*3; }", -2);
            this.AssertReturnValue("int g() { return ((1 << 2) + 4) >> 3; }", ((1 << 2) + 4) >> 3);
            this.AssertReturnValue("int g() { return 1.1 > 1.0 && 1.0 > 1.02; }", false);
            this.AssertReturnValue("int g() { return 1.01 > 1.0 || 1.0 > 1.02; }", true);
        }


        protected override ASTNode GenerateAST(string src)
            => new CASTBuilder().BuildFromSource(src, p => p.externalDeclaration());
    }
}
