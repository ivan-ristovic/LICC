using NUnit.Framework;
using LICC.AST.Nodes;
using LICC.AST.Nodes.Common;
using LICC.Core;
using LICC.Core.Issues;

namespace LICC.Tests.Core.Comparer
{
    internal sealed class DeclarationTests : ComparerTestsBase
    {
        [Test]
        public void DeclarationOrderTests()
        {
            this.Compare(
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "float"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "y")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "double"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "z")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "time_t"),
                        new DeclListNode(1, new ArrDeclNode(1, new IdNode(1, "t")))
                    )
                ),
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "double"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "z")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "float"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "y")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "time_t"),
                        new DeclListNode(1, new ArrDeclNode(1, new IdNode(1, "t")))
                    )
                )
            );

            this.Compare(
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "float"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "y")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "double"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "z")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "time_t"),
                        new DeclListNode(1, 
                            new ArrDeclNode(1, 
                                new IdNode(1, "arr"), 
                                new LitExprNode(1, 3),
                                new ArrInitExprNode(1, 
                                    new LitExprNode(1, 1),
                                    new LitExprNode(1, 2),
                                    new LitExprNode(1, 3)
                                )
                            )
                        )
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "public time_t"),
                        new DeclListNode(1, 
                            new FuncDeclNode(1, 
                                new IdNode(1, "t"), 
                                new FuncParamsNode(1, 
                                    new FuncParamNode(1, 
                                        new DeclSpecsNode(1, "int"), 
                                        new VarDeclNode(1, new IdNode(1, "x"))
                                    )
                                )
                            )
                        )
                    )
                ),
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "double"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "z")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "public time_t"),
                        new DeclListNode(1,
                            new FuncDeclNode(1,
                                new IdNode(1, "t"),
                                new FuncParamsNode(1,
                                    new FuncParamNode(1,
                                        new DeclSpecsNode(1, "int"),
                                        new VarDeclNode(1, new IdNode(1, "x"))
                                    )
                                )
                            )
                        )
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "float"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "y")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "time_t"),
                        new DeclListNode(1,
                            new ArrDeclNode(1,
                                new IdNode(1, "arr"),
                                new LitExprNode(1, 3),
                                new ArrInitExprNode(1,
                                    new LitExprNode(1, 1),
                                    new LitExprNode(1, 2),
                                    new LitExprNode(1, 3)
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
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "float"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "y")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "double"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "z")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "time_t"),
                        new DeclListNode(1, new ArrDeclNode(1, new IdNode(1, "t")))
                    )
                ),
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "double"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "z")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "float"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "y")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "time_t"),
                        new DeclListNode(1, new ArrDeclNode(1, new IdNode(1, "t")))
                    )
                ),
                new MatchIssues()
                    .AddWarning(
                        new MissingDeclarationWarning(
                            new DeclSpecsNode(1, "int"), 
                            new VarDeclNode(1, new IdNode(1, "x"))
                        )
                    )
            );

            this.Compare(
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "float"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "y")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "double"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "z")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "public time_t"),
                        new DeclListNode(1,
                            new FuncDeclNode(1,
                                new IdNode(1, "t"),
                                new FuncParamsNode(1,
                                    new FuncParamNode(1,
                                        new DeclSpecsNode(1, "int"),
                                        new VarDeclNode(1, new IdNode(1, "x"))
                                    )
                                )
                            )
                        )
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "time_t"),
                        new DeclListNode(1,
                            new ArrDeclNode(1,
                                new IdNode(1, "arr"),
                                new LitExprNode(1, 3),
                                new ArrInitExprNode(1,
                                    new LitExprNode(1, 1),
                                    new LitExprNode(1, 2),
                                    new LitExprNode(1, 3)
                                )
                            )
                        )
                    )
                ),
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "double"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "z")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "public time_t"),
                        new DeclListNode(1,
                            new FuncDeclNode(1,
                                new IdNode(1, "t"),
                                new FuncParamsNode(1,
                                    new FuncParamNode(1,
                                        new DeclSpecsNode(1, "int"),
                                        new VarDeclNode(1, new IdNode(1, "x"))
                                    )
                                )
                            )
                        )
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "float"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "y")))
                    )
                ),
                new MatchIssues()
                    .AddWarning(
                        new MissingDeclarationWarning(
                            new DeclSpecsNode(1, "int"),
                            new VarDeclNode(1, new IdNode(1, "x"))
                        )
                    )
                    .AddWarning(
                        new MissingDeclarationWarning(
                            new DeclSpecsNode(1, "time_t"),
                            new ArrDeclNode(1,
                                new IdNode(1, "arr"),
                                new LitExprNode(1, 3),
                                new ArrInitExprNode(1,
                                    new LitExprNode(1, 1),
                                    new LitExprNode(1, 2),
                                    new LitExprNode(1, 3)
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
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "double"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "z")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "float"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "y")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "time_t"),
                        new DeclListNode(1, new ArrDeclNode(1, new IdNode(1, "t")))
                    )
                ),
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "float"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "y")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "double"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "z")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "time_t"),
                        new DeclListNode(1, new ArrDeclNode(1, new IdNode(1, "t")))
                    )
                ),
                new MatchIssues()
                    .AddWarning(
                        new ExtraDeclarationWarning(
                            new DeclSpecsNode(1, "int"),
                            new VarDeclNode(1, new IdNode(1, "x"))
                        )
                    )
            );

            this.Compare(
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "double"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "z")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "public time_t"),
                        new DeclListNode(1,
                            new FuncDeclNode(1,
                                new IdNode(1, "t"),
                                new FuncParamsNode(1,
                                    new FuncParamNode(1,
                                        new DeclSpecsNode(1, "int"),
                                        new VarDeclNode(1, new IdNode(1, "x"))
                                    )
                                )
                            )
                        )
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "float"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "y")))
                    )
                ),
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "float"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "y")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "double"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "z")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "public time_t"),
                        new DeclListNode(1,
                            new FuncDeclNode(1,
                                new IdNode(1, "t"),
                                new FuncParamsNode(1,
                                    new FuncParamNode(1,
                                        new DeclSpecsNode(1, "int"),
                                        new VarDeclNode(1, new IdNode(1, "x"))
                                    )
                                )
                            )
                        )
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "time_t"),
                        new DeclListNode(1,
                            new ArrDeclNode(1,
                                new IdNode(1, "arr"),
                                new LitExprNode(1, 3),
                                new ArrInitExprNode(1,
                                    new LitExprNode(1, 1),
                                    new LitExprNode(1, 2),
                                    new LitExprNode(1, 3)
                                )
                            )
                        )
                    )
                ),
                new MatchIssues()
                    .AddWarning(
                        new ExtraDeclarationWarning(
                            new DeclSpecsNode(1, "int"),
                            new VarDeclNode(1, new IdNode(1, "x"))
                        )
                    )
                    .AddWarning(
                        new ExtraDeclarationWarning(
                            new DeclSpecsNode(1, "time_t"),
                            new ArrDeclNode(1,
                                new IdNode(1, "arr"),
                                new LitExprNode(1, 3),
                                new ArrInitExprNode(1,
                                    new LitExprNode(1, 1),
                                    new LitExprNode(1, 2),
                                    new LitExprNode(1, 3)
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
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "float"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "y")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "double"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "z")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "time_t"),
                        new DeclListNode(1, new ArrDeclNode(1, new IdNode(1, "t")))
                    )
                ),
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "double"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "z")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "public", "float"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "y")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "time_t"),
                        new DeclListNode(1, new ArrDeclNode(1, new IdNode(1, "t")))
                    )
                ),
                new MatchIssues()
                    .AddWarning(
                        new DeclSpecsMismatchWarning(
                            new VarDeclNode(1, new IdNode(1, "y")),
                            new DeclSpecsNode(1, "float"),
                            new DeclSpecsNode(1, "public", "float")
                        )
                    )
            );

            this.Compare(
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "float"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "y")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "double"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "z")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "time_t"),
                        new DeclListNode(1, new ArrDeclNode(1, new IdNode(1, "t")))
                    )
                ),
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "double"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "z")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "public", "float"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "y")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "unsigned int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "time_t"),
                        new DeclListNode(1, new ArrDeclNode(1, new IdNode(1, "t")))
                    )
                ),
                new MatchIssues()
                    .AddWarning(
                        new DeclSpecsMismatchWarning(
                            new VarDeclNode(1, new IdNode(1, "x")),
                            new DeclSpecsNode(1, "int"),
                            new DeclSpecsNode(1, "unsigned int")
                        )
                    )
                    .AddWarning(
                        new DeclSpecsMismatchWarning(
                            new VarDeclNode(1, new IdNode(1, "y")),
                            new DeclSpecsNode(1, "float"),
                            new DeclSpecsNode(1, "public", "float")
                        )
                    )
            );
        }

        [Test]
        public void DeclaratorMismatchTests()
        {
            this.Compare(
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "float"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "y")))
                    )
                ),
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "float"),
                        new DeclListNode(1, new ArrDeclNode(1, new IdNode(1, "y")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x")))
                    )
                ),
                new MatchIssues()
                    .AddWarning(
                        new DeclaratorMismatchWarning(
                            new VarDeclNode(1, new IdNode(1, "y")),
                            new ArrDeclNode(1, new IdNode(1, "y"))
                        )
                    )
            );

            this.Compare(
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new ArrDeclNode(1, new IdNode(1, "x"), new LitExprNode(1, 3)))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "float"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "y")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "double"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "z")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "time_t"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "f")))
                    )
                ),
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "double"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "z")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "float"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "y")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "time_t"),
                        new DeclListNode(1, new FuncDeclNode(1, new IdNode(1, "f")))
                    )
                ),
                new MatchIssues()
                    .AddWarning(
                        new DeclaratorMismatchWarning(
                            new ArrDeclNode(1, new IdNode(1, "x"), new LitExprNode(1, 3)),
                            new VarDeclNode(1, new IdNode(1, "x"))
                        )
                    )
                    .AddWarning(
                        new DeclaratorMismatchWarning(
                            new VarDeclNode(1, new IdNode(1, "f")),
                            new FuncDeclNode(1, new IdNode(1, "f"))
                        )
                    )
            );
        }

        [Test]
        public void ArraySizeMismatchTests()
        {
            this.Compare(
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new ArrDeclNode(1, new IdNode(1, "x"), new LitExprNode(1, 3)))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "float"),
                        new DeclListNode(1, new ArrDeclNode(1, new IdNode(1, "y")))
                    )
                ),
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "float"),
                        new DeclListNode(1, new ArrDeclNode(1, new IdNode(1, "y"), new LitExprNode(1, 100)))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new ArrDeclNode(1, new IdNode(1, "x"), new IdNode(1, "z")))
                    )
                ),
                new MatchIssues()
                    .AddWarning(new SizeMismatchWarning("x", 1, "3", "z"))
                    .AddWarning(new SizeMismatchWarning("y", 1, "<unknown>", "100"))
            );

            this.Compare(
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, 
                            new ArrDeclNode(1, 
                                new IdNode(1, "x"),
                                new ArithmExprNode(1,
                                    new IdNode(1, "z"),
                                    ArithmOpNode.FromSymbol(1, "+"),
                                    new LitExprNode(1, 1)
                                )
                            )
                        )
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "float"),
                        new DeclListNode(1, new ArrDeclNode(1, new IdNode(1, "y")))
                    )
                ),
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "float"),
                        new DeclListNode(1, new ArrDeclNode(1, new IdNode(1, "y"), new IdNode(1, "n")))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new ArrDeclNode(1, new IdNode(1, "x"), new IdNode(1, "z")))
                    )
                ),
                new MatchIssues()
                    .AddWarning(new SizeMismatchWarning("x", 1, "1 + z", "z"))
                    .AddWarning(new SizeMismatchWarning("y", 1, "<unknown>", "n"))
            );
        }

        [Test]
        public void InitializerMismatchTests()
        {
            this.PartialCompare(
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x"), new LitExprNode(1, 3)))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "float"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "y")))
                    )
                ),
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "float"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "y"), new LitExprNode(1, 100)))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "x"), new IdNode(1, "z")))
                    )
                ),
                new MatchIssues()
                    .AddError(new InitializerMismatchError("x", 1, "3", "z"))
                    .AddError(new InitializerMismatchError("y", 1, "<unknown>", "100"))
            );

            this.PartialCompare(
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, 
                            new ArrDeclNode(1, 
                                new IdNode(1, "x"), 
                                new ArrInitExprNode(1, 
                                    new LitExprNode(1, 3),
                                    new IdNode(1, "x"),
                                    new LitExprNode(1, 3)
                                )
                            )
                        )
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "float"),
                        new DeclListNode(1, new ArrDeclNode(1, new IdNode(1, "y")))
                    )
                ),
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "float"),
                        new DeclListNode(1, new ArrDeclNode(1, new IdNode(1, "y"), new ArrInitExprNode(1)))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1,
                            new ArrDeclNode(1,
                                new IdNode(1, "x"),
                                new ArrInitExprNode(1,
                                    new LitExprNode(1, 2),
                                    new IdNode(1, "x"),
                                    new LitExprNode(1, 2)
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

            this.PartialCompare(
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1,
                            new ArrDeclNode(1,
                                new IdNode(1, "x"),
                                new ArrInitExprNode(1,
                                    new LitExprNode(1, 2),
                                    new IdNode(1, "x"),
                                    new LitExprNode(1, 3)
                                )
                            )
                        )
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "float"),
                        new DeclListNode(1, new ArrDeclNode(1, new IdNode(1, "y"), new ArrInitExprNode(1, new LitExprNode(1, 3))))
                    )
                ),
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "float"),
                        new DeclListNode(1, new ArrDeclNode(1, new IdNode(1, "y"), new ArrInitExprNode(1)))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1,
                            new ArrDeclNode(1,
                                new IdNode(1, "x"),
                                new ArrInitExprNode(1,
                                    new LitExprNode(1, 2),
                                    new ArithmExprNode(1, 
                                        new LitExprNode(1, 3), 
                                        ArithmOpNode.FromSymbol(1, "+"),
                                        new IdNode(1, "x")
                                    ),
                                    new LitExprNode(1, 2)
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

            this.PartialCompare(
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1,
                            new ArrDeclNode(1,
                                new IdNode(1, "x"),
                                new ArrInitExprNode(1,
                                    new LitExprNode(1, 2),
                                    new ArithmExprNode(1,
                                        new ArithmExprNode(1,
                                            new LitExprNode(1, 4),
                                            ArithmOpNode.FromSymbol(1, "-"),
                                            new LitExprNode(1, 1)
                                        ),
                                        ArithmOpNode.FromSymbol(1, "+"),
                                        new IdNode(1, "x")
                                    ),
                                    new LitExprNode(1, 3)
                                )
                            )
                        )
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "float"),
                        new DeclListNode(1, new ArrDeclNode(1, 
                            new IdNode(1, "y"), 
                            new ArrInitExprNode(1, new LitExprNode(1, 3), new LitExprNode(1, 4)))
                        )
                    )
                ),
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "float"),
                        new DeclListNode(1, new ArrDeclNode(1, new IdNode(1, "y"), new ArrInitExprNode(1)))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1,
                            new ArrDeclNode(1,
                                new IdNode(1, "x"),
                                new ArrInitExprNode(1,
                                    new LitExprNode(1, 2),
                                    new ArithmExprNode(1,
                                        new LitExprNode(1, 3),
                                        ArithmOpNode.FromSymbol(1, "+"),
                                        new IdNode(1, "x")
                                    ),
                                    new LitExprNode(1, 2)
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

        [Test]
        public void FunctionDeclarationTests()
        {
            this.Compare(
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new FuncDeclNode(1, new IdNode(1, "f")))
                    )
                ),
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "double"),
                        new DeclListNode(1, new FuncDeclNode(1, new IdNode(1, "f")))
                    )
                ),
                new MatchIssues()
                    .AddWarning(
                        new DeclSpecsMismatchWarning(
                            new FuncDeclNode(1, new IdNode(1, "f")),
                            new DeclSpecsNode(1, "int"),
                            new DeclSpecsNode(1, "double")
                        )
                    )
            );

            this.Compare(
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, 
                            new FuncDeclNode(1, 
                                new IdNode(1, "f"),
                                new FuncParamsNode(1, 
                                    new FuncParamNode(1, 
                                        new DeclSpecsNode(1, "int"), new VarDeclNode(1, new IdNode(1, "x"))
                                    ), 
                                    new FuncParamNode(1,
                                        new DeclSpecsNode(1, "int"), new VarDeclNode(1, new IdNode(1, "y"))
                                    )
                                )
                            )
                        )
                    )
                ),
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1,
                            new FuncDeclNode(1,
                                new IdNode(1, "f"),
                                new FuncParamsNode(1,
                                    new FuncParamNode(1,
                                        new DeclSpecsNode(1, "float"), new VarDeclNode(1, new IdNode(1, "x"))
                                    )
                                )
                            )
                        )
                    )
                ),
                new MatchIssues()
                    .AddWarning(new ParameterMismatchWarning("f", 1))
                    .AddWarning(
                        new ParameterMismatchWarning("f", 1, 1,
                            new FuncParamNode(1,
                                new DeclSpecsNode(1, "int"), new VarDeclNode(1, new IdNode(1, "x"))
                            ),
                            new FuncParamNode(1,
                                new DeclSpecsNode(1, "float"), new VarDeclNode(1, new IdNode(1, "x"))
                            )
                        )
                    )
            );
        }

        [Test]
        public void MixedTests()
        {
            this.Compare(
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1,
                            new ArrDeclNode(1,
                                new IdNode(1, "x"),
                                new ArrInitExprNode(1,
                                    new LitExprNode(1, 3),
                                    new IdNode(1, "x"),
                                    new LitExprNode(1, 3)
                                )
                            )
                        )
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "float"),
                        new DeclListNode(1, new ArrDeclNode(1, new IdNode(1, "y"), new LitExprNode(1, 3)))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "time_t"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "t")))
                    )
                ),
                new SourceNode(
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new ArrDeclNode(1, new IdNode(1, "y"), new LitExprNode(1, 4), new ArrInitExprNode(1)))
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1,
                            new ArrDeclNode(1,
                                new IdNode(1, "x"),
                                new ArrInitExprNode(1,
                                    new LitExprNode(1, 2),
                                    new IdNode(1, "x"),
                                    new LitExprNode(1, 2)
                                )
                            )
                        )
                    ),
                    new DeclStatNode(1,
                        new DeclSpecsNode(1, "int"),
                        new DeclListNode(1, new VarDeclNode(1, new IdNode(1, "ex")))
                    )
                ),
                new MatchIssues()
                    .AddError(new InitializerMismatchError("x", 1, "3", "2", 0))
                    .AddError(new InitializerMismatchError("x", 1, "3", "2", 2))
                    .AddWarning(
                        new DeclSpecsMismatchWarning(
                            new ArrDeclNode(1, new IdNode(1, "y"), new LitExprNode(1, 4), new ArrInitExprNode(1)),
                            new DeclSpecsNode(1, "float"),
                            new DeclSpecsNode(1, "int")
                        )
                    )
                    .AddWarning(new SizeMismatchWarning("y", 1, "3", "4"))
                    .AddError(new InitializerMismatchError("y", 1, null, "[]"))
                    .AddWarning(
                        new MissingDeclarationWarning(
                            new DeclSpecsNode(1, "time_t"),
                            new VarDeclNode(1, new IdNode(1, "t"))
                        )
                    )
                    .AddWarning(
                        new ExtraDeclarationWarning(
                            new DeclSpecsNode(1, "int"),
                            new VarDeclNode(1, new IdNode(1, "ex"))
                        )
                    )
            );
        }
    }
}
