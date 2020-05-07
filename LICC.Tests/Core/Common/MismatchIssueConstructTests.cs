using System;
using NUnit.Framework;
using LICC.AST.Nodes;
using LICC.Core.Issues;

namespace LICC.Tests.Core.Common
{
    internal sealed class MismatchIssueConstructTests
    {
        [Test]
        public void DeclaratorMismatchWarningConstructTests()
        {
            Assert.That(() => new DeclaratorMismatchWarning(
                new VarDeclNode(1, new IdNode(1, "a")),
                new VarDeclNode(2, new IdNode(2, "a"))
            ), Throws.InstanceOf<ArgumentException>());
            Assert.That(() => new DeclaratorMismatchWarning(
                new ArrDeclNode(1, new IdNode(1, "a")),
                new ArrDeclNode(2, new IdNode(2, "a"))
            ), Throws.InstanceOf<ArgumentException>());
            Assert.That(() => new DeclaratorMismatchWarning(
                new VarDeclNode(1, new IdNode(1, "a")),
                new VarDeclNode(2, new IdNode(2, "b"))
            ), Throws.Nothing);
            Assert.That(() => new DeclaratorMismatchWarning(
                new ArrDeclNode(1, new IdNode(1, "a")),
                new VarDeclNode(2, new IdNode(2, "a"))
            ), Throws.Nothing);
        }

        [Test]
        public void DeclSpecsMismatchWarningConstructTests()
        {
            Assert.That(() => new DeclSpecsMismatchWarning(
                new VarDeclNode(1, new IdNode(1, "a")),
                new DeclSpecsNode(1),
                new DeclSpecsNode(2)
            ), Throws.InstanceOf<ArgumentException>());
            Assert.That(() => new DeclSpecsMismatchWarning(
                new ArrDeclNode(1, new IdNode(1, "a")),
                new DeclSpecsNode(1, "int"),
                new DeclSpecsNode(2, "int")
            ), Throws.InstanceOf<ArgumentException>());
            Assert.That(() => new DeclSpecsMismatchWarning(
                new VarDeclNode(1, new IdNode(1, "a")),
                new DeclSpecsNode(1),
                new DeclSpecsNode(2, "int")
            ), Throws.Nothing);
            Assert.That(() => new DeclSpecsMismatchWarning(
                new ArrDeclNode(1, new IdNode(1, "a")),
                new DeclSpecsNode(1, "private", "int"),
                new DeclSpecsNode(2, "int")
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
                new FuncParamNode(1, new DeclSpecsNode(1), new VarDeclNode(1, new IdNode(1, "x"))),
                new FuncParamNode(2, new DeclSpecsNode(2), new VarDeclNode(2, new IdNode(2, "x")))
            ), Throws.InstanceOf<ArgumentException>());
            Assert.That(() => new ParameterMismatchWarning(
                "a", 1, 0,
                new FuncParamNode(1, new DeclSpecsNode(1, "const", "int"), new VarDeclNode(1, new IdNode(1, "x"))),
                new FuncParamNode(2, new DeclSpecsNode(2, "const", "int"), new VarDeclNode(2, new IdNode(2, "x")))
            ), Throws.InstanceOf<ArgumentException>());
            Assert.That(() => new ParameterMismatchWarning(
                "b", 1, true
            ), Throws.Nothing);
            Assert.That(() => new ParameterMismatchWarning(
                "a", 1, 0,
                new FuncParamNode(1, new DeclSpecsNode(1, "const", "int"), new VarDeclNode(1, new IdNode(1, "x"))),
                new FuncParamNode(2, new DeclSpecsNode(2, "", "int"), new VarDeclNode(2, new IdNode(2, "x")))
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

        [Test]
        public void BlockEndValueMismatchErrorConstructTests()
        {
            Assert.That(() => new BlockEndValueMismatchError(
                "a", 1, 2, 2
            ), Throws.InstanceOf<ArgumentException>());
            Assert.That(() => new BlockEndValueMismatchError(
                "a", 1, "x", "x"
            ), Throws.InstanceOf<ArgumentException>());
            Assert.That(() => new BlockEndValueMismatchError(
                "b", 1, 1, 2
            ), Throws.Nothing);
            Assert.That(() => new BlockEndValueMismatchError(
                "a", 1, "x", "y"
            ), Throws.Nothing);
        }
    }
}
