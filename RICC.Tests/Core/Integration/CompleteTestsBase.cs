using NUnit.Framework;
using RICC.AST.Builders.C;
using RICC.AST.Builders.Lua;
using RICC.AST.Builders.Pseudo;
using RICC.AST.Nodes;
using RICC.Core;

namespace RICC.Tests.Core.Integration
{
    internal abstract class CompleteTestsBase
    {
        [Test]
        public abstract void NoDifferenceTests();

        [Test]
        public abstract void DifferenceTests();


        public virtual ASTNode FromCSource(string src) 
            => new CASTBuilder().BuildFromSource(src);
        
        public virtual ASTNode FromLuaSource(string src) 
            => new LuaASTBuilder().BuildFromSource(src);
        
        public virtual ASTNode FromPseudoSource(string src) 
            => new PseudoASTBuilder().BuildFromSource(src);


        protected void Compare(ASTNode src, ASTNode dst, MatchIssues? expectedIssues = null)
        {
            MatchIssues issues = new ASTNodeComparer(src, dst).AttemptMatch();
            expectedIssues ??= new MatchIssues();
            Assert.That(issues, Is.EqualTo(expectedIssues));
        }
    }
}
