using System.Linq;
using NUnit.Framework;
using RICC.AST.Builders.Lua;
using RICC.AST.Nodes;
using RICC.Tests.AST.Builders.Common;

namespace RICC.Tests.AST.Builders.Lua
{
    internal sealed class ChunkTests : TranslationUnitTestsBase
    {
        [Test]
        public void BasicTest()
        {
            TranslationUnitNode tu = this.AssertTranslationUnit(@"x = 2");
            Assert.That(tu.Children.Single(), Is.InstanceOf<DeclarationStatementNode>());
        }

        [Test]
        public void MultipleDeclarationsTest()
        {
            TranslationUnitNode tu = this.AssertTranslationUnit(@"
                x = 2
                y = 3
            ");
            Assert.That(tu.Children, Is.All.InstanceOf<DeclarationStatementNode>());
        }

        [Test]
        public void DeclarationListTest()
        {
            // Needs update now that tmp variables have been added
            Assert.Inconclusive();

            TranslationUnitNode tu = this.AssertTranslationUnit(@"
                x, y = 2, 3
            ");
            Assert.That(tu.Children, Is.All.InstanceOf<DeclarationStatementNode>());
        }

        [Test]
        public void MixedDeclarationTest()
        {
            // Needs update now that tmp variables have been added
            Assert.Inconclusive();

            TranslationUnitNode tu = this.AssertTranslationUnit(@"
                x = 4

                y = 3

                x, y = y, x
            ");
            Assert.That(tu.Children, Has.Exactly(4).Items);
            Assert.That(tu.Children.ElementAt(0), Is.InstanceOf<DeclarationStatementNode>());
            Assert.That(tu.Children.ElementAt(1), Is.InstanceOf<DeclarationStatementNode>());
            Assert.That(tu.Children.ElementAt(2), Is.InstanceOf<ExpressionStatementNode>());
            Assert.That(tu.Children.ElementAt(3), Is.InstanceOf<ExpressionStatementNode>());
        }


        protected override ASTNode GenerateAST(string src) 
            => new LuaASTBuilder().BuildFromSource(src);
    }
}
