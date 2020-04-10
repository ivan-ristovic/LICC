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

            MatchIssues issues = new MatchIssues()
                .AddWarning(
                    new DeclSpecsMismatchWarning(
                        new FunctionDeclaratorNode(1,
                            new IdentifierNode(1, "swap"),
                            new FunctionParametersNode(1,
                                new FunctionParameterNode(1,
                                    new DeclarationSpecifiersNode(1, "integer"),
                                    new VariableDeclaratorNode(1, new IdentifierNode(1, "x"))
                                ),
                                new FunctionParameterNode(1,
                                    new DeclarationSpecifiersNode(1, "integer"),
                                    new VariableDeclaratorNode(1, new IdentifierNode(1, "y"))
                                )
                            )
                        ),
                        new DeclarationSpecifiersNode(1, "void"),
                        new DeclarationSpecifiersNode(1, "object")
                    )
                )
                .AddWarning(
                    new ParameterMismatchWarning(
                        "swap", 1, 0,
                        new FunctionParameterNode(1, new DeclarationSpecifiersNode(1, "integer"), new VariableDeclaratorNode(1, new IdentifierNode(1, "x"))),
                        new FunctionParameterNode(1, new DeclarationSpecifiersNode(1), new VariableDeclaratorNode(1, new IdentifierNode(1, "x")))
                    )
                )
                .AddWarning(
                    new ParameterMismatchWarning(
                        "swap", 1, 1,
                        new FunctionParameterNode(1, new DeclarationSpecifiersNode(1, "integer"), new VariableDeclaratorNode(1, new IdentifierNode(1, "y"))),
                        new FunctionParameterNode(1, new DeclarationSpecifiersNode(1), new VariableDeclaratorNode(1, new IdentifierNode(1, "y")))
                    )
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
                issues
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
                        tmp = x
                        x = y
                        y = tmp
                    end
                "),
                issues
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
                            declare integer tmp = x
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
                            new FunctionDeclaratorNode(1,
                                new IdentifierNode(1, "swap"),
                                new FunctionParametersNode(1,
                                    new FunctionParameterNode(1,
                                        new DeclarationSpecifiersNode(1, "integer"),
                                        new VariableDeclaratorNode(1, new IdentifierNode(1, "x"))
                                    ),
                                    new FunctionParameterNode(1,
                                        new DeclarationSpecifiersNode(1, "integer"),
                                        new VariableDeclaratorNode(1, new IdentifierNode(1, "y"))
                                    )
                                )
                            ),
                            new DeclarationSpecifiersNode(1, "void"),
                            new DeclarationSpecifiersNode(1, "object")
                        )
                    )
                    .AddWarning(
                        new ParameterMismatchWarning(
                            "swap", 1, 0,
                            new FunctionParameterNode(1, new DeclarationSpecifiersNode(1, "integer"), new VariableDeclaratorNode(1, new IdentifierNode(1, "x"))),
                            new FunctionParameterNode(1, new DeclarationSpecifiersNode(1), new VariableDeclaratorNode(1, new IdentifierNode(1, "x")))
                        )
                    )
                    .AddWarning(
                        new ParameterMismatchWarning(
                            "swap", 1, 1,
                            new FunctionParameterNode(1, new DeclarationSpecifiersNode(1, "integer"), new VariableDeclaratorNode(1, new IdentifierNode(1, "y"))),
                            new FunctionParameterNode(1, new DeclarationSpecifiersNode(1), new VariableDeclaratorNode(1, new IdentifierNode(1, "y")))
                        )
                    )
            );
        }
    }
}
