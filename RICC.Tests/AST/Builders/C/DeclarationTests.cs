using System.Linq;
using NUnit.Framework;
using RICC.AST.Nodes;
using RICC.AST.Nodes.Common;
using RICC.AST.Visitors;

namespace RICC.Tests.AST.Builders.C
{
    internal sealed class DeclarationTests
    {
        [Test]
        public void SimpleDeclarationTest()
        {
            ASTNode ast = CASTProvider.BuildFromSource("int x; float y; Point z;time_t y_y;");
            this.AssertVariableDeclaration(ast.Children.ElementAt(0), "x", "int");
            this.AssertVariableDeclaration(ast.Children.ElementAt(1), "y", "float");
            this.AssertVariableDeclaration(ast.Children.ElementAt(2), "z", "Point");
            this.AssertVariableDeclaration(ast.Children.ElementAt(3), "y_y", "time_t");
        }

        [Test]
        public void DeclarationSpecifierTest()
        {
            ASTNode ast2 = CASTProvider.BuildFromSource("static time_t x;");
            this.AssertVariableDeclaration(ast2.Children.First(), "x", "time_t", DeclarationSpecifiersFlags.Private | DeclarationSpecifiersFlags.Static);
            ASTNode ast1 = CASTProvider.BuildFromSource("static extern unsigned int x;");
            this.AssertVariableDeclaration(ast1.Children.First(), "x", "unsigned int", DeclarationSpecifiersFlags.Public | DeclarationSpecifiersFlags.Static);
        }

        [Test]
        public void InitializerDeclarationTest()
        {
            ASTNode ast = CASTProvider.BuildFromSource("static signed int x = 5;");
            this.AssertVariableDeclaration(ast.Children.First(), "x", "int", DeclarationSpecifiersFlags.Private | DeclarationSpecifiersFlags.Static, 5);
        }

        [Test]
        public void InitializerExpressionDeclarationTest()
        {
            ASTNode ast = CASTProvider.BuildFromSource("unsigned short x = 1 << 2 * 4;");
            this.AssertVariableDeclaration(ast.Children.First(), "x", "unsigned short", DeclarationSpecifiersFlags.Private, 1 << 8);
        }

        [Test]
        public void SimpleFunctionDeclarationTest()
        {
            // TODO when function declarations are done
            Assert.Inconclusive();

            // ASTNode ast = CASTProvider.BuildFromSource("void f();");
        }

        [Test]
        public void SimpleDeclarationListTest()
        {
            ASTNode ast = CASTProvider.BuildFromSource("static unsigned int x, y, z;");
            this.AssertVariableDeclarationList(
                ast.Children.First(),
                "unsigned int",
                DeclarationSpecifiersFlags.Private | DeclarationSpecifiersFlags.Static,
                ("x", null), ("y", null), ("z", null)
            );
        }

        [Test]
        public void IntDeclarationListWithInitializersTest()
        {
            ASTNode ast = CASTProvider.BuildFromSource("extern static int x, y = 7 + (4 - 3), z = 3, w = 3*4 + 7*5, t = 2 >> (3 << 4);");
            this.AssertVariableDeclarationList(
                ast.Children.First(),
                "int",
                DeclarationSpecifiersFlags.Public | DeclarationSpecifiersFlags.Static,
                ("x", null), ("y", 7 + (4 - 3)), ("z", 3), ("w", 47), ("t", 2 >> (3 << 4)) 
            );
        }

        [Test]
        public void FloatDeclarationListWithInitializersTest()
        {
            ASTNode ast = CASTProvider.BuildFromSource("float x, y = 7.1 + 4.2, z = 3.0, w = 3.2*4.45 + 7.2*5.11 - (5.0/2.5);");
            this.AssertVariableDeclarationList(
                ast.Children.First(),
                "float",
                DeclarationSpecifiersFlags.Private,
                ("x", null), ("y", 11.3), ("z", 3.0), ("w", 49.032)
            );
        }

        [Test]
        public void BoolDeclarationListWithInitializersTest()
        {
            ASTNode ast = CASTProvider.BuildFromSource("bool x, y = 1 == 1, z = 3 <= 4, w = 4 != (3 + 1);");
            this.AssertVariableDeclarationList(
                ast.Children.First(),
                "bool",
                DeclarationSpecifiersFlags.Private,
                ("x", null), ("y", true), ("z", true), ("w", false)
            );
        }

        [Test]
        public void StringDeclarationListWithInitializersTest()
        {
            ASTNode ast = CASTProvider.BuildFromSource(@"char* w1, w2 = ""abc"", w3 = ""aa"" + ""bb"";");
            this.AssertVariableDeclarationList(
                ast.Children.First(),
                "char*",
                DeclarationSpecifiersFlags.Private,
                ("w1", null), ("w2", "abc"), ("w3", "aabb")
            );
        }


        private void AssertVariableDeclaration(ASTNode node, 
                                               string identifier,
                                               string type,
                                               DeclarationSpecifiersFlags declSpecs = DeclarationSpecifiersFlags.Private,
                                               object? value = null) 
        {
            DeclarationStatementNode decl = node.As<DeclarationStatementNode>();
            Assert.That(decl.Children, Has.Count.EqualTo(2));

            DeclarationSpecifiersNode declSpecsNode = decl.Children.ElementAt(0).As<DeclarationSpecifiersNode>();
            Assert.That(declSpecsNode.Parent, Is.EqualTo(decl));
            Assert.That(declSpecsNode.Specifiers, Is.EqualTo(declSpecs));
            Assert.That(declSpecsNode.TypeName, Is.EqualTo(type));
            Assert.That(declSpecsNode.Children, Is.Empty);

            DeclarationListNode declList = decl.Children.ElementAt(1).As<DeclarationListNode>();
            Assert.That(declList.Parent, Is.EqualTo(decl));
            VariableDeclarationNode var = declList.Declarations.First().As<VariableDeclarationNode>();
            Assert.That(var.Parent, Is.EqualTo(declList));
            Assert.That(var.Identifier, Is.EqualTo(identifier));
            Assert.That(var.Children.First().As<IdentifierNode>().Identifier, Is.EqualTo(identifier));
            if (var.Initializer is { }) {
                Assert.That(var.Initializer.Parent, Is.EqualTo(var));
                Assert.That(ExpressionEvaluator.TryEvaluateAs(var.Initializer, out object? result));
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.EqualTo(value).Within(1e-10));
            }
        }

        private void AssertVariableDeclarationList(ASTNode node,
                                                   string type,
                                                   DeclarationSpecifiersFlags declSpecs,
                                                   params (string Identifier, object? value)[] vars)
        {
            DeclarationStatementNode decl = node.As<DeclarationStatementNode>();
            Assert.That(decl.Children, Has.Count.EqualTo(2));

            DeclarationSpecifiersNode declSpecsNode = decl.Children.ElementAt(0).As<DeclarationSpecifiersNode>();
            Assert.That(declSpecsNode.Parent, Is.EqualTo(decl));
            Assert.That(declSpecsNode.Specifiers, Is.EqualTo(declSpecs));
            Assert.That(declSpecsNode.TypeName, Is.EqualTo(type));
            Assert.That(declSpecsNode.Children, Is.Empty);

            DeclarationListNode declList = decl.Children.ElementAt(1).As<DeclarationListNode>();
            Assert.That(declList.Parent, Is.EqualTo(decl));
            Assert.That(declList.Declarations.Select(var => ExtractIdentifierAndValue(var)), Is.EqualTo(vars).Within(1e-10));


            static (string, object?) ExtractIdentifierAndValue(DeclarationNode declNode)
            {
                VariableDeclarationNode var = declNode.As<VariableDeclarationNode>();
                return var.Initializer is null ? (var.Identifier, (object?)null)
                                               : (var.Identifier, ExpressionEvaluator.Evaluate(var.Initializer));
            }
        }
    }
}
