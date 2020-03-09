﻿using NUnit.Framework;
using RICC.AST.Nodes;

namespace RICC.Tests.AST.Builders.Common
{
    internal abstract class BlockTestsBase : ASTBuilderTestBase
    {
        protected BlockStatementNode AssertBlock(string src, bool empty = false)
        {
            BlockStatementNode block = this.GenerateAST(src).As<BlockStatementNode>();
            Assert.That(block, Is.Not.Null);
            Assert.That(block.Children, empty ? Is.Empty : Is.Not.Empty);
            this.AssertChildrenParentProperties(block);
            return block;
        }
    }
}
