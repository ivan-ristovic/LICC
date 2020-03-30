using System.Collections.Generic;
using NUnit.Framework;
using RICC.AST.Builders.Pseudo;
using RICC.AST.Nodes;
using RICC.AST.Visitors;
using RICC.Exceptions;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace RICC.Tests.AST.Visitors
{
    internal sealed class ExpressionEvaluatorTests
    {
        private readonly Dictionary<string, Expr> symbols = new Dictionary<string, Expr> {
            { "one" , 1 },
            { "two" , 2 },
            { "twoo" , Expr.Variable("two") },
            { "four" , Expr.Parse("twoo + 2") },
            { "cycle1" , Expr.Variable("cycle1") },
            { "cycle2" , Expr.Variable("cycle2") },
        };


        [Test]
        public void ConstantExpressionTests()
        {
            Assert.That(this.Evaluate("4"), Is.EqualTo("4"));
            Assert.That(this.Evaluate("1 + 3"), Is.EqualTo("4"));
            Assert.That(this.Evaluate("1 + (3*2)"), Is.EqualTo("7"));
            Assert.That(this.Evaluate("(1/1) + (3*2)"), Is.EqualTo("7"));
            Assert.That(this.Evaluate("(1/1) - (1 + 1) + (3*2)"), Is.EqualTo("5"));
            Assert.That(this.Evaluate("(1 + 3)*2 - 1"), Is.EqualTo("7"));
            Assert.That(this.Evaluate("(1 + 3)*3 - 1"), Is.EqualTo("11"));
        }

        [Test]
        public void VariableExpressionTests()
        {
            Assert.That(this.Evaluate("1 + a"), Is.EqualTo("1 + a"));
            Assert.That(this.Evaluate("a + 1"), Is.EqualTo("1 + a"));
            Assert.That(this.Evaluate("a * 2 + 1"), Is.EqualTo("1 + 2*a"));
            Assert.That(this.Evaluate("a + a + 1"), Is.EqualTo("1 + 2*a"));
            Assert.That(this.Evaluate("1 + one"), Is.EqualTo("2"));
            Assert.That(this.Evaluate("two + 1"), Is.EqualTo("3"));
            Assert.That(this.Evaluate("two * 2 + 1"), Is.EqualTo("5"));
            Assert.That(this.Evaluate("twoo + four + 1"), Is.EqualTo("7"));
            Assert.That(this.Evaluate("twoo + four + five"), Is.EqualTo("6 + five"));
        }

        [Test]
        public void InfiniteCycleTest()
        {
            Assert.That(() => this.Evaluate("cycle1"), Throws.InstanceOf<EvaluationException>());
        }


        private string Evaluate(string expr)
            => ExpressionEvaluator.TryEvaluate(this.GenerateAST(expr), this.symbols).ToString();

        private ExpressionNode GenerateAST(string src)
            => new PseudoASTBuilder().BuildFromSource(src, p => p.exp()).As<ExpressionNode>();
    }
}
