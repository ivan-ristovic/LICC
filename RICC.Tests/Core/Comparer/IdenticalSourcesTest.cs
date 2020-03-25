using NUnit.Framework;
using RICC.AST.Nodes;

namespace RICC.Tests.Core.Comparer
{
    internal sealed class IdenticalSourcesTest : ComparerTestsBase
    {
        [Test]
        public void VariableDeclarationOrderTests()
        {
            this.Compare(
                new TranslationUnitNode(
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
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "t")))
                    )
                ),
                new TranslationUnitNode(
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
                        new DeclaratorListNode(1, new VariableDeclaratorNode(1, new IdentifierNode(1, "t")))
                    )
                )
            );
        }
    }
}
