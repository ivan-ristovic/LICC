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
            this.AssertVariableDeclaration(ast.Children.ElementAt(0), "int", "x");
            this.AssertVariableDeclaration(ast.Children.ElementAt(1), "float", "y");
            this.AssertVariableDeclaration(ast.Children.ElementAt(2), "Point", "z");
            this.AssertVariableDeclaration(ast.Children.ElementAt(3), "time_t", "y_y");
        }

        [Test]
        public void DeclarationSpecifierTest()
        {
            ASTNode ast2 = CASTProvider.BuildFromSource("static time_t x;");
            this.AssertVariableDeclaration(ast2.Children.ElementAt(0), "time_t", "x", DeclarationSpecifiersFlags.Private | DeclarationSpecifiersFlags.Static);
            ASTNode ast1 = CASTProvider.BuildFromSource("static extern unsigned int x;");
            this.AssertVariableDeclaration(ast1.Children.ElementAt(0), "unsigned int", "x", DeclarationSpecifiersFlags.Public | DeclarationSpecifiersFlags.Static);
        }

        [Test]
        public void InitializerDeclarationTest()
        {
            // TODO when initializers are done
            Assert.Inconclusive();

            ASTNode ast = CASTProvider.BuildFromSource("static int x = 5;");
            this.AssertVariableDeclaration(ast.Children.ElementAt(0), "int", "x", DeclarationSpecifiersFlags.Private | DeclarationSpecifiersFlags.Static, 5);
        }


        private void AssertVariableDeclaration(ASTNode node, 
                                               string type, 
                                               string identifier,
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

            VariableDeclarationNode var = decl.Children.ElementAt(1).As<VariableDeclarationNode>();
            Assert.That(var.Parent, Is.EqualTo(decl));
            Assert.That(var.Value, Is.EqualTo(value));
            Assert.That(var.Identifier, Is.EqualTo(identifier));
            Assert.That(var.Children.Single().As<IdentifierNode>().Identifier, Is.EqualTo(identifier));
        }
    }
}
