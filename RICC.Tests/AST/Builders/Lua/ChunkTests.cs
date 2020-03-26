﻿using System.Linq;
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
        public void FunctionTest()
        {
            TranslationUnitNode tu = this.AssertTranslationUnit(@"function two() return 2 end");
            Assert.That(tu.Children.Single(), Is.InstanceOf<FunctionDefinitionNode>());
        }


        protected override ASTNode GenerateAST(string src) 
            => new LuaASTBuilder().BuildFromSource(src);
    }
}
