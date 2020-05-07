using LICC.AST.Builders.Lua;
using LICC.AST.Exceptions;
using LICC.AST.Nodes;
using LICC.Tests.AST.Builders.Common;
using NUnit.Framework;

namespace LICC.Tests.AST.Builders.Lua
{
    internal sealed class BuildingErrorTests : BuildingErrorTestsBase
    {
        [Test]
        public void EmptySourceTest()
        {
            this.AssertThrows<SyntaxErrorException>("");
        }

        // TODO


        protected override ASTNode GenerateAST(string src)
            => new LuaASTBuilder().BuildFromSource(src);
    }
}
