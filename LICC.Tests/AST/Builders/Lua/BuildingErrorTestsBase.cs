using NUnit.Framework;
using LICC.AST.Builders.Lua;
using LICC.AST.Nodes;
using LICC.Exceptions;
using LICC.Tests.AST.Builders.Common;

namespace LICC.Tests.AST.Builders.Lua
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
