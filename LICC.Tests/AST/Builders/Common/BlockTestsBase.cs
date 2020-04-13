using NUnit.Framework;
using LICC.AST.Nodes;

namespace LICC.Tests.AST.Builders.Common
{
    internal abstract class BlockTestsBase : ASTBuilderTestBase
    {
        protected BlockStatNode AssertBlock(string src, bool empty = false)
        {
            BlockStatNode block = this.GenerateAST(src).As<BlockStatNode>();
            Assert.That(block, Is.Not.Null);
            Assert.That(block.Children, empty ? Is.Empty : Is.Not.Empty);
            this.AssertChildrenParentProperties(block);
            return block;
        }
    }
}
