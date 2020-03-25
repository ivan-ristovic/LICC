using System;
using NUnit.Framework;
using RICC.AST.Nodes;
using RICC.Core.Common;

namespace RICC.Tests.Core.Common
{
    internal sealed class MismatchIssueConstructTests
    {
        [Test]
        public void DeclaratorMismatchWarningConstructTests()
        {
            Assert.That(() => new DeclaratorMismatchWarning(
                new VariableDeclaratorNode(1, new IdentifierNode(1, "a")),
                new VariableDeclaratorNode(2, new IdentifierNode(2, "a"))
            ), Throws.InstanceOf<ArgumentException>());
            Assert.That(() => new DeclaratorMismatchWarning(
                new ArrayDeclaratorNode(1, new IdentifierNode(1, "a")),
                new ArrayDeclaratorNode(2, new IdentifierNode(2, "a"))
            ), Throws.InstanceOf<ArgumentException>());
            Assert.That(() => new DeclaratorMismatchWarning(
                new VariableDeclaratorNode(1, new IdentifierNode(1, "a")),
                new VariableDeclaratorNode(2, new IdentifierNode(2, "b"))
            ), Throws.Nothing);
            Assert.That(() => new DeclaratorMismatchWarning(
                new ArrayDeclaratorNode(1, new IdentifierNode(1, "a")),
                new VariableDeclaratorNode(2, new IdentifierNode(2, "a"))
            ), Throws.Nothing);
        }

        [Test]
        public void DeclSpecsMismatchWarningConstructTests()
        {
            Assert.That(() => new DeclSpecsMismatchWarning(
                new VariableDeclaratorNode(1, new IdentifierNode(1, "a")),
                new DeclarationSpecifiersNode(1),
                new DeclarationSpecifiersNode(2)
            ), Throws.InstanceOf<ArgumentException>());
            Assert.That(() => new DeclSpecsMismatchWarning(
                new ArrayDeclaratorNode(1, new IdentifierNode(1, "a")),
                new DeclarationSpecifiersNode(1, "int"),
                new DeclarationSpecifiersNode(2, "int")
            ), Throws.InstanceOf<ArgumentException>());
            Assert.That(() => new DeclSpecsMismatchWarning(
                new VariableDeclaratorNode(1, new IdentifierNode(1, "a")),
                new DeclarationSpecifiersNode(1),
                new DeclarationSpecifiersNode(2, "int")
            ), Throws.Nothing);
            Assert.That(() => new DeclSpecsMismatchWarning(
                new ArrayDeclaratorNode(1, new IdentifierNode(1, "a")),
                new DeclarationSpecifiersNode(1, "private", "int"),
                new DeclarationSpecifiersNode(2, "int")
            ), Throws.Nothing);
        }

        [Test]
        public void InitializerMismatchErrorConstructTests()
        {
            Assert.That(() => new InitializerMismatchError(
                "a", 1, 2, 2
            ), Throws.InstanceOf<ArgumentException>());
            Assert.That(() => new InitializerMismatchError(
                "a", 1, "x", "x"
            ), Throws.InstanceOf<ArgumentException>());
            Assert.That(() => new InitializerMismatchError(
                "b", 1, 1, 2
            ), Throws.Nothing);
            Assert.That(() => new InitializerMismatchError(
                "a", 1, "x", "y"
            ), Throws.Nothing);
        }
    }
}
