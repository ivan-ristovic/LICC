using System.Linq;
using NUnit.Framework;
using RICC.AST.Builders;
using RICC.AST.Nodes;

namespace RICC.Tests.AST.Builders.Common
{
    internal abstract class BlockTestsBase<TBuilder> where TBuilder : IASTBuilder, new()
    {
        protected BlockStatementNode AssertFunctionBlock(string src, bool empty = false)
        {
            ASTNode root = new TBuilder().BuildFromSource(src);
            FunctionDefinitionNode f = root.Children.Single().As<FunctionDefinitionNode>();
            BlockStatementNode block = f.As<FunctionDefinitionNode>().Definition;
            Assert.That(block, Is.Not.Null);
            Assert.That(block.Parent, Is.EqualTo(f));
            Assert.That(block.Children, empty ? Is.Empty : Is.Not.Empty);
            return block;
        }
    }
}
