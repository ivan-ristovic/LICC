using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using RICC.AST.Nodes;

namespace RICC.Tests.AST.Builders.C
{
    internal sealed class ExpressionTests
    {
        [Test]
        public void LiteralExpressionTest()
        {
            // TODO support chars properly
            ASTNode ast = CASTProvider.BuildFromSource("int x = 3; float y = 2.3; char c = 'a'; string s = \"abc\";");
            AssertExpressionValuesFromDeclaration(ast, ("x", 3), ("y", 2.3), ("c", "'a'"), ("s", "abc"));
        }

        [Test]
        public void ArithmeticExpressionTest()
        {
            ASTNode ast = CASTProvider.BuildFromSource("int x = 1 << (1 + 1 * 2) >> 3; float y = 2.3 + 4.0 / 2.0; double c = 3.3 + (4.1 - 1.1) * 2.0;");
            AssertExpressionValuesFromDeclaration(ast, ("x", 1), ("y", 4.3), ("c", 9.3));
        }

        [Test]
        public void RelationalExpressionTest()
        {
            ASTNode ast = CASTProvider.BuildFromSource("bool a = 1 > 1, b = 1 >= 1, c = 2 > 1, d = 3 < 1, e = 1 != 1, f = (1 + 1) == 2");
            AssertExpressionValuesFromDeclaration(ast, ("a", false), ("b", true), ("c", true), ("d", false), ("e", false), ("f", true));

            ast = CASTProvider.BuildFromSource("bool a = 1.1 > 1.0, b = 1.101 >= 1.1, c = (1 + 3 * 2) > 7, d = 3.0 + 0.1 > 3.0, e = 1.01 != 1.0, f = (1 << 1) == 2");
            AssertExpressionValuesFromDeclaration(ast, ("a", true), ("b", true), ("c", false), ("d", true), ("e", true), ("f", true));
        }


        private static void AssertExpressionValuesFromDeclaration(ASTNode ast, params (string Identifier, object value)[] vars)
        {
            IEnumerable<DeclarationNode> decls = ast.Children.SelectMany(d => d.Children.ElementAt(1).As<DeclarationListNode>().Declarations);
            Assert.That(decls.Select(var => ExtractIdentifierAndValue(var)), Is.EqualTo(vars).Within(1e-10));


            static (string, object?) ExtractIdentifierAndValue(DeclarationNode declNode)
            {
                VariableDeclarationNode var = declNode.As<VariableDeclarationNode>();
                return (var.Identifier, var.Initializer?.Evaluate());
            }
        }
    }
}
