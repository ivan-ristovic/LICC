using NUnit.Framework;
using RICC.AST.Builders.Lua;
using RICC.AST.Nodes;
using RICC.AST.Nodes.Common;
using RICC.Tests.AST.Builders.Common;

namespace RICC.Tests.AST.Builders.Lua
{
    internal sealed class DeclarationTests : DeclarationTestsBase
    {
        [Test]
        public void SimpleDeclarationTest()
        {
            this.AssertVariableDeclaration("x = 2", "x", "object", value: 2);
            this.AssertVariableDeclaration("local y", "y", "object", AccessModifiers.Private, value: null);
        }

        [Test]
        public void DeclarationListTests()
        {
            this.AssertVariableDeclarationList("x, y = 3, 4", "object", vars: new (string, object?)[] { ("x", 3), ("y", 4) });
            this.AssertVariableDeclarationList("x, y = 3", "object", vars: new (string, object?)[] { ("x", 3), ("y", null) });
            this.AssertVariableDeclarationList("x, y = 3, 4, 5", "object", vars: new (string, object?)[] { ("x", 3), ("y", 4) });
            this.AssertVariableDeclarationList("x, y = \"a\", 'b'", "object", vars: new (string, object?)[] { ("x", "a"), ("y", "b") });
            this.AssertVariableDeclarationList("x, y = true, 3.4", "object", vars: new (string, object?)[] { ("x", true), ("y", 3.4) });
            this.AssertVariableDeclarationList("x, y = false, nil", "object", vars: new (string, object?)[] { ("x", false), ("y", null) });
            this.AssertVariableDeclarationList("x, y = 3.2e2, 2.33e-2", "object", vars: new (string, object?)[] { ("x", 3.2e2), ("y", 2.33e-2) });
        }


        protected override ASTNode GenerateAST(string src)
            => new LuaASTBuilder().BuildFromSource(src, p => p.block());
    }
}
