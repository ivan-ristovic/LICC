using NUnit.Framework;
using RICC.AST.Nodes;
using RICC.AST.Nodes.Common;
using RICC.AST.Visitors;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace RICC.Tests.AST.Visitors
{
    internal sealed class SymbolicExpressionBuilderTests
    {
        [Test]
        public void ConstantTests()
        {
            this.AssertParse(new LiteralNode(1, 3), Expr.Parse("3"));
            this.AssertParse(new LiteralNode(1, 4.3), Expr.Parse("4.3"));
            this.AssertParse(new NullLiteralNode(1), Expr.Undefined);
        }

        [Test]
        public void VariableTests()
        {
            this.AssertParse(new IdentifierNode(1, "x"), Expr.Variable("x"));
            this.AssertParse(new IdentifierNode(1, "xyz"), Expr.Variable("xyz"));
        }

        [Test]
        public void ArithmeticExpressionTests()
        {
            this.AssertParse(
                new ArithmeticExpressionNode(1,
                    new LiteralNode(1, 3),
                    ArithmeticOperatorNode.FromSymbol(1, "+"),
                    new LiteralNode(1, 1)
                ),
                Expr.Parse("4")
            );

            this.AssertParse(
                new ArithmeticExpressionNode(1,
                    new IdentifierNode(1, "x"),
                    ArithmeticOperatorNode.FromSymbol(1, "+"),
                    new LiteralNode(1, 1)
                ),
                Expr.Parse("1 + x")
            );
            
            this.AssertParse(
                new ArithmeticExpressionNode(1,
                    new IdentifierNode(1, "x"),
                    ArithmeticOperatorNode.FromSymbol(1, "+"),
                    new ArithmeticExpressionNode(1,
                        new IdentifierNode(1, "x"),
                        ArithmeticOperatorNode.FromSymbol(1, "-"),
                        new LiteralNode(1, 1)
                    )
                ),
                Expr.Parse("-1 + 2*x")
            );
            
            this.AssertWildcardParse(
                new ArithmeticExpressionNode(1,
                    new IdentifierNode(1, "x"),
                    ArithmeticOperatorNode.FromSymbol(1, "+"),
                    new ArithmeticExpressionNode(1,
                        new IdentifierNode(1, "x"),
                        ArithmeticOperatorNode.FromBitwiseSymbol(1, "<<"),
                        new LiteralNode(1, 1)
                    )
                ),
                "? + x"
            );
        }

        [Test]
        public void LogicExpressionTests()
        {
            this.AssertParse(
                new LogicExpressionNode(1,
                    new LiteralNode(1, true),
                    BinaryLogicOperatorNode.FromSymbol(1, "&&"),
                    new LiteralNode(1, false)
                ),
                "False"
            );

            this.AssertParse(
                new LogicExpressionNode(1,
                    new LiteralNode(1, true),
                    BinaryLogicOperatorNode.FromSymbol(1, "&&"),
                    new LogicExpressionNode(1,
                        new LiteralNode(1, false),
                        BinaryLogicOperatorNode.FromSymbol(1, "||"),
                        new LiteralNode(1, true)
                    )
                ),
                "True"
            );

            this.AssertWildcardParse(
                new LogicExpressionNode(1,
                    new IdentifierNode(1, "x"),
                    BinaryLogicOperatorNode.FromSymbol(1, "&&"),
                    new LiteralNode(1, 1)
                ),
                "?"
            );
        }


        private void AssertParse(ASTNode node, Expr expected)
            => this.AssertParse(node, expected.ToString());

        private void AssertParse(ASTNode node, string expected)
            => Assert.That(this.Parse(node).ToString(), Is.EqualTo(expected));

        private void AssertWildcardParse(ASTNode node, string expected)
        {
            string wildcardExpr = SymbolicExpressionBuilder.WildcardReplace(this.Parse(node).ToString());
            Assert.That(wildcardExpr, Is.EqualTo(expected.ToString()));
        }

        private Expr Parse(ASTNode node) 
            => new SymbolicExpressionBuilder(node).Parse();
    }
}
