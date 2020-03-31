using NUnit.Framework;
using RICC.AST.Nodes;
using RICC.AST.Nodes.Common;
using RICC.Core;
using RICC.Core.Common;

namespace RICC.Tests.Core.Comparer
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
        }
    }
}
