using LICC.AST.Nodes;
using LICC.Core;
using LICC.Core.Common;
using NUnit.Framework;

namespace LICC.Tests.Core.Integration
{
    internal sealed class SwapTests : CompleteTestsBase
    {
        [Test]
        public override void SemanticEquivallenceTests()
        {
            this.Compare(
                this.FromPseudoSource(@"
                    algorithm Swap 
                    begin
                        procedure swap(x : integer, y : integer)
                        begin
                            declare integer tmp = x
                            x = y  
                            y = tmp
                        end
                    end
                "),
                this.FromCSource(@"
                    void swap(int x, int y) {
                        int tmp = x;
                        x = y;
                        y = tmp;
                    }
                ")
            );

            this.Compare(
                this.FromPseudoSource(@"
                    algorithm Swap 
                    begin
                        procedure swap(x : integer, y : integer)
                        begin
                            declare integer tmp = x
                            x = y  
                            y = tmp
                        end
                    end
                "),
                this.FromLuaSource(@"
                    function swap(x, y)
                        x, y = y, x
                    end
                "),
                new MatchIssues()
                    .AddWarning(
                        new DeclSpecsMismatchWarning(
                            new FuncDeclNode(1,
                                new IdNode(1, "swap"),
                                new FuncParamsNode(1,
                                    new FuncParamNode(1,
                                        new DeclSpecsNode(1, "object"),
                                        new VarDeclNode(1, new IdNode(1, "x"))
                                    ),
                                    new FuncParamNode(1,
                                        new DeclSpecsNode(1, "object"),
                                        new VarDeclNode(1, new IdNode(1, "y"))
                                    )
                                )
                            ),
                            new DeclSpecsNode(1, "void"),
                            new DeclSpecsNode(1, "object")
                        )
                    )
                    .AddWarning(
                        new ParameterMismatchWarning(
                            "swap", 1, 1,
                            new FuncParamNode(1, new DeclSpecsNode(1, "integer"), new VarDeclNode(1, new IdNode(1, "x"))),
                            new FuncParamNode(1, new DeclSpecsNode(1), new VarDeclNode(1, new IdNode(1, "x")))
                        )
                    )
                    .AddWarning(
                        new ParameterMismatchWarning(
                            "swap", 1, 2,
                            new FuncParamNode(1, new DeclSpecsNode(1, "integer"), new VarDeclNode(1, new IdNode(1, "y"))),
                            new FuncParamNode(1, new DeclSpecsNode(1), new VarDeclNode(1, new IdNode(1, "y")))
                        )
                    )
                    .AddWarning(
                        new MissingDeclarationWarning(
                            new DeclSpecsNode(1, "integer"),
                            new VarDeclNode(1,
                                new IdNode(1, "tmp"),
                                new IdNode(1, "x")
                            )
                        )
                    )
            );


            this.Compare(
                this.FromPseudoSource(@"
                    algorithm Swap 
                    begin
                        procedure swap(x : object, y : object)
                        begin
                            declare object tmp = x
                            x = y  
                            y = tmp
                        end
                    end
                "),
                this.FromLuaSource(@"
                    function swap(x, y)
                        tmp = x
                        x = y
                        y = tmp
                    end
                "),
                new MatchIssues()
                    .AddWarning(
                        new DeclSpecsMismatchWarning(
                            new FuncDeclNode(1,
                                new IdNode(1, "swap"),
                                new FuncParamsNode(1,
                                    new FuncParamNode(1,
                                        new DeclSpecsNode(1, "object"),
                                        new VarDeclNode(1, new IdNode(1, "x"))
                                    ),
                                    new FuncParamNode(1,
                                        new DeclSpecsNode(1, "object"),
                                        new VarDeclNode(1, new IdNode(1, "y"))
                                    )
                                )
                            ),
                            new DeclSpecsNode(1, "void"),
                            new DeclSpecsNode(1, "object")
                        )
                    )
            );
        }

        [Test]
        public override void DifferenceTests()
        {
            this.Compare(
                this.FromPseudoSource(@"
                    algorithm Swap 
                    begin
                        procedure swap(x : integer, y : integer)
                        begin
                            declare integer tmp
                            tmp = x
                            x = y  
                            y = tmp
                        end
                    end
                "),
                this.FromLuaSource(@"
                    function swap(x, y)
                        x, y = x, y
                    end
                "),
                new MatchIssues()
                    .AddWarning(
                        new DeclSpecsMismatchWarning(
                            new FuncDeclNode(1,
                                new IdNode(1, "swap"),
                                new FuncParamsNode(1,
                                    new FuncParamNode(1,
                                        new DeclSpecsNode(1, "object"),
                                        new VarDeclNode(1, new IdNode(1, "x"))
                                    ),
                                    new FuncParamNode(1,
                                        new DeclSpecsNode(1, "object"),
                                        new VarDeclNode(1, new IdNode(1, "y"))
                                    )
                                )
                            ),
                            new DeclSpecsNode(1, "void"),
                            new DeclSpecsNode(1, "object")
                        )
                    )
                    .AddWarning(
                        new ParameterMismatchWarning(
                            "swap", 1, 1,
                            new FuncParamNode(1, new DeclSpecsNode(1, "integer"), new VarDeclNode(1, new IdNode(1, "x"))),
                            new FuncParamNode(1, new DeclSpecsNode(1), new VarDeclNode(1, new IdNode(1, "x")))
                        )
                    )
                    .AddWarning(
                        new ParameterMismatchWarning(
                            "swap", 1, 2,
                            new FuncParamNode(1, new DeclSpecsNode(1, "integer"), new VarDeclNode(1, new IdNode(1, "y"))),
                            new FuncParamNode(1, new DeclSpecsNode(1), new VarDeclNode(1, new IdNode(1, "y")))
                        )
                    )
                    .AddWarning(
                        new MissingDeclarationWarning(new DeclSpecsNode(1, "integer"), new VarDeclNode(1, new IdNode(1, "tmp")))
                    )
                    .AddError(new BlockEndValueMismatchError("x", 1, "y", "x"))
                    .AddError(new BlockEndValueMismatchError("y", 1, "x", "y"))
            );
        }
    }
}
