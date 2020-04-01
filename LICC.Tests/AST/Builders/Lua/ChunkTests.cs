using System.Linq;
using NUnit.Framework;
using LICC.AST.Builders.Lua;
using LICC.AST.Nodes;
using LICC.Tests.AST.Builders.Common;

namespace LICC.Tests.AST.Builders.Lua
{
    internal sealed class ChunkTests : SourceComponentTestsBase
    {
        [Test]
        public void BasicTest()
        {
            SourceComponentNode tu = this.AssertTranslationUnit(@"x = 2");
            Assert.That(tu.Children.Single(), Is.InstanceOf<DeclarationStatementNode>());
        }

        [Test]
        public void FunctionTest()
        {
            SourceComponentNode tu = this.AssertTranslationUnit(@"function two() return 2 end");
            Assert.That(tu.Children.Single(), Is.InstanceOf<FunctionDefinitionNode>());
        }


        protected override ASTNode GenerateAST(string src) 
            => new LuaASTBuilder().BuildFromSource(src);
    }
}
