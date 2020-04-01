using NUnit.Framework;
using RICC.AST.Builders.Lua;
using RICC.AST.Nodes.Common;
using RICC.Tests.AST.Builders.Common;

namespace RICC.Tests.AST.Builders.Lua
{
    internal sealed class DeclarationTests : DeclarationTestsBase<LuaASTBuilder>
    {
        [Test]
        public void SimpleDeclarationTest()
        {
            this.AssertVariableDeclaration("x = 2", "x", "object", value: 2);
            this.AssertVariableDeclaration("local y = 2", "y", "object", value: 2);
        }
    }
}
