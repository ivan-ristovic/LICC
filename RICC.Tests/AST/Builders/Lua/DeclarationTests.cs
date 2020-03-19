using System.Linq;
using NUnit.Framework;
using RICC.AST.Builders.Lua;
using RICC.AST.Nodes;
using RICC.AST.Nodes.Common;
using RICC.AST.Visitors;
using RICC.Tests.AST.Builders.Common;

namespace RICC.Tests.AST.Builders.Lua
{
    internal sealed class DeclarationTests : DeclarationTestsBase
    {
        [Test]
        public void SimpleDeclarationTests()
        {
            this.AssertVariableDeclaration("x = 2", "x", "object", value: 2);
            this.AssertVariableDeclaration("local y", "y", "object", AccessModifiers.Private);
            // TODO initializers for locals
        }

        [Test]
        public void DeclarationListTests()
        {
            // Needs update now that tmp variables have been added
            Assert.Inconclusive();

            this.AssertVariableDeclarationList("x, y = 3, 4", "object", vars: new (string, object?)[] { ("x", 3), ("y", 4) });
            this.AssertVariableDeclarationList("x, y = 3", "object", vars: new (string, object?)[] { ("x", 3), ("y", null) });
            this.AssertVariableDeclarationList("x, y = 3, 4, 5", "object", vars: new (string, object?)[] { ("x", 3), ("y", 4) });
            this.AssertVariableDeclarationList("x, y = \"a\", 'b'", "object", vars: new (string, object?)[] { ("x", "a"), ("y", "b") });
            this.AssertVariableDeclarationList("x, y = true, 3.4", "object", vars: new (string, object?)[] { ("x", true), ("y", 3.4) });
            this.AssertVariableDeclarationList("x, y = false, nil", "object", vars: new (string, object?)[] { ("x", false), ("y", null) });
            this.AssertVariableDeclarationList("x, y = 3.2e2, 2.33e-2", "object", vars: new (string, object?)[] { ("x", 3.2e2), ("y", 2.33e-2) });
        }

        [Test]
        public void AddRepeatDeclarationStatementTest()
        {
            // Needs update now that tmp variables have been added
            Assert.Inconclusive();

            TranslationUnitNode ast = new LuaASTBuilder().BuildFromSource(@"
                x = 4
                x, y = 1, '2'
            ").As<TranslationUnitNode>();
            Assert.That(ast.Children, Has.Exactly(3).Items);

            DeclarationStatementNode xdecl = ast.Children[0].As<DeclarationStatementNode>();
            DeclarationStatementNode ydecl = ast.Children[1].As<DeclarationStatementNode>();
            ExpressionStatementNode xass = ast.Children[2].As<ExpressionStatementNode>();

            AssertDeclarationStatement(xdecl, "x", 4);
            AssertDeclarationStatement(ydecl, "y", "2");

            AssignmentExpressionNode expr = xass.Expression.As<AssignmentExpressionNode>();
            Assert.That(expr.LeftOperand.As<IdentifierNode>().Identifier, Is.EqualTo("x"));
            Assert.That(ExpressionEvaluator.Evaluate(expr.RightOperand), Is.EqualTo(1));


            static void AssertDeclarationStatement(DeclarationStatementNode stat, string name, object? value = null)
            {
                Assert.That(stat.Specifiers.TypeName, Is.EqualTo("object"));

                DeclarationSpecifiersNode declSpecs = stat.Specifiers;
                Assert.That(declSpecs.Keywords.AccessModifiers, Is.EqualTo(AccessModifiers.Unspecified));
                Assert.That(declSpecs.Keywords.QualifierFlags, Is.EqualTo(QualifierFlags.None));

                VariableDeclaratorNode decl = stat.DeclaratorList.Declarations.Single().As<VariableDeclaratorNode>();
                Assert.That(decl.Identifier, Is.EqualTo(name));
                if (value is { }) {
                    Assert.That(decl.Initializer, Is.Not.Null);
                    Assert.That(ExpressionEvaluator.Evaluate(decl.Initializer!), Is.EqualTo(value));
                } else {
                    Assert.That(decl.Initializer, Is.Null);
                }
            }
        }

        [Test]
        public void ArrayDeclarationTest()
        {
            Assert.Inconclusive();
        }


        protected override ASTNode GenerateAST(string src)
            => new LuaASTBuilder().BuildFromSource(src, p => p.block());
    }
}
