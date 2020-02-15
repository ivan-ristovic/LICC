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

        [Test]
        public void FunctionDefenitionEqualityTest()
        {
            ASTNode ast1 = new FunctionDefinitionNode(
                1,
                new DeclarationSpecifiersNode(2, "public static", "void"),
                new FunctionDeclaratorNode(
                    2, 
                    new IdentifierNode(2, "f"), 
                    new FunctionParametersNode(
                        2,
                        new FunctionParameterNode(
                            3, 
                            new DeclarationSpecifiersNode(3, "const", "time_t"),
                            new ArrayDeclaratorNode(3, new IdentifierNode(3, "arr"))
                        )
                    )
                ),
                new BlockStatementNode(3, new JumpStatementNode(4, new LiteralNode(4, "2", "u")))
            );
            ASTNode ast2 = new FunctionDefinitionNode(
                1,
                new DeclarationSpecifiersNode(3, "public static", "void"),
                new FunctionDeclaratorNode(
                    3,
                    new IdentifierNode(2, "f"),
                    new FunctionParametersNode(
                        4,
                        new FunctionParameterNode(
                            4,
                            new DeclarationSpecifiersNode(5, "const", "time_t"),
                            new ArrayDeclaratorNode(6, new IdentifierNode(6, "arr"))
                        )
                    )
                ),
                new BlockStatementNode(7, new JumpStatementNode(8, new LiteralNode(8, "2", "u")))
            );
            AssertNodes(ast1, ast2, eq: true);
        }

        [Test]
        public void FunctionDefenitionDifferenceTest1()
        {
            ASTNode ast1 = new FunctionDefinitionNode(
                1,
                new DeclarationSpecifiersNode(2, "public static", "void"),
                new FunctionDeclaratorNode(
                    2,
                    new IdentifierNode(2, "f"),
                    new FunctionParametersNode(
                        2,
                        new FunctionParameterNode(
                            3,
                            new DeclarationSpecifiersNode(3, "", "time_t"),
                            new ArrayDeclaratorNode(3, new IdentifierNode(3, "arr"))
                        )
                    )
                ),
                new BlockStatementNode(3, new JumpStatementNode(4, new LiteralNode(4, "2", "u")))
            );
            ASTNode ast2 = new FunctionDefinitionNode(
                1,
                new DeclarationSpecifiersNode(3, "public static", "void"),
                new FunctionDeclaratorNode(
                    3,
                    new IdentifierNode(2, "f"),
                    new FunctionParametersNode(
                        4,
                        new FunctionParameterNode(
                            4,
                            new DeclarationSpecifiersNode(5, "const", "time_t"),
                            new ArrayDeclaratorNode(6, new IdentifierNode(6, "arr"))
                        )
                    )
                ),
                new BlockStatementNode(7, new JumpStatementNode(8, new LiteralNode(8, "2", "u")))
            );
            AssertNodes(ast1, ast2, eq: false);
        }

        [Test]
        public void FunctionDefenitionDifferenceTest2()
        {
            ASTNode ast1 = new FunctionDefinitionNode(
                1,
                new DeclarationSpecifiersNode(2, "public static", "void"),
                new FunctionDeclaratorNode(
                    2,
                    new IdentifierNode(2, "f"),
                    new FunctionParametersNode(
                        2,
                        new FunctionParameterNode(
                            3,
                            new DeclarationSpecifiersNode(3, "const", "time_t"),
                            new VariableDeclaratorNode(3, new IdentifierNode(3, "arr"))
                        )
                    )
                ),
                new BlockStatementNode(3, new JumpStatementNode(4, new LiteralNode(4, "2", "u")))
            );
            ASTNode ast2 = new FunctionDefinitionNode(
                1,
                new DeclarationSpecifiersNode(3, "public static", "void"),
                new FunctionDeclaratorNode(
                    3,
                    new IdentifierNode(2, "f"),
                    new FunctionParametersNode(
                        4,
                        new FunctionParameterNode(
                            4,
                            new DeclarationSpecifiersNode(5, "const", "time_t"),
                            new ArrayDeclaratorNode(6, new IdentifierNode(6, "arr"))
                        )
                    )
                ),
                new BlockStatementNode(7, new JumpStatementNode(8, new LiteralNode(8, "2", "u")))
            );
            AssertNodes(ast1, ast2, eq: false);
        }

        [Test]
        public void FunctionDefenitionDifferenceTest3()
        {
            ASTNode ast1 = new FunctionDefinitionNode(
                1,
                new DeclarationSpecifiersNode(2, "public static", "void"),
                new FunctionDeclaratorNode(
                    2,
                    new IdentifierNode(2, "f"),
                    new FunctionParametersNode(
                        2,
                        new FunctionParameterNode(
                            3,
                            new DeclarationSpecifiersNode(3, "const", "time_t"),
                            new ArrayDeclaratorNode(3, new IdentifierNode(3, "arr"))
                        )
                    )
                ),
                new BlockStatementNode(3, new JumpStatementNode(4, new LiteralNode(4, "2", "u")))
            );
            ASTNode ast2 = new FunctionDefinitionNode(
                1,
                new DeclarationSpecifiersNode(3, "public static", "int"),
                new FunctionDeclaratorNode(
                    3,
                    new IdentifierNode(2, "f"),
                    new FunctionParametersNode(
                        4,
                        new FunctionParameterNode(
                            4,
                            new DeclarationSpecifiersNode(5, "const", "time_t"),
                            new ArrayDeclaratorNode(6, new IdentifierNode(6, "arr"))
                        )
                    )
                ),
                new BlockStatementNode(7, new JumpStatementNode(8, new LiteralNode(8, "2", "l")))
            );
            AssertNodes(ast1, ast2, eq: false);
        }

        [Test]
        public void FunctionDefenitionDifferenceTest4()
        {
            ASTNode ast1 = new FunctionDefinitionNode(
                1,
                new DeclarationSpecifiersNode(2, "public static", "void"),
                new FunctionDeclaratorNode(
                    2,
                    new IdentifierNode(2, "f"),
                    new FunctionParametersNode(
                        2,
                        new FunctionParameterNode(
                            3,
                            new DeclarationSpecifiersNode(3, "", "time_t"),
                            new ArrayDeclaratorNode(3, new IdentifierNode(3, "arr"))
                        )
                    )
                ),
                new BlockStatementNode(3, new JumpStatementNode(4, new LiteralNode(4, "2", "u")))
            );
            ASTNode ast2 = new FunctionDefinitionNode(
                1,
                new DeclarationSpecifiersNode(3, "public static", "void"),
                new FunctionDeclaratorNode(
                    3,
                    new IdentifierNode(2, "f"),
                    new FunctionParametersNode(
                        4,
                        new FunctionParameterNode(
                            4,
                            new DeclarationSpecifiersNode(5, "const", "time_t"),
                            new ArrayDeclaratorNode(6, new IdentifierNode(6, "arr"))
                        )
                    )
                ),
                new BlockStatementNode(7, new JumpStatementNode(8, new LiteralNode(8, "2", "u")), new EmptyStatementNode(10))
            );
            AssertNodes(ast1, ast2, eq: false);
        }


        private static void AssertNodes(ASTNode ast1, ASTNode ast2, bool eq = true)
        {
            Assert.That(ast1, eq ? Is.EqualTo(ast2) : Is.Not.EqualTo(ast2));
            Assert.That(ast1 == ast2, Is.EqualTo(eq));
            Assert.That(ast1 != ast2, Is.EqualTo(!eq));
        }
    }
}
