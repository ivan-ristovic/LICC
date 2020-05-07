using NUnit.Framework;
using LICC.AST.Nodes;
using LICC.Core;
using LICC.Core.Issues;

namespace LICC.Tests.Core
{
    internal sealed class MatchIssuesEqualityTests
    {
        [Test]
        public void NullComparisonTests()
        {
            Assert.That(new MatchIssues(), Is.Not.EqualTo(null));
            Assert.That(null, Is.Not.EqualTo(new MatchIssues()));
        }

        [Test]
        public void EqualityTests()
        {
            Assert.That(new MatchIssues(), Is.EqualTo(new MatchIssues()));

            var e1 = new InitializerMismatchError("x", 1, 1, 3);
            var e2 = new InitializerMismatchError("x", 1, 1, 2);
            Assert.That(new MatchIssues().AddError(e1), Is.Not.EqualTo(new MatchIssues()));
            Assert.That(new MatchIssues().AddError(e1), Is.Not.EqualTo(new MatchIssues().AddError(e2)));
            Assert.That(new MatchIssues().AddError(e1), Is.EqualTo(new MatchIssues().AddError(e1)));
            Assert.That(new MatchIssues().AddError(e2), Is.EqualTo(new MatchIssues().AddError(e2)));
            Assert.That(new MatchIssues().AddError(e1).AddError(e2), Is.EqualTo(new MatchIssues().AddError(e1).AddError(e2)));
            Assert.That(new MatchIssues().AddError(e1).AddError(e2), Is.Not.EqualTo(new MatchIssues().AddError(e2).AddError(e1)));

            var w1 = new MissingDeclarationWarning(new DeclSpecsNode(1), new VarDeclNode(1, new IdNode(1, "x")));
            var w2 = new MissingDeclarationWarning(new DeclSpecsNode(1), new VarDeclNode(1, new IdNode(1, "y")));
            Assert.That(new MatchIssues().AddWarning(w1), Is.Not.EqualTo(new MatchIssues()));
            Assert.That(new MatchIssues().AddWarning(w1), Is.Not.EqualTo(new MatchIssues().AddWarning(w2)));
            Assert.That(new MatchIssues().AddWarning(w1), Is.EqualTo(new MatchIssues().AddWarning(w1)));
            Assert.That(new MatchIssues().AddWarning(w2), Is.EqualTo(new MatchIssues().AddWarning(w2)));
            Assert.That(new MatchIssues().AddWarning(w1).AddWarning(w2), Is.EqualTo(new MatchIssues().AddWarning(w1).AddWarning(w2)));
            Assert.That(new MatchIssues().AddWarning(w1).AddWarning(w2), Is.Not.EqualTo(new MatchIssues().AddWarning(w2).AddWarning(w1)));

            Assert.That(new MatchIssues().AddWarning(w1).AddError(e2), Is.EqualTo(new MatchIssues().AddWarning(w1).AddError(e2)));
            Assert.That(new MatchIssues().AddWarning(w1).AddError(e1), Is.EqualTo(new MatchIssues().AddWarning(w1).AddError(e1)));
            Assert.That(new MatchIssues().AddError(e1).AddWarning(w1), Is.Not.EqualTo(new MatchIssues().AddWarning(w1).AddError(e1)));
        }
    }
}
