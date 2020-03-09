using System.Linq;
using NUnit.Framework;
using RICC.AST.Nodes;

namespace RICC.Tests.AST.Builders.Common
{
    internal abstract class BlockTestsBase : ASTBuilderTestBase
    {
        protected BlockStatementNode AssertFunctionBlock(string src, bool empty = false)
        {
            ASTNode root = this.GenerateAST(src);
            FunctionDefinitionNode f = root.Children.Single().As<FunctionDefinitionNode>();
            BlockStatementNode block = f.As<FunctionDefinitionNode>().Definition;
            Assert.That(block, Is.Not.Null);
            Assert.That(block.Parent, Is.EqualTo(f));
            Assert.That(block.Children, empty ? Is.Empty : Is.Not.Empty);
            return block;
        }
    }
}
