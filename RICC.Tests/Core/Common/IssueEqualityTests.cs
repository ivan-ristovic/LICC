using System.Collections.Generic;
using NUnit.Framework;
using RICC.AST.Nodes;
using RICC.Core.Common;

namespace RICC.Tests.Core.Common
{
    internal sealed class IssueEqualityTests
    {
        [Test]
        public void DeclaratorMismatchWarningEqualityTests()
        {
            var x = new DeclaratorMismatchWarning(
                new VariableDeclaratorNode(1, new IdentifierNode(1, "a")),
                new VariableDeclaratorNode(2, new IdentifierNode(2, "b"))
            );
            var y = new DeclaratorMismatchWarning(
                new VariableDeclaratorNode(1, new IdentifierNode(1, "a")),
                new VariableDeclaratorNode(1, new IdentifierNode(3, "b"))
            );
            var z = new DeclaratorMismatchWarning(
                new ArrayDeclaratorNode(1, new IdentifierNode(1, "a")),
                new VariableDeclaratorNode(2, new IdentifierNode(2, "a"))
            );
            var t = new DeclaratorMismatchWarning(
                new ArrayDeclaratorNode(1, new IdentifierNode(1, "a")),
                new ArrayDeclaratorNode(2, new IdentifierNode(2, "b"))
            );
            var w = new DeclaratorMismatchWarning(
                new ArrayDeclaratorNode(1, new IdentifierNode(1, "a")),
                new ArrayDeclaratorNode(2, new IdentifierNode(3, "b"))
            );

            this.AssertEquality(new[] { (x, y), (t, w) }, z);
        }

        [Test]
        public void DeclSpecsMismatchWarningEqualityTests()
        {
            var x = new DeclSpecsMismatchWarning(
                new VariableDeclaratorNode(1, new IdentifierNode(1, "a")),
                new DeclarationSpecifiersNode(1, "private", "int"),
                new DeclarationSpecifiersNode(2, "public", "int")
            );
            var y = new DeclSpecsMismatchWarning(
                new VariableDeclaratorNode(1, new IdentifierNode(1, "a")),
                new DeclarationSpecifiersNode(1, "private", "int"),
                new DeclarationSpecifiersNode(2, "public", "int")
            );
            var z = new DeclSpecsMismatchWarning(
                new ArrayDeclaratorNode(1, new IdentifierNode(1, "a")),
                new DeclarationSpecifiersNode(1, "", "int"),
                new DeclarationSpecifiersNode(2, "public", "int")
            );
            var t = new DeclSpecsMismatchWarning(
                new ArrayDeclaratorNode(1, new IdentifierNode(1, "a")),
                new DeclarationSpecifiersNode(1, "private", "point"),
                new DeclarationSpecifiersNode(2, "public", "int")
            );
            var w = new DeclSpecsMismatchWarning(
                new ArrayDeclaratorNode(1, new IdentifierNode(1, "a")),
                new DeclarationSpecifiersNode(1, "private", "point"),
                new DeclarationSpecifiersNode(2, "public", "int")
            );

            this.AssertEquality(new[] { (x, y), (t, w) }, z);
        }

        [Test]
        public void ExtraDeclarationWarningEqualityTests()
        {
            var x = new ExtraDeclarationWarning(
                new DeclarationSpecifiersNode(1, "private", "int"),
                new VariableDeclaratorNode(1, new IdentifierNode(1, "a"))
            );
            var y = new ExtraDeclarationWarning(
                new DeclarationSpecifiersNode(1, "private", "int"),
                new VariableDeclaratorNode(1, new IdentifierNode(1, "a"))
            );
            var z = new ExtraDeclarationWarning(
                new DeclarationSpecifiersNode(1, "", "int"),
                new ArrayDeclaratorNode(1, new IdentifierNode(1, "a"))
            );
            var t = new ExtraDeclarationWarning(
                new DeclarationSpecifiersNode(1, "private", "point"),
                new ArrayDeclaratorNode(1, new IdentifierNode(1, "a"))
            );
            var w = new ExtraDeclarationWarning(
                new DeclarationSpecifiersNode(1, "private", "point"),
                new ArrayDeclaratorNode(1, new IdentifierNode(1, "a"))
            );
            var k = new ExtraDeclarationWarning(
                new DeclarationSpecifiersNode(1, "", "int"),
                new ArrayDeclaratorNode(1, new IdentifierNode(1, "b"))
            );
            var l = new ExtraDeclarationWarning(
                new DeclarationSpecifiersNode(1, "private", "int"),
                new ArrayDeclaratorNode(1, new IdentifierNode(1, "b"))
            );

            this.AssertEquality(new[] { (x, y), (t, w) }, z, k, l);
        }
        
        [Test]
        public void InitializerMismatchErrorEqualityTests()
        {
            var x0 = new InitializerMismatchError("a", 1, 3, 2);
            var x1 = new InitializerMismatchError("a", 2, 3, 2);
            var x2 = new InitializerMismatchError("aa", 1, 3, 1);
            var x3 = new InitializerMismatchError("ab", 1, 3, 1);
            var x4 = new InitializerMismatchError("c", 1, '3', '2');
            var x5 = new InitializerMismatchError("c", 3, '3', '2');
            var x6 = new InitializerMismatchError("ab", 1, 3.1, 6.1);
            var x7 = new InitializerMismatchError("ab", 1, 4.1, 6.1);
            var x8 = new InitializerMismatchError("c", 1, "a", "b");
            var x9 = new InitializerMismatchError("c", 2, "a", "c");

            this.AssertEquality(new[] { (x0, x1), (x4, x5) }, x2, x3, x6, x7, x8, x9);
        }

        [Test]
        public void MissingDeclarationWarningEqualityTests()
        {
            var x = new MissingDeclarationWarning(
                new DeclarationSpecifiersNode(1, "private", "int"),
                new VariableDeclaratorNode(1, new IdentifierNode(1, "a"))
            );
            var y = new MissingDeclarationWarning(
                new DeclarationSpecifiersNode(1, "private", "int"),
                new VariableDeclaratorNode(1, new IdentifierNode(1, "a"))
            );
            var z = new MissingDeclarationWarning(
                new DeclarationSpecifiersNode(1, "", "int"),
                new ArrayDeclaratorNode(1, new IdentifierNode(1, "a"))
            );
            var t = new MissingDeclarationWarning(
                new DeclarationSpecifiersNode(1, "private", "point"),
                new ArrayDeclaratorNode(1, new IdentifierNode(1, "a"))
            );
            var w = new MissingDeclarationWarning(
                new DeclarationSpecifiersNode(1, "private", "point"),
                new ArrayDeclaratorNode(1, new IdentifierNode(1, "a"))
            );
            var k = new MissingDeclarationWarning(
                new DeclarationSpecifiersNode(1, "", "int"),
                new ArrayDeclaratorNode(1, new IdentifierNode(1, "b"))
            );
            var l = new MissingDeclarationWarning(
                new DeclarationSpecifiersNode(1, "private", "int"),
                new ArrayDeclaratorNode(1, new IdentifierNode(1, "b"))
            );

            this.AssertEquality(new[] { (x, y), (t, w) }, z, k, l);
        }


        private void AssertEquality<T>(IReadOnlyList<(T, T)> equalPairs, params T[] others)
            where T : BaseIssue
        {
            foreach ((T x, T y) in equalPairs) {
                AssertEq(x, y, true);
                foreach (T e in others) {
                    AssertEq(e, x, false);
                    AssertEq(e, y, false);
                }
            }

            for (int i = 0; i < equalPairs.Count; i++) {
                for (int j = i + 1; j < equalPairs.Count; j++) {
                    T x = equalPairs[i].Item1;
                    T y = equalPairs[i].Item2;
                    T z = equalPairs[j].Item1;
                    T t = equalPairs[j].Item2;
                    AssertEq(x, z, false);
                    AssertEq(x, t, false);
                    AssertEq(y, z, false);
                    AssertEq(y, t, false);
                }
            }


            static void AssertEq<T>(T x, T y, bool equal)
                where T : BaseIssue
            {
                Assert.That(x, equal ? Is.EqualTo(y) : Is.Not.EqualTo(y));
                Assert.That(y, equal ? Is.EqualTo(x) : Is.Not.EqualTo(x));
                Assert.That(x == y, equal ? Is.EqualTo(true) : Is.EqualTo(false));
                Assert.That(x != y, equal ? Is.EqualTo(false) : Is.EqualTo(true));
            }
        }
    }
}
