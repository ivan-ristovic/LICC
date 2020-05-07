using NUnit.Framework;
using LICC.AST.Nodes;
using LICC.AST.Nodes.Common;
using LICC.Core;
using LICC.Core.Issues;

namespace LICC.Tests.Core.Comparer
{
    internal sealed class BlockTests : ComparerTestsBase
    {
        [Test]
        public void BasicVariableAssignmentTests()
        {
            this.Compare(
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x"), new LitExprNode(1, 1)))
                    ),
                    new ExprStatNode(1,
                        new AssignExprNode(1,
                            new IdNode(1, "x"),
                            AssignOpNode.FromSymbol(1, "="),
                            new LitExprNode(1, 2)
                        )
                    )
                ),
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x"), new LitExprNode(1, 1)))
                    ),
                    new ExprStatNode(1,
                        new AssignExprNode(1,
                            new IdNode(1, "x"),
                            AssignOpNode.FromSymbol(1, "="),
                            new ArithmExprNode(1,
                                new IdNode(1, "x"),
                                ArithmOpNode.FromSymbol(1, "+"),
                                new LitExprNode(1, 1)
                            )
                        )
                    )
                )
            );

            this.Compare(
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x"), new LitExprNode(1, 1)))
                    ),
                    new ExprStatNode(1,
                        new AssignExprNode(1,
                            new IdNode(1, "x"),
                            AssignOpNode.FromSymbol(1, "="),
                            new LitExprNode(1, 3)
                        )
                    )
                ),
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x"), new LitExprNode(1, 1)))
                    ),
                    new ExprStatNode(1,
                        new AssignExprNode(1,
                            new IdNode(1, "x"),
                            AssignOpNode.FromSymbol(1, "="),
                            new ArithmExprNode(1,
                                new IdNode(1, "x"),
                                ArithmOpNode.FromSymbol(1, "+"),
                                new LitExprNode(1, 1)
                            )
                        )
                    )
                ),
                new MatchIssues()
                    .AddError(new BlockEndValueMismatchError("x", 1, "3", "2"))
            );

            this.Compare(
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x"), new IdNode(1, "p")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "y"), new IdNode(1, "x")))
                    ),
                    new ExprStatNode(1,
                        new AssignExprNode(1,
                            new IdNode(1, "y"),
                            AssignOpNode.FromSymbol(1, "="),
                            new ArithmExprNode(1,
                                new IdNode(1, "x"),
                                ArithmOpNode.FromSymbol(1, "+"),
                                new LitExprNode(1, 1)
                            )
                        )
                    ),
                    new ExprStatNode(1,
                        new AssignExprNode(1,
                            new IdNode(1, "x"),
                            AssignOpNode.FromSymbol(1, "="),
                            new ArithmExprNode(1,
                                new IdNode(1, "y"),
                                ArithmOpNode.FromSymbol(1, "+"),
                                new IdNode(1, "y")
                            )
                        )
                    )
                ),
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x"), new IdNode(1, "p")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "y"), new IdNode(1, "x")))
                    ),
                    new ExprStatNode(1,
                        new AssignExprNode(1,
                            new IdNode(1, "y"),
                            AssignOpNode.FromSymbol(1, "="),
                            new ArithmExprNode(1,
                                new IdNode(1, "x"),
                                ArithmOpNode.FromSymbol(1, "+"),
                                new LitExprNode(1, 2)
                            )
                        )
                    ),
                    new ExprStatNode(1,
                        new AssignExprNode(1,
                            new IdNode(1, "x"),
                            AssignOpNode.FromSymbol(1, "="),
                            new ArithmExprNode(1,
                                new LitExprNode(1, 3),
                                ArithmOpNode.FromSymbol(1, "*"),
                                new IdNode(1, "y")
                            )
                        )
                    )
                ),
                new MatchIssues()
                    .AddError(new BlockEndValueMismatchError("x", 1, "2*(1 + p)", "3*(2 + p)"))
                    .AddError(new BlockEndValueMismatchError("y", 1, "1 + p", "2 + p"))
            );
        }

        [Test]
        public void MultipleVariableAssignmentTests()
        {
            this.Compare(
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x"), new LitExprNode(1, 1)))
                    ),
                    new ExprStatNode(1,
                        new ExprListNode(1,
                            new AssignExprNode(1,
                                new IdNode(1, "x"),
                                AssignOpNode.FromSymbol(1, "="),
                                new LitExprNode(1, 2)
                            ),
                            new AssignExprNode(1,
                                new IdNode(1, "x"),
                                AssignOpNode.FromSymbol(1, "="),
                                new LitExprNode(1, 3)
                            )
                        )
                    )
                ),
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x"), new LitExprNode(1, 1)))
                    ),
                    new ExprStatNode(1,
                        new AssignExprNode(1,
                            new IdNode(1, "x"),
                            AssignOpNode.FromSymbol(1, "="),
                            new ArithmExprNode(1,
                                new IdNode(1, "x"),
                                ArithmOpNode.FromSymbol(1, "+"),
                                new LitExprNode(1, 2)
                            )
                        )
                    )
                )
            );

            this.Compare(
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x"), new LitExprNode(1, 1)))
                    ),
                    new ExprStatNode(1,
                        new ExprListNode(1,
                            new AssignExprNode(1,
                                new IdNode(1, "x"),
                                AssignOpNode.FromSymbol(1, "="),
                                new LitExprNode(1, 2)
                            ),
                            new AssignExprNode(1,
                                new IdNode(1, "x"),
                                AssignOpNode.FromSymbol(1, "="),
                                new LitExprNode(1, 3)
                            )
                        )
                    )
                ),
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x"), new LitExprNode(1, 1)))
                    ),
                    new ExprStatNode(1,
                        new AssignExprNode(1,
                            new IdNode(1, "x"),
                            AssignOpNode.FromSymbol(1, "="),
                            new ArithmExprNode(1,
                                new IdNode(1, "x"),
                                ArithmOpNode.FromSymbol(1, "+"),
                                new LitExprNode(1, 1)
                            )
                        )
                    )
                ),
                new MatchIssues()
                    .AddError(new BlockEndValueMismatchError("x", 1, "3", "2"))
            );

            this.Compare(
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x"), new LitExprNode(1, 1)))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "y"), new LitExprNode(1, 2)))
                    )
                ),
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x"), new LitExprNode(1, 1)))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "y"), new LitExprNode(1, 2)))
                    ),
                    new ExprStatNode(1,
                        new ExprListNode(1,
                            new AssignExprNode(1,
                                new IdNode(1, "x"),
                                AssignOpNode.FromSymbol(1, "="),
                                new LitExprNode(1, 2)
                            ),
                            new AssignExprNode(1,
                                new IdNode(1, "x"),
                                AssignOpNode.FromSymbol(1, "="),
                                new LitExprNode(1, 1)
                            ),
                            new AssignExprNode(1,
                                new IdNode(1, "y"),
                                AssignOpNode.FromSymbol(1, "="),
                                new LitExprNode(1, 1)
                            ),
                            new AssignExprNode(1,
                                new IdNode(1, "y"),
                                AssignOpNode.FromSymbol(1, "="),
                                new LitExprNode(1, 2)
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
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x"), new LitExprNode(1, 1)))
                    ),
                    new ExprStatNode(1,
                        new ExprListNode(1,
                            new AssignExprNode(1,
                                new IdNode(1, "x"),
                                AssignOpNode.FromSymbol(1, "+="),
                                new LitExprNode(1, 2)
                            ),
                            new AssignExprNode(1,
                                new IdNode(1, "x"),
                                AssignOpNode.FromSymbol(1, "+="),
                                new LitExprNode(1, 3)
                            )
                        )
                    )
                ),
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x"), new LitExprNode(1, 1)))
                    ),
                    new ExprStatNode(1,
                        new AssignExprNode(1,
                            new IdNode(1, "x"),
                            AssignOpNode.FromSymbol(1, "+="),
                            new ArithmExprNode(1,
                                new IdNode(1, "x"),
                                ArithmOpNode.FromSymbol(1, "+"),
                                new LitExprNode(1, 4)
                            )
                        )
                    )
                )
            );

            this.Compare(
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x"), new LitExprNode(1, 1)))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "y"), new LitExprNode(1, 1)))
                    ),
                    new ExprStatNode(1,
                        new AssignExprNode(1,
                            new IdNode(1, "x"),
                            AssignOpNode.FromSymbol(1, "+="),
                            new IdNode(1, "y")
                        )
                    )
                ),
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x"), new LitExprNode(1, 1)))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "y"), new LitExprNode(1, 1)))
                    ),
                    new ExprStatNode(1,
                        new AssignExprNode(1,
                            new IdNode(1, "x"),
                            AssignOpNode.FromSymbol(1, "-="),
                            new UnaryExprNode(1,
                                UnaryOpNode.FromSymbol(1, "-"),
                                new IdNode(1, "y")
                            )
                        )
                    )
                )
            );

            this.Compare(
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x"), new LitExprNode(1, 1)))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "y"), new LitExprNode(1, 1)))
                    ),
                    new ExprStatNode(1,
                        new AssignExprNode(1,
                            new IdNode(1, "x"),
                            AssignOpNode.FromSymbol(1, "+="),
                            new IdNode(1, "y")
                        )
                    )
                ),
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x"), new LitExprNode(1, 1)))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "y"), new LitExprNode(1, 1)))
                    ),
                    new ExprStatNode(1,
                        new ExprListNode(1,
                            new AssignExprNode(1,
                                new IdNode(1, "x"),
                                AssignOpNode.FromSymbol(1, "+="),
                                new UnaryExprNode(1,
                                    UnaryOpNode.FromSymbol(1, "-"),
                                    new IdNode(1, "y")
                                )
                            ),
                            new AssignExprNode(1,
                                new IdNode(1, "y"),
                                AssignOpNode.FromSymbol(1, "+="),
                                new LitExprNode(1, 1)
                            )
                        )
                    )
                ),
                new MatchIssues()
                    .AddError(new BlockEndValueMismatchError("x", 1, "2", "0"))
                    .AddError(new BlockEndValueMismatchError("y", 1, "1", "2"))
            );
        }

        [Test]
        public void NestedBlockTests()
        {
            this.Compare(
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x"), new LitExprNode(1, 1)))
                    ),
                    new BlockStatNode(1,
                        new DeclStatNode(1,
                            new DeclSpecsNode(1, "int"),
                            new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "y"), new LitExprNode(1, 1)))
                        ),
                        new ExprStatNode(1,
                            new AssignExprNode(1,
                                new IdNode(1, "y"),
                                AssignOpNode.FromSymbol(1, "="),
                                new LitExprNode(1, 2)
                            )
                        ),
                        new ExprStatNode(1,
                            new AssignExprNode(1,
                                new IdNode(1, "x"),
                                AssignOpNode.FromSymbol(1, "="),
                                new IdNode(1, "y")
                            )
                        )
                    ),
                    new ExprStatNode(1, new IncExprNode(1, new IdNode(1, "x"))),
                    new ExprStatNode(1, new IncExprNode(1, new IdNode(1, "x")))
                ),
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x"), new LitExprNode(1, 1)))
                    ),
                    new BlockStatNode(1,
                        new DeclStatNode(1,
                            new DeclSpecsNode(1, "int"),
                            new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "y"), new LitExprNode(1, 1)))
                        ),
                        new ExprStatNode(1,
                            new AssignExprNode(1,
                                new IdNode(1, "x"),
                                AssignOpNode.FromSymbol(1, "="),
                                new IdNode(1, "y")
                            )
                        )
                    ),
                    new ExprStatNode(1, new IncExprNode(1, new IdNode(1, "x"))),
                    new ExprStatNode(1, new IncExprNode(1, new IdNode(1, "x")))
                ),
                new MatchIssues()
                    .AddError(new BlockEndValueMismatchError("y", 1, "2", "1"))
                    .AddError(new BlockEndValueMismatchError("x", 1, "2", "1"))
                    .AddError(new BlockEndValueMismatchError("x", 1, "4", "3"))
            );

            this.Compare(
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x"), new IdNode(1, "p")))
                    ),
                    new BlockStatNode(1,
                        new DeclStatNode(1,
                            new DeclSpecsNode(1, "int"),
                            new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "y")))
                        ),
                        new DeclStatNode(1,
                            new DeclSpecsNode(1, "int"),
                            new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "z")))
                        ),
                        new ExprStatNode(1,
                            new AssignExprNode(1,
                                new IdNode(1, "z"),
                                AssignOpNode.FromSymbol(1, "="),
                                new IdNode(1, "q")
                            )
                        ),
                        new ExprStatNode(1,
                            new AssignExprNode(1,
                                new IdNode(1, "y"),
                                AssignOpNode.FromSymbol(1, "="),
                                new IdNode(1, "z")
                            )
                        ),
                        new ExprStatNode(1,
                            new AssignExprNode(1,
                                new IdNode(1, "x"),
                                AssignOpNode.FromSymbol(1, "="),
                                new IdNode(1, "y")
                            )
                        )
                    )
                ),
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x"), new IdNode(1, "p")))
                    ),
                    new BlockStatNode(1,
                        new DeclStatNode(1,
                            new DeclSpecsNode(1, "int"),
                            new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "y")))
                        ),
                        new DeclStatNode(1,
                            new DeclSpecsNode(1, "int"),
                            new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "z")))
                        ),
                        new ExprStatNode(1,
                            new AssignExprNode(1,
                                new IdNode(1, "z"),
                                AssignOpNode.FromSymbol(1, "="),
                                new IdNode(1, "w")
                            )
                        ),
                        new ExprStatNode(1,
                            new AssignExprNode(1,
                                new IdNode(1, "y"),
                                AssignOpNode.FromSymbol(1, "="),
                                new IdNode(1, "z")
                            )
                        ),
                        new ExprStatNode(1,
                            new AssignExprNode(1,
                                new IdNode(1, "x"),
                                AssignOpNode.FromSymbol(1, "="),
                                new IdNode(1, "y")
                            )
                        )
                    )
                ), new MatchIssues()
                    .AddError(new BlockEndValueMismatchError("y", 1, "q", "w"))
                    .AddError(new BlockEndValueMismatchError("z", 1, "q", "w"))
                    .AddError(new BlockEndValueMismatchError("x", 1, "q", "w"))
                    .AddError(new BlockEndValueMismatchError("x", 1, "q", "w"))
            );
        }

        [Test]
        public void SwapValueTests()
        {
            this.Compare(
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x"), new IdNode(1, "k")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "y"), new IdNode(1, "t")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "tmp"), new IdNode(1, "x")))
                    ),
                    new ExprStatNode(1,
                        new AssignExprNode(1,
                            new IdNode(1, "x"),
                            AssignOpNode.FromSymbol(1, "="),
                            new IdNode(1, "y")
                        )
                    ),
                    new ExprStatNode(1,
                        new AssignExprNode(1,
                            new IdNode(1, "y"),
                            AssignOpNode.FromSymbol(1, "="),
                            new IdNode(1, "tmp")
                        )
                    )
                ),
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x"), new IdNode(1, "k")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "y"), new IdNode(1, "t")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "tmp"), new IdNode(1, "x")))
                    ),
                    new ExprStatNode(1,
                        new AssignExprNode(1,
                            new IdNode(1, "y"),
                            AssignOpNode.FromSymbol(1, "="),
                            new IdNode(1, "tmp")
                        )
                    ),
                    new ExprStatNode(1,
                        new AssignExprNode(1,
                            new IdNode(1, "x"),
                            AssignOpNode.FromSymbol(1, "="),
                            new IdNode(1, "y")
                        )
                    )
                ),
                new MatchIssues()
                    .AddError(new BlockEndValueMismatchError("x", 1, "t", "k"))
            );
        }
    }
}
