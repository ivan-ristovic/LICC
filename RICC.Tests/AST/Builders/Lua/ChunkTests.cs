using NUnit.Framework;
using RICC.AST.Builders.Lua;
using RICC.AST.Nodes;
using RICC.Tests.AST.Builders.Common;

namespace RICC.Tests.AST.Builders.Lua
{
    internal sealed class ChunkTests : TranslationUnitTestsBase
    {
        [Test]
        public void SimpleChunkTest()
        {
            Assert.Inconclusive();
        }


        protected override ASTNode GenerateAST(string src) 
            => new LuaASTBuilder().BuildFromSource(src);
    }
}
