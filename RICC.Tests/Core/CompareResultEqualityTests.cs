using NUnit.Framework;
using RICC.AST.Nodes;
using RICC.Core;
using RICC.Core.Common;

namespace RICC.Tests.Core
{
    internal sealed class CompareResultEqualityTests
    {
        [Test]
        public void NullComparisonTests()
        {
            Assert.That(new ComparerResult(), Is.Not.EqualTo(null));
            Assert.That(null, Is.Not.EqualTo(new ComparerResult()));
        }

        [Test]
        public void EqualityTests()
        {
            Assert.That(new ComparerResult(), Is.EqualTo(new ComparerResult()));

            var e1 = new InitializerMismatchError("x", 1, 1, 3);
            var e2 = new InitializerMismatchError("x", 1, 1, 2);
            Assert.That(new ComparerResult().WithError(e1), Is.Not.EqualTo(new ComparerResult()));
            Assert.That(new ComparerResult().WithError(e1), Is.Not.EqualTo(new ComparerResult().WithError(e2)));
            Assert.That(new ComparerResult().WithError(e1), Is.EqualTo(new ComparerResult().WithError(e1)));
            Assert.That(new ComparerResult().WithError(e2), Is.EqualTo(new ComparerResult().WithError(e2)));
            Assert.That(new ComparerResult().WithError(e1).WithError(e2), Is.EqualTo(new ComparerResult().WithError(e1).WithError(e2)));
            Assert.That(new ComparerResult().WithError(e1).WithError(e2), Is.Not.EqualTo(new ComparerResult().WithError(e2).WithError(e1)));

            var w1 = new MissingDeclarationWarning(new DeclarationSpecifiersNode(1), new VariableDeclaratorNode(1, new IdentifierNode(1, "x")));
            var w2 = new MissingDeclarationWarning(new DeclarationSpecifiersNode(1), new VariableDeclaratorNode(1, new IdentifierNode(1, "y")));
            Assert.That(new ComparerResult().WithWarning(w1), Is.Not.EqualTo(new ComparerResult()));
            Assert.That(new ComparerResult().WithWarning(w1), Is.Not.EqualTo(new ComparerResult().WithWarning(w2)));
            Assert.That(new ComparerResult().WithWarning(w1), Is.EqualTo(new ComparerResult().WithWarning(w1)));
            Assert.That(new ComparerResult().WithWarning(w2), Is.EqualTo(new ComparerResult().WithWarning(w2)));
            Assert.That(new ComparerResult().WithWarning(w1).WithWarning(w2), Is.EqualTo(new ComparerResult().WithWarning(w1).WithWarning(w2)));
            Assert.That(new ComparerResult().WithWarning(w1).WithWarning(w2), Is.Not.EqualTo(new ComparerResult().WithWarning(w2).WithWarning(w1)));

            Assert.That(new ComparerResult().WithWarning(w1).WithError(e2), Is.EqualTo(new ComparerResult().WithWarning(w1).WithError(e2)));
            Assert.That(new ComparerResult().WithWarning(w1).WithError(e1), Is.EqualTo(new ComparerResult().WithWarning(w1).WithError(e1)));
            Assert.That(new ComparerResult().WithError(e1).WithWarning(w1), Is.Not.EqualTo(new ComparerResult().WithWarning(w1).WithError(e1)));
        }
    }
}
