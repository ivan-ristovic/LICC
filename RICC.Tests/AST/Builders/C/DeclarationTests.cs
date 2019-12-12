using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using RICC.AST.Nodes;
using RICC.AST.Nodes.Common;

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
            ASTNode ast = CASTProvider.BuildFromSource("static int x = 5;");
            this.AssertVariableDeclaration(ast.Children.First(), "x", "int", DeclarationSpecifiersFlags.Private | DeclarationSpecifiersFlags.Static, 5);
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
            ASTNode ast = CASTProvider.BuildFromSource("static int x, y, z;");
            this.AssertVariableDeclarationList(
                ast.Children.First(),
                "int",
                DeclarationSpecifiersFlags.Private | DeclarationSpecifiersFlags.Static,
                ("x", null), ("y", null), ("z", null)
            );
        }

        [Test]
        public void DeclarationListWithInitializersTest()
        {
            ASTNode ast = CASTProvider.BuildFromSource("extern static int x, y = 7 + 4, z = 3;");
            this.AssertVariableDeclarationList(
                ast.Children.First(),
                "int",
                DeclarationSpecifiersFlags.Public | DeclarationSpecifiersFlags.Static,
                ("x", null), ("y", 11), ("z", 3) 
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
            Assert.That(declSpecsNode.Type, Is.EqualTo(type));
            Assert.That(declSpecsNode.Children, Is.Empty);

            DeclarationListNode declList = decl.Children.ElementAt(1).As<DeclarationListNode>();
            Assert.That(declList.Parent, Is.EqualTo(decl));
            VariableDeclarationNode var = declList.Declarations.First().As<VariableDeclarationNode>();
            Assert.That(var.Parent, Is.EqualTo(declList));
            Assert.That(var.Identifier, Is.EqualTo(identifier));
            Assert.That(var.Children.First().As<IdentifierNode>().Identifier, Is.EqualTo(identifier));
            if (var.Initializer is { }) {
                object? res = var.Initializer.Evaluate();
                Assert.That(res, Is.Not.Null);
                Assert.That(res, Is.EqualTo(value));
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
            Assert.That(declSpecsNode.Type, Is.EqualTo(type));
            Assert.That(declSpecsNode.Children, Is.Empty);

            DeclarationListNode declList = decl.Children.ElementAt(1).As<DeclarationListNode>();
            Assert.That(declList.Parent, Is.EqualTo(decl));
            Assert.That(declList.Declarations.Select(var => ExtractIdentifierAndValue(var)), Is.EqualTo(vars));


            static (string, object?) ExtractIdentifierAndValue(DeclarationNode declNode)
            {
                VariableDeclarationNode var = declNode.As<VariableDeclarationNode>();
                return (var.Identifier, var.Initializer?.Evaluate());
            }
        }
    }
}
