using System.Linq;
using NUnit.Framework;
using RICC.AST.Builders.C;
using RICC.AST.Nodes;
using RICC.Tests.AST.Builders.Common;

namespace RICC.Tests.AST.Builders.C
{
    internal sealed class TranslationUnitTests : TranslationUnitTestsBase
    {
        [Test]
        public void BasicTest()
        {
            TranslationUnitNode tu = this.AssertTranslationUnit(@"void f() { }");
            Assert.That(tu.Children.Single(), Is.InstanceOf<FunctionDefinitionNode>());
        }

        [Test]
        public void MultipleFunctionsTest()
        {
            TranslationUnitNode tu = this.AssertTranslationUnit(@"
                int f(int x) { 
                    int y = 3;
                    return x + y;
                }

                static float st_x() {
                    return 3.5f;
                }
            ");
            Assert.That(tu.Children, Is.All.InstanceOf<FunctionDefinitionNode>());
        }

        [Test]
        public void MixedDeclarationTest()
        {
            TranslationUnitNode tu = this.AssertTranslationUnit(@"
                int f(int x) { 
                    int y = 3;
                    return x + y;
                }

                extern static unsigned int x, y = 5;

                static float st_x() {
                    return 3.5f;
                }
            ");
            Assert.That(tu.Children.ElementAt(0), Is.InstanceOf<FunctionDefinitionNode>());
            Assert.That(tu.Children.ElementAt(1), Is.InstanceOf<DeclarationStatementNode>());
            Assert.That(tu.Children.ElementAt(2), Is.InstanceOf<FunctionDefinitionNode>());
        }

        [Test]
        public void EmptySourceTest()
        {
            this.AssertTranslationUnit("", empty: true);
        }


        protected override ASTNode GenerateAST(string src)
            => new CASTBuilder().BuildFromSource(src);
    }
}
