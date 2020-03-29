﻿using NUnit.Framework;
using RICC.AST.Nodes;
using RICC.AST.Nodes.Common;
using RICC.Core;
using RICC.Core.Common;

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

        [Test]
        public void MissingDeclarationTests()
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
                        new DeclarationSpecifiersNode(1, "time_t"),
                        new DeclaratorListNode(1, new ArrayDeclaratorNode(1, new IdentifierNode(1, "t")))
                    )
                ),
                new MatchIssues()
                    .AddWarning(
                        new MissingDeclarationWarning(
                            new DeclarationSpecifiersNode(1, "int"), 
                            new VariableDeclaratorNode(1, new IdentifierNode(1, "x"))
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
                    )
                ),
                new MatchIssues()
                    .AddWarning(
                        new MissingDeclarationWarning(
                            new DeclarationSpecifiersNode(1, "int"),
                            new VariableDeclaratorNode(1, new IdentifierNode(1, "x"))
                        )
                    )
                    .AddWarning(
                        new MissingDeclarationWarning(
                            new DeclarationSpecifiersNode(1, "time_t"),
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
            );
        }

        [Test]
        public void ExtraDeclarationTests()
        {
            this.Compare(
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
                        new DeclarationSpecifiersNode(1, "time_t"),
                        new DeclaratorListNode(1, new ArrayDeclaratorNode(1, new IdentifierNode(1, "t")))
                    )
                ),
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
                new MatchIssues()
                    .AddWarning(
                        new ExtraDeclarationWarning(
                            new DeclarationSpecifiersNode(1, "int"),
                            new VariableDeclaratorNode(1, new IdentifierNode(1, "x"))
                        )
                    )
            );

            this.Compare(
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
                    )
                ),
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
                ),
                new MatchIssues()
                    .AddWarning(
                        new ExtraDeclarationWarning(
                            new DeclarationSpecifiersNode(1, "int"),
                            new VariableDeclaratorNode(1, new IdentifierNode(1, "x"))
                        )
                    )
                    .AddWarning(
                        new ExtraDeclarationWarning(
                            new DeclarationSpecifiersNode(1, "time_t"),
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
            );
        }

        [Test]
        public void DeclSpecsMismatchTests()
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
                        new DeclarationSpecifiersNode(1, "public", "float"),
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
                ),
                new MatchIssues()
                    .AddWarning(
                        new DeclSpecsMismatchWarning(
                            new VariableDeclaratorNode(1, new IdentifierNode(1, "y")),
                            new DeclarationSpecifiersNode(1, "float"),
                            new DeclarationSpecifiersNode(1, "public", "float")
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
                        new DeclaratorListNode(1, new ArrayDeclaratorNode(1, new IdentifierNode(1, "t")))
                    )
                ),
                new SourceComponentNode(
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "double"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "z")))
                    ),
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "public", "float"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "y")))
                    ),
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "unsigned int"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "x")))
                    ),
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "time_t"),
                        new DeclaratorListNode(1, new ArrayDeclaratorNode(1, new IdentifierNode(1, "t")))
                    )
                ),
                new MatchIssues()
                    .AddWarning(
                        new DeclSpecsMismatchWarning(
                            new VariableDeclaratorNode(1, new IdentifierNode(1, "x")),
                            new DeclarationSpecifiersNode(1, "int"),
                            new DeclarationSpecifiersNode(1, "unsigned int")
                        )
                    )
                    .AddWarning(
                        new DeclSpecsMismatchWarning(
                            new VariableDeclaratorNode(1, new IdentifierNode(1, "y")),
                            new DeclarationSpecifiersNode(1, "float"),
                            new DeclarationSpecifiersNode(1, "public", "float")
                        )
                    )
            );
        }

        [Test]
        public void DeclaratorMismatchTests()
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
                    )
                ),
                new SourceComponentNode(
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "float"),
                        new DeclaratorListNode(1, new ArrayDeclaratorNode(1, new IdentifierNode(1, "y")))
                    ),
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "x")))
                    )
                ),
                new MatchIssues()
                    .AddWarning(
                        new DeclaratorMismatchWarning(
                            new VariableDeclaratorNode(1, new IdentifierNode(1, "y")),
                            new ArrayDeclaratorNode(1, new IdentifierNode(1, "y"))
                        )
                    )
            );

            this.Compare(
                new SourceComponentNode(
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new ArrayDeclaratorNode(1, new IdentifierNode(1, "x"), new LiteralNode(1, 3)))
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
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "f")))
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
                        new DeclaratorListNode(1, new FunctionDeclaratorNode(1, new IdentifierNode(1, "f")))
                    )
                ),
                new MatchIssues()
                    .AddWarning(
                        new DeclaratorMismatchWarning(
                            new ArrayDeclaratorNode(1, new IdentifierNode(1, "x"), new LiteralNode(1, 3)),
                            new VariableDeclaratorNode(1, new IdentifierNode(1, "x"))
                        )
                    )
                    .AddWarning(
                        new DeclaratorMismatchWarning(
                            new VariableDeclaratorNode(1, new IdentifierNode(1, "f")),
                            new FunctionDeclaratorNode(1, new IdentifierNode(1, "f"))
                        )
                    )
            );
        }

        [Test]
        public void ArraySizeMismatchTests()
        {

            this.Compare(
                new SourceComponentNode(
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new ArrayDeclaratorNode(1, new IdentifierNode(1, "x"), new LiteralNode(1, 3)))
                    ),
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "float"),
                        new DeclaratorListNode(1, new ArrayDeclaratorNode(1, new IdentifierNode(1, "y")))
                    )
                ),
                new SourceComponentNode(
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "float"),
                        new DeclaratorListNode(1, new ArrayDeclaratorNode(1, new IdentifierNode(1, "y"), new LiteralNode(1, 100)))
                    ),
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new ArrayDeclaratorNode(1, new IdentifierNode(1, "x"), new IdentifierNode(1, "z")))
                    )
                ),
                new MatchIssues()
                    .AddWarning(new SizeMismatchWarning("x", 1, "3", "z"))
                    .AddWarning(new SizeMismatchWarning("y", 1, null, "100"))
            );
        }

        [Test]
        public void InitializerMismatchTests()
        {
            this.Compare(
                new SourceComponentNode(
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "x"), new LiteralNode(1, 3)))
                    ),
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "float"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "y")))
                    )
                ),
                new SourceComponentNode(
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "float"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "y"), new LiteralNode(1, 100)))
                    ),
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "x"), new IdentifierNode(1, "z")))
                    )
                ),
                new MatchIssues()
                    .AddError(new InitializerMismatchError("x", 1, "3", "z"))
                    .AddError(new InitializerMismatchError("y", 1, null, "100"))
            );

            this.Compare(
                new SourceComponentNode(
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1, 
                            new ArrayDeclaratorNode(1, 
                                new IdentifierNode(1, "x"), 
                                new ArrayInitializerListNode(1, 
                                    new LiteralNode(1, 3),
                                    new IdentifierNode(1, "x"),
                                    new LiteralNode(1, 3)
                                )
                            )
                        )
                    ),
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "float"),
                        new DeclaratorListNode(1, new ArrayDeclaratorNode(1, new IdentifierNode(1, "y")))
                    )
                ),
                new SourceComponentNode(
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "float"),
                        new DeclaratorListNode(1, new ArrayDeclaratorNode(1, new IdentifierNode(1, "y"), new ArrayInitializerListNode(1)))
                    ),
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1,
                            new ArrayDeclaratorNode(1,
                                new IdentifierNode(1, "x"),
                                new ArrayInitializerListNode(1,
                                    new LiteralNode(1, 2),
                                    new IdentifierNode(1, "x"),
                                    new LiteralNode(1, 2)
                                )
                            )
                        )
                    )
                ),
                new MatchIssues()
                    .AddError(new InitializerMismatchError("x", 1, "3", "2", 0))
                    .AddError(new InitializerMismatchError("x", 1, "3", "2", 2))
                    .AddError(new InitializerMismatchError("y", 1, null, "[]"))
            );

            this.Compare(
                new SourceComponentNode(
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1,
                            new ArrayDeclaratorNode(1,
                                new IdentifierNode(1, "x"),
                                new ArrayInitializerListNode(1,
                                    new LiteralNode(1, 2),
                                    new IdentifierNode(1, "x"),
                                    new LiteralNode(1, 3)
                                )
                            )
                        )
                    ),
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "float"),
                        new DeclaratorListNode(1, new ArrayDeclaratorNode(1, new IdentifierNode(1, "y"), new ArrayInitializerListNode(1, new LiteralNode(1, 3))))
                    )
                ),
                new SourceComponentNode(
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "float"),
                        new DeclaratorListNode(1, new ArrayDeclaratorNode(1, new IdentifierNode(1, "y"), new ArrayInitializerListNode(1)))
                    ),
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1,
                            new ArrayDeclaratorNode(1,
                                new IdentifierNode(1, "x"),
                                new ArrayInitializerListNode(1,
                                    new LiteralNode(1, 2),
                                    new ArithmeticExpressionNode(1, 
                                        new LiteralNode(1, 3), 
                                        new ArithmeticOperatorNode(1, "+", BinaryOperations.ArithmeticFromSymbol("+")),
                                        new IdentifierNode(1, "x")
                                    ),
                                    new LiteralNode(1, 2)
                                )
                            )
                        )
                    )
                ),
                new MatchIssues()
                    .AddError(new InitializerMismatchError("x", 1, "x", "3 + x", 1))
                    .AddError(new InitializerMismatchError("x", 1, "3", "2", 2))
                    .AddError(new InitializerMismatchError("y", 1, "[3]", "[]", 3))
            );

            this.Compare(
                new SourceComponentNode(
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1,
                            new ArrayDeclaratorNode(1,
                                new IdentifierNode(1, "x"),
                                new ArrayInitializerListNode(1,
                                    new LiteralNode(1, 2),
                                    new ArithmeticExpressionNode(1,
                                        new ArithmeticExpressionNode(1,
                                            new LiteralNode(1, 4),
                                            new ArithmeticOperatorNode(1, "-", BinaryOperations.ArithmeticFromSymbol("-")),
                                            new LiteralNode(1, 1)
                                        ),
                                        new ArithmeticOperatorNode(1, "+", BinaryOperations.ArithmeticFromSymbol("+")),
                                        new IdentifierNode(1, "x")
                                    ),
                                    new LiteralNode(1, 3)
                                )
                            )
                        )
                    ),
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "float"),
                        new DeclaratorListNode(1, new ArrayDeclaratorNode(1, 
                            new IdentifierNode(1, "y"), 
                            new ArrayInitializerListNode(1, new LiteralNode(1, 3), new LiteralNode(1, 4)))
                        )
                    )
                ),
                new SourceComponentNode(
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "float"),
                        new DeclaratorListNode(1, new ArrayDeclaratorNode(1, new IdentifierNode(1, "y"), new ArrayInitializerListNode(1)))
                    ),
                    new DeclarationStatementNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new DeclaratorListNode(1,
                            new ArrayDeclaratorNode(1,
                                new IdentifierNode(1, "x"),
                                new ArrayInitializerListNode(1,
                                    new LiteralNode(1, 2),
                                    new ArithmeticExpressionNode(1,
                                        new LiteralNode(1, 3),
                                        new ArithmeticOperatorNode(1, "+", BinaryOperations.ArithmeticFromSymbol("+")),
                                        new IdentifierNode(1, "x")
                                    ),
                                    new LiteralNode(1, 2)
                                )
                            )
                        )
                    )
                ),
                new MatchIssues()
                    .AddError(new InitializerMismatchError("x", 1, "3", "2", 2))
                    .AddError(new InitializerMismatchError("y", 1, "[3,4]", "[]", 3))
            );
        }

        // TODO
    }
}
