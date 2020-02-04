using System;
using System.Linq;
using NUnit.Framework;
using RICC.AST.Nodes;
using RICC.AST.Visitors;

namespace RICC.Tests.AST.Builders.C
{
    internal sealed class ExpressionTests
    {
        [Test]
        public void LiteralExpressionTest()
        {
            this.Evaluate("int x = 3;", 3);
            this.Evaluate("float y = 2.3;", 2.3);
            this.Evaluate("char c = 'a';", 'a');
            this.Evaluate("string s = \"abc\";", "abc");

            this.TestInitializerSuffix("T x = 1u;", "U", 1U, typeof(uint));
            this.TestInitializerSuffix("T x = 1U;", "U", 1U, typeof(uint));
            this.TestInitializerSuffix("T x = 1l;", "L", 1L, typeof(long));
            this.TestInitializerSuffix("T x = 1L;", "L", 1L, typeof(long));
            this.TestInitializerSuffix("T x = 1ll;", "LL", 1L, typeof(long));
            this.TestInitializerSuffix("T x = 1ul;", "UL", 1UL, typeof(ulong));
            this.TestInitializerSuffix("T x = 1ull;", "ULL", 1UL, typeof(ulong));
            this.TestInitializerSuffix("T x = 1Ul;", "UL", 1UL, typeof(ulong));
            this.TestInitializerSuffix("T x = 1ULL;", "ULL", 1UL, typeof(ulong));
            this.TestInitializerSuffix("T x = 1LL;", "LL", 1L, typeof(long));
            this.TestInitializerSuffix("T x = 1ll;", "LL", 1L, typeof(long));

            this.TestInitializerSuffix("T x = 01U;", "U", 1U, typeof(uint));
            this.TestInitializerSuffix("T x = 077u;", "U", Convert.ToUInt32("77", fromBase: 8), typeof(uint));
            this.TestInitializerSuffix("T x = 037777777777u;", "U", Convert.ToUInt32("37777777777", fromBase: 8), typeof(uint));
            this.TestInitializerSuffix("T x = 01L;", "L", 1L, typeof(long));
            this.TestInitializerSuffix("T x = 07L;", "L", 7L, typeof(long));
            this.TestInitializerSuffix("T x = 012345671234567l;", "L", Convert.ToInt64("12345671234567", fromBase: 8), typeof(long));
            this.TestInitializerSuffix("T x = 012345671234567ll;", "LL", Convert.ToInt64("12345671234567", fromBase: 8), typeof(long));
            this.TestInitializerSuffix("T x = 012345671234567LL;", "LL", Convert.ToInt64("12345671234567", fromBase: 8), typeof(long));
            this.TestInitializerSuffix("T x = 01ul;", "UL", 1UL, typeof(ulong));
            this.TestInitializerSuffix("T x = 01Ul;", "UL", 1UL, typeof(ulong));
            this.TestInitializerSuffix("T x = 077UL;", "UL", Convert.ToUInt64("77", fromBase: 8), typeof(ulong));
            this.TestInitializerSuffix("T x = 012345671234567uL;", "UL", Convert.ToUInt64("12345671234567", fromBase: 8), typeof(ulong));
            this.TestInitializerSuffix("T x = 01ull;", "ULL", 1UL, typeof(ulong));
            this.TestInitializerSuffix("T x = 01Ull;", "ULL", 1UL, typeof(ulong));
            this.TestInitializerSuffix("T x = 077ULL;", "ULL", Convert.ToUInt64("77", fromBase: 8), typeof(ulong));
            this.TestInitializerSuffix("T x = 012345671234567uLL;", "ULL", Convert.ToUInt64("12345671234567", fromBase: 8), typeof(ulong));

            this.TestInitializerSuffix("T x = 0x1u;", "U", 0x1U, typeof(uint));
            this.TestInitializerSuffix("T x = 0xAFu;", "U", 0xAF, typeof(uint));
            this.TestInitializerSuffix("T x = 0xFFFFFFFFu;", "U", 0xFFFFFFFF, typeof(uint));
            this.TestInitializerSuffix("T x = 0xFFFFFFFFFFl;", "L", 0xFFFFFFFFFFL, typeof(long));
            this.TestInitializerSuffix("T x = 0xAFl;", "L", 0xAFL, typeof(long));
            this.TestInitializerSuffix("T x = 0xAFL;", "L", 0xAFL, typeof(long));
            this.TestInitializerSuffix("T x = 0xFll;", "LL", 0xFUL, typeof(long));
            this.TestInitializerSuffix("T x = 0xFLL;", "LL", 0xFUL, typeof(long));
            this.TestInitializerSuffix("T x = 0xFFFFFFFFFFul;", "UL", 0xFFFFFFFFFFUL, typeof(ulong));
            this.TestInitializerSuffix("T x = 0xFFFFFFFFFFuL;", "UL", 0xFFFFFFFFFFUL, typeof(ulong));
            this.TestInitializerSuffix("T x = 0xFFFFFFFFFFUl;", "UL", 0xFFFFFFFFFFUL, typeof(ulong));
            this.TestInitializerSuffix("T x = 0xFULL;", "ULL", 0xFUL, typeof(ulong));
            this.TestInitializerSuffix("T x = 0xFuLL;", "ULL", 0xFUL, typeof(ulong));
            this.TestInitializerSuffix("T x = 0xFUll;", "ULL", 0xFUL, typeof(ulong));
            this.TestInitializerSuffix("T x = 0xFull;", "ULL", 0xFUL, typeof(ulong));
        }

        [Test]
        public void ArithmeticExpressionTest()
        {
            this.Evaluate("int x = 1 << (1 + 1 * 2) >> 3;", 1);
            this.Evaluate("float y = 2.3 + 4.0 / 2.0;", 4.3);
            this.Evaluate("double z = 3.3 + (4.1 - 1.1) * 2.0;", 9.3);
            this.Evaluate("double x = 1 << (1 + 1 * 2) >> 3;", 1);
            this.Evaluate("float y = 2.3 + 4 / 2;", 4.3);
            this.Evaluate("double z = 3.3 + (4.1 - 1.1) * 2;", 9.3);
        }

        [Test]
        public void ArithmeticBitwiseExpressionTest()
        {
            this.Evaluate("int x = 1 | ~0;", ~0);
            this.Evaluate("int x = 1 | ~1;", ~0);
            this.Evaluate("int x = 1 & ~0;", 1 & ~0);
            this.Evaluate("int x = (1 << 1) & ~0;", (1 << 1) & ~0);
            this.Evaluate("int x = (1 << 10 >> 2) ^ (~0 << 10);", (1 << 10 >> 2) ^ (~0 << 10));
        }

        [Test]
        public void RelationalExpressionTest()
        {
            this.Evaluate("bool a = 1 > 1;", false);
            this.Evaluate("bool b = 1 >= 1;", true);
            this.Evaluate("bool c = 2 > 1;", true);
            this.Evaluate("bool d = 3 < 1;", false);
            this.Evaluate("bool e = 1 != 1;", false);
            this.Evaluate("bool f = (1 + 1) == 2;", true);
            this.Evaluate("bool g = 1.1 > 1.0;", true);
            this.Evaluate("bool h = 1.101 >= 1.1;", true);
            this.Evaluate("bool i = (1 + 3 * 2) > 7;", false);
            this.Evaluate("bool j = 3.0 + 0.1 > 3.0;", true);
            this.Evaluate("bool k = 1.01 != 1.0;", true);
            this.Evaluate("bool l = (1 << 1) == 2;", true);
            this.Evaluate("bool m = (2 >> 1) == 1;", true);
        }

        [Test]
        public void LogicExpressionTest()
        {
            this.Evaluate("bool t = 1 || 1;", true);
            this.Evaluate("bool t = 1 || 0;", true);
            this.Evaluate("bool t = 0 || 0;", false);
            this.Evaluate("bool t = 0 && 0;", false);
            this.Evaluate("bool t = 0 && 1;", false);
            this.Evaluate("bool t = 1 && 0;", false);
            this.Evaluate("bool t = 1 && 1;", true);
            this.Evaluate("bool t = 0.0001 && 1.1;", true);
            this.Evaluate("bool a = 1 > 1 && 2 < 3;", false);
            this.Evaluate("bool b = 1 >= 1 && 2 < 3;", true);
            this.Evaluate("bool b = 1 >= 1 || 3 < 3;", true);
            this.Evaluate("bool b = 1 > 1 || 3 >= 3;", true);
            this.Evaluate("bool b = 1 > 1 || 3 > 3;", false);
            this.Evaluate("bool c = 2 > 1 && 1 != 2 && 2 <= 3;", true);
            this.Evaluate("bool c = 2 > 1 && 1 != 2 && 2 > 3;", false);
            this.Evaluate("bool d = 3 < 1 || 2 < 1 || 1 > 1;", false);
            this.Evaluate("bool d = 3 < 1 || 2 > 1 || 1 == 1;", true);
            this.Evaluate("bool e = 1 != 1 || 1 == 1;", true);
            this.Evaluate("bool f = (1 + 1) == 2 || 2 == 2;", true);
            this.Evaluate("bool g = 1.1 > 1.0 && 1.0 > 1.02;", false);
            this.Evaluate("bool g = 1.1 > 1.0 || 1.0 > 1.02;", true);
            this.Evaluate("bool h = 1.101 >= 1.1 && (7 > 3.2 || 2 > 3);", true);
            this.Evaluate("bool i = (1 + 3 * 2 > 3 && 4 != 2.0) && 8 > 7;", true);
            this.Evaluate("bool j = 1 != 0 && 1 > 1 || 1 == 1;", true);
            this.Evaluate("bool j = 1 != 0 && (1 == 1 || 1 == 3);", true);
            this.Evaluate("bool j = 1 != 0 && (1 != 1 || 1 == 3);", false);
            this.Evaluate("bool k = 3 > 2 || 3 > 1 && 1 > 1;", true);
            this.Evaluate("bool k = (3 > 2 || 3 > 1) && 1 > 1;", false);
            this.Evaluate("bool l = (1 << 1) == 2 && (3 / 2 == 1);", true);
            this.Evaluate("bool l = (1 << 1) == 2 && (3 / 2 != 1);", false);
            this.Evaluate("bool l = (1 << 1) == 2 || (3 / 2 != 1);", true);
            this.Evaluate("bool l = (1 << 2) == 2 || (3 / 2 != 1);", false);
            this.Evaluate("bool l = (1 << 1) == (4 >> 1) || 1 != 1;", true);
        }

        [Test]
        public void FunctionArgumentExpressionTest()
        {
            AssertArgumentExpressionValue("void g() { g(); }");
            AssertArgumentExpressionValue("void g() { g(3); }", 3);
            AssertArgumentExpressionValue("void g() { g(3, 2); }", 3, 2);
            AssertArgumentExpressionValue("void g() { g(3 + 1, 2 * 3); }", 3 + 1, 2 * 3);
            AssertArgumentExpressionValue("void g() { g(((1 << 2) + 4) >> 3); }", ((1 << 2) + 4) >> 3);
            AssertArgumentExpressionValue("void g() { g(1.1 > 1.0 && 1.0 > 1.02); }", false);
            AssertArgumentExpressionValue("void g() { g(1.01 > 1.0 || 1.0 > 1.02); }", true);


            static void AssertArgumentExpressionValue(string f, params object[] argValues)
            {
                FunctionDefinitionNode fnode = CASTProvider.BuildFromSource(f).Children.First().As<FunctionDefinitionNode>();
                FunctionCallExpressionNode fcall = fnode.Definition.Children.First().Children.First().As<FunctionCallExpressionNode>();
                Assert.That(fcall.Identifier, Is.EqualTo(fnode.Identifier));
                Assert.That(fcall.Parent, Is.EqualTo(fnode.Definition.Children.First()));

                if (argValues is null || !argValues.Any()) {
                    Assert.That(fcall.Arguments, Is.Null);
                } else {
                    Assert.That(fcall.Arguments, Is.Not.Null);
                    Assert.That(fcall.Arguments!.Expressions.Count, Is.EqualTo(argValues.Length));
                    foreach ((ExpressionNode arg, object? expected) in fcall.Arguments!.Expressions.Zip(argValues))
                        Assert.That(ExpressionEvaluator.Evaluate(arg), Is.EqualTo(expected).Within(1e-10));
                }
            }
        }

        [Test]
        public void FunctionReturnExpressionTest()
        {
            AssertReturnExpressionValue("int g() { return 3; }", 3);
            AssertReturnExpressionValue("int g() { return 3.3; }", 3.3);
            AssertReturnExpressionValue("int g() { return 3 + 1 - 2*3; }", -2);
            AssertReturnExpressionValue("int g() { return ((1 << 2) + 4) >> 3; }", ((1 << 2) + 4) >> 3);
            AssertReturnExpressionValue("int g() { return 1.1 > 1.0 && 1.0 > 1.02; }", false);
            AssertReturnExpressionValue("int g() { return 1.01 > 1.0 || 1.0 > 1.02; }", true);


            static void AssertReturnExpressionValue(string f, object? expected)
            {
                FunctionDefinitionNode fnode = CASTProvider.BuildFromSource(f).Children.First().As<FunctionDefinitionNode>();
                JumpStatementNode node = fnode.Definition.Children.Last().As<JumpStatementNode>();

                Assert.That(node.GotoLabel, Is.Null);
                if (expected is null) {
                    Assert.That(node.ReturnExpression, Is.Null);
                } else {
                    Assert.That(node.ReturnExpression, Is.Not.Null);
                    if (node.ReturnExpression is { })
                        Assert.That(ExpressionEvaluator.Evaluate(node.ReturnExpression), Is.EqualTo(expected).Within(1e-10));
                }
            }
        }

        [Test]
        public void UnaryExpressionTests()
        {
            Assert.That(this.ParseInitializer("int x = y++;"), Is.InstanceOf<IncrementExpressionNode>());
            Assert.That(this.ParseInitializer("int x = y--;"), Is.InstanceOf<DecrementExpressionNode>());
            Assert.That(this.ParseInitializer("int x = ++y;"), Is.InstanceOf<UnaryExpressionNode>());
            Assert.That(this.ParseInitializer("int x = --y;"), Is.InstanceOf<UnaryExpressionNode>());
            this.Evaluate("int x = ++1;", 2);
            this.Evaluate("int x = --1;", 0);
            this.Evaluate("int x = ~0;", ~0);
            this.Evaluate("int x = ~(~0);", 0);
            this.Evaluate("bool x = !0;", true);
            this.Evaluate("bool x = !(1 > 2);", true);
            this.Evaluate("bool x = !1;", false);
            this.Evaluate("bool x = !(1 != 0);", false);
            this.Evaluate("bool x = (!1) != (!1);", false);
            this.Evaluate("bool x = (!1) != 0;", false);
        }


        private void Evaluate<T>(string decl, T expected)
        {
            ExpressionNode init = this.ParseInitializer(decl);
            Assert.That(ExpressionEvaluator.TryEvaluateAs(init, out T result));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(expected).Within(1e-10));
        }

        private ExpressionNode ParseInitializer(string code)
        {
            ASTNode ast = CASTProvider.BuildFromSource(code);
            ExpressionNode? init = ast.Children
                .First().As<DeclarationStatementNode>()
                .Children.ElementAt(1).As<DeclaratorListNode>()
                .Declarations
                .First().As<VariableDeclaratorNode>()
                .Initializer;
            Assert.That(init, Is.Not.Null);
            return init!;
        }

        private void TestInitializerSuffix(string code, string suffix, object value, Type type)
        {
            ASTNode ast = CASTProvider.BuildFromSource(code);
            DeclarationStatementNode decl = ast.Children.First().As<DeclarationStatementNode>();
            DeclaratorListNode declList = decl.Children.ElementAt(1).As<DeclaratorListNode>();
            VariableDeclaratorNode var = declList.Declarations.First().As<VariableDeclaratorNode>();
            Assert.That(var.Initializer, Is.Not.Null);
            Assert.That(var.Initializer, Is.InstanceOf<LiteralNode>());
            Assert.That(var.Initializer!.As<LiteralNode>().Value.GetType(), Is.EqualTo(type));
            Assert.That(var.Initializer!.As<LiteralNode>().Suffix, Is.EqualTo(suffix));
            Assert.That(ExpressionEvaluator.Evaluate(var.Initializer!), Is.EqualTo(value).Within(1e-10));
        }
    }
}
