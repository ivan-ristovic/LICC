using NUnit.Framework;
using RICC.AST.Nodes;

namespace RICC.Tests.AST
{
    internal sealed class EqualityTests
    {
        [Test]
        public void BasicEqualityTest()
        {
            ASTNode ast1 = new TranslationUnitNode(new BlockStatementNode(1));
            ASTNode ast2 = new TranslationUnitNode(new BlockStatementNode(100));
            AssertNodes(ast1, ast2, eq: true);
        }

        [Test]
        public void BasicDifferenceTest()
        {
            ASTNode ast1 = new TranslationUnitNode(new BlockStatementNode(1));
            ASTNode ast2 = new TranslationUnitNode(new VariableDeclaratorNode(10, new IdentifierNode(10, "x")));
            AssertNodes(ast1, ast2, eq: false);
        }

        [Test]
        public void ExtraNodesDifferenceTest()
        {
            ASTNode ast1 = new TranslationUnitNode(new BlockStatementNode(1));
            ASTNode ast2 = new TranslationUnitNode(new BlockStatementNode(1), new BlockStatementNode(2));
            AssertNodes(ast1, ast2, eq: false);
        }

        [Test]
        public void BasicChildrenEqualityTest()
        {
            ASTNode ast1 = new TranslationUnitNode(new VariableDeclaratorNode(10, new IdentifierNode(10, "x")));
            ASTNode ast2 = new TranslationUnitNode(new VariableDeclaratorNode(12, new IdentifierNode(12, "x")));
            AssertNodes(ast1, ast2, eq: true);
        }

        [Test]
        public void BasicChildrenDifferenceTest()
        {
            ASTNode ast1 = new TranslationUnitNode(new VariableDeclaratorNode(10, new IdentifierNode(10, "x")));
            ASTNode ast2 = new TranslationUnitNode(new VariableDeclaratorNode(10, new IdentifierNode(10, "y")));
            AssertNodes(ast1, ast2, eq: false);
        }

        [Test]
        public void VariableDeclarationEqualityTest()
        {
            ASTNode ast1 = new DeclarationStatementNode(
                1,
                new DeclarationSpecifiersNode(1, "public static", " int"),
                new DeclaratorListNode(
                    1,
                    new VariableDeclaratorNode(
                        2,
                        new IdentifierNode(2, "x"),
                        new ArithmeticExpressionNode(
                            3,
                            new LiteralNode(4, 1),
                            new ArithmeticOperatorNode(4, "+", (a, _) => a),
                            new LiteralNode(4, 1)
                        )
                    )
                )
            );
            ASTNode ast2 = new DeclarationStatementNode(
                2,
                new DeclarationSpecifiersNode(2, "public static", "int"),
                new DeclaratorListNode(
                    1,
                    new VariableDeclaratorNode(
                        2,
                        new IdentifierNode(2, "x"),
                        new ArithmeticExpressionNode(
                            3,
                            new LiteralNode(4, 1),
                            new ArithmeticOperatorNode(4, "+", (_, b) => b),
                            new LiteralNode(4, 1)
                        )
                    )
                )
            );
            AssertNodes(ast1, ast2, eq: true);
        }

        [Test]
        public void VariableDeclarationDifferenceTest1()
        {
            ASTNode ast1 = new DeclarationStatementNode(
                1,
                new DeclarationSpecifiersNode(1, "public static", "int"),
                new DeclaratorListNode(
                    1,
                    new VariableDeclaratorNode(
                        2,
                        new IdentifierNode(2, "x"),
                        new ArithmeticExpressionNode(
                            3,
                            new LiteralNode(4, 2),
                            new ArithmeticOperatorNode(4, "+", (a, _) => a),
                            new LiteralNode(4, 1)
                        )
                    )
                )
            );
            ASTNode ast2 = new DeclarationStatementNode(
                2,
                new DeclarationSpecifiersNode(2, "public static", "int"),
                new DeclaratorListNode(
                    1,
                    new VariableDeclaratorNode(
                        2,
                        new IdentifierNode(2, "x"),
                        new ArithmeticExpressionNode(
                            3,
                            new LiteralNode(4, 1),
                            new ArithmeticOperatorNode(4, "+", (_, b) => b),
                            new LiteralNode(4, 1)
                        )
                    )
                )
            );
            AssertNodes(ast1, ast2, eq: false);
        }

        [Test]
        public void VariableDeclarationDifferenceTest2()
        {
            ASTNode ast1 = new DeclarationStatementNode(
                1,
                new DeclarationSpecifiersNode(1, "public static", "int"),
                new DeclaratorListNode(
                    1,
                    new VariableDeclaratorNode(
                        2,
                        new IdentifierNode(2, "x"),
                        new ArithmeticExpressionNode(
                            3,
                            new LiteralNode(4, 1),
                            new ArithmeticOperatorNode(4, "-", (a, _) => a),
                            new LiteralNode(4, 1)
                        )
                    )
                )
            );
            ASTNode ast2 = new DeclarationStatementNode(
                2,
                new DeclarationSpecifiersNode(2, "public static", "int"),
                new DeclaratorListNode(
                    1,
                    new VariableDeclaratorNode(
                        2,
                        new IdentifierNode(2, "x"),
                        new ArithmeticExpressionNode(
                            3,
                            new LiteralNode(4, 1),
                            new ArithmeticOperatorNode(4, "+", (_, b) => b),
                            new LiteralNode(4, 1)
                        )
                    )
                )
            );
            AssertNodes(ast1, ast2, eq: false);
        }

        [Test]
        public void VariableDeclarationDifferenceTest3()
        {
            ASTNode ast1 = new DeclarationStatementNode(
                1,
                new DeclarationSpecifiersNode(1, "static", "int"),
                new DeclaratorListNode(
                    1,
                    new VariableDeclaratorNode(
                        2,
                        new IdentifierNode(2, "x"),
                        new ArithmeticExpressionNode(
                            3,
                            new LiteralNode(4, 1),
                            new ArithmeticOperatorNode(4, "+", (a, _) => a),
                            new LiteralNode(4, 1)
                        )
                    )
                )
            );
            ASTNode ast2 = new DeclarationStatementNode(
                2,
                new DeclarationSpecifiersNode(2, "public static", "int"),
                new DeclaratorListNode(
                    1,
                    new VariableDeclaratorNode(
                        2,
                        new IdentifierNode(2, "x"),
                        new ArithmeticExpressionNode(
                            3,
                            new LiteralNode(4, 1),
                            new ArithmeticOperatorNode(4, "+", (_, b) => b),
                            new LiteralNode(4, 1)
                        )
                    )
                )
            );
            AssertNodes(ast1, ast2, eq: false);
        }

        [Test]
        public void VariableDeclarationDifferenceTest4()
        {
            ASTNode ast1 = new DeclarationStatementNode(
                1,
                new DeclarationSpecifiersNode(1, "public static", "float"),
                new DeclaratorListNode(
                    1,
                    new VariableDeclaratorNode(
                        2,
                        new IdentifierNode(2, "x"),
                        new ArithmeticExpressionNode(
                            3,
                            new LiteralNode(4, 1),
                            new ArithmeticOperatorNode(4, "+", (a, _) => a),
                            new LiteralNode(4, 1)
                        )
                    )
                )
            );
            ASTNode ast2 = new DeclarationStatementNode(
                2,
                new DeclarationSpecifiersNode(2, "public static", "int"),
                new DeclaratorListNode(
                    1,
                    new VariableDeclaratorNode(
                        2,
                        new IdentifierNode(2, "x"),
                        new ArithmeticExpressionNode(
                            3,
                            new LiteralNode(4, 1),
                            new ArithmeticOperatorNode(4, "+", (_, b) => b),
                            new LiteralNode(4, 1)
                        )
                    )
                )
            );
            AssertNodes(ast1, ast2, eq: false);
        }

        // TODO func def tests


        private static void AssertNodes(ASTNode ast1, ASTNode ast2, bool eq = true)
        {
            Assert.That(ast1, eq ? Is.EqualTo(ast2) : Is.Not.EqualTo(ast2));
            Assert.That(ast1 == ast2, Is.EqualTo(eq));
            Assert.That(ast1 != ast2, Is.EqualTo(!eq));
        }
    }
}
