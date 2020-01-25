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
            this.Evaluate<int>("int x = 3;", 3);
            this.Evaluate<double>("float y = 2.3;", 2.3);
            // TODO support chars properly
            this.Evaluate<string>("char c = 'a';", "'a'");
            this.Evaluate<string>("string s = \"abc\";", "abc");
        }

        [Test]
        public void ArithmeticExpressionTest()
        {
            this.Evaluate<int>("int x = 1 << (1 + 1 * 2) >> 3;", 1);
            this.Evaluate<double>("float y = 2.3 + 4.0 / 2.0;", 4.3);
            this.Evaluate<double>("double z = 3.3 + (4.1 - 1.1) * 2.0;", 9.3);
            this.Evaluate<int>("double x = 1 << (1 + 1 * 2) >> 3;", 1);
            this.Evaluate<double>("float y = 2.3 + 4 / 2;", 4.3);
            this.Evaluate<double>("double z = 3.3 + (4.1 - 1.1) * 2;", 9.3);
        }

        [Test]
        public void ArithmeticBitwiseExpressionTest()
        {
            this.Evaluate<int>("int x = 1 | ~0;", ~0);
            this.Evaluate<int>("int x = 1 | ~1;", ~0);
            this.Evaluate<int>("int x = 1 & ~0;", 1 & ~0);
            this.Evaluate<int>("int x = (1 << 1) & ~0;", (1 << 1) & ~0);
            this.Evaluate<int>("int x = (1 << 10 >> 2) ^ (~0 << 10);", (1 << 10 >> 2) ^ (~0 << 10));
        }

        [Test]
        public void RelationalExpressionTest()
        {
            this.Evaluate<bool>("bool a = 1 > 1;", false);
            this.Evaluate<bool>("bool b = 1 >= 1;", true);
            this.Evaluate<bool>("bool c = 2 > 1;", true);
            this.Evaluate<bool>("bool d = 3 < 1;", false);
            this.Evaluate<bool>("bool e = 1 != 1;", false);
            this.Evaluate<bool>("bool f = (1 + 1) == 2;", true);
            this.Evaluate<bool>("bool g = 1.1 > 1.0;", true);
            this.Evaluate<bool>("bool h = 1.101 >= 1.1;", true);
            this.Evaluate<bool>("bool i = (1 + 3 * 2) > 7;", false);
            this.Evaluate<bool>("bool j = 3.0 + 0.1 > 3.0;", true);
            this.Evaluate<bool>("bool k = 1.01 != 1.0;", true);
            this.Evaluate<bool>("bool l = (1 << 1) == 2;", true);
            this.Evaluate<bool>("bool m = (2 >> 1) == 1;", true);
        }

        [Test]
        public void LogicExpressionTest()
        {
            this.Evaluate<bool>("bool t = 1 || 1;", true);
            this.Evaluate<bool>("bool t = 1 || 0;", true);
            this.Evaluate<bool>("bool t = 0 || 0;", false);
            this.Evaluate<bool>("bool t = 0 && 0;", false);
            this.Evaluate<bool>("bool t = 0 && 1;", false);
            this.Evaluate<bool>("bool t = 1 && 0;", false);
            this.Evaluate<bool>("bool t = 1 && 1;", true);
            this.Evaluate<bool>("bool t = 0.0001 && 1.1;", true);
            this.Evaluate<bool>("bool a = 1 > 1 && 2 < 3;", false);
            this.Evaluate<bool>("bool b = 1 >= 1 && 2 < 3;", true);
            this.Evaluate<bool>("bool b = 1 >= 1 || 3 < 3;", true);
            this.Evaluate<bool>("bool b = 1 > 1 || 3 >= 3;", true);
            this.Evaluate<bool>("bool b = 1 > 1 || 3 > 3;", false);
            this.Evaluate<bool>("bool c = 2 > 1 && 1 != 2 && 2 <= 3;", true);
            this.Evaluate<bool>("bool c = 2 > 1 && 1 != 2 && 2 > 3;", false);
            this.Evaluate<bool>("bool d = 3 < 1 || 2 < 1 || 1 > 1;", false);
            this.Evaluate<bool>("bool d = 3 < 1 || 2 > 1 || 1 == 1;", true);
            this.Evaluate<bool>("bool e = 1 != 1 || 1 == 1;", true);
            this.Evaluate<bool>("bool f = (1 + 1) == 2 || 2 == 2;", true);
            this.Evaluate<bool>("bool g = 1.1 > 1.0 && 1.0 > 1.02;", false);
            this.Evaluate<bool>("bool g = 1.1 > 1.0 || 1.0 > 1.02;", true);
            this.Evaluate<bool>("bool h = 1.101 >= 1.1 && (7 > 3.2 || 2 > 3);", true);
            this.Evaluate<bool>("bool i = (1 + 3 * 2 > 3 && 4 != 2.0) && 8 > 7;", true);
            this.Evaluate<bool>("bool j = 1 != 0 && 1 > 1 || 1 == 1;", true);
            this.Evaluate<bool>("bool j = 1 != 0 && (1 == 1 || 1 == 3);", true);
            this.Evaluate<bool>("bool j = 1 != 0 && (1 != 1 || 1 == 3);", false);
            this.Evaluate<bool>("bool k = 3 > 2 || 3 > 1 && 1 > 1;", true);
            this.Evaluate<bool>("bool k = (3 > 2 || 3 > 1) && 1 > 1;", false);
            this.Evaluate<bool>("bool l = (1 << 1) == 2 && (3 / 2 == 1);", true);
            this.Evaluate<bool>("bool l = (1 << 1) == 2 && (3 / 2 != 1);", false);
            this.Evaluate<bool>("bool l = (1 << 1) == 2 || (3 / 2 != 1);", true);
            this.Evaluate<bool>("bool l = (1 << 2) == 2 || (3 / 2 != 1);", false);
            this.Evaluate<bool>("bool l = (1 << 1) == (4 >> 1) || 1 != 1;", true);
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
            this.Evaluate<int>("int x = ++1;", 2);
            this.Evaluate<int>("int x = --1;", 0);
            this.Evaluate<int>("int x = ~0;", ~0);
            this.Evaluate<int>("int x = ~(~0);", 0);
            this.Evaluate<bool>("bool x = !0;", true);
            this.Evaluate<bool>("bool x = !(1 > 2);", true);
            this.Evaluate<bool>("bool x = !1;", false);
            this.Evaluate<bool>("bool x = !(1 != 0);", false);
            this.Evaluate<bool>("bool x = (!1) != (!1);", false);
            this.Evaluate<bool>("bool x = (!1) != 0;", false);
        }


        private void Evaluate<T>(string decl, object expected)
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
    }
}
