using NUnit.Framework;
using RICC.AST.Nodes;
using RICC.Core;

namespace RICC.Tests.Core.Comparer
{
    internal abstract class ComparerTestsBase
    {
        protected void Compare(ASTNode src, ASTNode dst, MatchIssues? expectedIssues = null)
        {
            MatchIssues issues = new ASTNodeComparer(src, dst).AttemptMatch();
            expectedIssues ??= new MatchIssues();
            CollectionAssert.AreEqual(expectedIssues, issues);
        }

        protected void PartialCompare(ASTNode src, ASTNode dst, MatchIssues expectedIssues)
        {
            MatchIssues issues = new ASTNodeComparer(src, dst).AttemptMatch();
            CollectionAssert.AreEqual(expectedIssues, issues.Take(expectedIssues.Count));
        }
    }
}
