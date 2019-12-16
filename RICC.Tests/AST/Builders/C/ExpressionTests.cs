using System;
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
            AssertExpressionValue("int x = 3", 3);
            AssertExpressionValue("float y = 2.3", 2.3);
            AssertExpressionValue("char c = 'a'", "'a'");
            AssertExpressionValue("string s = \"abc\"", "abc");
        }

        [Test]
        public void ArithmeticExpressionTest()
        {
            AssertExpressionValue("int x = 1 << (1 + 1 * 2) >> 3", 1);
            AssertExpressionValue("float y = 2.3 + 4.0 / 2.0", 4.3);
            AssertExpressionValue("double z = 3.3 + (4.1 - 1.1) * 2.0", 9.3);
        }

        [Test]
        public void ArithmeticExpressionImplicitCastTest()
        {
            AssertExpressionValue("double x = 1 << (1 + 1 * 2) >> 3", 1);
            AssertExpressionValue("float y = 2.3 + 4 / 2", 4.3);
            AssertExpressionValue("double z = 3.3 + (4.1 - 1.1) * 2", 9.3);
        }

        [Test]
        public void RelationalExpressionTest()
        {
            AssertExpressionValue("bool a = 1 > 1", false);
            AssertExpressionValue("bool b = 1 >= 1", true);
            AssertExpressionValue("bool c = 2 > 1", true);
            AssertExpressionValue("bool d = 3 < 1", false);
            AssertExpressionValue("bool e = 1 != 1", false);
            AssertExpressionValue("bool f = (1 + 1) == 2", true);
            AssertExpressionValue("bool g = 1.1 > 1.0", true);
            AssertExpressionValue("bool h = 1.101 >= 1.1", true);
            AssertExpressionValue("bool i = (1 + 3 * 2) > 7", false);
            AssertExpressionValue("bool j = 3.0 + 0.1 > 3.0", true);
            AssertExpressionValue("bool k = 1.01 != 1.0", true);
            AssertExpressionValue("bool l = (1 << 1) == 2", true);
            AssertExpressionValue("bool m = (2 >> 1) == 1", true);
        }

        [Test]
        public void BooleanExpressionTest()
        {
            AssertExpressionValue("bool a = 1 > 1 && 2 < 3", false);
            AssertExpressionValue("bool b = 1 >= 1 && 2 < 3", true);
            AssertExpressionValue("bool b = 1 >= 1 || 3 < 3", true);
            AssertExpressionValue("bool b = 1 > 1 || 3 >= 3", true);
            AssertExpressionValue("bool b = 1 > 1 || 3 > 3", false);
            AssertExpressionValue("bool c = 2 > 1 && 1 != 2 && 2 <= 3", true);
            AssertExpressionValue("bool c = 2 > 1 && 1 != 2 && 2 > 3", false);
            AssertExpressionValue("bool d = 3 < 1 || 2 < 1 || 1 > 1", false);
            AssertExpressionValue("bool d = 3 < 1 || 2 > 1 || 1 == 1", true);
            AssertExpressionValue("bool e = 1 != 1 || 1 == 1", true);
            AssertExpressionValue("bool f = (1 + 1) == 2 || 2 == 2", true);
            AssertExpressionValue("bool g = 1.1 > 1.0 && 1.0 > 1.02", false);
            AssertExpressionValue("bool g = 1.1 > 1.0 || 1.0 > 1.02", true);
            AssertExpressionValue("bool h = 1.101 >= 1.1 && (7 > 3.2 || 2 > 3)", true);
            AssertExpressionValue("bool i = (1 + 3 * 2 > 3 && 4 != 2.0) && 8 > 7", true);
            AssertExpressionValue("bool j = 1 != 0 && 1 > 1 || 1 == 1", true);
            AssertExpressionValue("bool j = 1 != 0 && (1 == 1 || 1 == 3)", true);
            AssertExpressionValue("bool j = 1 != 0 && (1 != 1 || 1 == 3)", false);
            AssertExpressionValue("bool k = 3 > 2 || 3 > 1 && 1 > 1", true);
            AssertExpressionValue("bool k = (3 > 2 || 3 > 1) && 1 > 1", false);
            AssertExpressionValue("bool l = (1 << 1) == 2 && (3 / 2 == 1)", true);
            AssertExpressionValue("bool l = (1 << 1) == 2 && (3 / 2 != 1)", false);
            AssertExpressionValue("bool l = (1 << 1) == 2 || (3 / 2 != 1)", true);
            AssertExpressionValue("bool l = (1 << 2) == 2 || (3 / 2 != 1)", false);
            AssertExpressionValue("bool l = (1 << 1) == (4 >> 1) || 1 != 1", true);
        }


        private static void AssertExpressionValue(string decl, object expected)
        {
            ASTNode ast = CASTProvider.BuildFromSource(decl);
            object value = ast.Children
                .First().As<DeclarationStatementNode>()
                .Children.ElementAt(1).As<DeclarationListNode>()
                .Declarations
                .First().As<VariableDeclarationNode>()
                .Initializer?.Evaluate()
                ?? throw new ArgumentException("Missing initializer in test");
            Assert.That(value, Is.EqualTo(expected).Within(1e-10));
        }
    }
}
