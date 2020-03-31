using NUnit.Framework;
using RICC.AST.Nodes;
using RICC.AST.Nodes.Common;

namespace RICC.Tests.AST
{
    internal sealed class ASTNodeSubstituteTests
    {
        [Test]
        public void BasicTest()
        {
            var ast1 = new IdentifierListNode(1, new IdentifierNode(1, "x"), new IdentifierNode(1, "y"));
            var ast2 = new IdentifierListNode(1, new IdentifierNode(1, "X"), new IdentifierNode(1, "y"));
            Assert.That(ast1.Substitute(new IdentifierNode(2, "x"), new IdentifierNode(2, "X")), Is.EqualTo(ast2));
        }

        [Test]
        public void MultipleSubstituteTest()
        {
            var ast1 = new IdentifierListNode(1, new IdentifierNode(1, "x"), new IdentifierNode(1, "x"), new IdentifierNode(1, "y"));
            var ast2 = new IdentifierListNode(1, new IdentifierNode(1, "X"), new IdentifierNode(1, "X"), new IdentifierNode(1, "y"));
            Assert.That(ast1.Substitute(new IdentifierNode(2, "x"), new IdentifierNode(2, "X")), Is.EqualTo(ast2));
        }

        [Test]
        public void ExpressionSubstituteTest()
        {
            var ast1 = new ArithmeticExpressionNode(1,
                new IdentifierNode(1, "x"),
                ArithmeticOperatorNode.FromSymbol(1, "+"),
                new IdentifierNode(1, "x")
            );
            var ast2 = new ArithmeticExpressionNode(1,
                new LiteralNode(1, 1),
                ArithmeticOperatorNode.FromSymbol(1, "+"),
                new LiteralNode(1, 1)
            );
            Assert.That(ast1.Substitute(new IdentifierNode(2, "x"), new LiteralNode(2, 1)), Is.EqualTo(ast2));
        }

        [Test]
        public void SelfReferenceSubstituteTest()
        {
            var ast1 = new ArithmeticExpressionNode(1,
                new IdentifierNode(1, "x"),
                ArithmeticOperatorNode.FromSymbol(1, "+"),
                new ArithmeticExpressionNode(1,
                    new IdentifierNode(1, "y"),
                    ArithmeticOperatorNode.FromSymbol(1, "+"),
                    new IdentifierNode(1, "x")
                )
            );

            var repl = new ArithmeticExpressionNode(1,
                new IdentifierNode(1, "y"),
                new ArithmeticOperatorNode(1, "-", BinaryOperations.ArithmeticFromSymbol("-")),
                new IdentifierNode(1, "x")
            );
            var ast2 = new ArithmeticExpressionNode(1,
                new IdentifierNode(1, "x"),
                ArithmeticOperatorNode.FromSymbol(1, "+"),
                new ArithmeticExpressionNode(1,
                    repl,
                    ArithmeticOperatorNode.FromSymbol(1, "+"),
                    new IdentifierNode(1, "x")
                )
            );

            Assert.That(ast1.Substitute(new IdentifierNode(2, "y"), repl), Is.EqualTo(ast2));
        }

        [Test]
        public void SubstituteKeepsOriginalTest()
        {
            ASTNode ast1 = new IdentifierListNode(1, new IdentifierNode(1, "x"), new IdentifierNode(1, "y"));
            ASTNode ast2 = new IdentifierListNode(1, new IdentifierNode(1, "X"), new IdentifierNode(1, "y"));
            Assert.That(ast1.Substitute(new IdentifierNode(2, "x"), new IdentifierNode(2, "X")), Is.EqualTo(ast2));
            Assert.That(ast1, Is.EqualTo(new IdentifierListNode(1, new IdentifierNode(1, "x"), new IdentifierNode(1, "y"))));
        }
    }
}
