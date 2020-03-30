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
                            new AssignmentOperatorNode(1, "=", BinaryOperations.AssignmentFromSymbol("=")),
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
                            new AssignmentOperatorNode(1, "=", BinaryOperations.AssignmentFromSymbol("=")),
                            new ArithmeticExpressionNode(1,
                                new IdentifierNode(1, "x"),
                                new ArithmeticOperatorNode(1, "+", BinaryOperations.ArithmeticFromSymbol("+")),
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
                            new AssignmentOperatorNode(1, "=", BinaryOperations.AssignmentFromSymbol("=")),
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
                            new AssignmentOperatorNode(1, "=", BinaryOperations.AssignmentFromSymbol("=")),
                            new ArithmeticExpressionNode(1,
                                new IdentifierNode(1, "x"),
                                new ArithmeticOperatorNode(1, "+", BinaryOperations.ArithmeticFromSymbol("+")),
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
