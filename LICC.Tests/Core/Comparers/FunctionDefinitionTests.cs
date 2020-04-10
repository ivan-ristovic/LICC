using NUnit.Framework;
using LICC.AST.Nodes;
using LICC.AST.Nodes.Common;
using LICC.Core;
using LICC.Core.Common;

namespace LICC.Tests.Core.Comparer
{
    internal sealed class FunctionDefinitionTests : ComparerTestsBase
    {
        [Test]
        public void EmptyFunctionNoParamsTests()
        {
            this.Compare(
                new SourceComponentNode(
                    new FunctionDefinitionNode(1,
                        new DeclarationSpecifiersNode(1, "static", "void"),
                        new FunctionDeclaratorNode(1, new IdentifierNode(1, "f")),
                        new BlockStatementNode(1)
                    )
                ),
                new SourceComponentNode(
                    new FunctionDefinitionNode(1,
                        new DeclarationSpecifiersNode(1, "static", "void"),
                        new FunctionDeclaratorNode(1, new IdentifierNode(1, "f")),
                        new BlockStatementNode(1)
                    )
                )
            );

            this.Compare(
                new SourceComponentNode(
                    new FunctionDefinitionNode(1,
                        new DeclarationSpecifiersNode(1, "void"),
                        new FunctionDeclaratorNode(1, new IdentifierNode(1, "f")),
                        new BlockStatementNode(1)
                    )
                ),
                new SourceComponentNode(
                    new FunctionDefinitionNode(1,
                        new DeclarationSpecifiersNode(1, "void"),
                        new FunctionDeclaratorNode(1, new IdentifierNode(1, "f")),
                        new BlockStatementNode(1, new EmptyStatementNode(1))
                    )
                )
            );

            this.Compare(
                new SourceComponentNode(
                    new FunctionDefinitionNode(1,
                        new DeclarationSpecifiersNode(1, "static", "void"),
                        new FunctionDeclaratorNode(1, new IdentifierNode(1, "f")),
                        new BlockStatementNode(1)
                    )
                ),
                new SourceComponentNode(
                    new FunctionDefinitionNode(1,
                        new DeclarationSpecifiersNode(1, "void"),
                        new FunctionDeclaratorNode(1, new IdentifierNode(1, "f")),
                        new BlockStatementNode(1)
                    )
                ),
                new MatchIssues()
                    .AddWarning(
                        new DeclSpecsMismatchWarning(
                            new FunctionDeclaratorNode(1, new IdentifierNode(1, "f")),
                            new DeclarationSpecifiersNode(1, "static", "void"),
                            new DeclarationSpecifiersNode(1, "void")
                        )
                    )
            );

            this.Compare(
                new SourceComponentNode(
                    new FunctionDefinitionNode(1,
                        new DeclarationSpecifiersNode(1, "void"),
                        new FunctionDeclaratorNode(1, new IdentifierNode(1, "f")),
                        new BlockStatementNode(1)
                    )
                ),
                new SourceComponentNode(
                    new FunctionDefinitionNode(1,
                        new DeclarationSpecifiersNode(1, "int"),
                        new FunctionDeclaratorNode(1, new IdentifierNode(1, "f")),
                        new BlockStatementNode(1)
                    )
                ),
                new MatchIssues()
                    .AddWarning(
                        new DeclSpecsMismatchWarning(
                            new FunctionDeclaratorNode(1, new IdentifierNode(1, "f")),
                            new DeclarationSpecifiersNode(1, "void"),
                            new DeclarationSpecifiersNode(1, "int")
                        )
                    )
            );
        }
    }
}
