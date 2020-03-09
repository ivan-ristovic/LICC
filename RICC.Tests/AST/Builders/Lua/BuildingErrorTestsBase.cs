using NUnit.Framework;
using RICC.AST.Builders.Lua;
using RICC.AST.Nodes;
using RICC.Exceptions;
using RICC.Tests.AST.Builders.Common;

namespace RICC.Tests.AST.Builders.Lua
{
    internal sealed class BuildingErrorTests : BuildingErrorTestsBase
    {
        [Test]
        public void EmptySourceTest()
        {
            this.AssertThrows<SyntaxException>("");
        }

        // TODO


        protected override ASTNode GenerateAST(string src)
            => new LuaASTBuilder().BuildFromSource(src);
    }
}
