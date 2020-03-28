using NUnit.Framework;
using RICC.AST.Nodes;

namespace RICC.Tests.Core.Comparer
{
    internal sealed class DeclarationTests : ComparerTestsBase
    {
        [Test]
        public void DeclarationOrderTests()
        {
            this.Compare(
                new SourceComponentNode(
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "x")))
                    ),
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "float"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "y")))
                    ),
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "double"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "z")))
                    ),
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "time_t"),
                        new DeclaratorListNode(1, new ArrayDeclaratorNode(1, new IdentifierNode(1, "t")))
                    )
                ),
                new SourceComponentNode(
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "double"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "z")))
                    ),
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "float"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "y")))
                    ),
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "x")))
                    ),
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "time_t"),
                        new DeclaratorListNode(1, new ArrayDeclaratorNode(1, new IdentifierNode(1, "t")))
                    )
                )
            );

            this.Compare(
                new SourceComponentNode(
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "x")))
                    ),
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "float"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "y")))
                    ),
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "double"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "z")))
                    ),
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "time_t"),
                        new DeclaratorListNode(1, 
                            new ArrayDeclaratorNode(1, 
                                new IdentifierNode(1, "arr"), 
                                new LiteralNode(1, 3),
                                new ArrayInitializerListNode(1, 
                                    new LiteralNode(1, 1),
                                    new LiteralNode(1, 2),
                                    new LiteralNode(1, 3)
                                )
                            )
                        )
                    ),
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "public time_t"),
                        new DeclaratorListNode(1, 
                            new FunctionDeclaratorNode(1, 
                                new IdentifierNode(1, "t"), 
                                new FunctionParametersNode(1, 
                                    new FunctionParameterNode(1, 
                                        new DeclarationSpecifiersNode(1, "int"), 
                                        new VariableDeclaratorNode(1, new IdentifierNode(1, "x"))
                                    )
                                )
                            )
                        )
                    )
                ),
                new SourceComponentNode(
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "double"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "z")))
                    ),
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "public time_t"),
                        new DeclaratorListNode(1,
                            new FunctionDeclaratorNode(1,
                                new IdentifierNode(1, "t"),
                                new FunctionParametersNode(1,
                                    new FunctionParameterNode(1,
                                        new DeclarationSpecifiersNode(1, "int"),
                                        new VariableDeclaratorNode(1, new IdentifierNode(1, "x"))
                                    )
                                )
                            )
                        )
                    ),
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "float"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "y")))
                    ),
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "x")))
                    ),
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "time_t"),
                        new DeclaratorListNode(1,
                            new ArrayDeclaratorNode(1,
                                new IdentifierNode(1, "arr"),
                                new LiteralNode(1, 3),
                                new ArrayInitializerListNode(1,
                                    new LiteralNode(1, 1),
                                    new LiteralNode(1, 2),
                                    new LiteralNode(1, 3)
                                )
                            )
                        )
                    )
                )
            );
        }

        // TODO
    }
}
