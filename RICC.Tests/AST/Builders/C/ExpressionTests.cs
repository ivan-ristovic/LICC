using System;
using NUnit.Framework;
using RICC.AST.Builders.C;
using RICC.AST.Nodes;
using RICC.Tests.AST.Builders.Common;

namespace RICC.Tests.AST.Builders.C
{
    internal sealed class ExpressionTests : ExpressionTestsBase<CASTBuilder>
    {
        [Test]
        public void LiteralExpressionTest()
        {
            this.AssertInitializerValue("int x = 3;", 3);
            this.AssertInitializerValue("float y = 2.3;", 2.3);
            this.AssertInitializerValue("char c = 'a';", 'a');
            this.AssertInitializerValue("string s = \"abc\";", "abc");

            this.AssertLiteralSuffix("T x = 1u;", "U", 1U, typeof(uint));
            this.AssertLiteralSuffix("T x = 1U;", "U", 1U, typeof(uint));
            this.AssertLiteralSuffix("T x = 1l;", "L", 1L, typeof(long));
            this.AssertLiteralSuffix("T x = 1L;", "L", 1L, typeof(long));
            this.AssertLiteralSuffix("T x = 1ll;", "LL", 1L, typeof(long));
            this.AssertLiteralSuffix("T x = 1ul;", "UL", 1UL, typeof(ulong));
            this.AssertLiteralSuffix("T x = 1ull;", "ULL", 1UL, typeof(ulong));
            this.AssertLiteralSuffix("T x = 1Ul;", "UL", 1UL, typeof(ulong));
            this.AssertLiteralSuffix("T x = 1ULL;", "ULL", 1UL, typeof(ulong));
            this.AssertLiteralSuffix("T x = 1LL;", "LL", 1L, typeof(long));
            this.AssertLiteralSuffix("T x = 1ll;", "LL", 1L, typeof(long));

            this.AssertLiteralSuffix("T x = 01U;", "U", 1U, typeof(uint));
            this.AssertLiteralSuffix("T x = 077u;", "U", Convert.ToUInt32("77", fromBase: 8), typeof(uint));
            this.AssertLiteralSuffix("T x = 037777777777u;", "U", Convert.ToUInt32("37777777777", fromBase: 8), typeof(uint));
            this.AssertLiteralSuffix("T x = 01L;", "L", 1L, typeof(long));
            this.AssertLiteralSuffix("T x = 07L;", "L", 7L, typeof(long));
            this.AssertLiteralSuffix("T x = 012345671234567l;", "L", Convert.ToInt64("12345671234567", fromBase: 8), typeof(long));
            this.AssertLiteralSuffix("T x = 012345671234567ll;", "LL", Convert.ToInt64("12345671234567", fromBase: 8), typeof(long));
            this.AssertLiteralSuffix("T x = 012345671234567LL;", "LL", Convert.ToInt64("12345671234567", fromBase: 8), typeof(long));
            this.AssertLiteralSuffix("T x = 01ul;", "UL", 1UL, typeof(ulong));
            this.AssertLiteralSuffix("T x = 01Ul;", "UL", 1UL, typeof(ulong));
            this.AssertLiteralSuffix("T x = 077UL;", "UL", Convert.ToUInt64("77", fromBase: 8), typeof(ulong));
            this.AssertLiteralSuffix("T x = 012345671234567uL;", "UL", Convert.ToUInt64("12345671234567", fromBase: 8), typeof(ulong));
            this.AssertLiteralSuffix("T x = 01ull;", "ULL", 1UL, typeof(ulong));
            this.AssertLiteralSuffix("T x = 01Ull;", "ULL", 1UL, typeof(ulong));
            this.AssertLiteralSuffix("T x = 077ULL;", "ULL", Convert.ToUInt64("77", fromBase: 8), typeof(ulong));
            this.AssertLiteralSuffix("T x = 012345671234567uLL;", "ULL", Convert.ToUInt64("12345671234567", fromBase: 8), typeof(ulong));

            this.AssertLiteralSuffix("T x = 0x1u;", "U", 0x1U, typeof(uint));
            this.AssertLiteralSuffix("T x = 0xAFu;", "U", 0xAF, typeof(uint));
            this.AssertLiteralSuffix("T x = 0xFFFFFFFFu;", "U", 0xFFFFFFFF, typeof(uint));
            this.AssertLiteralSuffix("T x = 0xFFFFFFFFFFl;", "L", 0xFFFFFFFFFFL, typeof(long));
            this.AssertLiteralSuffix("T x = 0xAFl;", "L", 0xAFL, typeof(long));
            this.AssertLiteralSuffix("T x = 0xAFL;", "L", 0xAFL, typeof(long));
            this.AssertLiteralSuffix("T x = 0xFll;", "LL", 0xFUL, typeof(long));
            this.AssertLiteralSuffix("T x = 0xFLL;", "LL", 0xFUL, typeof(long));
            this.AssertLiteralSuffix("T x = 0xFFFFFFFFFFul;", "UL", 0xFFFFFFFFFFUL, typeof(ulong));
            this.AssertLiteralSuffix("T x = 0xFFFFFFFFFFuL;", "UL", 0xFFFFFFFFFFUL, typeof(ulong));
            this.AssertLiteralSuffix("T x = 0xFFFFFFFFFFUl;", "UL", 0xFFFFFFFFFFUL, typeof(ulong));
            this.AssertLiteralSuffix("T x = 0xFULL;", "ULL", 0xFUL, typeof(ulong));
            this.AssertLiteralSuffix("T x = 0xFuLL;", "ULL", 0xFUL, typeof(ulong));
            this.AssertLiteralSuffix("T x = 0xFUll;", "ULL", 0xFUL, typeof(ulong));
            this.AssertLiteralSuffix("T x = 0xFull;", "ULL", 0xFUL, typeof(ulong));
        }

        [Test]
        public void ArithmeticExpressionTest()
        {
            this.AssertInitializerValue("int x = 1 << (1 + 1 * 2) >> 3;", 1);
            this.AssertInitializerValue("float y = 2.3 + 4.0 / 2.0;", 4.3);
            this.AssertInitializerValue("double z = 3.3 + (4.1 - 1.1) * 2.0;", 9.3);
            this.AssertInitializerValue("double x = 1 << (1 + 1 * 2) >> 3;", 1);
            this.AssertInitializerValue("float y = 2.3 + 4 / 2;", 4.3);
            this.AssertInitializerValue("double z = 3.3 + (4.1 - 1.1) * 2;", 9.3);
        }

        [Test]
        public void ArithmeticBitwiseExpressionTest()
        {
            this.AssertInitializerValue("int x = 1 | ~0;", ~0);
            this.AssertInitializerValue("int x = 1 | ~1;", ~0);
            this.AssertInitializerValue("int x = 1 & ~0;", 1 & ~0);
            this.AssertInitializerValue("int x = (1 << 1) & ~0;", (1 << 1) & ~0);
            this.AssertInitializerValue("int x = (1 << 10 >> 2) ^ (~0 << 10);", (1 << 10 >> 2) ^ (~0 << 10));
        }

        [Test]
        public void RelationalExpressionTest()
        {
            this.AssertInitializerValue("bool a = 1 > 1;", false);
            this.AssertInitializerValue("bool b = 1 >= 1;", true);
            this.AssertInitializerValue("bool c = 2 > 1;", true);
            this.AssertInitializerValue("bool d = 3 < 1;", false);
            this.AssertInitializerValue("bool e = 1 != 1;", false);
            this.AssertInitializerValue("bool f = (1 + 1) == 2;", true);
            this.AssertInitializerValue("bool g = 1.1 > 1.0;", true);
            this.AssertInitializerValue("bool h = 1.101 >= 1.1;", true);
            this.AssertInitializerValue("bool i = (1 + 3 * 2) > 7;", false);
            this.AssertInitializerValue("bool j = 3.0 + 0.1 > 3.0;", true);
            this.AssertInitializerValue("bool k = 1.01 != 1.0;", true);
            this.AssertInitializerValue("bool l = (1 << 1) == 2;", true);
            this.AssertInitializerValue("bool m = (2 >> 1) == 1;", true);
        }

        [Test]
        public void LogicExpressionTest()
        {
            this.AssertInitializerValue("bool t = 1 || 1;", true);
            this.AssertInitializerValue("bool t = 1 || 0;", true);
            this.AssertInitializerValue("bool t = 0 || 0;", false);
            this.AssertInitializerValue("bool t = 0 && 0;", false);
            this.AssertInitializerValue("bool t = 0 && 1;", false);
            this.AssertInitializerValue("bool t = 1 && 0;", false);
            this.AssertInitializerValue("bool t = 1 && 1;", true);
            this.AssertInitializerValue("bool t = 0.0001 && 1.1;", true);
            this.AssertInitializerValue("bool a = 1 > 1 && 2 < 3;", false);
            this.AssertInitializerValue("bool b = 1 >= 1 && 2 < 3;", true);
            this.AssertInitializerValue("bool b = 1 >= 1 || 3 < 3;", true);
            this.AssertInitializerValue("bool b = 1 > 1 || 3 >= 3;", true);
            this.AssertInitializerValue("bool b = 1 > 1 || 3 > 3;", false);
            this.AssertInitializerValue("bool c = 2 > 1 && 1 != 2 && 2 <= 3;", true);
            this.AssertInitializerValue("bool c = 2 > 1 && 1 != 2 && 2 > 3;", false);
            this.AssertInitializerValue("bool d = 3 < 1 || 2 < 1 || 1 > 1;", false);
            this.AssertInitializerValue("bool d = 3 < 1 || 2 > 1 || 1 == 1;", true);
            this.AssertInitializerValue("bool e = 1 != 1 || 1 == 1;", true);
            this.AssertInitializerValue("bool f = (1 + 1) == 2 || 2 == 2;", true);
            this.AssertInitializerValue("bool g = 1.1 > 1.0 && 1.0 > 1.02;", false);
            this.AssertInitializerValue("bool g = 1.1 > 1.0 || 1.0 > 1.02;", true);
            this.AssertInitializerValue("bool h = 1.101 >= 1.1 && (7 > 3.2 || 2 > 3);", true);
            this.AssertInitializerValue("bool i = (1 + 3 * 2 > 3 && 4 != 2.0) && 8 > 7;", true);
            this.AssertInitializerValue("bool j = 1 != 0 && 1 > 1 || 1 == 1;", true);
            this.AssertInitializerValue("bool j = 1 != 0 && (1 == 1 || 1 == 3);", true);
            this.AssertInitializerValue("bool j = 1 != 0 && (1 != 1 || 1 == 3);", false);
            this.AssertInitializerValue("bool k = 3 > 2 || 3 > 1 && 1 > 1;", true);
            this.AssertInitializerValue("bool k = (3 > 2 || 3 > 1) && 1 > 1;", false);
            this.AssertInitializerValue("bool l = (1 << 1) == 2 && (3 / 2 == 1);", true);
            this.AssertInitializerValue("bool l = (1 << 1) == 2 && (3 / 2 != 1);", false);
            this.AssertInitializerValue("bool l = (1 << 1) == 2 || (3 / 2 != 1);", true);
            this.AssertInitializerValue("bool l = (1 << 2) == 2 || (3 / 2 != 1);", false);
            this.AssertInitializerValue("bool l = (1 << 1) == (4 >> 1) || 1 != 1;", true);
        }

        [Test]
        public void FunctionArgumentExpressionTest()
        {
            this.AssertParameterValues("void g() { g(); }");
            this.AssertParameterValues("void g() { g(3); }", 3);
            this.AssertParameterValues("void g() { g(3, 2); }", 3, 2);
            this.AssertParameterValues("void g() { g(3 + 1, 2 * 3); }", 3 + 1, 2 * 3);
            this.AssertParameterValues("void g() { g(((1 << 2) + 4) >> 3); }", ((1 << 2) + 4) >> 3);
            this.AssertParameterValues("void g() { g(1.1 > 1.0 && 1.0 > 1.02); }", false);
            this.AssertParameterValues("void g() { g(1.01 > 1.0 || 1.0 > 1.02); }", true);


        }

        [Test]
        public void FunctionReturnExpressionTest()
        {
            this.AssertReturnValue("int g() { return 3; }", 3);
            this.AssertReturnValue("int g() { return 3.3; }", 3.3);
            this.AssertReturnValue("int g() { return 3 + 1 - 2*3; }", -2);
            this.AssertReturnValue("int g() { return ((1 << 2) + 4) >> 3; }", ((1 << 2) + 4) >> 3);
            this.AssertReturnValue("int g() { return 1.1 > 1.0 && 1.0 > 1.02; }", false);
            this.AssertReturnValue("int g() { return 1.01 > 1.0 || 1.0 > 1.02; }", true);
        }

        [Test]
        public void UnaryExpressionTests()
        {
            Assert.That(this.AssertInitializer("int x = y++;"), Is.InstanceOf<IncrementExpressionNode>());
            Assert.That(this.AssertInitializer("int x = y--;"), Is.InstanceOf<DecrementExpressionNode>());
            Assert.That(this.AssertInitializer("int x = ++y;"), Is.InstanceOf<UnaryExpressionNode>());
            Assert.That(this.AssertInitializer("int x = --y;"), Is.InstanceOf<UnaryExpressionNode>());
            this.AssertInitializerValue("int x = ++1;", 2);
            this.AssertInitializerValue("int x = --1;", 0);
            this.AssertInitializerValue("int x = ~0;", ~0);
            this.AssertInitializerValue("int x = ~(~0);", 0);
            this.AssertInitializerValue("bool x = !0;", true);
            this.AssertInitializerValue("bool x = !(1 > 2);", true);
            this.AssertInitializerValue("bool x = !1;", false);
            this.AssertInitializerValue("bool x = !(1 != 0);", false);
            this.AssertInitializerValue("bool x = (!1) != (!1);", false);
            this.AssertInitializerValue("bool x = (!1) != 0;", false);
        }
    }
}
