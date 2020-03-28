using System.Linq;
using NUnit.Framework;
using RICC.AST.Builders.C;
using RICC.AST.Nodes;
using RICC.Tests.AST.Builders.Common;

namespace RICC.Tests.AST.Builders.C
{
    internal sealed class SourceComponentTests : SourceComponentTestsBase
    {
        [Test]
        public void BasicTest()
        {
            SourceComponentNode sc = this.AssertTranslationUnit(@"void f() { }");
            Assert.That(sc.Children.Single(), Is.InstanceOf<FunctionDefinitionNode>());
        }

        [Test]
        public void MultipleFunctionsTest()
        {
            SourceComponentNode sc = this.AssertTranslationUnit(@"
                int f(int x) { 
                    int y = 3;
                    return x + y;
                }

                static float st_x() {
                    return 3.5f;
                }
            ");
            Assert.That(sc.Children, Is.All.InstanceOf<FunctionDefinitionNode>());
        }

        [Test]
        public void MixedDeclarationTest()
        {
            SourceComponentNode sc = this.AssertTranslationUnit(@"
                int f(int x) { 
                    int y = 3;
                    return x + y;
                }

                extern static unsigned int x, y = 5;

                static float st_x() {
                    return 3.5f;
                }
            ");
            Assert.That(sc.Children.ElementAt(0), Is.InstanceOf<FunctionDefinitionNode>());
            Assert.That(sc.Children.ElementAt(1), Is.InstanceOf<DeclarationStatementNode>());
            Assert.That(sc.Children.ElementAt(2), Is.InstanceOf<FunctionDefinitionNode>());
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
