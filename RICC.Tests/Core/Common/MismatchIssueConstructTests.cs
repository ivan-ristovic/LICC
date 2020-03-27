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

        [Test]
        public void ParameterMismatchWarningConstructTests()
        {
            Assert.That(() => new ParameterMismatchWarning(
                "a", 1, 1,
                new FunctionParameterNode(1, new DeclarationSpecifiersNode(1), new VariableDeclaratorNode(1, new IdentifierNode(1, "x"))),
                new FunctionParameterNode(2, new DeclarationSpecifiersNode(2), new VariableDeclaratorNode(2, new IdentifierNode(2, "x")))
            ), Throws.InstanceOf<ArgumentException>());
            Assert.That(() => new ParameterMismatchWarning(
                "a", 1, 0,
                new FunctionParameterNode(1, new DeclarationSpecifiersNode(1, "const", "int"), new VariableDeclaratorNode(1, new IdentifierNode(1, "x"))),
                new FunctionParameterNode(2, new DeclarationSpecifiersNode(2, "const", "int"), new VariableDeclaratorNode(2, new IdentifierNode(2, "x")))
            ), Throws.InstanceOf<ArgumentException>());
            Assert.That(() => new ParameterMismatchWarning(
                "b", 1, true
            ), Throws.Nothing);
            Assert.That(() => new ParameterMismatchWarning(
                "a", 1, 0,
                new FunctionParameterNode(1, new DeclarationSpecifiersNode(1, "const", "int"), new VariableDeclaratorNode(1, new IdentifierNode(1, "x"))),
                new FunctionParameterNode(2, new DeclarationSpecifiersNode(2, "", "int"), new VariableDeclaratorNode(2, new IdentifierNode(2, "x")))
            ), Throws.Nothing);
        }

        [Test]
        public void SizeMismatchWarningConstructTests()
        {
            Assert.That(() => new SizeMismatchWarning(
                "a", 1, 2, 2
            ), Throws.InstanceOf<ArgumentException>());
            Assert.That(() => new SizeMismatchWarning(
                "a", 1, "x", "x"
            ), Throws.InstanceOf<ArgumentException>());
            Assert.That(() => new SizeMismatchWarning(
                "b", 1, 1, 2
            ), Throws.Nothing);
            Assert.That(() => new SizeMismatchWarning(
                "a", 1, "x", "y"
            ), Throws.Nothing);
        }
    }
}
