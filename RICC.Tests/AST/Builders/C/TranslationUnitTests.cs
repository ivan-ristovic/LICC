using NUnit.Framework;
using RICC.AST.Nodes;

namespace RICC.Tests.AST.Builders.C
{
    internal sealed class TranslationUnitTests
    {
        [Test]
        public void BasicTest()
        {
            ASTNode ast = CASTProvider.BuildFromSource(@"void f() { }");
            this.AssertTranslationUnit(ast.As<TranslationUnitNode>());
        }

        [Test]
        public void MultipleFunctionsTest()
        {
            ASTNode ast = CASTProvider.BuildFromSource(@"
                int f(int x) { 
                    int y = 3;
                    return x + y;
                }

                static float st_x() {
                    return 3.5f;
                }
            ");
            this.AssertTranslationUnit(ast.As<TranslationUnitNode>());
        }

        [Test]
        public void EmptySourceTest()
        {
            ASTNode ast = CASTProvider.BuildFromSource("");
            Assert.That(ast, Is.Not.Null);
            Assert.That(ast, Is.InstanceOf<TranslationUnitNode>());
            Assert.That(ast.Children, Is.Empty);
            Assert.That(ast.Line, Is.EqualTo(1));
            Assert.That(ast.Parent, Is.Null);
        }


        private void AssertTranslationUnit(TranslationUnitNode tu)
        {
            Assert.That(tu, Is.Not.Null);
            Assert.That(tu, Is.InstanceOf<TranslationUnitNode>());
            Assert.That(tu.Children, Is.Not.Empty);
            Assert.That(tu.Line, Is.EqualTo(1));
            Assert.That(tu.Parent, Is.Null);
            Assert.That(tu.Children, Is.Not.All.Null);
        }
    }
}
