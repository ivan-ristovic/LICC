using NUnit.Framework;
using LICC.AST.Nodes;
using LICC.AST.Nodes.Common;
using LICC.Core;
using LICC.Core.Common;

namespace LICC.Tests.Core.Comparer
{
    internal sealed class BlockTests : ComparerTestsBase
    {
        [Test]
        public void BasicVariableAssignmentTests()
        {
            this.Compare(
                new SourceComponentNode(
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "x"), new LiteralNode(1, 1)))
                    ),
                    new ExpressionStatementNode(1,
                        new AssignmentExpressionNode(1,
                            new IdentifierNode(1, "x"),
                            AssignmentOperatorNode.FromSymbol(1, "="),
                            new LiteralNode(1, 2)
                        )
                    )
                ),
                new SourceComponentNode(
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "x"), new LiteralNode(1, 1)))
                    ),
                    new ExpressionStatementNode(1,
                        new AssignmentExpressionNode(1,
                            new IdentifierNode(1, "x"),
                            AssignmentOperatorNode.FromSymbol(1, "="),
                            new ArithmeticExpressionNode(1,
                                new IdentifierNode(1, "x"),
                                ArithmeticOperatorNode.FromSymbol(1, "+"),
                                new LiteralNode(1, 1)
                            )
                        )
                    )
                )
            );

            this.Compare(
                new SourceComponentNode(
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "x"), new LiteralNode(1, 1)))
                    ),
                    new ExpressionStatementNode(1,
                        new AssignmentExpressionNode(1,
                            new IdentifierNode(1, "x"),
                            AssignmentOperatorNode.FromSymbol(1, "="),
                            new LiteralNode(1, 3)
                        )
                    )
                ),
                new SourceComponentNode(
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "x"), new LiteralNode(1, 1)))
                    ),
                    new ExpressionStatementNode(1,
                        new AssignmentExpressionNode(1,
                            new IdentifierNode(1, "x"),
                            AssignmentOperatorNode.FromSymbol(1, "="),
                            new ArithmeticExpressionNode(1,
                                new IdentifierNode(1, "x"),
                                ArithmeticOperatorNode.FromSymbol(1, "+"),
                                new LiteralNode(1, 1)
                            )
                        )
                    )
                ),
                new MatchIssues()
                    .AddError(new BlockEndValueMismatchError("x", 1, "3", "2"))
            );

            this.Compare(
                new SourceComponentNode(
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "x"), new IdentifierNode(1, "p")))
                    ),
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "y"), new IdentifierNode(1, "x")))
                    ),
                    new ExpressionStatementNode(1,
                        new AssignmentExpressionNode(1,
                            new IdentifierNode(1, "y"),
                            AssignmentOperatorNode.FromSymbol(1, "="),
                            new ArithmeticExpressionNode(1,
                                new IdentifierNode(1, "x"),
                                ArithmeticOperatorNode.FromSymbol(1, "+"),
                                new LiteralNode(1, 1)
                            )
                        )
                    ),
                    new ExpressionStatementNode(1,
                        new AssignmentExpressionNode(1,
                            new IdentifierNode(1, "x"),
                            AssignmentOperatorNode.FromSymbol(1, "="),
                            new ArithmeticExpressionNode(1,
                                new IdentifierNode(1, "y"),
                                ArithmeticOperatorNode.FromSymbol(1, "+"),
                                new IdentifierNode(1, "y")
                            )
                        )
                    )
                ),
                new SourceComponentNode(
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "x"), new IdentifierNode(1, "p")))
                    ),
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "y"), new IdentifierNode(1, "x")))
                    ),
                    new ExpressionStatementNode(1,
                        new AssignmentExpressionNode(1,
                            new IdentifierNode(1, "y"),
                            AssignmentOperatorNode.FromSymbol(1, "="),
                            new ArithmeticExpressionNode(1,
                                new IdentifierNode(1, "x"),
                                ArithmeticOperatorNode.FromSymbol(1, "+"),
                                new LiteralNode(1, 2)
                            )
                        )
                    ),
                    new ExpressionStatementNode(1,
                        new AssignmentExpressionNode(1,
                            new IdentifierNode(1, "x"),
                            AssignmentOperatorNode.FromSymbol(1, "="),
                            new ArithmeticExpressionNode(1,
                                new LiteralNode(1, 3),
                                ArithmeticOperatorNode.FromSymbol(1, "*"),
                                new IdentifierNode(1, "y")
                            )
                        )
                    )
                ),
                new MatchIssues()
                    .AddError(new BlockEndValueMismatchError("x", 1, "2*y", "3*y"))
                    .AddError(new BlockEndValueMismatchError("y", 1, "1 + x", "2 + x"))
            );
        }

        [Test]
        public void MultipleVariableAssignmentTests()
        {
            this.Compare(
                new SourceComponentNode(
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "x"), new LiteralNode(1, 1)))
                    ),
                    new ExpressionStatementNode(1,
                        new ExpressionListNode(1,
                            new AssignmentExpressionNode(1,
                                new IdentifierNode(1, "x"),
                                AssignmentOperatorNode.FromSymbol(1, "="),
                                new LiteralNode(1, 2)
                            ),
                            new AssignmentExpressionNode(1,
                                new IdentifierNode(1, "x"),
                                AssignmentOperatorNode.FromSymbol(1, "="),
                                new LiteralNode(1, 3)
                            )
                        )
                    )
                ),
                new SourceComponentNode(
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "x"), new LiteralNode(1, 1)))
                    ),
                    new ExpressionStatementNode(1,
                        new AssignmentExpressionNode(1,
                            new IdentifierNode(1, "x"),
                            AssignmentOperatorNode.FromSymbol(1, "="),
                            new ArithmeticExpressionNode(1,
                                new IdentifierNode(1, "x"),
                                ArithmeticOperatorNode.FromSymbol(1, "+"),
                                new LiteralNode(1, 2)
                            )
                        )
                    )
                )
            );

            this.Compare(
                new SourceComponentNode(
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "x"), new LiteralNode(1, 1)))
                    ),
                    new ExpressionStatementNode(1,
                        new ExpressionListNode(1,
                            new AssignmentExpressionNode(1,
                                new IdentifierNode(1, "x"),
                                AssignmentOperatorNode.FromSymbol(1, "="),
                                new LiteralNode(1, 2)
                            ),
                            new AssignmentExpressionNode(1,
                                new IdentifierNode(1, "x"),
                                AssignmentOperatorNode.FromSymbol(1, "="),
                                new LiteralNode(1, 3)
                            )
                        )
                    )
                ),
                new SourceComponentNode(
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "x"), new LiteralNode(1, 1)))
                    ),
                    new ExpressionStatementNode(1,
                        new AssignmentExpressionNode(1,
                            new IdentifierNode(1, "x"),
                            AssignmentOperatorNode.FromSymbol(1, "="),
                            new ArithmeticExpressionNode(1,
                                new IdentifierNode(1, "x"),
                                ArithmeticOperatorNode.FromSymbol(1, "+"),
                                new LiteralNode(1, 1)
                            )
                        )
                    )
                ),
                new MatchIssues()
                    .AddError(new BlockEndValueMismatchError("x", 1, "3", "2"))
            );

            this.Compare(
                new SourceComponentNode(
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "x"), new LiteralNode(1, 1)))
                    ),
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "y"), new LiteralNode(1, 2)))
                    )
                ),
                new SourceComponentNode(
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "x"), new LiteralNode(1, 1)))
                    ),
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "y"), new LiteralNode(1, 2)))
                    ),
                    new ExpressionStatementNode(1,
                        new ExpressionListNode(1,
                            new AssignmentExpressionNode(1,
                                new IdentifierNode(1, "x"),
                                AssignmentOperatorNode.FromSymbol(1, "="),
                                new LiteralNode(1, 2)
                            ),
                            new AssignmentExpressionNode(1,
                                new IdentifierNode(1, "x"),
                                AssignmentOperatorNode.FromSymbol(1, "="),
                                new LiteralNode(1, 1)
                            ),
                            new AssignmentExpressionNode(1,
                                new IdentifierNode(1, "y"),
                                AssignmentOperatorNode.FromSymbol(1, "="),
                                new LiteralNode(1, 1)
                            ),
                            new AssignmentExpressionNode(1,
                                new IdentifierNode(1, "y"),
                                AssignmentOperatorNode.FromSymbol(1, "="),
                                new LiteralNode(1, 2)
                            )
                        )
                    )
                )
            );
        }

        [Test]
        public void ComplexAssignmentTests()
        {
            this.Compare(
                new SourceComponentNode(
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "x"), new LiteralNode(1, 1)))
                    ),
                    new ExpressionStatementNode(1,
                        new ExpressionListNode(1,
                            new AssignmentExpressionNode(1,
                                new IdentifierNode(1, "x"),
                                AssignmentOperatorNode.FromSymbol(1, "+="),
                                new LiteralNode(1, 2)
                            ),
                            new AssignmentExpressionNode(1,
                                new IdentifierNode(1, "x"),
                                AssignmentOperatorNode.FromSymbol(1, "+="),
                                new LiteralNode(1, 3)
                            )
                        )
                    )
                ),
                new SourceComponentNode(
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "x"), new LiteralNode(1, 1)))
                    ),
                    new ExpressionStatementNode(1,
                        new AssignmentExpressionNode(1,
                            new IdentifierNode(1, "x"),
                            AssignmentOperatorNode.FromSymbol(1, "+="),
                            new ArithmeticExpressionNode(1,
                                new IdentifierNode(1, "x"),
                                ArithmeticOperatorNode.FromSymbol(1, "+"),
                                new LiteralNode(1, 4)
                            )
                        )
                    )
                )
            );

            this.Compare(
                new SourceComponentNode(
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "x"), new LiteralNode(1, 1)))
                    ),
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "y"), new LiteralNode(1, 1)))
                    ),
                    new ExpressionStatementNode(1,
                        new AssignmentExpressionNode(1,
                            new IdentifierNode(1, "x"),
                            AssignmentOperatorNode.FromSymbol(1, "+="),
                            new IdentifierNode(1, "y")
                        )
                    )
                ),
                new SourceComponentNode(
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "x"), new LiteralNode(1, 1)))
                    ),
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "y"), new LiteralNode(1, 1)))
                    ),
                    new ExpressionStatementNode(1,
                        new AssignmentExpressionNode(1,
                            new IdentifierNode(1, "x"),
                            AssignmentOperatorNode.FromSymbol(1, "-="),
                            new UnaryExpressionNode(1,
                                UnaryOperatorNode.FromSymbol(1, "-"),
                                new IdentifierNode(1, "y")
                            )
                        )
                    )
                )
            );

            this.Compare(
                new SourceComponentNode(
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "x"), new LiteralNode(1, 1)))
                    ),
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "y"), new LiteralNode(1, 1)))
                    ),
                    new ExpressionStatementNode(1,
                        new AssignmentExpressionNode(1,
                            new IdentifierNode(1, "x"),
                            AssignmentOperatorNode.FromSymbol(1, "+="),
                            new IdentifierNode(1, "y")
                        )
                    )
                ),
                new SourceComponentNode(
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "x"), new LiteralNode(1, 1)))
                    ),
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "y"), new LiteralNode(1, 1)))
                    ),
                    new ExpressionStatementNode(1,
                        new ExpressionListNode(1,
                            new AssignmentExpressionNode(1,
                                new IdentifierNode(1, "x"),
                                AssignmentOperatorNode.FromSymbol(1, "+="),
                                new UnaryExpressionNode(1,
                                    UnaryOperatorNode.FromSymbol(1, "-"),
                                    new IdentifierNode(1, "y")
                                )
                            ),
                            new AssignmentExpressionNode(1,
                                new IdentifierNode(1, "y"),
                                AssignmentOperatorNode.FromSymbol(1, "+="),
                                new LiteralNode(1, 1)
                            )
                        )
                    )
                ),
                new MatchIssues()
                    .AddError(new BlockEndValueMismatchError("x", 1, "1 + y", "1 - y"))
                    .AddError(new BlockEndValueMismatchError("y", 1, "1", "2"))
            );
        }

        [Test]
        public void NestedBlockTests()
        {
            this.Compare(
                new SourceComponentNode(
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "x"), new LiteralNode(1, 1)))
                    ),
                    new BlockStatementNode(1,
                        new DeclarationStatementNode(1,
                            new DeclarationSpecifiersNode(1, "int"),
                            new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "y"), new LiteralNode(1, 1)))
                        ),
                        new ExpressionStatementNode(1,
                            new AssignmentExpressionNode(1,
                                new IdentifierNode(1, "y"),
                                AssignmentOperatorNode.FromSymbol(1, "="),
                                new LiteralNode(1, 2)
                            )
                        ),
                        new ExpressionStatementNode(1,
                            new AssignmentExpressionNode(1,
                                new IdentifierNode(1, "x"),
                                AssignmentOperatorNode.FromSymbol(1, "="),
                                new IdentifierNode(1, "y")
                            )
                        )
                    ),
                    new ExpressionStatementNode(1, new IncrementExpressionNode(1, new IdentifierNode(1, "x"))),
                    new ExpressionStatementNode(1, new IncrementExpressionNode(1, new IdentifierNode(1, "x")))
                ),
                new SourceComponentNode(
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "x"), new LiteralNode(1, 1)))
                    ),
                    new BlockStatementNode(1,
                        new DeclarationStatementNode(1,
                            new DeclarationSpecifiersNode(1, "int"),
                            new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "y"), new LiteralNode(1, 1)))
                        ),
                        new ExpressionStatementNode(1,
                            new AssignmentExpressionNode(1,
                                new IdentifierNode(1, "x"),
                                AssignmentOperatorNode.FromSymbol(1, "="),
                                new IdentifierNode(1, "y")
                            )
                        )
                    ),
                    new ExpressionStatementNode(1, new IncrementExpressionNode(1, new IdentifierNode(1, "x")))
                ),
                new MatchIssues()
                    .AddError(new BlockEndValueMismatchError("y", 1, "2", "1"))
                    .AddError(new BlockEndValueMismatchError("x", 1, "4", "2"))
            );
        }
    }
}
